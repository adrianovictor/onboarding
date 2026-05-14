import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

export interface StepItem {
  label: string;
}

@Component({
  selector: 'app-stepper',
  imports: [CommonModule],
  templateUrl: './stepper.component.html',
  styleUrl: './stepper.component.scss'
})
export class StepperComponent {
  @Input() steps: StepItem[] = [];
  @Input() currentStep = 0;

  isCompleted(index: number): boolean {
    return index < this.currentStep;
  }

  isActive(index: number): boolean {
    return index === this.currentStep;
  }
}
