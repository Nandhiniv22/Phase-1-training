import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
  users: any[] = [];

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.adminService.getUsers().subscribe(data => {
      this.users = data;
    });
  }

  block(userId: number) {
    this.adminService.blockUser(userId).subscribe({
      next: () => {
        this.loadUsers(); // reload list after block
      },
      error: (err) => console.error(err)
    });
  }

  unblock(userId: number) {
    this.adminService.unblockUser(userId).subscribe({
      next: () => {
        this.loadUsers(); // reload list after unblock
      },
      error: (err) => console.error(err)
    });
  }
}
