import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApplicantFormService } from '../../services/applicant-form.service';

@Component({
  selector: 'app-confirmation',
  imports: [FormsModule],
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.scss'
})
export class ConfirmationComponent {
  @Output() submit = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();

  private formService = inject(ApplicantFormService);

  form = this.formService.getForm();
  accepted = false;

  onSubmit(): void {
    if (!this.accepted) return;
    this.submit.emit();
  }
}
