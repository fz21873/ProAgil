import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseURL = 'http://localhost:5000/api/user/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  usuario: string;

  constructor(private http: HttpClient ) { }

  // tslint:disable-next-line:typedef
  login(model: any){
    return this.http
     .post(`${this.baseURL}login`, model).pipe(
       map((response: any) => {
         const user = response;
         if (user){
           console.log(user.user.userName);
           localStorage.setItem('token', user.token);
           localStorage.setItem('usuario', user.user.userName);
           this.decodedToken = this.jwtHelper.decodeToken(user.token);
         }
       })
     );
  }
  // tslint:disable-next-line:typedef
  register(model: any){
    return this.http .post(`${this.baseURL}register`, model);
  }

  // tslint:disable-next-line:typedef
  loggedIn(){
    const token = localStorage.getItem('token');
    this.usuario = localStorage.getItem('usuario');
    return !this.jwtHelper.isTokenExpired(token);
  }

}
