import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';

@Component({
  selector: 'app-friendsdash',
  imports: [],
  templateUrl: './friendsdash.html',
  styleUrl: './friendsdash.css'
})
export class Friendsdash implements OnInit{

  friends : any = [];

  constructor(public service : AuthService) {}
  
  ngOnInit(): void {
    this.service.fetchFriends(this.service.userDetails.id).subscribe({
      next: res => this.friends = res,
      error: err => console.log(err)
    })
  }

  
}
