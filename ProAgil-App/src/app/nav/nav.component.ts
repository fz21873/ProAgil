import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  constructor(private authService: AuthService
    ,         public router: Router
    ,         public toastr: ToastrService) { }

  // tslint:disable-next-line:typedef
  ngOnInit() {
  }

  // tslint:disable-next-line:typedef
  loggedIn(){
     return this.authService.loggedIn();
  }

  // tslint:disable-next-line:typedef
  entrar(){
    this.router.navigate(['/user/login']);
  }
  // tslint:disable-next-line:typedef
  logout(){
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    this.toastr.show('Vocé esta Deslogado!');
    this.router.navigate(['/user/login']);
  }

  // tslint:disable-next-line:typedef
  showMenu(){
    return this.router.url !== '/user/login';
  }
}
