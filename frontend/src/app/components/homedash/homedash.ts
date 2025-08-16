import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';
import { userModel } from '../../shared/models/auth.model';

@Component({
  selector: 'app-homedash',
  imports: [],
  templateUrl: './homedash.html',
  styleUrl: './homedash.css'
})
export class Homedash implements OnInit {
  users : userModel[] = []
  constructor(public service : AuthService) {}
  
  ngOnInit(): void {
    this.service.fetchUsers(this.service.userDetails.id).subscribe({
      next: (res : any) => {
        this.users = res
        console.log(res)
      }
    })
  }

  sendRequest(id : number){
    this.service.sendFriendRequest(id).subscribe({
      next: res => {
        alert("Request has been sent");
        window.location.reload()
      },
      error: err => console.log(err)
    })
  }
}
