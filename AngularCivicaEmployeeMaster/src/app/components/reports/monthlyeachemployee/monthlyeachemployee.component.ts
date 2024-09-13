import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Employee } from 'src/app/models/employee.model';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-monthlyeachemployee',
  templateUrl: './monthlyeachemployee.component.html',
  styleUrls: ['./monthlyeachemployee.component.css']
})
export class MonthlyeachemployeeComponent {
  employee : Employee[] | undefined |null;;
  employeeId : number | undefined;
  loading:boolean=false;
  isAuthenticated:boolean=false;
  username:string |null|undefined;
  search: string = '';
  totalItems: number = 0;
  pageNumber: number = 1;
  pageSize: number = 4;
  sort:string="asc";
  totalPages: number = 0;
  minSearchLength: number = 3;

 
  constructor(private employeeService : EmployeeService,private router:Router) {}

  ngOnInit():void{
    this.getAllEmployeesCount();
    

  }

  getAllEmployeesCount():void{
    let searchQuery=this.search
    if(searchQuery.length<3)
      {
        searchQuery='';
      }
    this.employeeService.getAllEmployeesCount(searchQuery).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.getAllEmployeeWithPagination();
        } else {
          console.error('Failed to fetch contacts count', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts count.', error);
        this.loading = false;
      }
    });

  }

  getAllEmployeeWithPagination():void{
    this.loading = true;
    if (this.pageNumber > this.totalPages) {
      console.log('Requested page does not exist.');
      return;
    }
    let searchQuery=this.search
    if(searchQuery.length<3)
      {
        searchQuery='';
      }
    this.employeeService.getAllEmployeeWithPagination(this.pageNumber,this.pageSize,this.sort,searchQuery).subscribe({
      next:(response:ApiResponse<Employee[]>)=>{
        if(response.success){
          
          this.employee = response.data;
          console.log(this.employee);
        
        }else{
          console.error('Failed to fetch employee',response.message);

        }
        this.loading = false;
      },
      error :(error)=>{
        console.error('Error fetching employee.',error);
        this.loading = false;
      }
    });
  }

  changePageSize(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageNumber = 1; 
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);
    this.getAllEmployeeWithPagination();
  }
  sortAsc()
{
this.sort = 'asc'
this.pageNumber = 1;
this.getAllEmployeeWithPagination();
}

sortDesc()
{
this.sort = 'desc'
this.pageNumber = 1;
this.getAllEmployeeWithPagination();

}
onSearch() {
  if (this.search.length >= this.minSearchLength || this.search.length === 0) {
    // If search length meets the minimum requirement or is 0, perform search or clear search
    this.pageNumber = 1;
    this.getAllEmployeeWithPagination();
    this.getAllEmployeesCount();
  } else {
    // If search length is less than minSearchLength, show validation message and fetch all employees
    this.pageNumber = 1; // Reset page number to 1 to display correct pagination
    this.getAllEmployeesCount();
    this.getAllEmployeeWithPagination(); // This ensures search is cleared and all employees are fetched
   
  }
  // No need to handle cases where search.length is between 1 and minSearchLength - 1, as they are covered by the HTML ngIf condition
}

clearSearch() {
  this.search = '';
  this.pageNumber = 1;
  this.getAllEmployeeWithPagination();
  this.getAllEmployeesCount();
}


  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.getAllEmployeeWithPagination();
  }

}
