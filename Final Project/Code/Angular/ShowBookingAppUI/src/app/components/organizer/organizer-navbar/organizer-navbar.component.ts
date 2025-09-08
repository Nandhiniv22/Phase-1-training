import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-organizer-navbar',
  templateUrl: './organizer-navbar.component.html',
  styleUrls: ['./organizer-navbar.component.css']
})
export class OrganizerNavbarComponent {
  constructor(private router: Router) {}

  logout() {
    localStorage.removeItem('token');  // or sessionStorage
    this.router.navigate(['/login']);
  }
}
