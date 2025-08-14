import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  constructor(public service : AuthService, private router : Router) {}

  onBoard(){
    if(this.service.fullName == ""){
      alert("Enter Full Name");
    }
    else if(this.service.email == ""){
      alert("Enter your email");
    }
    else if(this.service.password == ""){
      alert("Enter the password");
    }
    else{
      localStorage.setItem("registerData", JSON.stringify({
        fullName : this.service.fullName,
        email : this.service.email,
        password : this.service.password
      }))
      this.router.navigateByUrl("/onboard");
    }
  }
}
