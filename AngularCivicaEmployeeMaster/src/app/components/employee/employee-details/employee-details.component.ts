import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Employee } from 'src/app/models/employee.model';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-employee-details',
  templateUrl: './employee-details.component.html',
  styleUrls: ['./employee-details.component.css']
})
export class EmployeeDetailsComponent implements OnInit{

  employeeId : number|undefined;
  employee : Employee={
    id:1,
    employeeEmail:'',
    departmentId:1,
    firstName:'',
    lastName:'',
    gender:'',
    basicSalary:10000,
    hra:100,
    allowance:100,
    grossSalary:100,
    pfDeduction:100,
    profTax:100,
    grossDeductions:100,
    totalSalary:1000,
    dateOfJoining:'',
    employeeDepartment:{
      departmentId:1,
      departmentName:'',
    }
  }

  constructor(private employeeService:EmployeeService,private route:ActivatedRoute){}

  ngOnInit(): void {
   this.route.params.subscribe((params)=>{
    this.employeeId = params['employeeId'];
    this.loadEmployeeDetails(this.employeeId);
   }); 
  }

  loadEmployeeDetails(employeeId:number|undefined){
    this.employeeService.getEmployeeById(employeeId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.employee = response.data;
          console.log(this.employee);
          
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

}
