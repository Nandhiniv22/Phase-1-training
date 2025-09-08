import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OrganizerService } from '../../../services/organizer.service';

@Component({
  selector: 'app-add-theatre',
  templateUrl: './add-theatre.component.html',
  styleUrls: ['./add-theatre.component.css']
})
export class AddTheatreComponent {
  theatre = {
    name: '',
    location: '',
    organizerId: 0
  };

  message: string = '';
  messageType: 'success' | 'error' | '' = ''; // ✅ added this

  constructor(private organizerService: OrganizerService, private router: Router) {}

  ngOnInit(): void {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.theatre.organizerId = Number(userId);
    }
  }

  onSubmit() {
    if (!this.theatre.name || !this.theatre.location) {
      this.message = 'All fields are required!';
      this.messageType = 'error'; 
      return;
    }

    this.organizerService.createTheatre(this.theatre).subscribe({
      next: () => {
        this.message = '✅ Theatre added successfully!';
        this.messageType = 'success'; // ✅ set type
        setTimeout(() => this.router.navigate(['/organizer/dashboard']), 1500);
      },
      error: () => {
        this.message = '❌ Failed to add theatre.';
        this.messageType = 'error'; // ✅ set type
      }
    });
  }
}
