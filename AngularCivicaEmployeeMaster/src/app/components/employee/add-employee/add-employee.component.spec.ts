import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AddEmployeeComponent } from './add-employee.component';
import { EmployeeDepartmentService } from 'src/app/services/employee-department.service';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { EmployeeDepartment } from 'src/app/models/employeeDepartment.model';
import { Router } from '@angular/router';

describe('AddEmployeeComponent', () => {
  let component: AddEmployeeComponent;
  let fixture: ComponentFixture<AddEmployeeComponent>;
  let employeeDepartmentService: EmployeeDepartmentService;
  let employeeService: EmployeeService;
  let router:Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule],
      declarations: [AddEmployeeComponent],
      providers: [EmployeeDepartmentService, EmployeeService]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEmployeeComponent);
    component = fixture.componentInstance;
    employeeDepartmentService = TestBed.inject(EmployeeDepartmentService);
    employeeService = TestBed.inject(EmployeeService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with default values', () => {
    expect(component.employeeForm).toBeDefined();
    expect(component.employeeForm.valid).toBeFalsy();
    expect(component.loading).toBeTrue();
  });

  it('should set up form controls with validators', () => {
    const firstNameControl = component.employeeForm.get('firstName');
    expect(firstNameControl).toBeTruthy();
    firstNameControl?.setValue('John');
    expect(firstNameControl?.valid).toBeTruthy();

    // Test invalid case (less than minLength)
    firstNameControl?.setValue('J'); 
    expect(firstNameControl?.valid).toBeFalsy();
  });

  it('should load departments on init', () => {
    const departments: EmployeeDepartment[] = [{ departmentId: 1, departmentName: 'Department 1' }];
    spyOn(employeeDepartmentService, 'getAllDepartments').and.returnValue(of({ success: true, data: departments ,message:''}));

    component.ngOnInit();
    
    expect(component.employeeDepartment).toEqual(departments);
    expect(component.loading).toBeFalsy();
  });

  it('should handle form submission when valid', () => {
    const mockEmployee = {
      firstName: 'John',
      lastName: 'Doe',
      employeeEmail: 'john.doe@example.com',
      departmentId: 1,
      gender: 'Male',
      dateofjoining: '2024-06-27',
      basicSalary: 50000,
      allowance: 5000
    };

    spyOn(employeeService, 'addEmployee').and.returnValue(of({ success: true,data:'', message: 'Employee added successfully' }));
    spyOn(router, 'navigate'); // Spy on router navigate method

    // Populate form fields
    component.employeeForm.patchValue(mockEmployee);

    component.OnSubmit();
    
    expect(employeeService.addEmployee);
    expect(component.loading).toBeFalsy();
    expect(router.navigate).toHaveBeenCalledWith(['/employee']);
  });

  it('should handle form submission when invalid', () => {
    spyOn(employeeService, 'addEmployee'); // Spy on addEmployee method

    // Submit form without filling required fields
    component.OnSubmit();

    expect(employeeService.addEmployee).not.toHaveBeenCalled();
  });

  // Additional test cases can be added to cover edge cases and error handling scenarios
  it('should handle false response for loadDepartments',()=>{
    //Arrange
    const departments: EmployeeDepartment[] = [{ departmentId: 1, departmentName: 'Department 1' }];
    const mockApiResponse : ApiResponse<EmployeeDepartment[]>={
      success:false,
      data:departments,
      message:'Failed fetching department'
    }
    spyOn(console,'error');
    spyOn(employeeDepartmentService,'getAllDepartments').and.returnValue(of(mockApiResponse));

    //Act
    component.loadDepartments();

    //Assert
    expect(console.error).toHaveBeenCalledOnceWith('Failed to fetch departments ',mockApiResponse.message);

  })

  it('should handle http error for loadDepartments',()=>{
    //Arrange
    const errorMessage:string='Failed fetching department';
    
    spyOn(console,'error');
    spyOn(employeeDepartmentService,'getAllDepartments').and.returnValue(throwError(errorMessage));

    //Act
    component.loadDepartments();

    //Assert
    expect(console.error).toHaveBeenCalledOnceWith('Error fetching departments : ',errorMessage);

  })

  it('should handle form submission with false response', () => {

    //Arrange
    const mockEmployee = {
      firstName: 'John',
      lastName: 'Doe',
      employeeEmail: 'john.doe@example.com',
      departmentId: 1,
      gender: 'Male',
      dateofjoining: '2024-06-27',
      basicSalary: 50000,
      allowance: 5000
    };

    spyOn(employeeService, 'addEmployee').and.returnValue(of({ success: false,data:'', message: 'Something went wrong.' }));
    spyOn(console, 'log'); 
    spyOn(window,'alert');

    // Populate form fields
    component.employeeForm.patchValue(mockEmployee);

    //Act
    component.OnSubmit();

    //Assert
    expect(alert).toHaveBeenCalledOnceWith('Something went wrong.')
  });

  it('should handle http error for form submission', () => {

    //Arrange
    const mockEmployee = {
      firstName: 'John',
      lastName: 'Doe',
      employeeEmail: 'john.doe@example.com',
      departmentId: 1,
      gender: 'Male',
      dateofjoining: '2024-06-27',
      basicSalary: 50000,
      allowance: 5000
    };

    const errorMessage:string='Something went wrong.';
    spyOn(console, 'log'); 
    spyOn(window,'alert');
    spyOn(employeeService, 'addEmployee').and.returnValue(
      throwError({ error: { message: errorMessage } })
    );

    // Populate form fields
    component.employeeForm.patchValue(mockEmployee);

    //Act
    component.OnSubmit();

    //Assert
    expect(alert).toHaveBeenCalledOnceWith('Something went wrong.')
  });
});
