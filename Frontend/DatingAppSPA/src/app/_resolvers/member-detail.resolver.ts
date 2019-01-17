import { Injectable } from '@angular/core';
import { User } from '../_modules/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class MemberDetailsResolver implements Resolve<User> {

    constructor(private userService: UserService, private router: Router, private alertifyService: AlertifyService) { }

    resolve(router: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(router.params['id'] as number).pipe(
            catchError(error => {
                this.alertifyService.error('Problem retrieving data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
