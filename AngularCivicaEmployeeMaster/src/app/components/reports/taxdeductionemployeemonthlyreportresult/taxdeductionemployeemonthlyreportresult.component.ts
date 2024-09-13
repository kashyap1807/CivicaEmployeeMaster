import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Employee } from 'src/app/models/employee.model';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-taxdeductionemployeemonthlyreportresult',
  templateUrl: './taxdeductionemployeemonthlyreportresult.component.html',
  styleUrls: ['./taxdeductionemployeemonthlyreportresult.component.css']
})
export class TaxdeductionemployeemonthlyreportresultComponent {
  selectedYear: number | undefined;
  selectedMonth: number | undefined;
  employeeId: number | undefined; // Assuming you fetch employeeId from route
  error1 : string = ''
  

  years: number[] = [];
  months: { value: number; name: string; }[] = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];

  salaryHeadTotals!: SalaryHeadTotal[];

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    // Fetch employeeId from route parameters
    this.route.params.subscribe(params => {
      this.employeeId = +params['employeeId']; // Assuming 'employeeId' is the route parameter
    });

    // Populate years dropdown (e.g., from 2010 to current year)
    const currentYear = new Date().getFullYear();
    for (let year = 2001; year <= currentYear; year++) {
      this.years.push(year);
    }

    // // Initialize selected values (optional)
    // this.selectedYear = currentYear;
    // this.selectedMonth = new Date().getMonth() + 1; // Current month
  }

  // fetchMonthlyProfTax() {
  //   // Call service method to fetch data based on selectedYear, selectedMonth, and employeeId
  //   this.employeeService.getMonthlyProfTaxOfEachEmployee(this.employeeId, this.selectedYear, this.selectedMonth)
  //     .subscribe((response: ApiResponse<SalaryHeadTotal[]>) => {
  //       this.salaryHeadTotals = response.data;
  //       // Handle any additional logic or error handling as needed
  //     }, error => {
  //       this.salaryHeadTotals = [];
  //       console.error('Error fetching data:', error);
  //       console.error(error.error.message);
  //       this.error1 = error.error.message;
  //       // Handle error response
  //     });
  // }

  fetchMonthlyProfTax() {
    if (this.selectedYear && this.selectedMonth) {
     

      // Call service method to fetch data based on selectedYear, selectedMonth, and employeeId
      this.employeeService.getMonthlyProfTaxOfEachEmployee(this.employeeId, this.selectedYear, this.selectedMonth)
        .subscribe((response: ApiResponse<SalaryHeadTotal[]>) => {
          this.salaryHeadTotals = response.data;
          
          // If no data found, display a message
          if (this.salaryHeadTotals.length === 0) {
            this.error1 = 'No data available for the selected year and month.';
          }
        }, error => {
          this.salaryHeadTotals = [];
          console.error('Error fetching data:', error);
          this.error1 = 'Error fetching data. Please try again later.';
        });
      } else {
        // Reset data if year or month is not selected
        this.salaryHeadTotals = [];
      }
  }
  onYearChange() {
    // Reset selectedMonth when year changes
    this.selectedMonth = undefined;
    // Fetch data if both year and month are selected
    if (this.selectedYear) {
      this.fetchMonthlyProfTax();
    }
  }
  
}
