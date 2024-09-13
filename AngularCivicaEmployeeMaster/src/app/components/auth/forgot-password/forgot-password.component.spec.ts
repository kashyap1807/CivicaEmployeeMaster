import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { FormControl, FormsModule, NgForm } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';

import { ForgotPasswordComponent } from './forgot-password.component';
import { AuthService } from 'src/app/services/helpers/auth.service';

describe('ForgotPasswordComponent', () => {
  let component: ForgotPasswordComponent;
  let fixture: ComponentFixture<ForgotPasswordComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let router: Router;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['forgotPassword']);

    TestBed.configureTestingModule({
      imports: [HttpClientModule, FormsModule, RouterTestingModule],
      declarations: [ForgotPasswordComponent],
      providers: [
        { provide: AuthService, useValue: authServiceSpy }
      ]
    });

    fixture = TestBed.createComponent(ForgotPasswordComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should check password match', () => {
    const newPasswordControl = new FormControl('password');
    const confirmNewPasswordControl = new FormControl('password');
  
    const form = {
      controls: {
        newPassword: newPasswordControl,
        confirmNewPassword: confirmNewPasswordControl
      },
      value: {
        newPassword: 'password',
        confirmNewPassword: 'password'
      }
    } as unknown as NgForm;
  
    // Assuming 'component' refers to your Angular component where checkPasswords method is defined
    component.checkPasswords(form);
    expect(form.controls['confirmNewPassword'].errors).toBeNull();
  
    // Simulate password mismatch
    confirmNewPasswordControl.setValue('differentPassword');
    component.checkPasswords(form);
    expect(form.controls['confirmNewPassword'].errors).toEqual({ passwordMismatch: true });
  });
  it('should navigate to /employee upon successful password reset', () => {
    const form = {
      valid: true,
      value: {
        passwordHintId: '123' // Mock form value
      }
    } as NgForm;
    spyOn(router,'navigate');

    const mockResponse = { success: true };
    (authService.forgotPassword as jasmine.Spy).and.returnValue(of(mockResponse));

    component.validateUser(form);

    expect(authService.forgotPassword).toHaveBeenCalledOnceWith(component.user);
    expect(router.navigate).toHaveBeenCalledWith(['/employee']);
  });

  it('should display an error message and  navigate if password reset fails', () => {
    const form = {
      valid: true,
      value: {
        passwordHintId: '123' // Mock form value
      }
    } as NgForm;

    const mockErrorResponse = { success: false, message: 'Password reset failed' };
    (authService.forgotPassword as jasmine.Spy).and.returnValue(of(mockErrorResponse));

    spyOn(window, 'alert'); // Spy on window.alert to check if it is called
    spyOn(console,'error');
    spyOn(router,'navigate');

    component.validateUser(form);

    expect(authService.forgotPassword).toHaveBeenCalledOnceWith(component.user);
    expect(window.alert).toHaveBeenCalledWith('Password reset failed');
    expect(router.navigate).toHaveBeenCalled(); // Router should not have been called
  });

  it('should handle error if AuthService throws an error', () => {
    const form = {
      valid: true,
      value: {
        passwordHintId: '123' // Mock form value
      }
    } as NgForm;
    spyOn(router,'navigate');

    const errorMessage = 'Internal Server Error';
    (authService.forgotPassword as jasmine.Spy).and.returnValue(throwError({ error: { message: errorMessage } }));

    spyOn(console, 'error'); // Spy on console.error to check if it is called
    spyOn(window, 'alert'); // Spy on window.alert to check if it is called

    component.validateUser(form);

    expect(console.error).toHaveBeenCalledWith('Failed to validate user');
    expect(window.alert).toHaveBeenCalledWith(errorMessage);
    
  });

  it('should not call AuthService and should navigate to /forgotpassword if form is invalid', () => {
    const form = { valid: false } as NgForm;
    spyOn(router,'navigate');

    
    component.validateUser(form);

    expect(authService.forgotPassword).not.toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/forgotpassword']);
  });

});
