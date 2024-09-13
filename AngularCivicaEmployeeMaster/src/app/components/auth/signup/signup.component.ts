import { Component } from '@angular/core';
import { AbstractControl, NgForm, ValidationErrors } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/helpers/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  
  user:User = {
    salutation:"",
    name: "",
    age: null,
    dateOfBirth:"",
    loginId:"",
    gender:"",
    email: "",
    phone: 0, 
    password: "",
    confirmPassword: "",
    passwordHintId:0,
    passwordHintAnswer:""
  };

  userSalutation = ['Mr.','Ms.', 'Miss.','Mrs'];
  

  loading=false;

  constructor(private authService:AuthService, private router:Router) {}

  checkPasswords(form: NgForm):void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(signUpForm:NgForm):void{
    
    if(signUpForm.valid){
      this.loading = true;
      this.user.passwordHintId = Number(signUpForm.value.passwordHintId);
      if(signUpForm.value.dateOfBirth==""){
        this.user.dateOfBirth =null;
      }
      //this.user.dateOfBirth ="2004-06-25T13:39:02.549Z";
      console.log(this.user);
      this.authService.signup(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            alert("Registered successfully");
            this.router.navigate(['/signin']);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.error('Failed to register user');
          alert(err.error.message);
          console.log(err);
          console.log(err.error);
          this.loading=false;
        },
        complete:()=>{
          console.log('completed');
          this.loading=false;
        }
      })
      this.router.navigate(['/signup']);
    }
    console.log(this.user);
  }
  // checkFutureDate(event: any) {
  //   const selectedDate = new Date(event.target.value);
  //   const currentDate = new Date();

  //   if (selectedDate > currentDate) {

  //       this.user.dateOfBirth = '';
  //   }
  // }

  maxDate(): string {
    // Get current date in YYYY-MM-DD format
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear()-18;

    return `${yyyy}-${mm}-${dd}`;
  }
  
}
