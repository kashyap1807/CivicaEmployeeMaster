import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EachemployeesalaryslipmonthlyComponent } from './eachemployeesalaryslipmonthly.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { Employee } from 'src/app/models/employee.model';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

describe('EachemployeesalaryslipmonthlyComponent', () => {
  let component: EachemployeesalaryslipmonthlyComponent;
  let fixture: ComponentFixture<EachemployeesalaryslipmonthlyComponent>;
  let employeeServiceSpy: jasmine.SpyObj<EmployeeService>;
  let activatedRoute: ActivatedRoute;


  beforeEach(() => {
  employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['getEmployeeById']);

    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [EachemployeesalaryslipmonthlyComponent],
      providers: [
        { provide: EmployeeService, useValue: employeeServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ employeeId: 1 })
          }
        }
      ]
    });
    fixture = TestBed.createComponent(EachemployeesalaryslipmonthlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    activatedRoute = TestBed.inject(ActivatedRoute);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should load employee details on initialization', () => {
    const mockEmployee: Employee = {
      id: Number(1),
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    };

    employeeServiceSpy.getEmployeeById.and.returnValue(of({ success: true, data: mockEmployee, message: '' }));

    fixture.detectChanges();

    expect(employeeServiceSpy.getEmployeeById).toHaveBeenCalledWith(mockEmployee.id);
    expect(component.employee).toEqual(mockEmployee);
  });
  it('should handle error when loading employee details', () => {
    employeeServiceSpy.getEmployeeById.and.returnValue(throwError({ error: { message: 'Failed to fetch employee' } }));

    fixture.detectChanges();

    expect(component.employee).toEqual(jasmine.objectContaining({
      id: 1,
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    }));
    expect(component.noRecordFound).toBe(false); // Assuming noRecordFound should be false on error
  });
  it('should load employee details successfully', () => {
    const mockEmployee: Employee = {
      id: 1,
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    };

    employeeServiceSpy.getEmployeeById.and.returnValue(of({ success: true, data: mockEmployee,message:'' }));
  fixture.detectChanges();
  component.loadEmployeeDetails(mockEmployee.id);

    expect(employeeServiceSpy.getEmployeeById).toHaveBeenCalledWith(1);
    expect(component.employee).toEqual(mockEmployee);
    expect(component.noRecordFound).toBe(false);
  });
  it('should log failure message when API response indicates failure', () => {
    employeeServiceSpy.getEmployeeById.and.returnValue(throwError({ error: {success:false, message: 'Failed to fetch employee' } }));
    spyOn(console, 'log');

    // Trigger change detection to ensure ngOnInit is called
    fixture.detectChanges();
    component.loadEmployeeDetails(1);
 });
  it('should handle error when loading employee details', () => {
    employeeServiceSpy.getEmployeeById.and.returnValue(throwError({ error: { message: 'Failed to fetch employee' } }));

    fixture.detectChanges();
    component.loadEmployeeDetails(1);


    expect(employeeServiceSpy.getEmployeeById).toHaveBeenCalledWith(1);
    expect(component.employee).toEqual(jasmine.objectContaining({
      id: 1,
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    }));
    expect(component.noRecordFound).toBe(false); // Assuming noRecordFound should be true on error
  });
 it('should set noRecordFound to false when selected date is valid', () => {
    component.employee.dateOfJoining = '2000-01-01'; // Assuming a specific join date for testing
    component.selectedYear = 2024;
    component.selectedMonth = 6; // July (0-indexed)

    component.generateMonthlySalarySlip();

    expect(component.noRecordFound).toBeFalse();
  });

  it('should set noRecordFound to true when selected date is before employee\'s join date', () => {
    component.employee.dateOfJoining = '2000-01-01'; // Assuming a specific join date for testing
    component.selectedYear = 1999;
    component.selectedMonth = 11; // December (0-indexed)

    component.generateMonthlySalarySlip();

    expect(component.noRecordFound).toBeTrue();
  });

  it('should set noRecordFound to true when selected date is after current month\'s date', () => {
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();
    const currentMonth = currentDate.getMonth();

    component.selectedYear = currentYear;
    component.selectedMonth = currentMonth + 1; // Next month (0-indexed)

    component.generateMonthlySalarySlip();

    expect(component.noRecordFound).toBeTrue();
  });

  it('should set noRecordFound to true when selectedYear is not provided', () => {
    component.selectedMonth = 6; // July (0-indexed)

    component.generateMonthlySalarySlip();

    expect(component.noRecordFound).toBeTrue();
  });

  it('should set noRecordFound to true when selectedMonth is not provided', () => {
    component.selectedYear = 2024;

    component.generateMonthlySalarySlip();

    expect(component.noRecordFound).toBeTrue();
  });
  

  it('should initialize with default employee', () => {
    expect(component.employee).toEqual({
      id: 1,
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    });
  });

  it('should load employee details on ngOnInit', () => {
    const mockEmployee: Employee = {
      id: 1,
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    };

    employeeServiceSpy.getEmployeeById.and.returnValue(of({ success: true, data: mockEmployee ,message:''}));

    fixture.detectChanges();
  
    expect(component.employee).toEqual(mockEmployee);
    expect(employeeServiceSpy.getEmployeeById).toHaveBeenCalledWith(1);
  });

  it('should handle error when loading employee details', () => {
const mockError = { error: { message: 'Error fetching employee details' } };
    spyOn(window, 'alert'); // Spy on window.alert

    // Mock getEmployeeById to throw an error
    employeeServiceSpy.getEmployeeById.and.returnValue(throwError(mockError));

component.ngOnInit();    

    // Verify that the employee object remains unchanged (optional, depends on your error handling)
    expect(component.employee).toEqual({
      id: 1,
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    });

    // Verify that window.alert was called with the expected error message
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
  });

  it('should log "Completed" on complete', () => {
    spyOn(console, 'log'); // Spy on console.log method
  
    // Mock an empty response for getEmployeeById
    employeeServiceSpy.getEmployeeById.and.returnValue(of());
  
    // Trigger change detection to call ngOnInit
component.ngOnInit();
  
    // Expect that console.log has been called with 'Completed'
    expect(console.log).toHaveBeenCalledWith('Completed');
  });

  it('should handle loadEmployeeDetails with false success response', () => {
    //Arrange
    const errorMessage:string='Error detching employee';
    const mockEmployee: Employee = {
      id: Number(1),
      employeeEmail: '',
      departmentId: 1,
      firstName: '',
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
        departmentName: '',
      }
    };
    spyOn(console,'log');
    employeeServiceSpy.getEmployeeById.and.returnValue(of({ success: false, data: mockEmployee, message: errorMessage }));

    //Act
    component.employeeId =1;
    component.loadEmployeeDetails(component.employeeId);
    //Assert
   expect(console.log).toHaveBeenCalledWith('Failed to fetch employee',errorMessage);
  });
  
  
});
