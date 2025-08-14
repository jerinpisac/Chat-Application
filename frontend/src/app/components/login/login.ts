import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../shared/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  constructor(public service : AuthService, private router : Router) {}

  login(){
    this.service.loginUser().subscribe({
      next: (res:any) => {
        alert("User logged in successfully");
        localStorage.setItem("userDetails", JSON.stringify(res.user));
        this.service.userDetails = res.user;
        console.log(this.service.userDetails)
        this.router.navigateByUrl("/dashboard");
      },
      error: err => {
        alert(err.error)
      }
    })
  }
}
