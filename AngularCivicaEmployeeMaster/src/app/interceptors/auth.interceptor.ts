import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalstorageService } from '../services/localstorage.service';
import { localStorageKeys } from '../services/localStorageKeys';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private localStorageHelper:LocalstorageService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.localStorageHelper.getItem(localStorageKeys.TockenName);
    if(token){
      const cloneRequst = request.clone({
        headers:request.headers.set('Authorization',`Bearer ${token}`)
      });
      return next.handle(cloneRequst);
    }else{
      return next.handle(request);
    } 
  }
}
