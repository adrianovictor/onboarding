import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [
    FormsModule,
    RouterModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  agency = '';
  account = '';

  constructor(private router: Router) {}

  login(): void {
    this.router.navigate(['/login']);
  }

  openAccount(): void {
    this.router.navigate(['/open-account']);
  }
}
