import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApplicantFormService } from '../../services/applicant-form.service';

@Component({
  selector: 'app-personal-data',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './personal-data.component.html',
  styleUrl: './personal-data.component.scss'
})
export class PersonalDataComponent {
  @Output() next = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private formService = inject(ApplicantFormService);

  form = this.fb.group({
    fullName:   ['', Validators.required],
    cpf:        ['', Validators.required],
    birthDate:  ['', Validators.required],
    motherName: ['', Validators.required],
    email:      ['', [Validators.required, Validators.email]],
    phone:      ['', Validators.required]
  });

  isInvalid(field: string) {
    const c = this.form.get(field);
    return c?.invalid && c?.touched;
  }

  continue(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.formService.savePersonalData(this.form.value as any);
    this.next.emit();
  }
}
