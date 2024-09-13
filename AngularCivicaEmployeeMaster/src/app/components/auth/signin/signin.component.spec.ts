import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SigninComponent } from './signin.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from 'src/app/services/helpers/auth.service';
import { Router } from '@angular/router';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('SigninComponent', () => {
  let component: SigninComponent;
  let fixture: ComponentFixture<SigninComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let routerSpy: Router;
  let localStorageService : jasmine.SpyObj<LocalstorageService>;

  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService',['signin'])
    localStorageService = jasmine.createSpyObj('LocalstorageService',['setItem'])
    TestBed.configureTestingModule({
      imports:[HttpClientModule,FormsModule,RouterTestingModule],
      declarations: [SigninComponent],
      providers: [{provide: AuthService, useValue: authService},
        {provide: LocalstorageService, useValue: localStorageService},]
    });
    fixture = TestBed.createComponent(SigninComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
    routerSpy = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should login successfully',()=>{
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    authService.signin.and.returnValue(of(mockResponse));
    component.username = 'username';
    component.password = 'password';

    component.login();

    expect(authService.signin).toHaveBeenCalledWith('username', 'password');
    expect(localStorageService.setItem).toHaveBeenCalledWith('jwtToken', '');
    expect(localStorageService.setItem).toHaveBeenCalledWith('loginId', 'username');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
    expect(component.loading).toBe(false);


  });

  it('should handle login failure', () => {
    authService.signin.and.returnValue(of({ success: false,data: '', message: "Login failed" }));

    spyOn(window, 'alert'); // Spy on window.alert to check if it is called

    component.username = 'username';
    component.password = 'password';

    component.login();

    expect(authService.signin).toHaveBeenCalledWith('username', 'password');
    expect(window.alert).toHaveBeenCalledWith("Login failed");
    expect(component.loading).toBe(false);
  });
  
  
  it('should handle http error', () => {

    const mockError = { error: { message: 'HTTP error' } };
    authService.signin.and.returnValue(throwError(mockError));

    spyOn(window, 'alert'); // Spy on window.alert to check if it is called

    component.username = 'username';
    component.password = 'password';

    component.login();

    expect(authService.signin).toHaveBeenCalledWith('username', 'password');
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
    expect(component.loading).toBe(false);
  });
  it('should set alert message when api returns error', () => {
    // Arrange
    component.username = "test";
    component.password = "fakePassword"
    const mockError = { error: { message: 'HTTP Error' } }
    spyOn(window, 'alert')
    authService.signin.and.returnValue(throwError(mockError))

    // Act
    component.login()

    // Assert
    expect(authService.signin).toHaveBeenCalledOnceWith(component.username, component.password)
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
  });
});