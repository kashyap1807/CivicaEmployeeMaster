import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EmployeeDetailsComponent } from './employee-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { Employee } from 'src/app/models/employee.model';

describe('EmployeeDetailsComponent', () => {
  let component: EmployeeDetailsComponent;
  let fixture: ComponentFixture<EmployeeDetailsComponent>;
  let mockEmployeeService: jasmine.SpyObj<EmployeeService>;

  beforeEach(() => {
    const employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['getEmployeeById']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [EmployeeDetailsComponent],
      providers: [
        { provide: EmployeeService, useValue: employeeServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ employeeId: 1 }) // Simulate ActivatedRoute params
          }
        }
      ]
    });

    fixture = TestBed.createComponent(EmployeeDetailsComponent);
    component = fixture.componentInstance;
    mockEmployeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
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

    mockEmployeeService.getEmployeeById.and.returnValue(of({ success: true, data: mockEmployee ,message:''}));

    fixture.detectChanges();
  
    expect(component.employee).toEqual(mockEmployee);
    expect(mockEmployeeService.getEmployeeById).toHaveBeenCalledWith(1);
  });

  it('should handle error when loading employee details', () => {
    const errorMessage = 'Error fetching employee details';
  
    spyOn(window, 'alert'); // Spy on window.alert
  
    mockEmployeeService.getEmployeeById.and.returnValue(throwError({ error: { message: errorMessage } }));
  
    fixture.detectChanges();
   
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
    expect(window.alert).toHaveBeenCalledWith(errorMessage); // Now this line should work correctly
  });
  

  it('should log "Completed" on complete', () => {
    spyOn(console, 'log');
    mockEmployeeService.getEmployeeById.and.returnValue(of());

    fixture.detectChanges();
  
    expect(console.log).toHaveBeenCalledWith('Completed');
  });
 
});
