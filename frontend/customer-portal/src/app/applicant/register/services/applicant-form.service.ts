import { Injectable, signal } from '@angular/core';

export interface PersonalData {
  fullName: string;
  cpf: string;
  birthDate: string;
  motherName: string;
  email: string;
  phone: string;
}

export interface Address {
  zipCode: string;
  street: string;
  number: string;
  complement: string;
  neighborhood: string;
  city: string;
  state: string;
}

export interface FinancialInfo {
  occupation: string;
  monthlyIncome: number;
  employer: string;
  estimatedAssets: number;
  isPep: string;
}

export interface ApplicantForm {
  personalData: PersonalData | null;
  address: Address | null;
  financialInfo: FinancialInfo | null;
  documents: { front: File | null; back: File | null };
}

@Injectable({
  providedIn: 'root'
})
export class ApplicantFormService {
  private form = signal<ApplicantForm>({
    personalData: null,
    address: null,
    financialInfo: null,
    documents: { front: null, back: null }
  });

  getForm() {
    return this.form.asReadonly();
  }

  savePersonalData(data: PersonalData): void {
    this.form.update(f => ({ ...f, personalData: data }));
  }

  saveAddress(data: Address): void {
    this.form.update(f => ({ ...f, address: data }));
  }

  saveFinancialInfo(data: FinancialInfo): void {
    this.form.update(f => ({ ...f, financialInfo: data }));
  }

  saveDocuments(front: File, back: File): void {
    this.form.update(f => ({ ...f, documents: { front, back } }));
  }

  reset(): void {
    this.form.set({
      personalData: null,
      address: null,
      financialInfo: null,
      documents: { front: null, back: null }
    });
  }
}
