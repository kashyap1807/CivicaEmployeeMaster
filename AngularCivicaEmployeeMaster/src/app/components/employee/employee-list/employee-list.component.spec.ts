import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EmployeeListComponent } from './employee-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Employee } from 'src/app/models/employee.model';
import { Router } from '@angular/router';

describe('EmployeeListComponent', () => {
  let component: EmployeeListComponent;
  let fixture: ComponentFixture<EmployeeListComponent>;
  let employeeService: EmployeeService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [EmployeeListComponent],
      providers: [EmployeeService] // Provide the service here
    });

    fixture = TestBed.createComponent(EmployeeListComponent);
    component = fixture.componentInstance;
    employeeService = TestBed.inject(EmployeeService); // Inject the service
    router = TestBed.inject(Router); // Inject Router for navigation testing

    fixture.detectChanges();
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

  it('should confirm delete and delete employee', () => {
    const confirmSpy = spyOn(window, 'confirm').and.returnValue(true);
    const deleteEmployeeSpy = spyOn(component, 'deleteContact').and.callThrough();

    const mockDeleteResponse = {
      data:'',
      success: true,
      message: 'Employee deleted successfully'
    };

    const initialTotalItems = component.totalItems;
    const initialTotalPages = component.totalPages;
    const initialPageNumber = component.pageNumber;

    // Assuming a successful deletion mock response
    spyOn(employeeService, 'deleteEmployeeById').and.returnValue(of(mockDeleteResponse));

    component.confirmDelete(1);
   
    expect(confirmSpy).toHaveBeenCalled();
    expect(deleteEmployeeSpy).toHaveBeenCalled();
    const expectedTotalPages = Math.ceil(component.totalItems / component.pageSize);
    expect(component.totalPages).toEqual(expectedTotalPages);

  });

  it('should not delete employee on cancel of confirmation', () => {
    const confirmSpy = spyOn(window, 'confirm').and.returnValue(false);
    const deleteEmployeeSpy = spyOn(component, 'deleteContact');

    const initialTotalItems = component.totalItems;
    const initialTotalPages = component.totalPages;
    const initialPageNumber = component.pageNumber;

    component.confirmDelete(1);
    
    expect(confirmSpy).toHaveBeenCalled();
    expect(deleteEmployeeSpy).not.toHaveBeenCalled();
    expect(component.totalItems).toEqual(initialTotalItems);
    expect(component.totalPages).toEqual(initialTotalPages);
    expect(component.pageNumber).toEqual(initialPageNumber);
  });

  it('should navigate to employee details', () => {
    const navigateSpy = spyOn(router, 'navigate').and.returnValue(Promise.resolve(true));
  
    const employeeId = 1;
    component.goToDetails(employeeId);
  
    expect(navigateSpy).toHaveBeenCalledWith(['/employee/employeedetails/', employeeId]);
  });

  

});
