import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./landing/landing.component').then(m => m.LandingComponent) },
  { path: 'open-account', loadComponent: () => import('./applicant/register/register.component').then(m => m.RegisterComponent) },
  { path: '**', redirectTo: '' }
];
