import { Component, OnInit } from '@angular/core';
import { MatFormField, MatLabel, MatOption, MatSelect } from '@angular/material/select';
import { AuthService } from '../../shared/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { registerDetailsModel } from '../../shared/models/auth.model';

@Component({
  selector: 'app-onboard',
  imports: [MatFormField, MatSelect, MatLabel, MatOption, CommonModule, FormsModule],
  templateUrl: './onboard.html',
  styleUrl: './onboard.css'
})
export class Onboard implements OnInit{
  constructor(public service : AuthService, private router : Router) {}
  
  ngOnInit(): void {
    const user = JSON.parse(localStorage.getItem("registerData")!) as registerDetailsModel;
    this.service.fullName = user.fullName;
    this.service.email = user.email;
    this.service.password = user.password;
  }

  generate(){
    this.service.idx = Math.floor(Math.random() * 100) + 1; 
  }

  register(){
    this.service.registerUser().subscribe({
      next: res => {
        alert("User registered!");
        localStorage.removeItem("registerData");
        this.service.fullName = "";
        this.service.email = "";
        this.service.password = "";
        this.router.navigateByUrl("/login");
      },
      error: err => console.log(err)
    })
  }
}
