import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { PatientService } from '../../../core/services/patient-service';
import { Patient } from '../../../types/patient-types';


@Component({
  selector: 'app-patient-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-detail.component.html'
})
export class PatientDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private patientService = inject(PatientService);
  
  patient = signal<Patient | null>(null);
  activeTab = signal<'records' | 'diagnosis' | 'plans' | 'xrays'>('records');

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      // Fetching specific patient from your /api/patients/{id}
      this.patientService.getPatientById(id).subscribe(data => this.patient.set(data));
    }
  }
}