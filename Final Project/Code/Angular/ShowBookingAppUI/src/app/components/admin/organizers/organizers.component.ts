import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-organizers',
  templateUrl: './organizers.component.html',
})
export class OrganizersComponent implements OnInit {
  unapprovedOrganizers: any[] = [];
  approvedOrganizers: any[] = [];

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadOrganizers();
  }

  loadOrganizers(): void {
    this.adminService.getOrganizers().subscribe({
      next: (res) => {
        this.unapprovedOrganizers = res.unapproved || [];
        this.approvedOrganizers = res.approved || [];
      },
      error: (err) => console.error('Error loading organizers', err),
    });
  }

  approve(userId: number): void {
    this.adminService.approveOrganizer(userId).subscribe(() => {
      this.loadOrganizers(); // reload list after approval
    });
  }

  remove(userId: number): void {
    this.adminService.removeOrganizer(userId).subscribe(() => {
      this.loadOrganizers(); // reload list after removal
    });
  }
}
