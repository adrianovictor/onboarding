import { Component, EventEmitter, inject, Output } from '@angular/core';
import { ApplicantFormService } from '../../services/applicant-form.service';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-address',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './address.component.html',
  styleUrl: './address.component.scss'
})
export class AddressComponent {
  @Output() next = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private formService = inject(ApplicantFormService);

  form = this.fb.group({
    zipCode:      ['', Validators.required],
    street:       ['', Validators.required],
    number:       ['', Validators.required],
    complement:   [''],
    neighborhood: ['', Validators.required],
    city:         ['', Validators.required],
    state:        ['', Validators.required]
  });

  ufs = ['AC','AL','AP','AM','BA','CE','DF','ES','GO','MA','MT','MS',
        'MG','PA','PB','PR','PE','PI','RJ','RN','RS','RO','RR','SC',
        'SP','SE','TO'];  

  continue(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.formService.saveAddress(this.form.value as any);
    this.next.emit();
  }
}
