import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  loginForm: FormGroup = this.fb.group({
    tenantCode: ['', [Validators.required, Validators.minLength(4)]],
    username: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  isLoading = false;
  errorMessage = '';

  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.isLoading = false;
        console.log('Login successful', res);
        this.router.navigate(['/doctor-dashboard']); 
      },
      error: (err) => {
        this.isLoading = false;
        // Handle specific error messages from backend or generic one
        this.errorMessage = err.status === 401 
          ? 'Invalid Clinic Code, Username or Password.' 
          : 'A connection error occurred. Please try again later.';
      }
    });
  }
}