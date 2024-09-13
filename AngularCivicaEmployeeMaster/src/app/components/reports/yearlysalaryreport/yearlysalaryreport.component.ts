import { Component } from '@angular/core';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-yearlysalaryreport',
  templateUrl: './yearlysalaryreport.component.html',
  styleUrls: ['./yearlysalaryreport.component.css']
})
export class YearlysalaryreportComponent {

  selectedYear: number | null = null;
  years: number[] = this.generateYears(2001, new Date().getFullYear()); // Example list of years
  salaryHeadTotal: SalaryHeadTotal[] = [];
  yearlyAggregatedSalaries: SalaryHeadTotal | null = null; // Object to store aggregated salaries for the selected year// Array to store aggregated salaries for each year

  constructor(private employeeService: EmployeeService) { }


  fetchSalaryDataByYear(): void {
    if (this.selectedYear) {
      this.employeeService.getTotalSalaryByYear(this.selectedYear).subscribe({
        next: (response: ApiResponse<SalaryHeadTotal[]>) => {
          if (response.success) {
            this.salaryHeadTotal = response.data;
            this.aggregateSalariesByYear();
          } else {
            console.error('Failed to fetch salary data:', response.message);
          }
        },
        error: (error) => {
          console.error('Error fetching salary data:', error);
        }
      });
    }
  }

  aggregateSalariesByYear(): void {
  

    // Initialize an object to store aggregated salary for the selected year
    const aggregatedSalary: SalaryHeadTotal = {
      basicSalary: 0,
      hra: 0,
      allowance: 0,
      grossSalary: 0,
      pfDeduction: 0,
      profTax: 0,
      grossDeductions: 0,
      totalSalary: 0,
      head: 0, // Set to appropriate value if needed
      year: this.selectedYear || 0, // Ensure selectedYear is assigned correctly
      month: 0, // Set to appropriate value if needed
      dateOfJoining: '' // Set to appropriate value if needed
    };

    // Iterate through salaryHeadTotal and aggregate all fields for the selected year
    this.salaryHeadTotal.forEach(item => {
      aggregatedSalary.basicSalary += item.basicSalary;
      aggregatedSalary.hra += item.hra;
      aggregatedSalary.allowance += item.allowance;
      aggregatedSalary.grossSalary += item.grossSalary;
      aggregatedSalary.pfDeduction += item.pfDeduction;
      aggregatedSalary.profTax += item.profTax;
      aggregatedSalary.grossDeductions += item.grossDeductions;
      aggregatedSalary.totalSalary += item.totalSalary;
    });

    // Assign the aggregated salary to yearlyAggregatedSalaries
    this.yearlyAggregatedSalaries = aggregatedSalary;
  }

  generateYears(startYear: number, endYear: number): number[] {
    const years: number[] = [];
    for (let year = startYear; year <= endYear; year++) {
      years.push(year);
    }
    return years;
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
      console.error('c');
    }
  }

  onYearChange(): void {
    // Reset data when year changes
    this.salaryHeadTotal = []; // Clear previous data
    this.yearlyAggregatedSalaries = null; // Clear aggregated salary
    if (this.selectedYear) {
      this.fetchSalaryDataByYear();
    }
  }

}
