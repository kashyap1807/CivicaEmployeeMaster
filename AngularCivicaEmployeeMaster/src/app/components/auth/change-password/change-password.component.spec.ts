import { ComponentFixture, TestBed} from '@angular/core/testing';
import { ChangePasswordComponent } from './change-password.component';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { FormControl, FormsModule, NgForm } from '@angular/forms';
import { AuthService } from 'src/app/services/helpers/auth.service';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';

describe('ChangePasswordComponent', () => {
  let component: ChangePasswordComponent;
  let fixture: ComponentFixture<ChangePasswordComponent>;
  let authService: AuthService;
  let httpClient: HttpClient;
  let router:Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, RouterTestingModule, FormsModule],
      declarations: [ChangePasswordComponent],
      providers: [AuthService, HttpClient]
    });

    fixture = TestBed.createComponent(ChangePasswordComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    httpClient = TestBed.inject(HttpClient);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize username in ngOnInit', () => {
    const mockUsername = 'testuser';
    spyOn(authService, 'getUserName').and.returnValue(of(mockUsername));

    component.ngOnInit();
  
    expect(component.username).toEqual(mockUsername);
  });

  it('should set password mismatch error if passwords do not match', () => {
    const mockForm = {
      controls: {
        newPassword: new FormControl('password'),
        confirmNewPassword: new FormControl('password123') // passwords do not match
      }
    } as unknown as NgForm;

    component.checkPasswords(mockForm);

    expect(mockForm.controls['confirmNewPassword'].errors).toEqual({ passwordMismatch: true });
  });

  it('should not set password mismatch error if passwords match', () => {
    const mockForm = {
      controls: {
        newPassword: new FormControl('password'),
        confirmNewPassword: new FormControl('password') // passwords match
      }
    } as unknown as NgForm;

    component.checkPasswords(mockForm);

    expect(mockForm.controls['confirmNewPassword'].errors).toBeNull();
  });

  it('should call authService.changePassword and navigate to /employee on successful submit', () => {
    spyOn(authService, 'changePassword').and.returnValue(of({ success: true ,data:'',message:''}));
    spyOn(router, 'navigate');

    component.user = { oldPassword: 'oldpass', newPassword: 'newpass', confirmNewPassword: 'newpass', loginId: 'testuser' };
    component.onSubmit({ valid: true } as NgForm);
   
    expect(authService.changePassword).toHaveBeenCalledWith(component.user);
    expect(router.navigate).toHaveBeenCalledWith(['/employee']);
  });

  // it('should handle error on authService.changePassword call', fakeAsync(() => {
  //   const errorMessage = 'Failed to update password';
  //   spyOn(authService, 'changePassword').and.returnValue(throwError({ error: { message: errorMessage } }));
  //   spyOn(window, 'alert');

  //   component.onSubmit({ valid: true } as NgForm);
  //   tick();

  //   expect(window.alert).toHaveBeenCalledWith(errorMessage);
  // }));

  it('should not call authService.changePassword if form is invalid', () => {
    spyOn(authService, 'changePassword');
    spyOn(router, 'navigate');

    component.onSubmit({ valid: false } as NgForm);
   

    expect(authService.changePassword).not.toHaveBeenCalled();
    expect(router.navigate).not.toHaveBeenCalled();
});
  
});
