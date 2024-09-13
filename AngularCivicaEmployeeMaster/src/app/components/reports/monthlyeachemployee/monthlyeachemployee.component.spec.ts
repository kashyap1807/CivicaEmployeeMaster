import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyeachemployeeComponent } from './monthlyeachemployee.component';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { concatMap, of, throwError } from 'rxjs';
import { Employee } from 'src/app/models/employee.model';

describe('MonthlyeachemployeeComponent', () => {
  let component: MonthlyeachemployeeComponent;
  let fixture: ComponentFixture<MonthlyeachemployeeComponent>;
  let employeeService: EmployeeService;
  let employeeServiceSpy: jasmine.SpyObj<EmployeeService>;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [MonthlyeachemployeeComponent],
      providers: [EmployeeService] 
    });
    fixture = TestBed.createComponent(MonthlyeachemployeeComponent);
    component = fixture.componentInstance;
    employeeServiceSpy = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
    employeeService = TestBed.inject(EmployeeService);
    fixture.detectChanges();
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should initialize component correctly', () => {
    // Test ngOnInit
    spyOn(component, 'getAllEmployeesCount');
    component.ngOnInit();
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
  });
  it('should fetch employees count', () => {
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: 10,
      message: 'Employees count fetched successfully'
    };

    spyOn(employeeService, 'getAllEmployeesCount').and.returnValue(of(mockApiResponse));

    component.getAllEmployeesCount();
   
    expect(component.totalItems).toEqual(mockApiResponse.data);
    expect(component.totalPages).toEqual(Math.ceil(mockApiResponse.data / component.pageSize));
  });

  it('should handle error while fetching employees count', () => {
    const errorMessage = 'Error fetching employees count';

    spyOn(employeeService, 'getAllEmployeesCount').and.returnValue(throwError(errorMessage));

    component.getAllEmployeesCount();
    
    expect(component.totalItems).toEqual(0); // Ensure default value
    expect(component.totalPages).toEqual(0); // Ensure default value
    // You can add more error handling expectations if needed
  });

  it('should fetch employees with pagination', () => {
    const mockEmployees: Employee[] = [
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
    ];

    const mockApiResponse: ApiResponse<Employee[]> = {
      success: true,
      data: mockEmployees,
      message: 'Employees fetched successfully'
    };

    spyOn(employeeService, 'getAllEmployeeWithPagination').and.returnValue(of(mockApiResponse));

    component.getAllEmployeeWithPagination();
    fixture.detectChanges(); // Detect changes after async operation

    expect(component.employee);
  });

  it('should handle error while fetching employees with pagination', () => {
    const errorMessage = 'Error fetching employees';

    spyOn(employeeService, 'getAllEmployeeWithPagination').and.returnValue(throwError(errorMessage));

    component.getAllEmployeeWithPagination();
   
    expect(component.employee).toBeUndefined(); // Ensure data is not set on error
    // You can add more error handling expectations if needed
  });

  it('should change page size', () => {
    const pageSize = 10;
    component.totalItems = 25; // Assuming some total items
    component.changePageSize(pageSize);
    expect(component.pageSize).toEqual(pageSize);
    expect(component.pageNumber).toEqual(1);
    expect(component.totalPages).toEqual(Math.ceil(component.totalItems / pageSize));
  });

  it('should sort in ascending order', () => {
    const sort = 'asc';
    component.sortAsc();
    expect(component.sort).toEqual(sort);
    expect(component.pageNumber).toEqual(1);
    // You can add expectations related to fetching employees with pagination
  });

  it('should sort in descending order', () => {
    const sort = 'desc';
    component.sortDesc();
    expect(component.sort).toEqual(sort);
    expect(component.pageNumber).toEqual(1);
    // You can add expectations related to fetching employees with pagination
  });

  it('should perform search with search query length >= minSearchLength', () => {
    const searchQuery = 'John';
    component.search = searchQuery;
    spyOn(component, 'getAllEmployeeWithPagination');
    spyOn(component, 'getAllEmployeesCount');
    component.onSearch();
    expect(component.pageNumber).toEqual(1);
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalled();
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
  });
  it('should handle error and log message when fetching employee count fails', () => {
    const mockError = { mockError: { error: { message : "Network Error" }}};
    // Mock the service to return an error observable
    spyOn(employeeService,'getAllEmployeesCount').and.returnValue(throwError({mockError}));
    spyOn(console, 'error'); // Spy on console.error to capture logs

    // Simulate component behavior
    component.search = 'exampleSearchQuery'; // Assuming searchQuery length >= 3
    component.loading = true; // Simulate loading state

    // Call the method to test
    component.getAllEmployeesCount();

    // Assert error handling behavior
    expect(console.error).toHaveBeenCalled(); // Verify console.error was called with expected message
    expect(component.loading).toBeFalse(); // Assert loading state is false after error handing
    // Note: Since it's an error scenario, totalItems and totalPages should remain unchanged in this test case
  });

  it('should perform search with search query length < minSearchLength', () => {
    const searchQuery = 'Jo'; // Less than minSearchLength
    component.search = searchQuery;
    spyOn(component, 'getAllEmployeeWithPagination');
    spyOn(component, 'getAllEmployeesCount');
    component.onSearch();
    expect(component.pageNumber).toEqual(1);
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
  });

  it('should clear search', () => {
    spyOn(component, 'getAllEmployeeWithPagination');
    spyOn(component, 'getAllEmployeesCount');
    component.clearSearch();
    expect(component.search).toEqual('');
    expect(component.pageNumber).toEqual(1);
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalled();
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
  });
  it('should set page number and fetch employees synchronously', () => {
    const pageNumber = 2;
    spyOn(component,'getAllEmployeeWithPagination')
    component.pageNumber = 2; 
  
    component.changePage(pageNumber);

    expect(component.pageNumber).toBe(pageNumber);
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalled();
  });
  it('should fail success', () =>{
    //Arrange
    const mockApiResponse: ApiResponse<number> = {
      success: false,
      message: 'Employees not fetched',
      data:0
    };

    spyOn(console,'error');
    spyOn(employeeService,'getAllEmployeesCount').and.returnValue(of(mockApiResponse));
    //Act
    component.search = 'abcd';
    component.getAllEmployeesCount();
    
    //Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count',mockApiResponse.message);
  });

  it('should return success as true for getAllEmployeeWithPagination',()=>{
    //Arrange
    const mockEmployees: Employee[] = [
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
    ];

    const mockApiResponse: ApiResponse<Employee[]> = {
      success: true,
      message: '',
      data:mockEmployees
    }

    //Act
    component.pageNumber=2;
    component.totalPages=3;
    component.search='user';
    component.pageSize =2;
    component.sort = 'asc';
    spyOn(console,'log');
    spyOn(employeeService,'getAllEmployeeWithPagination').and.returnValue(of(mockApiResponse));
    component.getAllEmployeeWithPagination();
    
    

    //Assert
    expect(component.employee).toEqual(mockApiResponse.data);
    expect(console.log).toHaveBeenCalledWith(mockApiResponse.data)
    
  })

  it('should return success as false for getAllEmployeeWithPagination',()=>{
    //Arrange
    
    const mockApiResponse: ApiResponse<Employee[]> = {
      success: false,
      message: 'Employees not fetched',
      data:[]
    }

    //Act
    component.pageNumber=2;
    component.totalPages=3;
    component.search='user';
    component.pageSize =2;
    component.sort = 'asc';
    spyOn(console,'error');
    spyOn(employeeService,'getAllEmployeeWithPagination').and.returnValue(of(mockApiResponse));
    component.getAllEmployeeWithPagination();
    
    

    //Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch employee',mockApiResponse.message)
    
  })

  it('should handle error response for getAllEmployeeWithPagination',()=>{
    //Arrange
    
    const errorMessahe:string = 'Employees not fetched';

    //Act
    component.pageNumber=2;
    component.totalPages=3;
    component.search='user';
    component.pageSize =2;
    component.sort = 'asc';
    spyOn(console,'error');
    spyOn(employeeService,'getAllEmployeeWithPagination').and.returnValue(throwError(errorMessahe));
    component.getAllEmployeeWithPagination();
    
    

    //Assert
    expect(console.error).toHaveBeenCalledWith('Error fetching employee.',errorMessahe)
    
  })
});
