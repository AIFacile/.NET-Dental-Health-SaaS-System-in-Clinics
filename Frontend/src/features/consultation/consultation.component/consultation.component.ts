import { CommonModule } from "@angular/common";
import { Component, OnInit, inject, signal } from "@angular/core";
import { ReactiveFormsModule, FormBuilder, Validators, FormArray } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ConsultationService } from "../../../core/services/consultation-service";
import { HealthRecordDto, DiagnosisDto, TreatmentPlanDto, VisitDto, DiagnosisItemDto } from "../../../types/consultation-types";
import { Patient } from "../../../types/patient-types";
import { switchMap } from "rxjs/operators";

@Component({
  selector: 'app-consultation',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './consultation.component.html'
})
export class ConsultationComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private consultationService = inject(ConsultationService);
  private fb = inject(FormBuilder);
  selectedTeeth = signal<string[]>([]);

  // IDs form Route
  patient = signal<Patient | null>(null);
  patientId = signal<string>('');
  visitId = signal<string>('');

  // UI State
  activeTab = signal<'overview' | 'record' | 'diagnosis' | 'plan'>('overview');
  isLoading = signal<boolean>(false);

  // Data State
  healthRecords = signal<HealthRecordDto[]>([]);
  diagnosisHistory = signal<DiagnosisDto[]>([]);
  visits = signal<VisitDto[]>([]);
  currentPlan = signal<TreatmentPlanDto | null>(null);

  // Forms
  diagnosisForm = this.fb.group({
    summary: [''],
    items: this.fb.array([])
  });

  treatmentForm = this.fb.group({
    planType: ['', Validators.required],
    estimatedCost: [0],
    steps: this.fb.array([])
  });

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.patientId.set(params['patientId']);
      this.visitId.set(params['visitId']);
      this.loadData();
    });
  }

  loadData() {
    this.isLoading.set(true);
    const pid = this.patientId();
    const vid = this.visitId();

    this.consultationService.getHealthRecords(pid).subscribe(data => this.healthRecords.set(data));
    this.consultationService.getDiagnosis(pid).subscribe(data => this.diagnosisHistory.set(data));
    this.consultationService.getPatient(pid).subscribe(data => this.patient.set(data));
    this.consultationService.getVisits(pid).subscribe(data => this.visits.set(data));
    // Load other necessary data...
    this.isLoading.set(false);
  }

  // --- Diagnosis Logic ---

  toggleTooth(toothNum: string) {
    const current = this.selectedTeeth();
    const index = current.indexOf(toothNum);

    if (index > -1) {
      this.selectedTeeth.set(current.filter(t => t !== toothNum));
    } else {
      this.selectedTeeth.set([...current, toothNum]);
      this.addDiagnosisItem(toothNum);
    }
  }

  get diagnosisItems() {
    return this.diagnosisForm.get('items') as FormArray;
  }

  addDiagnosisItem(toothPos: string = '') {
    const itemGroup = this.fb.group({
      toothPosition: [toothPos, Validators.required],
      diseaseName: ['', Validators.required],
      severity: ['Moderate'],
      notes: ['']
    });
    this.diagnosisItems.push(itemGroup);
  }

  saveDiagnosis() {
    const { summary, items } = this.diagnosisForm.getRawValue();
    
    const payload = {
      visitId: this.visitId(),
      status: "In Treatment",
      summary: summary || '',
      items: (items as any[]).map(item => ({
        toothPosition: item.toothPosition,
        diseaseName: item.diseaseName,
        severity: item.severity,
        notes: item.notes
      }))
    };
  
    this.consultationService.saveDiagnosis(this.patientId(), payload).subscribe({
      next: (result) => {
        alert('Diagnosis and Visit status updated!');
        this.diagnosisForm.patchValue(result);
      },
      error: (err) => alert('Save failed: ' + err.message)
    });
  }

  commonDiagnoses = [
    { category: 'Caries', items: ['Deep Caries', 'Enamel Caries', 'Secondary Caries'] },
    { category: 'Endo', items: ['Acute Pulpitis', 'Chronic Pulpitis', 'Periapical Periodontitis'] },
    { category: 'Periodontal', items: ['Gingivitis', 'Periodontitis', 'Calculus'] },
    { category: 'Other', items: ['Tooth Fracture', 'Wisdom Tooth Impaction', 'Dentin Hypersensitivity'] }
  ];

  quickFill(disease: string) {
    const items = this.diagnosisItems;
    if (items.length > 0) {

      const lastItem = items.at(items.length - 1);
      if (!lastItem.get('diseaseName')?.value) {
        lastItem.patchValue({ diseaseName: disease });
        return;
      }
    }

    this.addDiagnosisItem('');
    items.at(items.length - 1).patchValue({ diseaseName: disease });
  }

  // --- Treatment Plan Logic ---
  get planSteps() {
    return this.treatmentForm.get('steps') as FormArray;
  }

  addStep() {
    const stepGroup = this.fb.group({
      stepOrder: [this.planSteps.length + 1],
      description: ['', Validators.required],
      cost: [0, Validators.required],
      status: [0] // Pending
    });
    this.planSteps.push(stepGroup);
  }

  calculateTotalCost() {
    const steps = this.treatmentForm.value.steps as any[];
    const total = steps.reduce((acc, step) => acc + (step.cost || 0), 0);
    this.treatmentForm.patchValue({ estimatedCost: total });
  }

  saveTreatmentPlan() {
    const payload = {
      patientId: this.patientId(),
      visitId: this.visitId(),
      ...this.treatmentForm.value
    };
    // Call service...
    console.log('Saving Plan:', payload);
    alert('Plan Saved (Mock)');
  }

  goBack() {
    const hasUnsavedChanges = this.diagnosisForm.dirty || this.treatmentForm.dirty;

    if (hasUnsavedChanges) {
      if (confirm('You have unsaved diagnosis or treatment steps. Are you sure you want to discard changes and go back?')) {
        this.router.navigate(['/doctor-dashboard']);
      }
    } else {
      this.router.navigate(['/doctor-dashboard']);
    }
  }

  /**
   * 功能 2：正式结束诊疗
   * 逻辑：触发业务流程更新（如：标记 Visit 为 Completed，通知财务生成账单）
   */
  // onEndConsultation() {
  //   // 1. 业务验证：如果诊断列表为空，给予提醒
  //   if (this.diagnosisItems.length === 0) {
  //     alert('Please add at least one diagnosis item before ending the session.');
  //     this.activeTab.set('diagnosis'); // 自动跳转到诊断页
  //     return;
  //   }

  //   // 2. 二次确认
  //   if (confirm('Confirm ending this session? This will finalize the record and notify the billing department.')) {
  //     this.isLoading.set(true);

  //     // 3. 调用 Service 更新 Visit 状态
  //     // 假设你有这样一个 API：PATCH /api/visits/{id}/complete
  //     this.consultationService.completeVisit(this.visitId()).subscribe({
  //       next: () => {
  //         this.isLoading.set(false);
  //         alert('Consultation completed. The patient has been moved to Billing queue.');
  //         this.router.navigate(['/doctor-dashboard']);
  //       },
  //       error: (err) => {
  //         this.isLoading.set(false);
  //         console.error(err);
  //       }
  //     });
  //   }
  // }
  // 历史记录模型
  patientHistory = signal<any[]>([]);
  medicalAlerts = signal<string[]>(['Penicillin Allergy', 'Hypertension']); // 示例过敏史

  loadHealthRecord(patientId: string) {
    this.consultationService.getHealthRecords(patientId).subscribe({
      next: (data) => this.patientHistory.set(data),
      error: (err) => console.error('Failed to load history', err)
    });
  }
}