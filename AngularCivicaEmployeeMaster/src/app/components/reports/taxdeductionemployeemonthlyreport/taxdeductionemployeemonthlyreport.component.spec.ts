import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TaxdeductionemployeemonthlyreportComponent } from './taxdeductionemployeemonthlyreport.component';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Employee } from 'src/app/models/employee.model';
import { of, throwError } from 'rxjs';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { Router } from '@angular/router';

describe('TaxdeductionemployeemonthlyreportComponent', () => {
  let component: TaxdeductionemployeemonthlyreportComponent;
  let fixture: ComponentFixture<TaxdeductionemployeemonthlyreportComponent>;
  let employeeService: jasmine.SpyObj<EmployeeService>;
  let router: Router;
  beforeEach(waitForAsync(() => {
    const employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['getAllEmployeesCount', 'getAllEmployeeWithPagination', 'deleteEmployeeById']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([]) , FormsModule],
      declarations: [TaxdeductionemployeemonthlyreportComponent],
      providers: [{ provide: EmployeeService, useValue: employeeServiceSpy }]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaxdeductionemployeemonthlyreportComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router); // Inject Router for navigating
    employeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
 
  it('should log error and set loading to false on API error', () => {
    const errorMessage = 'HTTP error occurred';
    employeeService.getAllEmployeesCount.and.returnValue(throwError(errorMessage));

    spyOn(console, 'error');

    component.getAllEmployeesCount();

    expect(console.error).toHaveBeenCalledWith('Error fetching contacts count.', errorMessage); // Check that console.error was called with the correct error message
    expect(component.loading).toBe(false); // Check that loading flag was set to false
  });
  it('should fetch employee count and pagination data on initialization', () => {
    const responseCount: ApiResponse<number> = { success: true, data: 10, message: '' };
    const responseEmployees: ApiResponse<Employee[]> = { success: true, data: [{
      id: 0,
      employeeEmail: '',
      departmentId: 0,
      firstName: '',
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
      employeeDepartment: {departmentId:1,departmentName:'abc'}
    }], message: '' };

    employeeService.getAllEmployeesCount.and.returnValue(of(responseCount));
    employeeService.getAllEmployeeWithPagination.and.returnValue(of(responseEmployees));

    component.ngOnInit();

    expect(component.totalItems).toBe(10);
    expect(component.totalPages).toBe(Math.ceil(10 / component.pageSize));
    expect(component.employee).toEqual(responseEmployees.data);
    expect(employeeService.getAllEmployeesCount).toHaveBeenCalled();
    expect(employeeService.getAllEmployeeWithPagination).toHaveBeenCalled();
  });

  it('should change page size and update pagination', () => {
    const responseCount: ApiResponse<number> = { success: true, data: 20, message: '' };
    const responseEmployees: ApiResponse<Employee[]> = { success: true, data: [{
      id: 0,
      employeeEmail: '',
      departmentId: 0,
      firstName: '',
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
      employeeDepartment: {departmentId:1,departmentName:'abc'}
    }], message: '' };

    // Mock initial data load
    employeeService.getAllEmployeesCount.and.returnValue(of(responseCount));
    employeeService.getAllEmployeeWithPagination.and.returnValue(of(responseEmployees));

    // Simulate component initialization
    component.ngOnInit();

    // Mock subsequent data load after changing page size
    const responseEmployeesNewPageSize: ApiResponse<Employee[]> = { success: true, data: [{
      id: 0,
      employeeEmail: '',
      departmentId: 0,
      firstName: '',
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
      employeeDepartment: {departmentId:1,departmentName:'abc'}
    }], message: '' };
    employeeService.getAllEmployeeWithPagination.and.returnValue(of(responseEmployeesNewPageSize));

    component.totalItems = 20; // Assuming totalItems is already set
    component.pageSize = 5;
    component.totalPages = Math.ceil(20 / component.pageSize);

    component.changePageSize(10);

    expect(component.pageSize).toBe(10);
    expect(component.pageNumber).toBe(1); // Should reset to first page
    expect(component.totalPages).toBe(Math.ceil(20 / 10));
    expect(employeeService.getAllEmployeeWithPagination).toHaveBeenCalledWith(1, 10, 'asc', ''); // Verify the call with updated parameters
  });

  it('should sort employees in ascending order', () => {
    component.sortAsc();

    expect(component.sort).toBe('asc');
    expect(component.pageNumber).toBe(1);
    // expect(employeeService.getAllEmployeeWithPagination).toHaveBeenCalled();
  });
  it('should sort employees in descending order', () => {
    component.sortDesc();

    expect(component.sort).toBe('desc');
    expect(component.pageNumber).toBe(1);
    // expect(employeeService.getAllEmployeeWithPagination).toHaveBeenCalled();
  });
  it('should call getAllEmployeeWithPagination and getAllEmployeesCount when search length meets minSearchLength', () => {
    component.search = 'search query'; // Set search length >= minSearchLength
    component.minSearchLength = 5; // Set minimum search length

    spyOn(component, 'getAllEmployeeWithPagination');
    spyOn(component, 'getAllEmployeesCount');

    component.onSearch();

    expect(component.pageNumber).toBe(1);
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalled();
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
  });
  it('should call getAllEmployeeWithPagination and getAllEmployeesCount when search length is 0', () => {
    component.search = ''; // Set search length to 0
    component.minSearchLength = 5; // Set minimum search length

    spyOn(component, 'getAllEmployeeWithPagination');
    spyOn(component, 'getAllEmployeesCount');

    component.onSearch();

    expect(component.pageNumber).toBe(1);
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalled();
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
  });
  it('should call getAllEmployeesCount and then getAllEmployeeWithPagination when search length is less than minSearchLength', () => {
    component.search = 'abc'; // Set search length < minSearchLength
    component.minSearchLength = 5; // Set minimum search length

    spyOn(component, 'getAllEmployeeWithPagination');
    spyOn(component, 'getAllEmployeesCount');

    component.onSearch();

    expect(component.pageNumber).toBe(1);
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalled();
  });
  it('should reset search, pageNumber, and call getAllEmployeeWithPagination and getAllEmployeesCount', () => {
    component.search = 'search query'; // Set an initial search value
    component.pageNumber = 2; // Set an initial page number

    spyOn(component, 'getAllEmployeeWithPagination');
    spyOn(component, 'getAllEmployeesCount');

    component.clearSearch();

    expect(component.search).toBe(''); // Check that search is reset
    expect(component.pageNumber).toBe(1); // Check that pageNumber is reset to 1
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalled();
    expect(component.getAllEmployeesCount).toHaveBeenCalled();
  });
  it('should update pageNumber and call getAllEmployeeWithPagination', () => {
    const pageNumber = 2; // Set a page number to change to

    spyOn(component, 'getAllEmployeeWithPagination');

    component.changePage(pageNumber);

    expect(component.pageNumber).toBe(pageNumber); // Check that pageNumber is updated
    expect(component.getAllEmployeeWithPagination).toHaveBeenCalledWith(); // Check that getAllEmployeeWithPagination is called with the updated pageNumber
  });
  it('should call deleteContact if user confirms deletion', () => {
    const id = 123; // Example ID for testing

    spyOn(window, 'confirm').and.returnValue(true); // Mock window.confirm to return true

    spyOn(component, 'deleteContact');

    component.confirmDelete(id);

    expect(window.confirm).toHaveBeenCalledWith('Are you sure?'); // Check that confirm dialog was shown with the correct message
    expect(component.Id).toBe(id); // Check that component's Id property is updated with the correct ID
    expect(component.deleteContact).toHaveBeenCalled(); // Check that deleteContact method was called
  });
  it('should not call deleteContact if user cancels deletion', () => {
    const id = 456; // Example ID for testing

    spyOn(window, 'confirm').and.returnValue(false); // Mock window.confirm to return false

    spyOn(component, 'deleteContact');

    component.confirmDelete(id);

    expect(window.confirm).toHaveBeenCalledWith('Are you sure?'); // Check that confirm dialog was shown with the correct message
    expect(component.Id).toBeUndefined(); // Check that component's Id property remains undefined
    expect(component.deleteContact).not.toHaveBeenCalled(); // Ensure deleteContact method was not called
  });
  it('should update totalItems, totalPages, pageNumber, and call getAllEmployeesCount on successful deletion', () => {
    component.Id = 123; // Example ID for testing
    component.totalItems = 10;
    component.pageSize = 5;
    component.pageNumber = 2;
    component.totalPages = 2;
    const mockApiResponse: ApiResponse<string> = {
      success: true,
      message: 'Delete failed',
      data:''
    };
   
    employeeService.deleteEmployeeById.and.returnValue(of(mockApiResponse));

    spyOn(component, 'getAllEmployeesCount');

    component.deleteContact();

    expect(component.totalItems).toBe(9); // Check that totalItems is updated
    expect(component.totalPages).toBe(2); // Check that totalPages remains the same
    expect(component.pageNumber).toBe(2); // Check that pageNumber remains the same
    expect(component.getAllEmployeesCount).toHaveBeenCalled(); // Check that getAllEmployeesCount was called
  });
  it('should update pageNumber to totalPages', () => {
    component.totalPages = 5; // Example totalPages for testing

    component.pageNumber = 5; // Set an initial pageNumber

    component.pageNumber = component.totalPages;

    expect(component.pageNumber).toBe(component.totalPages); // Check that pageNumber is updated to totalPages
  });
  it('should navigate to employee details page with correct ID', () => {
    const navigateSpy = spyOn(router, 'navigate'); // Spy on router.navigate method

    const id = 123; // Example ID for testing

    component.goToDetails(id);

    expect(navigateSpy).toHaveBeenCalledWith(['/employee/employeedetails/', id]); // Check that router.navigate was called with the correct route and ID
  });
  it('should alert the error message on unsuccessful deletion', () => {
    component.Id = 456; // Example ID for testing
    const mockApiResponse: ApiResponse<string> = {
      success: false,
      message: 'Delete failed',
      data:''
    };
   
    employeeService.deleteEmployeeById.and.returnValue(of(mockApiResponse));

    spyOn(window, 'alert');

    component.deleteContact();

    expect(window.alert).toHaveBeenCalledWith('Delete failed'); // Check that alert was called with the correct message
  });

  // Test case for error handling
  it('should alert the error message and log the error on HTTP error', () => {
    component.Id = 789; // Example ID for testing

    const errorMessage = 'HTTP error occurred';
    employeeService.deleteEmployeeById.and.returnValue(throwError({ error: { message: errorMessage } }));

    spyOn(window, 'alert');
    spyOn(console, 'log');

    component.deleteContact();

    expect(window.alert).toHaveBeenCalledWith(errorMessage); // Check that alert was called with the correct message
    expect(console.log).toHaveBeenCalled(); // Check that console.log was called to log the error
  });
});
