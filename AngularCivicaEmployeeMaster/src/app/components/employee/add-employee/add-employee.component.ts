import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { EmployeeDepartment } from 'src/app/models/employeeDepartment.model';
import { EmployeeDepartmentService } from 'src/app/services/employee-department.service';
import { EmployeeService } from 'src/app/services/helpers/employee.service';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent {
  
  loading:boolean=false;
  employeeDepartment : EmployeeDepartment[] =[];
  employeeForm!: FormGroup;
  
  constructor(
    private employeeDepartmentService : EmployeeDepartmentService,
    private employeeService : EmployeeService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit():void{
    this.employeeForm = this.fb.group({
      firstName : ['',[Validators.required, Validators.minLength(3)]],
      lastName: ['',[Validators.required, Validators.minLength(3)]],
      employeeEmail: ['',[Validators.required, Validators.email]],
      departmentId : [null, [Validators.required, this.employeeValidator]],
      gender: [, Validators.required],
      dateofjoining: ['',[Validators.required,this.validateJoiningDate]],
      basicSalary:[0, [Validators.required, Validators.min(0)]],
      allowance:[0, [Validators.required, Validators.min(0)]]
     
    })
    this.loadDepartments();

  }

  get formControl(){
    return this.employeeForm.controls;
  }
   

  employeeValidator(control: any){
    return control.value ==''? {invalidEmploeey:true}:null;
  } 


  validateJoiningDate(control: AbstractControl): ValidationErrors | null {
    const selectedDate = new Date(control.value);
    const currentDate = new Date();
    const minDate = new Date('2001-01-01');
    const maxDate = new Date(); // Current date
  
    // Set hours, minutes, seconds, and milliseconds to 0 to compare only the date part
    selectedDate.setHours(0, 0, 0, 0);
    currentDate.setHours(0, 0, 0, 0);
  
    if (selectedDate < minDate || selectedDate > currentDate) {
     
      return { invalidBirthDate: true };
    }
    return null;
  } 

  loadDepartments():void{
    this.loading = true;
    this.employeeDepartmentService.getAllDepartments().subscribe({
      next:(response: ApiResponse<EmployeeDepartment[]>) =>{
        if(response.success){
          this.employeeDepartment = response.data;
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


  OnSubmit(){
    this.loading = true;

    if(this.employeeForm.valid){
    
      console.log(this.employeeForm.value);
      this.employeeService.addEmployee(this.employeeForm.value).subscribe({
        next: (response) => {
          if (response.success) {
            console.log("Employee created successfully:", response);
            alert(response.message)
            this.router.navigate(['/employee']);
          }
          else{
            
            alert(response.message)
          }
        },
        error:(err)=>{
          alert(err.error.message);
          this.loading = false;

        },
        complete:()=>{
          console.log("Completed");
          this.loading = false;

        }
      })
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
