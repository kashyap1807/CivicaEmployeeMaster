import { ComponentFixture, TestBed, flushMicrotasks } from '@angular/core/testing';

import { MonthlysalaryreportComponent } from './monthlysalaryreport.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { NgModule } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { of, throwError } from 'rxjs';
import { Employee } from 'src/app/models/employee.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';

describe('MonthlysalaryreportComponent', () => {
  let component: MonthlysalaryreportComponent;
  let fixture: ComponentFixture<MonthlysalaryreportComponent>;
  let employeeServicE : EmployeeService;
  let mockEmployeeService: jasmine.SpyObj<EmployeeService>;
  const mockEmployee: Employee = {
    id: 1,
    employeeEmail: 'test@example.com',
    departmentId: 2,
    firstName: 'John',
    lastName: 'Doe',
    gender: 'Male',
    basicSalary: 15000,
    hra: 200,
    allowance: 300,
    grossSalary: 15500,
    pfDeduction: 500,
    profTax: 200,
    grossDeductions: 700,
    totalSalary: 14800,
    dateOfJoining: '2023-01-01',
    employeeDepartment: {
      departmentId: 2,
      departmentName: 'IT',
    }
  };

  beforeEach(() => {
    const employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['getTotalSalaryByMonth']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [MonthlysalaryreportComponent],
      providers: [
        { provide: EmployeeService, useValue: employeeServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ year: '2023', month: 'January' }) // Simulate ActivatedRoute params
          }
        }
      ]
    });
    fixture = TestBed.createComponent(MonthlysalaryreportComponent);
    component = fixture.componentInstance;
    mockEmployeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
    
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch salary data when both year and month are present in queryParams', () => {
    const mockResponse = {
      success: true,
      data: [{
        basicSalary: 5000,
        hra: 2000,
        allowance: 1000,
        grossSalary: 8000,
        pfDeduction: 500,
        profTax: 200,
        grossDeductions: 700,
        totalSalary: 7300,
        head: 1,
        year: 2023,
        month: 1,
        dateOfJoining: '2021-01-01'
      }],
      message:''
    };
  
    // Spy on the fetchSalaryData method
    const fetchSalaryDataSpy = spyOn(component, 'fetchSalaryData').and.callThrough();
    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(of(mockResponse));
    
    // Mock the service response
  
    component.selectedYear = 2023;
    component.selectedMonth = 'January';
    component.ngOnInit();
  
    // Verify that fetchSalaryData was called
    expect(fetchSalaryDataSpy).toHaveBeenCalled();
    
    // Add more assertions to validate the result, e.g., check component.salaryHeadTotal
  });
  
  
  it('should fetch salary data when both year and month are selected', () => {
    //arrange
    component.selectedYear = 2023;
    component.selectedMonth = 'January'; // or the corresponding index

    const mockResponse = {
      success: true,
      data: [{
        basicSalary: 5000,
        hra: 2000,
        allowance: 1000,
        grossSalary: 8000,
        pfDeduction: 500,
        profTax: 200,
        grossDeductions: 700,
        totalSalary: 7300,
        head: 1,
        year: 2023,
        month: 1,
        dateOfJoining: '2021-01-01'
      }],
      message:''
    };
    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(of(mockResponse));
    
    //act
    component.fetchSalaryData();
    //assert
    expect(component.salaryHeadTotal.length).toBe(1);
    expect(component.salaryHeadTotal[0].year).toBe(2023);
    expect(component.salaryHeadTotal[0].month).toBe(1);
    // Add more expectations as needed
  });
  it('should handle error when fetching salary data fails', () => {
    component.selectedYear = 2023;
    component.selectedMonth = 'January'; // or the corresponding index

    const errorMessage = 'Internal Server Error';

    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(throwError({ status: 500, message: errorMessage }));

    component.fetchSalaryData();

    expect(component.salaryHeadTotal).toEqual([]);
    // Verify console.error message expectations
  });
  it('should log error message when salary data fetch fails', () => {
    const errorMessage = 'Failed to fetch salary data'; // This should match the error message you expect
    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(throwError({ status: 500, message: errorMessage }));

    // Mock response with failure
    const mockErrorResponse = {
      status: 500,
      message: errorMessage
    };
  
    // Spy on console.error to capture logs
    spyOn(console, 'error');

  
    // Mock the service to return an error observable
    
  
    component.selectedYear = 2023;
    component.selectedMonth = 'January';
  
    component.fetchSalaryData();
  
    // Verify that console.error was called with the expected message
    expect(console.error).toHaveBeenCalledWith('Error fetching salary data.', mockErrorResponse);
  });
  it('should handle error when salary data fetch fails', () => {
    const errorMessage = 'Error fetching salary data.';

    // Mock the service to return an error observable
    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(throwError({ data: null , message: errorMessage }));
    const mockErrorResponse = {
      data: null,
      message: 'Error fetching salary data.'
    };
    spyOn(console, 'error'); // Spy on console.error to capture logs

    component.selectedYear = 2023;
    component.selectedMonth = 'January';

    component.fetchSalaryData();

    // Since error handling is within the subscription, no need for asynchronous handling
    // Assert that console.error was called with the expected message
    expect(console.error).toHaveBeenCalledWith('Error fetching salary data.', mockErrorResponse);

    // Assert that salaryHeadTotal is empty after error
    expect(component.salaryHeadTotal).toEqual([]);
  });
  it('should fail success', () =>{
    //Arrange
    const mockApiResponse: ApiResponse<SalaryHeadTotal[]> = {
      success: false,
      message: 'Employees not fetched',
      data:[]
    };

    spyOn(console,'error');
    component.selectedYear = 2023;
    component.selectedMonth = 'January';
    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(of(mockApiResponse));
    // spyOn(employeeServicE,'getTotalSalaryByMonth').and.returnValue(of(mockApiResponse));
    //Act
    component.fetchSalaryData();
    
    //Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch salary data',mockApiResponse.message);
  })
  
  it('should handle when onYearChange is called', () => {
    //arrange
    component.selectedYear = 2023;
    component.selectedMonth = 'January'; // or the corresponding index

    const mockResponse = {
      success: true,
      data: [{
        basicSalary: 5000,
        hra: 2000,
        allowance: 1000,
        grossSalary: 8000,
        pfDeduction: 500,
        profTax: 200,
        grossDeductions: 700,
        totalSalary: 7300,
        head: 1,
        year: 2023,
        month: 1,
        dateOfJoining: '2021-01-01'
      }],
      message:''
    };
    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(of(mockResponse));
    
    //act
    component.onYearChange();
    //assert
    expect(component.salaryHeadTotal).toEqual([]);
    
  });

  it('should handle when onMonthChange is called', () => {
    //arrange
    component.selectedYear = 2023;
    component.selectedMonth = 'January'; // or the corresponding index

    const mockResponse = {
      success: true,
      data: [{
        basicSalary: 5000,
        hra: 2000,
        allowance: 1000,
        grossSalary: 8000,
        pfDeduction: 500,
        profTax: 200,
        grossDeductions: 700,
        totalSalary: 7300,
        head: 1,
        year: 2023,
        month: 1,
        dateOfJoining: '2021-01-01'
      }],
      message:''
    };
    mockEmployeeService.getTotalSalaryByMonth.and.returnValue(of(mockResponse));
    
    //act
    component.onMonthChange();
    //assert
    expect(component.salaryHeadTotal).toEqual(mockResponse.data);
    
  });
});
