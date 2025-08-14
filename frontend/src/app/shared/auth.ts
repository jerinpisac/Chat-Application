import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { loginModel, registerModel, userModel } from './models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http : HttpClient) {}
  id : number = 0;
  fullName : string = "";
  email : string = "";
  password : string = "";
  bio : string = "";
  language : string = "";
  profilePic : string = "";
  idx : number = 1;
  status : string = "online";
  userDetails : userModel = new userModel();
  requestLoginData : loginModel = new loginModel();
  requestRegisterData : registerModel = new registerModel();

  loginUser(){
    this.requestLoginData.email = this.email;
    this.requestLoginData.password = this.password;
    return this.http.post(environment.localhost + "/Auth/login", this.requestLoginData);
  }

  registerUser(){
    this.requestRegisterData.fullName = this.fullName;
    this.requestRegisterData.email = this.email; 
    this.requestRegisterData.password = this.password; 
    this.requestRegisterData.bio = this.bio; 
    this.requestRegisterData.language = this.language;  
    this.requestRegisterData.profilePic = '../../../../' + this.idx.toString() + ".png";
    this.requestRegisterData.status = this.status; 
    return this.http.post(environment.localhost + "/Auth/register", this.requestRegisterData);
  }

  fetchUsers(id : number){
    return this.http.post(environment.localhost + "/Auth/fetchusers", { id });
  }
}
