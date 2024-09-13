import { Component, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { EmployeeService } from 'src/app/services/helpers/employee.service';
import { saveAs } from 'file-saver';
import   jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { Employee } from 'src/app/models/employee.model';




@Component({
  selector: 'app-yearlyeachemployeesalary',
  templateUrl: './yearlyeachemployeesalary.component.html',
  styleUrls: ['./yearlyeachemployeesalary.component.css']
})
export class YearlyeachemployeesalaryComponent {
  @ViewChild('salarySlip', { static: false }) salarySlip!: ElementRef;
  selectedYear: number | undefined ;
  employeeFirstName:string='';
  employeeLastName:string='';
  employeeEmail:string='';
  employeeDepartment:string='';
  
  employeeId: number = 1; // Assuming a default employeeId for demonstration
  years: number[] = this.generateYears(2001, new Date().getFullYear()); // Example list of years // Example list of years
  yearlySalaryData: any[] = []; 
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
  employee1: Employee = {
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
  constructor(private employeeService: EmployeeService,private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
     this.employeeId = params['employeeId'];
     this.fetchYearlySalaryData(this.employeeId,this.selectedYear);
     this.loadEmployeeDetails(this.employeeId);
    }); 
   }

   loadEmployeeDetails(employeeId: number | undefined): void {
    this.employeeService.getEmployeeById(employeeId).subscribe({
      next: (response) => {
        if (response.success) {
          this.employee1 = response.data;
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

  // ngOnInit(): void {
  //   this.route.params.subscribe(params => {
  //     this.employeeId = params['employeeId'];
  //   });

  //   this.route.queryParams.subscribe(params => {
  //     this.employeeFirstName = params['firstname'];
  //     this.employeeLastName = params['lastName'];
  //     this.employeeEmail = params['email'];
  //     this.employeeDepartment = params['department'];
      
  //     // Example: Call a function that uses these parameters
  //     this.fetchYearlySalaryData(this.employeeId, this.selectedYear);
  //   });}

  fetchYearlySalaryData(employeeId:number,selectedYear:number|undefined): void {
    if (this.selectedYear) {
      this.employeeService.getYearlySalaryOfEachEmployee(employeeId, this.selectedYear).subscribe({
        next: (response: ApiResponse<SalaryHeadTotal[]>) => {
          if (response.success) {
            this.yearlySalaryData = response.data;
          } else {
            console.error('Failed to fetch yearly salary data:', response.message);
          }
        },
        error: (error) => {
          console.error('Error fetching yearly salary data:', error);
        }
      });
    }
  }

  onYearSelected(): void {
    this.yearlySalaryData=[]
    this.fetchYearlySalaryData(this.employeeId,this.selectedYear); // Fetch data when year is selected
  }

//   downloadInvoice(): void {
//     // Prepare invoice content (example)
//     let invoiceContent = `
//         Yearly Salary Slip for ${this.selectedYear}\n
//         ------------------------------\n
//         Basic Salary: ${this.yearlySalaryData[0].basicSalary}\n
//         HRA: ${this.yearlySalaryData[0].hra}\n
//         Allowance: ${this.yearlySalaryData[0].allowance}\n
//         Gross Salary: ${this.yearlySalaryData[0].grossSalary}\n
//         PF Deduction: ${this.yearlySalaryData[0].pfDeduction}\n
//         Prof Tax: ${this.yearlySalaryData[0].profTax}\n
//         Gross Deductions: ${this.yearlySalaryData[0].grossDeductions}\n
//         Total Salary: ${this.yearlySalaryData[0].totalSalary}\n
//     `;

//     // Convert to Blob and trigger download
//     const blob = new Blob([invoiceContent], { type: 'text/plain;charset=utf-8' });
//     saveAs(blob, `YearlySalarySlip_${this.selectedYear}.txt`);

// }


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

generateYears(startYear: number, endYear: number): number[] {
    const years: number[] = [];
    for (let year = startYear; year <= endYear; year++) {
      years.push(year);
    }
    return years;
  }





}

