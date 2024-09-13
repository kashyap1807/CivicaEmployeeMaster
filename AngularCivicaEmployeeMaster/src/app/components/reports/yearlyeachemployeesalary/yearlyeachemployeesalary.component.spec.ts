import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YearlyeachemployeesalaryComponent } from './yearlyeachemployeesalary.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Employee } from 'src/app/models/employee.model';

describe('YearlyeachemployeesalaryComponent', () => {
  let component: YearlyeachemployeesalaryComponent;
  let fixture: ComponentFixture<YearlyeachemployeesalaryComponent>;
  let employeeService: jasmine.SpyObj<EmployeeService>; // Mocked EmployeeService

  const mockEmployees: Employee = 
    {
      id: 1,
      firstName: 'Employee 1',
      lastName: 'LastName1',
      employeeEmail: 'employee@gmail.com',
      departmentId: 1,
      gender: 'M',
      basicSalary: 1000,
      hra: 100,
      allowance: 100,
      grossSalary: 100,
      pfDeduction: 100,
      profTax: 10,
      grossDeductions: 10,
      totalSalary: 100,
      dateOfJoining: '10,01,2023',
      employeeDepartment: { departmentId: 1, departmentName: 'department1' }
    }
  ;
  beforeEach(() => {
    
    employeeService = jasmine.createSpyObj('EmployeeService', ['getYearlySalaryOfEachEmployee','getEmployeeById']);
    TestBed.configureTestingModule({
      imports:[HttpClientModule,FormsModule,RouterTestingModule],
      declarations: [YearlyeachemployeesalaryComponent],
      providers: [
        { provide: EmployeeService, useValue: employeeService },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ employeeId: 1 }) // Mock ActivatedRoute with a sample employeeId
          }
        }
      ]
    });
    fixture = TestBed.createComponent(YearlyeachemployeesalaryComponent);
    component = fixture.componentInstance;
    employeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
  
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should fetch yearly salary data on initialization', () => {
    const mockEmployeeResponse: ApiResponse<Employee> = { success: true, data: mockEmployees, message: '' };
    const mockResponse: ApiResponse<SalaryHeadTotal[]> = {
      success: true,
      data: [{
        basicSalary: 10000,
        hra: 100,
        allowance: 100,
        grossSalary: 100,
        pfDeduction: 100,
        profTax: 100,
        grossDeductions: 100,
        totalSalary: 1000,
        head: 1,
        year: 2002,
        month: 1,
        dateOfJoining: ''
      }],
      message: ''
    };
    spyOn(component,'fetchYearlySalaryData');
    spyOn(component,'loadEmployeeDetails');

   
    employeeService.getEmployeeById.and.returnValue(of(mockEmployeeResponse));
    employeeService.getYearlySalaryOfEachEmployee.and.returnValue(of(mockResponse));

    component.ngOnInit(); // Trigger ngOnInit

    // expect(component.yearlySalaryData).toEqual(mockResponse.data);
    expect(component.fetchYearlySalaryData).toHaveBeenCalled();
    expect(component.loadEmployeeDetails).toHaveBeenCalled();
    

  });
  it('should load employee details on successful response', () => {
    // Mock data
    const mockEmployee: Employee = {
      id: 1, firstName: '', departmentId: 1,
      employeeEmail: '',
      lastName: '',
      gender: '',
      basicSalary: 10000,
      hra: 100,
      allowance: 100,
      grossSalary: 100,
      pfDeduction: 100,
      profTax: 100,
      grossDeductions: 100,
      totalSalary: 1000,
      dateOfJoining: '',
      employeeDepartment: {
        departmentId: 1,
        departmentName: ''
      }
    }; // Replace with actual employee object

    // Setup the spy method to return success response
    
    // Call the method to be tested
   spyOn(component,'loadEmployeeDetails')
    // Assert expected behavior
    expect(component.employee1).toEqual(mockEmployee); // Check if employee1 is set correctly
    
  });

  // it('should log error message on failed response', () => {
  //   const mockEmployee: Employee = {
  //     id: 1, firstName: 'John Doe', departmentId: 1,
  //     employeeEmail: '',
  //     lastName: '',
  //     gender: '',
  //     basicSalary: 0,
  //     hra: 0,
  //     allowance: 0,
  //     grossSalary: 0,
  //     pfDeduction: 0,
  //     profTax: 0,
  //     grossDeductions: 0,
  //     totalSalary: 0,
  //     dateOfJoining: '',
  //     employeeDepartment: {
  //       departmentId: 0,
  //       departmentName: ''
  //     }
  //   };
   
  //   const errorMessage = 'Failed to fetch employee';

   
  //   // Setup the spy method to return failure response
  //   spyOn(console, 'log'); // Spy on console.log to capture logs
   
  //   // Call the method to be tested
  // spyOn(component,'loadEmployeeDetails')
  //   // Assert expected behavior
  //   expect(console.log).toHaveBeenCalledWith(errorMessage); // Check console log for error message
  // });

  it('should alert error message on API error', () => {
    // Setup the spy method to throw an error
    employeeService.getEmployeeById.and.returnValue(throwError({ error: { message: 'API error' } }));
    spyOn(window,'alert');

    // Call the method to be tested
    component.loadEmployeeDetails(1);
  
    // Assert expected behavior
    expect(window.alert).toHaveBeenCalledWith('API error'); // Check alert for error message
  });

  it('should log completion message after observable completes', () => {
    const mockEmployee: Employee = {
      id: 1, firstName: 'John Doe', departmentId: 1,
      employeeEmail: '',
      lastName: '',
      gender: '',
      basicSalary: 0,
      hra: 0,
      allowance: 0,
      grossSalary: 0,
      pfDeduction: 0,
      profTax: 0,
      grossDeductions: 0,
      totalSalary: 0,
      dateOfJoining: '',
      employeeDepartment: {
        departmentId: 0,
        departmentName: ''
      }
    };
spyOn(console,'log')
    const mockResponse: ApiResponse<Employee> = {success: false, data:mockEmployee,message: 'Failed to fetch employee' }
    // Setup the spy method to return success response
    employeeService.getEmployeeById.and.returnValue(of(mockResponse));

    // Call the method to be tested
    component.loadEmployeeDetails(1);
    
    // Assert expected behavior
    expect(console.log).toHaveBeenCalledWith('Completed'); // Check console log for completion message
  });

  it('should fetchYearlySalaryData with success true response',()=>{
    //Arrange
    const mockResponse: ApiResponse<SalaryHeadTotal[]> = {
      success: true,
      data: [{
        basicSalary: 10000,
        hra: 100,
        allowance: 100,
        grossSalary: 100,
        pfDeduction: 100,
        profTax: 100,
        grossDeductions: 100,
        totalSalary: 1000,
        head: 1,
        year: 2002,
        month: 1,
        dateOfJoining: ''
      }],
      message: ''
    };

    employeeService.getYearlySalaryOfEachEmployee.and.returnValue(of(mockResponse));

    //Act
    component.selectedYear=2021;
    component.employeeId =1;
    component.onYearSelected();
    // component.fetchYearlySalaryData(1,component.selectedYear);

    //Assert
    expect(component.yearlySalaryData).toEqual(mockResponse.data)
  })

  it('should fetchYearlySalaryData with success false response',()=>{
    //Arrange
    const errorMessage ='Failed to fetch data';
    const mockEmployeeResponse: ApiResponse<Employee> = { success: true, data: mockEmployees, message: '' };
    const mockResponse: ApiResponse<SalaryHeadTotal[]> = {
      success: false,
      data: [],
      message: errorMessage
    };

    spyOn(console,'error');
    employeeService.getYearlySalaryOfEachEmployee.and.returnValue(of(mockResponse));

    //Act
    component.selectedYear=2021;
    component.fetchYearlySalaryData(1,component.selectedYear);

    //Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch yearly salary data:',mockResponse.message)
  })

  it('should handle fetchYearlySalaryData for error ',()=>{
    //Arrange
    const errorMessage ='Failed to fetch data';
    const mockEmployeeResponse: ApiResponse<Employee> = { success: true, data: mockEmployees, message: '' };
    // const mockResponse: ApiResponse<SalaryHeadTotal[]> = {
    //   success: false,
    //   data: [],
    //   message: errorMessage
    // };

    spyOn(console,'error');
    employeeService.getYearlySalaryOfEachEmployee.and.returnValue(throwError(errorMessage));

    //Act
    component.selectedYear=2021;
    component.fetchYearlySalaryData(1,component.selectedYear);

    //Assert
    expect(console.error).toHaveBeenCalledWith('Error fetching yearly salary data:',errorMessage)
  })

  it('should handle success on loadEmployeeDetails', () => {
    const mockEmployee: Employee = {
      id: 1, firstName: 'John Doe', departmentId: 1,
      employeeEmail: '',
      lastName: '',
      gender: '',
      basicSalary: 0,
      hra: 0,
      allowance: 0,
      grossSalary: 0,
      pfDeduction: 0,
      profTax: 0,
      grossDeductions: 0,
      totalSalary: 0,
      dateOfJoining: '',
      employeeDepartment: {
        departmentId: 0,
        departmentName: ''
      }
    };
    spyOn(console,'log')
    const mockResponse: ApiResponse<Employee> = {success: true, data:mockEmployee,message: '' }
    // Setup the spy method to return success response
    employeeService.getEmployeeById.and.returnValue(of(mockResponse));

    // Call the method to be tested
    component.loadEmployeeDetails(1);
    
    // Assert expected behavior
    expect(component.employee1).toEqual(mockResponse.data)
  });
});
