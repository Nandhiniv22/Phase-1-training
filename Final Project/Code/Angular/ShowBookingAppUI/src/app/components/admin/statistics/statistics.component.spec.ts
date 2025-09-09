import { Component } from '@angular/core';
import { ChartConfiguration, ChartType } from 'chart.js';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent {

  // Common Chart Options
  chartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: { position: 'top' }
    }
  };

  // Pie Chart
  pieChartType: ChartType = 'doughnut';
  pieChartLabels: string[] = [ 'Users', 'Organizers', 'Admins' ];
  pieChartData: any = {
    labels: this.pieChartLabels,
    datasets: [{
      data: [1200, 150, 10],
      backgroundColor: ['#0d6efd', '#198754', '#dc3545']
    }]
  };

  // Bar Chart
  barChartType: ChartType = 'bar';
  barChartLabels: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May'];
  barChartData = {
    labels: this.barChartLabels,
    datasets: [{
      label: 'Bookings',
      data: [300, 500, 400, 600, 750],
      backgroundColor: '#ffc107'
    }]
  };

  // Line Chart
  lineChartType: ChartType = 'line';
  lineChartLabels: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May'];
  lineChartData = {
    labels: this.lineChartLabels,
    datasets: [{
      label: 'Revenue ($)',
      data: [1000, 1500, 1800, 2000, 2500],
      fill: true,
      borderColor: '#6610f2',
      backgroundColor: 'rgba(102,16,242,0.2)'
    }]
  };
}
