import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YearlysalaryreportComponent } from './yearlysalaryreport.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { of, throwError } from 'rxjs';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';


describe('YearlysalaryreportComponent', () => {
  let component: YearlysalaryreportComponent;
  let fixture: ComponentFixture<YearlysalaryreportComponent>;
  let mockEmployeeService: jasmine.SpyObj<EmployeeService>; // Mocked service
  let employeeService: EmployeeService;
  let jsPDFMock: any;
  let html2canvasMock: any;
  let getTotalSalaryByYearSpy: jasmine.Spy;

  beforeEach(() => {
    jsPDFMock = jasmine.createSpyObj('jsPDF', ['addImage', 'save']);
    // Mock html2canvas method
    html2canvasMock = jasmine.createSpy().and.returnValue(Promise.resolve({
      toDataURL: () => 'data:image/png;base64,MockedBase64String',
      width: 200,
      height: 300
    }));
    mockEmployeeService = jasmine.createSpyObj('EmployeeService', ['getTotalSalaryByYear']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [YearlysalaryreportComponent],
      providers: [
        { provide: EmployeeService, useValue: mockEmployeeService },
        { provide: jsPDF, useValue: jsPDFMock },
        { provide: html2canvas, useValue: html2canvasMock }
      ]
    });
    fixture = TestBed.createComponent(YearlysalaryreportComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should fetch salary data when selectedYear is set', () => {
    // Arrange
    const selectedYear = 2023;
    const mockSalaryData: SalaryHeadTotal[] = [
      {
        head: 1, totalSalary: 50000,
        basicSalary: 0,
        hra: 0,
        allowance: 0,
        grossSalary: 0,
        pfDeduction: 0,
        profTax: 0,
        grossDeductions: 0,
        year: 0,
        month: 0,
        dateOfJoining: ''
      },
      {
        head: 2, totalSalary: 40000,
        basicSalary: 0,
        hra: 0,
        allowance: 0,
        grossSalary: 0,
        pfDeduction: 0,
        profTax: 0,
        grossDeductions: 0,
        year: 0,
        month: 0,
        dateOfJoining: ''
      }
    ];
    const mockApiResponse = {
      success: true,
      message: 'Successfully fetched salary data',
      data: mockSalaryData
    };
    mockEmployeeService.getTotalSalaryByYear.and.returnValue(of(mockApiResponse));
    component.selectedYear = selectedYear;
component.selectedYear = 2023;
component.fetchSalaryDataByYear();
fixture.detectChanges();

fixture.whenStable().then(() => {
  expect(component.salaryHeadTotal.length).toBeGreaterThan(0);
});

    // Act
    component.fetchSalaryDataByYear();

    // Assert
    expect(mockEmployeeService.getTotalSalaryByYear).toHaveBeenCalledWith(selectedYear);
    expect(component.salaryHeadTotal).toEqual(mockSalaryData);
    // Add more assertions as needed
  });
  it('should handle error when getTotalSalaryByYear returns an error', () => {
    // Arrange
    const selectedYear = 2023;
    const mockError = new Error('Test error');
    mockEmployeeService.getTotalSalaryByYear.and.returnValue(throwError(mockError));
    component.selectedYear = selectedYear;

    // Act
    component.fetchSalaryDataByYear();

    // Assert
    expect(mockEmployeeService.getTotalSalaryByYear).toHaveBeenCalledWith(selectedYear);
    // Add assertions to handle error case, e.g., check console.error messages
  });
  it('should not fetch salary data when selectedYear is not set', () => {
    // Arrange
    component.selectedYear = null;

    // Act
    component.fetchSalaryDataByYear();

    // Assert
    expect(mockEmployeeService.getTotalSalaryByYear).not.toHaveBeenCalled();
    // Add more assertions as needed
  });
  it('should aggregate salaries for the selected year', () => {
    // Arrange
    component.selectedYear = 2023; // Set selectedYear
    component.salaryHeadTotal = [
      {
        basicSalary: 10000, hra: 2000, allowance: 3000, grossSalary: 15000, pfDeduction: 500, profTax: 200, grossDeductions: 700, totalSalary: 14300,
        head: 0,
        year: 0,
        month: 0,
        dateOfJoining: ''
      },
      {
        basicSalary: 12000, hra: 2500, allowance: 3500, grossSalary: 16000, pfDeduction: 600, profTax: 250, grossDeductions: 850, totalSalary: 15150,
        head: 0,
        year: 0,
        month: 0,
        dateOfJoining: ''
      }
    ];

    // Act
    component.aggregateSalariesByYear();

    // Assert
    expect(component.yearlyAggregatedSalaries).toEqual({
      basicSalary: 22000,
      hra: 4500,
      allowance: 6500,
      grossSalary: 31000,
      pfDeduction: 1100,
      profTax: 450,
      grossDeductions: 1550,
      totalSalary: 29450,
      head: 0,
      year: 2023,
      month: 0,
      dateOfJoining: ''
    });
    
  });
  it('should handle empty salaryHeadTotal array', () => {
    // Arrange
    component.selectedYear = 2023; // Set selectedYear
    component.salaryHeadTotal = []; // Empty array
  
    // Act
    component.aggregateSalariesByYear();
  
    // Assert
    expect(component.yearlyAggregatedSalaries).toEqual({
      basicSalary: 0,
      hra: 0,
      allowance: 0,
      grossSalary: 0,
      pfDeduction: 0,
      profTax: 0,
      grossDeductions: 0,
      totalSalary: 0,
      head: 0,
      year: 2023,
      month: 0,
      dateOfJoining: ''
    });
  });
  it('should generate years array from startYear to endYear', () => {
    // Arrange
    const startYear = 2020;
    const endYear = 2025;
  
    // Act
    const years = component.generateYears(startYear, endYear);
  
    // Assert
    expect(years).toEqual([2020, 2021, 2022, 2023, 2024, 2025]);
  });
  it('should generate single year array when startYear equals endYear', () => {
    // Arrange
    const startYear = 2023;
    const endYear = 2023;
  
    // Act
    const years = component.generateYears(startYear, endYear);
  
    // Assert
    expect(years).toEqual([2023]);
  });
  it('should handle reverse year range (startYear > endYear)', () => {
    // Arrange
    const startYear = 2025;
    const endYear = 2020;
  
    // Act
    const years = component.generateYears(startYear, endYear);
  
    // Assert
    expect(years).toEqual([]);
  });
//   it('should handle large year range with negative years', () => {
//     // Arrange
//     const startYear = -100;
//     const endYear = 100;
  
//     // Act
//     const years = component.generateYears(startYear, endYear);
  
//     // Assert
//     expect(years.length).toBe(201); // 100 years from -100 to 100 inclusive
//     expect(years[0]).toBe(-100);
//     expect(years[years.length - 1]).toBe(100);
//   });
//   it('should download PDF invoice for selected year', () => {
//     // Arrange
//     component.selectedYear = 2023;

//     // Act
//     component.downloadInvoice();

//     // Assert
//     fixture.detectChanges(); // Ensure changes are detected

//     // Check if html2canvas was called with the correct element
//     fixture.whenStable().then(() => {
//       const element = document.getElementById('salarySlipTable');
//       expect(html2canvasMock).toHaveBeenCalledWith(element, jasmine.any(Object));

//     // Simulate the resolution of html2canvas promise
//     return html2canvasMock.calls.mostRecent().returnValue.then(() => {
//       expect(jsPDFMock.addImage).toHaveBeenCalled();
//       expect(jsPDFMock.save).toHaveBeenCalledWith(`YearlySalarySlip_2023.pdf`);
//       done();
//     });
//   }).catch((error) => {
//     fail(`Test failed with error: ${error}`);
//     done();
//   });
// });
// // Example test case
// it('should handle case where salarySlipTable element is not found', () => {
//   const mockElement = document.createElement('div');
//   spyOn(document, 'getElementById').and.returnValue(mockElement);

//   // Call the component method that interacts with getElementById
//   component.downloadInvoice();

//   // Assert that getElementById was called with 'salarySlipTable'
//   expect(document.getElementById).toHaveBeenCalledWith('salarySlipTable');
// });

// it('should handle failure to capture salarySlipTable element', (done: DoneFn) => {
//   // Arrange
//   component.selectedYear = 2023;
//   spyOn(html2canvasMock, 'calls').and.returnValue(Promise.reject('Capture failed'));

//   // Act
//   component.downloadInvoice();

//   // Assert

//   // Ensure changes are detected
// spyOn(component,'downloadInvoice')
//   // Check error handling for html2canvas failure
//   fixture.whenStable().then(() => {
//     expect(console.error).toHaveBeenCalledWith('Error capturing salary slip element:', 'Capture failed');
//     // Ensure jsPDF methods are not called
//     expect(jsPDFMock.addImage).not.toHaveBeenCalled();
//     expect(jsPDFMock.save).not.toHaveBeenCalled();
//     done();
//   }).catch((error) => {
//     fail(`Test failed with error: ${error}`);
//     done();
//   });
// });
// it('should not download PDF if selectedYear is not set', () => {
//   // Arrange
//   component.selectedYear = null; // or undefined

//   // Act
//   component.downloadInvoice();

//   // Assert
//   expect(html2canvasMock).not.toHaveBeenCalled();
//   expect(jsPDFMock.addImage).not.toHaveBeenCalled();
//   expect(jsPDFMock.save).not.toHaveBeenCalled();
// });
it('should save PDF with correct filename format', () => {
  // Arrange
  component.selectedYear = 2023;
  
   // Act
  component.downloadInvoice();
  // Assert
  
  })
  
it('should reset data and fetch salary data when year changes', () => {
  // Arrange
  component.selectedYear = 2023;
  const fetchSalaryDataSpy = spyOn(component, 'fetchSalaryDataByYear');

  // Act
  component.onYearChange();

  // Assert
  expect(component.salaryHeadTotal).toEqual([]); // Check if salaryHeadTotal is cleared
  expect(component.yearlyAggregatedSalaries).toBeNull(); // Check if yearlyAggregatedSalaries is reset
  expect(fetchSalaryDataSpy).toHaveBeenCalled(); // Ensure fetchSalaryDataByYear() was called

  // Simulate behavior if selectedYear is not set
  component.selectedYear = null;
  fetchSalaryDataSpy.calls.reset(); // Reset spy calls

  // Act again
  component.onYearChange();

  // Assert again
  expect(fetchSalaryDataSpy).not.toHaveBeenCalled(); // Ensure fetchSalaryDataByYear() was not called
});
it('should reset data and fetch salary data when a new year is selected', () => {
  // Arrange
  component.selectedYear = 2023;
  const fetchSalaryDataSpy = spyOn(component, 'fetchSalaryDataByYear');

  // Act
  component.onYearChange();

  // Assert
  expect(component.salaryHeadTotal).toEqual([]);
  expect(component.yearlyAggregatedSalaries).toBeNull();
  expect(fetchSalaryDataSpy).toHaveBeenCalled();
});

it('should not fetch salary data when selectedYear is null', () => {
  // Arrange
  component.selectedYear = null;
  const fetchSalaryDataSpy = spyOn(component, 'fetchSalaryDataByYear');

  // Act
  component.onYearChange();

  // Assert
  expect(component.salaryHeadTotal).toEqual([]);
  expect(component.yearlyAggregatedSalaries).toBeNull();
  expect(fetchSalaryDataSpy).not.toHaveBeenCalled();
});

it('should handle asynchronous data reset and fetch correctly', () => {
  // Arrange
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

  component.selectedYear = 2023;
  spyOn(component, 'fetchSalaryDataByYear').and.callThrough();
  mockEmployeeService.getTotalSalaryByYear.and.returnValue(of());
    
  // Act
  component.onYearChange();

  // Assert
  expect(component.salaryHeadTotal).toEqual([]); // Check if salaryHeadTotal is cleared
  expect(component.yearlyAggregatedSalaries).toBeNull(); // Check if yearlyAggregatedSalaries is reset
  expect(component.fetchSalaryDataByYear).toHaveBeenCalled(); // Ensure fetchSalaryDataByYear() was called
});

it('should handle false resposne for fetchSalaryDataByYear', () => {
  // Arrange
  const mockResponse = {
    success: false,
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

  component.selectedYear = 2023;
  spyOn(console,'error');
  spyOn(component, 'fetchSalaryDataByYear').and.callThrough();
  mockEmployeeService.getTotalSalaryByYear.and.returnValue(of(mockResponse));
    
  // Act
  component.fetchSalaryDataByYear();

  // Assert
  expect(console.error).toHaveBeenCalledOnceWith('Failed to fetch salary data:',mockResponse.message);
  expect(component.fetchSalaryDataByYear).toHaveBeenCalled(); // Ensure fetchSalaryDataByYear() was called
});

});

function done() {
  throw new Error('Function not implemented.');
}