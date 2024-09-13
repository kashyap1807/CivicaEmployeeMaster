import { Component } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-taxdeductionmonthlyreport',
  templateUrl: './taxdeductionmonthlyreport.component.html',
  styleUrls: ['./taxdeductionmonthlyreport.component.css']
})
export class TaxdeductionmonthlyreportComponent {

  selectedYear: number | null = null;
   selectedMonth: number = 0;
  years: number[] = this.generateYears(2001, new Date().getFullYear()); // Example list of years // Example list of years
  
  months = [
    { name: 'January', value: 1 },
    { name: 'February', value: 2 },
    { name: 'March', value: 3 },
    { name: 'April', value: 4 },
    { name: 'May', value: 5 },
    { name: 'June', value: 6 },
    { name: 'July', value: 7 },
    { name: 'August', value: 8 },
    { name: 'September', value: 9 },
    { name: 'October', value: 10 },
    { name: 'November', value: 11 },
    { name: 'December', value: 12 }
  ];
  monthNames: string[] = this.months.map(month => month.name);

  profTaxData: SalaryHeadTotal | null = null;

  constructor(private employeeService: EmployeeService) { }

  fetchProfTaxData(): void {
    if (this.selectedYear && this.selectedMonth) {
      this.employeeService.getProftaxByMonth(this.selectedMonth, this.selectedYear).subscribe({
        next: (response: ApiResponse<SalaryHeadTotal[]>) => {
          if (response.success && response.data.length > 0) {
            // Assuming API returns only one record for the selected month and year
            this.profTaxData = response.data[0];
          } else {
            console.error('No data found for the selected month and year.');
            this.profTaxData = null;
          }
        },
        error: (error) => {
          console.error('Error fetching Prof Tax data:', error);
          this.profTaxData = null;
        }
      });
    }
  }

  onYearSelected(): void {
    // Reset selected month when year changes
    this.selectedMonth = 0;
    this.profTaxData = null; // Clear previous data
  }

  onMonthSelected(): void {
    this.profTaxData = null; // Clear previous data
    this.fetchProfTaxData();
  }

  generateYears(startYear: number, endYear: number): number[] {
    const years: number[] = [];
    for (let year = startYear; year <= endYear; year++) {
      years.push(year);
    }
    return years;
  }

}
