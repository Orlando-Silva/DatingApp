import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  canActivate(): Observable<boolean> | Promise<boolean> | boolean {

    if (this.authService.isLogged()) {
      return true;
    }

    this.alertify.error('You shall not pass');
    this.router.navigate(['/home']);
    return false;

  }
}
