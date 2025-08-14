import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
  
  constructor(public service : AuthService) {}

}
