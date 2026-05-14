import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApplicantFormService } from '../../services/applicant-form.service';

@Component({
  selector: 'app-financial-info',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './financial-info.component.html',
  styleUrl: './financial-info.component.scss'
})
export class FinancialInfoComponent {
  @Output() next = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private formService = inject(ApplicantFormService);

  form = this.fb.group({
    occupation:      ['', Validators.required],
    monthlyIncome:   ['', Validators.required],
    employer:        [''],
    estimatedAssets: [''],
    isPep:           ['', Validators.required]
  });

  isInvalid(field: string): boolean {
    const c = this.form.get(field);
    return !!c?.invalid && !!c?.touched;
  }

  continue(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.formService.saveFinancialInfo(this.form.value as any);
    this.next.emit();
  }
}
