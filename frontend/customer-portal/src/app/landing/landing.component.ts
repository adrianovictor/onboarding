import { Component } from '@angular/core';
import { NavbarComponent } from '../share/navbar/navbar.component';
import { FooterComponent } from '../share/footer/footer.component';
import { RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';

interface Benefit {
  icon: string;
  title: string;
  description: string;
}

@Component({
  selector: 'app-landing',
  imports: [
    NavbarComponent,
    FooterComponent,
    RouterLink,
    NgIf
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent {
  pills = ['Sem anuidade', 'Rendimento automático', 'Pix gratuito'];

  stats = [
    { value: '500k+', label: 'clientes ativos' },
    { value: '4.9★', label: 'avaliação no app' },
    { value: '5 min', label: 'para abrir a conta' }
  ];

  benefits = [
    {
      icon: '💳',
      title: 'Zero tarifas',
      description: 'Conta corrente sem mensalidade, sem taxa de manutenção e Pix ilimitado.',
      color: '#0c447c'
    },
    {
      icon: '📈',
      title: 'Rendimento automático',
      description: 'Seu saldo rende todo dia útil automaticamente, acima da poupança.',
      color: '#0f6e56'
    },
    {
      icon: '🔒',
      title: 'Segurança total',
      description: 'Tecnologia de ponta para proteger seu dinheiro e seus dados.',
      color: '#712b13'
    },
    {
      icon: '🎧',
      title: 'Suporte 24 horas',
      description: 'Atendimento humano a qualquer hora pelo app, chat ou telefone.',
      color: '#533489'  // wait, let me use valid hex
    }
  ];

  steps = [
    {
      icon: '📝',
      title: 'Preencha seus dados',
      description: 'Nome, CPF, renda e endereço em poucos campos'
    },
    {
      icon: '📄',
      title: 'Envie seu documento',
      description: 'Foto do RG ou CNH, frente e verso'
    },
    {
      icon: '🔍',
      title: 'Análise rápida',
      description: 'Nossa equipe analisa em até 1 dia útil'
    },
    {
      icon: '✅',
      title: 'Conta aprovada!',
      description: 'Receba seu cartão e comece a usar'
    }
  ];
}
