import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { LoginRequest } from 'src/app/models/user.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  message: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    const loginData: LoginRequest = { email: this.email, password: this.password };
    this.authService.login(loginData).subscribe({
      next: (res) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('userRole', res.role);
        localStorage.setItem('userName', res.name);
        localStorage.setItem('userId', res.userId);

        this.message = 'Login successful! Redirecting...';

        if (res.role === 'Admin') this.router.navigate(['/admin/dashboard']);
        else if (res.role === 'Organizer') this.router.navigate(['/organizer/dashboard']);
        else this.router.navigate(['/user/home']);

      },
      error: () => this.message = 'Login failed. Check your credentials.'
    });
  }
}
