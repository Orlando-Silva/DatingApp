import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_modules/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm;
  user: User;

  constructor(private route: ActivatedRoute,
              private alertifyService: AlertifyService,
              private userService: UserService,
              private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  updateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user)
        .subscribe(next => {
          this.alertifyService.success('Your data has been updated.');
          this.editForm.reset(this.user);
        },
        error => {
          this.alertifyService.error(error);
        });
  }
}
