import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-monthlysalaryreport',
  templateUrl: './monthlysalaryreport.component.html',
  styleUrls: ['./monthlysalaryreport.component.css']
})
export class MonthlysalaryreportComponent {

  salaryHeadTotal : SalaryHeadTotal[] =[];
  selectedYear: number | null = null;
  selectedMonth: string | null = null;
  years: number[] = this.generateYears(2001, new Date().getFullYear()); // Example list of years
  months: string[] = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']; // Example list of months

  constructor(private employeeService:EmployeeService,private route:ActivatedRoute){}


  ngOnInit(): void {

    // this.route.queryParams.subscribe(params => {
      // this.selectedYear = params['year'] ? +params['year'] : null; // Convert to number
      // this.selectedMonth = params['month'] || null;
      if (this.selectedYear && this.selectedMonth) {
        this.fetchSalaryData();
      }
  
   }

   fetchSalaryData(): void {
    if (this.selectedYear && this.selectedMonth) {
      this.employeeService.getTotalSalaryByMonth(this.months.indexOf(this.selectedMonth) + 1, this.selectedYear)
        .subscribe({
          next: (response: ApiResponse<SalaryHeadTotal[]>) => {
            if (response.success) {
              this.salaryHeadTotal = response.data;
            } else {
              console.error('Failed to fetch salary data', response.message);
            }
          },
          error: (error) => {
            console.error('Error fetching salary data.', error);
            if (!error || !error.response || !error.response.data) {
              this.salaryHeadTotal = []; // Set salaryHeadTotal to empty array or handle as needed
            } else {
              this.salaryHeadTotal = error.response.data;
            }
          }
        });
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
        doc.save(`MonthalySalarySlip_${this.selectedYear}-${this.selectedMonth}.pdf`);
      });
    } else {
      console.error('Salary slip element not found or not initialized.');
    }
  }
  


  onYearChange(): void {
    this.selectedMonth = null; // Reset selected month when year changes
    this.fetchSalaryData(); // Fetch data for the new selected year
    this.salaryHeadTotal=[];
  }

  onMonthChange(): void {
    this.fetchSalaryData(); // Fetch data for the new selected month
  }
  generateYears(startYear: number, endYear: number): number[] {
    const years: number[] = [];
    for (let year = startYear; year <= endYear; year++) {
      years.push(year);
    }
    return years;
  }



}
