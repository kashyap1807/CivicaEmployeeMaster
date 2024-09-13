import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { User } from 'src/app/models/user.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ForgotPassword } from 'src/app/models/forgotpassword.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock:HttpTestingController;
 
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock=TestBed.inject(HttpTestingController);
  });
  afterEach(()=>{
    httpMock.verify();
    
  });
  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  //Register User
  it('should register user successfully',()=>{
    //arrange
    const registerUser:User={
      loginId: "",
      salutation: "string",
    name: "string",
    age: 21,
    dateOfBirth: "",
    gender: "string",
    email: "string",
    phone: 1234567876,
    password: "string",
    confirmPassword: "string",
    passwordHintId: 2,
    passwordHintAnswer: "string",
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"User register successfully.",
      data:""
    }
    //act
    service.signup(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5165/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user register',()=>{
    //arrange
    const registerUser:User={
      loginId: "",
      salutation: "string",
    name: "string",
    age: 21,
    dateOfBirth: "",
    gender: "string",
    email: "string",
    phone: 1234567876,
    password: "string",
    confirmPassword: "string",
    passwordHintId: 2,
    passwordHintAnswer: "string",
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.signup(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5165/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while register user',()=>{
    //arrange
    const registerUser:User={
      loginId: "",
      salutation: "string",
    name: "string",
    age: 21,
    dateOfBirth: "",
    gender: "string",
    email: "string",
    phone: 1234567876,
    password: "string",
    confirmPassword: "string",
    passwordHintId: 2,
    passwordHintAnswer: "string",
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.signup(registerUser).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5165/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);
  });

  
  
  it('should handle Http error while forget password',()=>{
    //arrange
    const ValidateForgotPassword:ForgotPassword={
      loginId: "string",
    newPassword: "string",
    confirmNewPassword: "string",
    passwordHintAnswer: "string",
    passwordHintId: 2
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.forgotPassword(ValidateForgotPassword).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5165/api/Auth/ForgotPassword');
    expect(req.request.method).toBe('PUT');
    req.flush({},mockHttpError);

  });
});
