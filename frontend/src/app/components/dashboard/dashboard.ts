import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth';
import { userModel } from '../../shared/models/auth.model';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit{
  constructor(public service : AuthService, private router : Router) {}
  altPic : string = "../../../../alt.png"
  ngOnInit(): void {
    const storedUser = localStorage.getItem('userDetails');
    if(storedUser){
      this.service.userDetails = JSON.parse(storedUser) as userModel;
    }
  }

  logout(){
    if(confirm("Do you want to log out?")){
      localStorage.clear();
      this.service.userDetails = new userModel();
      this.service.fullName, this.service.email, this.service.password, this.service.bio, this.service.language, this.service.status = ""
      this.router.navigateByUrl("/login");
    }
  }
}