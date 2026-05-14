import { Component, signal } from '@angular/core';
import { NavbarComponent } from '../../share/navbar/navbar.component';
import { FooterComponent } from '../../share/footer/footer.component';
import { StepItem, StepperComponent } from '../../share/stepper/stepper.component';
import { PersonalDataComponent } from './steps/personal-data/personal-data.component';
import { AddressComponent } from './steps/address/address.component';
import { ConfirmationComponent } from './steps/confirmation/confirmation.component';
import { DocumentsComponent } from './steps/documents/documents.component';
import { FinancialInfoComponent } from './steps/financial-info/financial-info.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [
    NavbarComponent, FooterComponent, StepperComponent,
    PersonalDataComponent, AddressComponent,
    FinancialInfoComponent, DocumentsComponent, ConfirmationComponent    
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  currentStep = signal(0);

  steps: StepItem[] = [
    { label: 'Dados pessoais' },
    { label: 'Endereço' },
    { label: 'Inf. financeiras' },
    { label: 'Documentos' },
    { label: 'Confirmação' }
  ];

  constructor(private router: Router) {}

  next(): void { this.currentStep.update(s => s + 1); }
  back(): void { this.currentStep.update(s => s - 1); }
  submit(): void { this.router.navigate(['/register/success']); }
}
