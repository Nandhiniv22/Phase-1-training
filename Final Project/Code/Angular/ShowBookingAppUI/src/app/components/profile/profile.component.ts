import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profile = {
    name: '',
    email: '',
    password: ''
  };

  role = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    this.authService.getProfile().subscribe({
      next: (res) => {
        this.profile.name = res.name;
        this.profile.email = res.email;
        this.role = res.role;
      },
      error: (err) => {
        console.error('Failed to load profile:', err);
        alert('Failed to load profile');
      }
    });
  }

  updateProfile() {
    this.authService.updateProfile(this.profile).subscribe({
      next: () => {
        alert('Profile updated successfully!');
        this.profile.password = ''; // clear password after update
      },
      error: (err) => {
        console.error('Failed to update profile:', err);
        alert('Failed to update profile');
      }
    });
  }

  cancel() {
    if (this.role === 'Organizer') {
      this.router.navigate(['/organizer/dashboard']);
    } else if (this.role === 'Admin') {
      this.router.navigate(['/admin/dashboard']);
    } else {
      this.router.navigate(['/user/home']);
    }
  }
}
