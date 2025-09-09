import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';
import { DatePipe } from '@angular/common';
import { userModel } from '../../shared/models/auth.model';

@Component({
  selector: 'app-requests',
  imports: [DatePipe],
  templateUrl: './requests.html',
  styleUrl: './requests.css'
})
export class Requests implements OnInit{
  requests : any = []
  currDate : Date = new Date;
  constructor(public service : AuthService) {}
  
  ngOnInit(): void {
    this.currDate = new Date();
    this.service.fetchRequests(this.service.userDetails.id).subscribe({
      next: res => this.requests = res,
      error: err => console.log(err)
    })
  }

  RemoveRequest(id : number){
    this.service.declineRequest(this.service.userDetails.id, id).subscribe({
      next: res => {
        alert("Removed request!");
        window.location.reload();
      },
      error: err => console.log(err)
    })
  }
}
