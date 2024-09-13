import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaxdeductionemployeemonthlyreportresultComponent } from './taxdeductionemployeemonthlyreportresult.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';

describe('TaxdeductionemployeemonthlyreportresultComponent', () => {
  let component: TaxdeductionemployeemonthlyreportresultComponent;
  let fixture: ComponentFixture<TaxdeductionemployeemonthlyreportresultComponent>;
  let mockEmployeeService: jasmine.SpyObj<EmployeeService>;

  const mockActivatedRoute = {
    params: of({ employeeId: '1' }) // Mock route parameters
  };

  beforeEach(async () => {
    mockEmployeeService = jasmine.createSpyObj('EmployeeService', ['getMonthlyProfTaxOfEachEmployee']);
    
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [TaxdeductionemployeemonthlyreportresultComponent],
      providers: [
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: EmployeeService, useValue: mockEmployeeService }
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TaxdeductionemployeemonthlyreportresultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize component correctly', () => {
    expect(component.years.length).toBeGreaterThan(0);
    expect(component.months.length).toBe(12); // 12 months should be initialized
  });

  it('should fetch data when fetchMonthlyProfTax() is called with selectedYear and selectedMonth', () => {
    component.selectedYear = 2023;
    component.selectedMonth = 5; // May
    component.employeeId = 1; // Mocked employeeId

    const mockResponse: ApiResponse<SalaryHeadTotal[]> = {
      data: [
        {
          basicSalary: 0,
          hra: 0,
          allowance: 0,
          grossSalary: 0,
          pfDeduction: 0,
          profTax: 0,
          grossDeductions: 0,
          totalSalary: 0,
          head: 0,
          year: 0,
          month: 0,
          dateOfJoining: ''
        }
      ],
      success: false,
      message: ''
    };

    mockEmployeeService.getMonthlyProfTaxOfEachEmployee.and.returnValue(of(mockResponse));

    component.fetchMonthlyProfTax();
    

    expect(component.salaryHeadTotals).toEqual(mockResponse.data);
    expect(component.error1).toEqual('');
  });

  it('should handle error when fetchMonthlyProfTax() encounters an error', () => {
    component.selectedYear = 2023;
    component.selectedMonth = 5; // May
    component.employeeId = 1; // Mocked employeeId

    mockEmployeeService.getMonthlyProfTaxOfEachEmployee.and.returnValue(
      throwError({ error: { message: 'Error fetching data' } })
    );

    component.fetchMonthlyProfTax();
   
    expect(component.salaryHeadTotals).toEqual([]);
    expect(component.error1).toEqual('Error fetching data. Please try again later.');
  });

  it('should reset data when fetchMonthlyProfTax() is called without selectedYear or selectedMonth', () => {
    component.fetchMonthlyProfTax();
    expect(component.salaryHeadTotals).toEqual([]);
    expect(component.error1).toEqual('');
  });

  it('should reset selectedMonth and fetch data when onYearChange() is called', () => {
    component.selectedYear = 2023;
    component.selectedMonth = 5; // May

    spyOn(component, 'fetchMonthlyProfTax');

    component.onYearChange();
   

    expect(component.selectedMonth).toBeUndefined();
    expect(component.fetchMonthlyProfTax).toHaveBeenCalled();
  });
});
