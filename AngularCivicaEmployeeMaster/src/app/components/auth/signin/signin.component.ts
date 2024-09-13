import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/helpers/auth.service';
import { localStorageKeys } from 'src/app/services/localStorageKeys';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent {
  loading:boolean = false;
  username:string = '';
  password: string = '';
 
  constructor(
    private authService:AuthService,
    private localStorageHelper:LocalstorageService,
    private router:Router,
    private cdr:ChangeDetectorRef ) {}
 
    login(){
      this.loading = true;
      this.authService.signin(this.username,this.password).subscribe({
        next:(response)=>{
          if(response.success){
            this.localStorageHelper.setItem(localStorageKeys.TockenName,response.data);
            this.localStorageHelper.setItem(localStorageKeys.userId,this.username);
            this.cdr.detectChanges();//Manually trigger change detection
            this.router.navigate(['/home']);
          }else{
            alert(response.message);
          }
          this.loading = false;
        },
        error:(err)=>{
          this.loading = false;
          alert(err.error.message);
        }
      })
    }
}