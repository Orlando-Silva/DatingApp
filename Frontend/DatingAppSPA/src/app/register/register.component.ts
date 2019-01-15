import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelledRegister = new EventEmitter();

  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model)
      .subscribe( () => {
        this.alertify.success('Registration Success');
      },
      error => {
        this.alertify.error(error);
      });
  }

  cancel() {
    this.cancelledRegister.emit(false);
  }

}
