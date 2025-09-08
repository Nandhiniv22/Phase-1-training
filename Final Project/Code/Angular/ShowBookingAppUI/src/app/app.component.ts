// app.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
   constructor(private router: Router) {}

  isAuthPage(): boolean {
    return this.router.url.includes('/login') || this.router.url.includes('/register');
  }

  isLanding(): boolean {
    return this.router.url === '/' || this.router.url.startsWith('/home');
  }

  isUser(): boolean {
    return this.router.url.startsWith('/user');
  }

  isOrganizer(): boolean {
    return this.router.url.startsWith('/organizer');
  }

  isAdmin(): boolean {
    return this.router.url.startsWith('/admin');
  }
}
