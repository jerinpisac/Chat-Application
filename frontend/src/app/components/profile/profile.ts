import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile implements OnInit{
  joinDate : string = "";
  constructor(public service : AuthService) {}
  
  ngOnInit(): void {
    this.joinDate = this.service.userDetails.joinedAt.split(" ")[0].toString();
  }

}
