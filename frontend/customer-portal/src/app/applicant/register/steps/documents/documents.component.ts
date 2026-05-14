import { Component, EventEmitter, inject, Output } from '@angular/core';
import { ApplicantFormService } from '../../services/applicant-form.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-documents',
  imports: [FormsModule],
  templateUrl: './documents.component.html',
  styleUrl: './documents.component.scss'
})
export class DocumentsComponent {
  @Output() next = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();

  private formService = inject(ApplicantFormService);

  frontFile: File | null = null;
  backFile: File | null = null;

  onFront(e: Event): void {
    this.frontFile = (e.target as HTMLInputElement).files?.[0] ?? null;
  }

  onBack(e: Event): void {
    this.backFile = (e.target as HTMLInputElement).files?.[0] ?? null;
  }

  continue(): void {
    if (!this.frontFile || !this.backFile) return;
    this.formService.saveDocuments(this.frontFile, this.backFile);
    this.next.emit();
  }
}
