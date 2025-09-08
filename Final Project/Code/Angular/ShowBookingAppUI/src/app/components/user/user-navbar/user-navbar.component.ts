import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-user-navbar',
  templateUrl: './user-navbar.component.html',
  styleUrls: ['./user-navbar.component.css']
})
export class UserNavbarComponent {
  constructor(private router: Router, private http: HttpClient) {}

  requestOrganizer() {
    const userId = localStorage.getItem('userId'); 
    this.http.post(`http://localhost:5227/api/user/request-organizer`, { userId })
      .subscribe({
        next: () => alert('Organizer request submitted. Admin approval pending.'),
        error: (err) => alert('Failed to submit request: ' + err.error)
      });
  }

  logout() {
    localStorage.removeItem('token');  // or sessionStorage
    this.router.navigate(['/login']);
  }
}
