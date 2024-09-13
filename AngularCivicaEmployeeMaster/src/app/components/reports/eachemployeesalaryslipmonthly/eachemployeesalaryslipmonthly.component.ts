import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { Employee } from 'src/app/models/employee.model';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-eachemployeesalaryslipmonthly',
  templateUrl: './eachemployeesalaryslipmonthly.component.html',
  styleUrls: ['./eachemployeesalaryslipmonthly.component.css']
})
export class EachemployeesalaryslipmonthlyComponent {
  selectedYear: number | null = null;
  selectedMonth: number | null = null;
  years: number[] = this.generateYearArray(); // Example list of years
  months: string[] = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']; // Example list of months
  employeeId: number | undefined;
  employee: Employee = {
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
  noRecordFound: boolean = false;

  constructor(private employeeService: EmployeeService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.employeeId = params['employeeId'];
      this.loadEmployeeDetails(this.employeeId);
    });
  }

  loadEmployeeDetails(employeeId: number | undefined): void {
    this.employeeService.getEmployeeById(employeeId).subscribe({
      next: (response) => {
        if (response.success) {
          this.employee = response.data;
          console.log(this.employee);
        } else {
          console.log('Failed to fetch employee', response.message);
        }
      },
      error: (err) => {
        alert(err.error.message);
      },
      complete: () => {
        console.log('Completed');
      }
    });
  }

  generateYearArray(): number[] {
    const currentYear = new Date().getFullYear();
    const years = [];
    for (let year = currentYear; year >= 2001; year--) {
      years.push(year);
    }
    return years;
  }

  generateMonthlySalarySlip(): void {
    if (this.selectedYear && this.selectedMonth) {
      // Convert selected month and year to Date object for comparison
      const selectedDate = new Date(this.selectedYear, this.selectedMonth , 1); // Month is 0-indexed in Date object

      // Check if selected date is greater than or equal to employee's date of joining
      const employeeJoinDate = new Date(this.employee.dateOfJoining);
      if (selectedDate < employeeJoinDate) {
        this.noRecordFound = true;
        return;
      }

      // Check if selected date is greater than or equal to current month's date
      const currentDate = new Date();
      currentDate.setHours(0, 0, 0, 0); // Set time to start of the day
      currentDate.setDate(1); // Set date to 1st of the month

      if (selectedDate > currentDate) {
        this.noRecordFound = true;
        return;
      }

      this.noRecordFound = false;
      // Implement logic to fetch and display monthly salary slip data
      // Example: this.employeeService.getMonthlySalarySlip(this.employeeId, this.selectedYear, this.selectedMonth).subscribe({
      //   next: (response) => {
      //     // Handle response and update UI accordingly
      //   },
      //   error: (err) => {
      //     console.error('Failed to fetch monthly salary slip', err);
      //   }
      // });
    } else {
      this.noRecordFound = true;
    }
  }

  downloadInvoice(): void {
    const doc = new jsPDF();
  
    // Options for PDF generation
    const options = {
      background: 'white',
      scale: 3, // Adjust scale to fit content properly
      useCORS: true, // Allow cross-origin content (if applicable)
      logging: true // Enable console logging for debugging
    };
  
    // Capture the table content as an image using html2canvas
    const element = document.getElementById('salarySlipTable');
    if (element) {
      html2canvas(element, options).then((canvas) => {
        // Convert the canvas to base64 image data
        const imgData = canvas.toDataURL('image/png');
  
        // Add the image to the PDF
        const imgHeight = canvas.height * 210 / canvas.width; // Scale height based on A4 dimensions
        doc.addImage(imgData, 'PNG', 10, 10, 190, imgHeight);
  
        // Save the PDF
        doc.save(`YearlySalarySlip_${this.selectedYear}.pdf`);
      });
    } else {
      console.error('Salary slip element not found or not initialized.');
    }
  }
  
}