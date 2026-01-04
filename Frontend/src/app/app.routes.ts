import { Routes } from '@angular/router';
import { LoginComponent } from '../features/auth/login/login.component';
import { DoctorDashboardComponent } from '../features/dashboard/doctor-dashboard.component';
import { authGuard } from '../core/guards/auth-guard';
import { RoleName } from '../types/auth-types';
import { PatientDetailComponent } from '../features/dashboard/patient-detail/patient-detail.component';

export const routes: Routes = [
    {path: '', redirectTo:'Login',pathMatch:'full'},
    {path: 'login', component:LoginComponent},
    { 
        path: 'doctor-dashboard', 
        component: DoctorDashboardComponent,
        canActivate: [authGuard],
        data: { roles: [RoleName.Doctor, RoleName.SuperAdmin] }
    },
    { path: 'patients', component: PatientDetailComponent },
    { path: 'patients/:id', component: PatientDetailComponent },
    { path: '**', redirectTo:'login'}
];
