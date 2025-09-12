import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';
import { userModel } from '../../shared/models/auth.model';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-homedash',
  imports: [RouterLink],
  templateUrl: './homedash.html',
  styleUrl: './homedash.css'
})
export class Homedash implements OnInit {
  users : userModel[] = []
  friends : any = []
  constructor(public service : AuthService) {}
  
  ngOnInit(): void {
    this.service.fetchUsers(this.service.userDetails.id).subscribe({
      next: (res : any) => {
        this.users = res
        console.log(res)
      }
    })

    this.service.fetchFriends(this.service.userDetails.id).subscribe({
      next: res => {
        this.friends = res;
        if(this.friends.length > 3) this.friends = this.friends.slice(0, 3);
      },
      error: err => console.log(err)
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
