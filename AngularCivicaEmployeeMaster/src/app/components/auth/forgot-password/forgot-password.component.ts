import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ForgotPassword } from 'src/app/models/forgotpassword.model';
import { AuthService } from 'src/app/services/helpers/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  user:ForgotPassword = {
    loginId: '',
    newPassword: '',
    confirmNewPassword: '',
    passwordHintAnswer: '',
    passwordHintId: 0
  };

  loading=false;

  constructor(private authService:AuthService, private router:Router) {}

  checkPasswords(form: NgForm):void {
    const newPassword = form.controls['newPassword'];
    const newConfirmPassword = form.controls['confirmNewPassword'];
 
    if (newPassword && newConfirmPassword && newPassword.value !== newConfirmPassword.value) {
      newConfirmPassword.setErrors({ passwordMismatch: true });
    } else {
      newConfirmPassword.setErrors(null);
    }
  }

  validateUser(forgotPasswordForm:NgForm):void{
    if(forgotPasswordForm.valid){
      this.user.passwordHintId = Number(forgotPasswordForm.value.passwordHintId);
      console.log(this.user);
      this.authService.forgotPassword(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.router.navigate(['/employee']);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.error('Failed to validate user');
          alert(err.error.message);
          console.log(err);
          console.log(err.error);
        },
        complete:()=>{
          console.log('completed');
        }
      })
      
    }
    this.router.navigate(['/forgotpassword']);
  }
}
