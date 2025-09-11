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
      },
      error: err => console.log(err)
    })
  }

  DeclineRequest(id : number){
    this.service.declineRequest(this.service.userDetails.id, id).subscribe({
      next: res => {
        alert("Declined request!");
        window.location.reload();
      },
      error: err => console.log(err)
    })
  }

  AcceptRequest(id : number){
    this.service.acceptRequest(this.service.userDetails.id, id).subscribe({
      next: res => {
        alert("Accepted request!");
        window.location.reload();
      },
      error: err => console.log(err)
    })
  }

  MarkAsRead(id : number, type : string){
    this.service.marksAsRead(id, this.service.userDetails.id, type).subscribe({
      next: res => {
        alert("Marked as read!");
        window.location.reload();
      },
      error: err => console.log(err)
    })
  }

}
