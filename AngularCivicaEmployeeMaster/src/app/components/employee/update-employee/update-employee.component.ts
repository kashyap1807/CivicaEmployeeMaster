import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { EmployeeDepartment } from 'src/app/models/employeeDepartment.model';
import { UpdateEmployee } from 'src/app/models/update-employee.model';
import { EmployeeDepartmentService } from 'src/app/services/employee-department.service';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-update-employee',
  templateUrl: './update-employee.component.html',
  styleUrls: ['./update-employee.component.css']
})
export class UpdateEmployeeComponent implements OnInit{

  employeeId:number|undefined;

  employee : UpdateEmployee = {
    id:0,
    employeeEmail:'',
    departmentId:1,
    firstName:'',
    lastName:'',
    gender:'',
    basicSalary:10000,
    allowance:100,
    dateOfJoining:''

  };

  departments: EmployeeDepartment[]=[];

  loading:boolean=false;

  constructor(private employeeService:EmployeeService,private departmentService:EmployeeDepartmentService,private route: ActivatedRoute,private router: Router){}

  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.loadDepartments();
      this.employeeId = +params['employeeId'];
      this.loadEmployeeDetails(this.employeeId);
    });
  }

  loadDepartments():void{
    this.loading = true;
    this.departmentService.getAllDepartments().subscribe({
      next:(response: ApiResponse<EmployeeDepartment[]>) =>{
        if(response.success){
          this.departments = response.data;
        }
        else{
          console.error('Failed to fetch departments ', response.message);
        }
        this.loading = false;
      },error:(error)=>{
        console.error('Error fetching departments : ',error);
        this.loading = false;
      }
    });
  }

 formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }
 

  loadEmployeeDetails(employeeId:number|undefined):void{
    this.employeeService.getEmployeeById(employeeId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.employee = response.data;
          this.employee.dateOfJoining = this.formatDate(new Date(this.employee.dateOfJoining));
        }else{
          console.log('Failed to fetch employee',response.message);
          
        }
      },
      error:(err)=>{
        alert(err.error.message);
      },
      complete:()=>{
        console.log('Completed');
        
      }
    });
  }

  isFutureDate(date: string): boolean {
    const today = new Date();
    const selectedDate = new Date(date);
    return selectedDate > today;
  }

  onSubmit(updateEmployeeTfForm:NgForm){
    if (updateEmployeeTfForm.valid) {

      if (this.isFutureDate(this.employee.dateOfJoining)) {
        updateEmployeeTfForm.controls['date'].setErrors({'futureDate': true});
      }
      
      this.loading = true;
      console.log(updateEmployeeTfForm.value);
      
      this.employeeService.UpdateEmployee(this.employee).subscribe({
        next:(response)=>{
          if(response.success){
            this.router.navigate(['/employee']);
            alert("Employee Updated Successfully!");
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.log(err.error.message);
          alert(err.error.message);
          this.loading=false;
          
        },
        complete:()=>{
          console.log("Completed");
          
        }
      });
    }
  }
  

  maxDate(): string {
    // Get current date in YYYY-MM-DD format
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
 
    return `${yyyy}-${mm}-${dd}`;
  }

  minDate():string
{    
const yyyy ='2000';
// Year 2000
const mm ='01';
// January is represented as '01'
const dd ='01';
// Always set the day as '01' for January 1st
return`${yyyy}-${mm}-${dd}`; }


  
}
