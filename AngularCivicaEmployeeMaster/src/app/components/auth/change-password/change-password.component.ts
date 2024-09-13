import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ChangePassword } from 'src/app/models/changepassword.model';
import { AuthService } from 'src/app/services/helpers/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
  username:string|null|undefined;
  user:ChangePassword = {
    oldPassword: '',
    newPassword:'',
    confirmNewPassword: '',
    loginId: ''
  };

  loading=false;

  constructor(private authService:AuthService, private router:Router, private route:ActivatedRoute) {}

  ngOnInit(): void {
    this.authService.getUserName().subscribe((username:string|null|undefined)=>{
      this.username = username;
    })
  }

  checkPasswords(form: NgForm):void {
    const password = form.controls['newPassword'];
    const confirmPassword = form.controls['confirmNewPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(updatePasswordForm:NgForm):void{
    if(updatePasswordForm.valid){
      this.user.loginId = this.username;
      console.log(this.user);
      this.authService.changePassword(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.authService.signOut();
            this.router.navigate(['/employee']);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.error('Failed to update password');
          alert(err.error.message);
        },
        complete:()=>{
          console.log('completed');
        }
      });
      this.router.navigate(['/updateuserauthpassword',this.username]);
    }
  }
}
