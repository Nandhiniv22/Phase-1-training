import { Component, OnInit } from '@angular/core';
import { ChartData, ChartType } from 'chart.js';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  stats: any = {};

  // Chart Data
  public pieChartLabels: string[] = ['Users', 'Approved Organizers', 'Pending Organizers', 'Blocked Users'];
  public pieChartData: ChartData<'pie', number[], string | string[]> = {
    labels: this.pieChartLabels,
    datasets: [
      { data: [0, 0, 0, 0] }  // initial values
    ]
  };
  public pieChartType: ChartType = 'pie';

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats() {
    this.adminService.getStatistics().subscribe({
      next: (data) => {
        this.stats = data;
        this.pieChartData = {
          labels: this.pieChartLabels,
          datasets: [
            {
              data: [
                data.totalUsers,
                data.approvedOrganizers,
                data.pendingOrganizers,
                data.blockedUsers
              ]
            }
          ]
        };
      },
      error: (err) => console.error(err)
    });
  }
} 