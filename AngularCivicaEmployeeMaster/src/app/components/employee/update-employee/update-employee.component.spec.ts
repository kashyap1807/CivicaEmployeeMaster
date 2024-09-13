import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, FormsModule, NgForm } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { UpdateEmployeeComponent } from './update-employee.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { EmployeeDepartmentService } from 'src/app/services/employee-department.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { EmployeeDepartment } from 'src/app/models/employeeDepartment.model';
import { UpdateEmployee } from 'src/app/models/update-employee.model';
import { Employee } from 'src/app/models/employee.model';
import { ActivatedRoute, Router } from '@angular/router';

describe('UpdateEmployeeComponent', () => {
  let component: UpdateEmployeeComponent;
  let fixture: ComponentFixture<UpdateEmployeeComponent>;
  let employeeService: EmployeeService;
  let departmentService: EmployeeDepartmentService;
  let route: ActivatedRoute;
  let routerSpy: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [UpdateEmployeeComponent],
      providers: [EmployeeService, EmployeeDepartmentService]
    });

    fixture = TestBed.createComponent(UpdateEmployeeComponent);
    component = fixture.componentInstance;
    employeeService = TestBed.inject(EmployeeService);
    departmentService = TestBed.inject(EmployeeDepartmentService);
    routerSpy = TestBed.inject(Router)

    spyOn(component, 'loadDepartments').and.callThrough();
    spyOn(component, 'loadEmployeeDetails').and.callThrough();
    spyOn(component, 'onSubmit').and.callThrough();

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load departments on init', () => {
    const departments: EmployeeDepartment[] = [
      { departmentId: 1, departmentName: 'Department 1' },
      { departmentId: 2, departmentName: 'Department 2' }
    ];

    spyOn(departmentService, 'getAllDepartments').and.returnValue(of({ success: true, data: departments, message: '' }));

    component.ngOnInit();

    expect(component.loadDepartments).toHaveBeenCalled();
    expect(component.departments).toEqual(departments);
    expect(component.loading).toBeFalse();
  });

  it('should handle error when loading departments fails', () => {
    const errorMessage = 'Failed to fetch departments';
  
    spyOn(departmentService, 'getAllDepartments').and.returnValue(of({ success: false,data:[],message: errorMessage }));
  
    component.ngOnInit();
  
    expect(component.loadDepartments).toHaveBeenCalled();
    expect(component.departments).toEqual([]);
    expect(component.loading).toBeFalse();
    
  });

  it('should log error message when loading departments fails', () => {
    const errorMessage = 'Failed to fetch departments';
    
    // Spy on console.error to check if it's called with the correct message
    spyOn(console, 'error');
  
    // Modify the getAllDepartments spy to return an error observable
    spyOn(departmentService, 'getAllDepartments').and.returnValue(
      throwError({ message: errorMessage })
    );
  
    // Trigger ngOnInit which calls loadDepartments()
    component.ngOnInit();
  
    // Expectations
    expect(component.loadDepartments).toHaveBeenCalled();
    expect(component.departments).toEqual([]); // Departments should be empty on error
    expect(component.loading).toBeFalse();
    expect(console.error).toHaveBeenCalledWith(
      'Error fetching departments : ',
      jasmine.any(Object) // Ensure console.error was called with an object (the error)
    );
  });

  it('should load employee details on success', () => {
    const employeeId = 1;
    const mockEmployee: Employee = {
      id: employeeId,
      employeeEmail: 'john.doe@example.com',
      departmentId: 1,
      firstName: 'John',
      lastName: 'Doe',
      gender: 'Male',
      basicSalary: 50000,
      hra: 10000,
      allowance: 5000,
      grossSalary: 65000,
      pfDeduction: 5000,
      profTax: 200,
      grossDeductions: 5200,
      totalSalary: 59800,
      dateOfJoining: new Date().toISOString(),
      employeeDepartment: { departmentId: 1, departmentName: 'IT' }
    };

    // Spy on employeeService.getEmployeeById to return a successful response
    spyOn(employeeService, 'getEmployeeById').and.returnValue(
      of({ success: true, data: mockEmployee, message: '' })
    );

    // Call loadEmployeeDetails with the mock employeeId
    component.loadEmployeeDetails(employeeId);

    // Expectations
    expect(employeeService.getEmployeeById).toHaveBeenCalledWith(employeeId);
    expect(component.employee).toEqual(mockEmployee);
    // Assuming formatDate function is tested separately, if not, you might need to test it as well.
    // Verify dateOfJoining is formatted correctly, based on your formatDate implementation
    expect(component.employee.dateOfJoining).toEqual(component.formatDate(new Date(mockEmployee.dateOfJoining)));
  });

  it('should handle error when loading employee details fails', () => {
    const employeeId = 1;
    const errorMessage = 'Failed to fetch employee';
    spyOn(window,'alert');

    // Spy on employeeService.getEmployeeById to return an error response
    spyOn(employeeService, 'getEmployeeById').and.returnValue(
      throwError({ error: { message: errorMessage } })
    );

    // Call loadEmployeeDetails with the mock employeeId
    component.loadEmployeeDetails(employeeId);

    // Expectations
    expect(employeeService.getEmployeeById).toHaveBeenCalledWith(employeeId);
    
    expect(window.alert).toHaveBeenCalledWith(errorMessage); // Check if alert was called with the error message
  });

  it('should navigate to /employee and show success alert on successful employee update', () => {
    // Mock form data and set valid status
    spyOn(routerSpy,'navigate');
    spyOn(window,'alert');
    
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    
    spyOn(employeeService,'UpdateEmployee').and.returnValue(of(mockResponse));

    
    component.onSubmit({ valid: true } as NgForm);

    // Expectations
    expect(employeeService.UpdateEmployee).toHaveBeenCalledWith(component.employee);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/employee']);
    expect(window.alert).toHaveBeenCalledWith('Employee Updated Successfully!');
    expect(component.loading).toBeFalse();
  });
  it('should alert error message on unsuccessful category modification', () => {
    // Arrange
    spyOn(window, 'alert'); // Spy on console.error method
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error modifying category' };
    spyOn(employeeService,'UpdateEmployee').and.returnValue(of(mockResponse));

    // Act
   
    component.onSubmit({ valid: true } as NgForm);

    // Assert
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message); // Check if console.error was called with the correct error message
  });

  // it('should set futureDate error on form control when date is in the future', () => {
  //   // const futureDate = new Date();
  //   // futureDate.setDate(futureDate.getDate() + 1); // Set date one day ahead of today

  //   const mockEmployee: Employee = {
  //     id: 1,
  //     employeeEmail: 'john.doe@example.com',
  //     departmentId: 1,
  //     firstName: 'John',
  //     lastName: 'Doe',
  //     gender: 'Male',
  //     basicSalary: 50000,
  //     hra: 10000,
  //     allowance: 5000,
  //     grossSalary: 65000,
  //     pfDeduction: 5000,
  //     profTax: 200,
  //     grossDeductions: 5200,
  //     totalSalary: 59800,
  //     dateOfJoining: '08-08-3000', // Set dateOfJoining to futureDate
  //     employeeDepartment: { departmentId: 1, departmentName: 'IT' }
  //   };

  //   spyOn(employeeService, 'getEmployeeById').and.returnValue(of({ success: true, data: mockEmployee, message: '' }));

  //   const mockNgForm = { value: { dateOfJoining: mockEmployee.dateOfJoining }, valid: true } as NgForm;
  //   mockNgForm.controls['dateOfJoining'] = new FormControl(mockEmployee.dateOfJoining);

  //   // Call onSubmit with mockNgForm
  //   component.onSubmit({ valid: true } as NgForm);

  //   // Expectations
  //   expect(component.isFutureDate(mockEmployee.dateOfJoining)).toBeTrue();
  //   expect(mockNgForm.controls['dateOfJoining'].hasError('futureDate')).toBeTrue();
  //   expect(employeeService.UpdateEmployee).not.toHaveBeenCalled(); // Ensure no update call is made
  //   expect(component.loading).toBeFalse();
  // });



  it('should handle error when loading employee details fails', () => {
    const employeeId = 1;
    const mockEmployee: Employee = {
      id: employeeId,
      employeeEmail: 'john.doe@example.com',
      departmentId: 1,
      firstName: 'John',
      lastName: 'Doe',
      gender: 'Male',
      basicSalary: 50000,
      hra: 10000,
      allowance: 5000,
      grossSalary: 65000,
      pfDeduction: 5000,
      profTax: 200,
      grossDeductions: 5200,
      totalSalary: 59800,
      dateOfJoining: new Date().toISOString(),
      employeeDepartment: { departmentId: 1, departmentName: 'IT' }
    };
    const errorMessage = 'Failed to fetch employee';

    // Spy on employeeService.getEmployeeById to return an error response
    spyOn(employeeService, 'getEmployeeById').and.returnValue(
      of({ success: false, data:mockEmployee, message: errorMessage })
    );
    spyOn(console,'log');

    // Call loadEmployeeDetails with the mock employeeId
    component.loadEmployeeDetails(employeeId);

    // Expectations
    expect(employeeService.getEmployeeById).toHaveBeenCalledWith(employeeId);
    
    expect(console.log).toHaveBeenCalledWith('Failed to fetch employee', errorMessage);
  });


  

  

  it('should handle error when loading employee details fails', () => {
    const employeeId = 1;
    const errorMessage = 'Failed to fetch employee details';

    spyOn(employeeService, 'getEmployeeById').and.returnValue(of());

    component.ngOnInit();

    expect(component.loadEmployeeDetails).toHaveBeenCalled();
    expect(component.employee).toEqual({
      id: 0,
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
      lastName: '',
      gender: '',
      basicSalary: 10000,
      allowance: 100,
      dateOfJoining: ''
    });
    
  });

  it('should set future date error when submitting with future date', () => {
    const futureDate = new Date();
    futureDate.setDate(futureDate.getDate() + 1); // Setting date to tomorrow

    spyOn(component, 'isFutureDate').and.returnValue(true);

    component.employee.dateOfJoining = component.formatDate(futureDate);

    component.onSubmit({ valid: false } as any);

    
    expect(component.onSubmit).toHaveBeenCalled();
    expect(component.loading).toBeTrue();
    expect(component.isFutureDate(component.employee.dateOfJoining)).toBeTrue();
  });


  it('should handle server error during employee update', () => {
    const errorMessage = 'Internal Server Error';
  
    spyOn(employeeService, 'UpdateEmployee').and.returnValue(throwError({ error: { message: errorMessage } }));
  
    component.onSubmit({ valid: true } as any);
  
    
    expect(component.onSubmit).toHaveBeenCalled();
    expect(component.loading).toBeFalse();
    
  });
  

  it('should handle invalid form submission', () => {
    component.onSubmit({ valid: false } as any);

   
    expect(component.onSubmit).toHaveBeenCalled();
    expect(component.loading).toBeTrue();
  });

  it('should correctly format date', () => {
    const date = new Date('2023-06-25');
    const formattedDate = component.formatDate(date);
    expect(formattedDate).toBe('2023-06-25');
  });

  it('should check if date is in future', () => {
    const futureDate = new Date();
    futureDate.setDate(futureDate.getDate() + 1); // Tomorrow's date

    const isFuture = component.isFutureDate(component.formatDate(futureDate));
    expect(isFuture).toBeTrue();
  });

  it('should check if date is not in future', () => {
    const pastDate = new Date();
    pastDate.setDate(pastDate.getDate() - 1); // Yesterday's date

    const isFuture = component.isFutureDate(component.formatDate(pastDate));
    expect(isFuture).toBeFalse();
  });

  // Add more tests as needed for edge cases, additional functionality, etc.

});
