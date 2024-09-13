import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap, Subject } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { localStorageKeys } from '../localStorageKeys';
import { LocalstorageService } from '../localstorage.service';
import { User } from 'src/app/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl ='http://localhost:5165/api/Auth/';
private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(localStorageKeys.TockenName));
private userNameSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(localStorageKeys.userId));

  constructor(private http:HttpClient, private localStorageHelper:LocalstorageService) { }

  signup(user:User):Observable<ApiResponse<string>>{
    const body = user;
    return this.http.post<ApiResponse<string>>('http://localhost:5165/api/Auth/Register',body);
  }

  signin(username:string, password:string):Observable<ApiResponse<string>>{
    const body = {username,password};
    return this.http.post<ApiResponse<string>>(this.apiUrl +"Login",body).pipe(
      tap(response=>{
        if(response.success){
          this.localStorageHelper.setItem(localStorageKeys.TockenName,response.data);
          this.localStorageHelper.setItem(localStorageKeys.userId,username);
          this.authState.next(this.localStorageHelper.hasItem(localStorageKeys.TockenName));
          this.userNameSubject.next(username);
        }
      })
    );
  }

  signOut(){
    this.localStorageHelper.removeItem(localStorageKeys.TockenName);
    this.localStorageHelper.removeItem(localStorageKeys.userId);
    this.authState.next(false);
    this.userNameSubject.next(null);
  }

  isAuthenticated(){
    return this.authState.asObservable();
  }

  getUserName():Observable<string | null | undefined>{
    return this.userNameSubject.asObservable();
  }

  forgotPassword(userDetail:any):Observable<ApiResponse<string>>{
    const body = userDetail;
    return this.http.put<ApiResponse<string>>(this.apiUrl+"ForgotPassword",body);
  }

  changePassword(userPasswordDetail:any):Observable<ApiResponse<string>>{
    const body = userPasswordDetail;
    return this.http.put<ApiResponse<string>>(this.apiUrl+"ChangePassword",body);
  }
}
