import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignupComponent } from './signup.component';
import { HttpClientModule } from '@angular/common/http';
import { FormControl, FormsModule, NgForm } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from 'src/app/services/helpers/auth.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let routerSpy: Router;
 
 
 
  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService',['signup'])
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,RouterTestingModule, FormsModule],
      declarations: [SignupComponent],
      providers: [{provide: AuthService, useValue: authService}]
    });
    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
    routerSpy = TestBed.inject(Router);
 
  });
 
  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should navigate to /signin on successful user addition', () => {
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    authService.signup.and.returnValue(of(mockResponse));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        loginId: 'test',
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          password: "Password@1234",
          confirmPassword: "Password@1234",
          fileName: '',
          imageByte: "", 
        },
      controls: {
               firstName: {value:'Test name'},
        lastName: {value:'last name'},       
          loginId:{value: "test"},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          password: {value: "Password@1234"},
          confirmPassword: {value: "Password@1234"},
          fileName: {value:''},
          imageByte: {value:""},
          
      }
    };
 
     component.onSubmit(form);
 
    expect(authService.signup).toHaveBeenCalledWith(component.user); // Verify addContact was called with component.contact
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/signin']);
    expect(component.loading).toBe(false);
  });
  it('should navigate to /signin on successful user addition without birthDate', () => {
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    authService.signup.and.returnValue(of(mockResponse));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        loginId: 'test',
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          password: "Password@1234",
          confirmPassword: "Password@1234",
          fileName: '',
          imageByte: "", 
          dateOfBirth: ''
        },
      controls: {
               firstName: {value:'Test name'},
        lastName: {value:'last name'},       
          loginId:{value: "test"},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          password: {value: "Password@1234"},
          confirmPassword: {value: "Password@1234"},
          fileName: {value:''},
          imageByte: {value:""},
          dateOfBirth: {value: ''}
          
      }
    };
 
     component.onSubmit(form);
 
    expect(authService.signup).toHaveBeenCalledWith(component.user); // Verify addContact was called with component.contact
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/signin']);
    expect(component.loading).toBe(false);
  });
 
  it('should alert error message on unsuccessful user addition', () => {
    spyOn(window,'alert');
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: '' };
    authService.signup.and.returnValue(of(mockResponse));
    spyOn(routerSpy,'navigate');

    
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        loginId: 'test',
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          password: "Password@1234",
          confirmPassword: "Password@1234",
          fileName: '',
          imageByte: "", 
        },
      controls: {
               firstName: {value:'Test name'},
        lastName: {value:'last name'},       
          loginId:{value: "test"},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          password: {value: "Password@1234"},
          confirmPassword: {value: "Password@1234"},
          fileName: {value:''},
          imageByte: {value:""},
          
      }
    };
 
     component.onSubmit(form);
     expect(routerSpy.navigate).toHaveBeenCalledWith(['/signup']);
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
    expect(component.loading).toBe(false);
  });
 
  it('should alert error message on HTTP error', () => {
    spyOn(console, 'log');
    spyOn(window, 'alert');
    spyOn(routerSpy,'navigate');
    const mockError = { error: { message: 'HTTP error' } };
    authService.signup.and.returnValue(throwError(mockError));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        loginId: 'test',
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          password: "Password@1234",
          confirmPassword: "Password@1234",
          fileName: '',
          imageByte: "", 
        },
      controls: {
               firstName: {value:'Test name'},
        lastName: {value:'last name'},       
          loginId:{value: "test"},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          password: {value: "Password@1234"},
          confirmPassword: {value: "Password@1234"},
          fileName: {value:''},
          imageByte: {value:""},
          
      }
    };
 
    component.onSubmit(form);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/signup']);

    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
    expect(component.loading).toBe(false);
  });
 
  it('should not call authService.signup on invalid form submission', () => {
    const form = <NgForm>{ valid: false };
 
    component.onSubmit(form);
 
    expect(authService.signup).not.toHaveBeenCalled();
    expect(component.loading).toBe(false);
  });


  it('should set passwordMismatch error if passwords do not match', () => {
    const passwordControl = new FormControl('password123');
    const confirmPasswordControl = new FormControl('differentPassword');

    const form= <NgForm><unknown> {
      controls: {
        password: passwordControl,
        confirmPassword: confirmPasswordControl
      }
    };

    component.checkPasswords(form as NgForm);

    // Assert that confirmPassword control has passwordMismatch error
    expect(confirmPasswordControl.errors).toEqual({ passwordMismatch: true });
  });

  it('should not set passwordMismatch error when passwords match', () => {
    const passwordControl = new FormControl('password123');
    const confirmPasswordControl = new FormControl('password123');

    const form= <NgForm><unknown> {
      controls: {
        password: passwordControl,
        confirmPassword: confirmPasswordControl
      }
    };

    component.checkPasswords(form as NgForm);

    // Assert that confirmPassword control does not have any errors
    expect(confirmPasswordControl.errors).toBeFalsy();
  });

  it('should not set passwordMismatch error when confirmPassword is null', () => {
    const passwordControl = new FormControl('password123');
    const confirmPasswordControl = new FormControl(null); // Simulate null value
    const form= <NgForm><unknown> {
      controls: {
        password: passwordControl,
        confirmPassword: confirmPasswordControl
      }
    };
    component.checkPasswords(form as NgForm);

    // Assert that confirmPassword control does not have any errors
    expect(confirmPasswordControl.errors).toEqual({ passwordMismatch: true });
  });

  it('should not set passwordMismatch error when password is null', () => {
    const passwordControl = new FormControl(null); // Simulate null value
    const confirmPasswordControl = new FormControl('password123');

    const form= <NgForm><unknown> {
      controls: {
        password: passwordControl,
        confirmPassword: confirmPasswordControl
      }
    };

    component.checkPasswords(form as NgForm);

    // Assert that confirmPassword control does not have any errors
    expect(confirmPasswordControl.errors).toEqual({ passwordMismatch: true });
  });

  it('should return a date string representing 18 years ago from today', () => {
    const today = new Date();
    const expectedYear = today.getFullYear() - 18;
    const expectedMonth = String(today.getMonth() + 1).padStart(2, '0');
    const expectedDay = String(today.getDate()).padStart(2, '0');

    const expectedDateString = `${expectedYear}-${expectedMonth}-${expectedDay}`;

    const actualDateString = component.maxDate();

    expect(actualDateString).toBe(expectedDateString);
  });


});
