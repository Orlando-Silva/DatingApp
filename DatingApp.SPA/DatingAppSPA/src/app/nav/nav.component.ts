import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Login Success!');
    },
    error => {
      this.alertify.error(error);
    });
  }

  loggedIn() {
    return this.authService.isLogged();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.success('loged out!');
  }

}
