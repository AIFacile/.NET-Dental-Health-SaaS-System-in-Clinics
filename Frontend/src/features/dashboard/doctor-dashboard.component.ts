import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth-service';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './doctor-dashboard.component.html'
})
export class DoctorDashboardComponent {
  public auth = inject(AuthService);

  // Track which menu item is selected
  activeSection = signal<string>('today'); 

  setSection(section: string) {
    this.activeSection.set(section);
  }
}