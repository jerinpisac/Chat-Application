import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';

@Component({
  selector: 'app-notificationsdash',
  imports: [],
  templateUrl: './notificationsdash.html',
  styleUrl: './notificationsdash.css'
})
export class Notificationsdash implements OnInit{
  notifications : any = [];
  constructor(private service : AuthService){}

  ngOnInit(): void {
    this.service.fetchNotifications(this.service.userDetails.id).subscribe({
      next: res => {
        this.notifications = res;
        console.log(this.notifications)
      },
      error: err => console.log(err)
    })
  }

}
