import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  /**
   * @request => Vai clonar qualquer requisição
   * @next => vamos manipular e retornar a requisição que foi clonada
   */
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser!: User;

    this.accountService.currentUser$.pipe(take(1)).subscribe((user: User) => {
      currentUser = user
      if(currentUser)
      {
        // Aqui estamos clonando a requisição
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`
          }
        });
      }
      // console.log(user);
    });
    return next.handle(request);
  }
}
