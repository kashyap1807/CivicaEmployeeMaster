import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';

import { TaxdeductionmonthlyreportComponent } from './taxdeductionmonthlyreport.component';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('TaxdeductionmonthlyreportComponent', () => {
  let component: TaxdeductionmonthlyreportComponent;
  let fixture: ComponentFixture<TaxdeductionmonthlyreportComponent>;
  let employeeService: EmployeeService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('EmployeeService', ['getProftaxByMonth']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [TaxdeductionmonthlyreportComponent],
      providers: [EmployeeService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TaxdeductionmonthlyreportComponent);
    component = fixture.componentInstance;
    employeeService = TestBed.inject(EmployeeService);
    httpTestingController = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch professional tax data for selected month and year', () => {
    const mockProfTaxData: SalaryHeadTotal[] = [
      {
        basicSalary: 50000,
        hra: 15000,
        allowance: 10000,
        grossSalary: 75000,
        pfDeduction: 5000,
        profTax: 200,
        grossDeductions: 5200,
        totalSalary: 69800,
        head: 1,
        year: 2024,
        month: 6,
        dateOfJoining: '2020-01-01'
      }
    ];

    const selectedYear = 2024;
    const selectedMonth = 6;

    // Simulate successful API response
    spyOn(employeeService, 'getProftaxByMonth').and.returnValue(of({ success: true, data: mockProfTaxData,message:'' }));

    // Set selected year and month
    component.selectedYear = selectedYear;
    component.selectedMonth = selectedMonth;

    // Trigger method to fetch data
    component.fetchProfTaxData();

    // Check that data is correctly set after API call
    expect(component.profTaxData).toEqual(mockProfTaxData[0]);

    // Verify that the service method was called with correct parameters
    expect(employeeService.getProftaxByMonth).toHaveBeenCalledWith(selectedMonth, selectedYear);
  });

  
  it('should handle error when fetching Prof Tax data', () => {
    // Setup the spy method to return an error
    spyOn(employeeService,'getProftaxByMonth').and.returnValue(throwError('API error'));

    // Set component properties to simulate selected year and month
    component.selectedYear = 2023;
    component.selectedMonth = 6; // Assuming month selection is 1-based index

    // Call the method to be tested
    component.fetchProfTaxData();
  spyOn(component,'fetchProfTaxData')
    // Assert expected behavior
    expect(employeeService.getProftaxByMonth).toHaveBeenCalledWith(6, 2023); // Check if service method was called with correct parameters
    expect(component.profTaxData).toBeNull(); // Check if profTaxData is null due to error
  });
  it('should clear professional tax data on year change', () => {
    component.profTaxData = {} as SalaryHeadTotal;

    component.onYearSelected();

    // Check that profTaxData is cleared
    expect(component.profTaxData).toBeNull();
    expect(component.selectedMonth).toEqual(0);
  });
  it('should fetch Prof Tax data on month selection', () => {
    // Mock data
    const mockApiResponse: ApiResponse<SalaryHeadTotal[]> = {
      success: true,
      data: [
        {
          basicSalary: 5000, hra: 1000, allowance: 500,
          grossSalary: 0,
          pfDeduction: 0,
          profTax: 0,
          grossDeductions: 0,
          totalSalary: 0,
          head: 0,
          year: 0,
          month: 0,
          dateOfJoining: ''
        } // Sample data, replace with actual SalaryHeadTotal object
      ],
      message: ''
    };

    // Setup the spy method to return the mock response
    spyOn(employeeService,'getProftaxByMonth').and.returnValue(of(mockApiResponse));

    // Set component properties to simulate selected year and month
    component.selectedYear = 2023;
    component.selectedMonth = 6; // Assuming month selection is 1-based index

    // Call the method to be tested
    component.onMonthSelected();
   
    // Assert expected behavior
    expect(component.profTaxData).toEqual(mockApiResponse.data[0]); // Check if profTaxData is set correctly
    expect(employeeService.getProftaxByMonth).toHaveBeenCalledWith(6, 2023); // Check if service method was called with correct parameters
  });

  it('should handle error when fetching Prof Tax data on month selection', () => {
    // Setup the spy method to return an error
    spyOn(employeeService,'getProftaxByMonth').and.returnValue(throwError('API error'));

    // Set component properties to simulate selected year and month
    component.selectedYear = 2023;
    component.selectedMonth = 6; // Assuming month selection is 1-based index

    // Call the method to be tested
    component.onMonthSelected();
   // Assert expected behavior
    expect(component.profTaxData).toBeNull(); // Check if profTaxData is null due to error
    expect(employeeService.getProftaxByMonth).toHaveBeenCalledWith(6, 2023); // Check if service method was called with correct parameters
  });

  // it('should handle error when fetching Prof Tax data', (() => {
  //   // Setup the spy method to return an error
  //   employeeService.getProftaxByMonth.and.returnValue(throwError('API error'));

  //   // Call the method to be tested
  //   component.fetchProfTaxData();

  //   // Assert expected behavior within the subscribe callback
  //   expect(component.profTaxData).toBeNull(); // Check if profTaxData is null due to error
  //   expect(console.error).toHaveBeenCalledWith('Error fetching Prof Tax data:', 'API error'); // Check console error message
  // }));

  it('should set profTaxData to null when no data found for selected month and year', () => {
    // Setup the spy method to return success response with empty data array
    spyOn(employeeService,'getProftaxByMonth').and.returnValue(of({ success: true, data: [],message:'' }));
spyOn(console,'error' )
    // Set component properties to simulate selected year and month
    component.selectedYear = 2023;
    component.selectedMonth = 6; // Assuming month selection is 1-based index

    // Call the method to be tested
    component.fetchProfTaxData();
    
    // Assert expected behavior
    expect(component.profTaxData).toBeNull(); // Check if profTaxData is null when no data found
    expect(console.error).toHaveBeenCalledWith('No data found for the selected month and year.'); // Check console error message
  });
});
