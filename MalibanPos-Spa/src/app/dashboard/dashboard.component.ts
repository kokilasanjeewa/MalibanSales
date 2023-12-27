import { Component, OnInit } from '@angular/core';
import { ConnectionService } from '../_services/connection.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  constructor(public conn:ConnectionService) {}

  ngOnInit() {
    this.conn.checkOnlineStatus;
  }
}
