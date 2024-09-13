import { EmployeeDepartment } from "./employeeDepartment.model";

export interface AddEmployee{
    
    employeeEmail:string;
    departmentId:number;
    firstName:string;
    lastName:string;
    gender:string;
    basicSalary:number;
    allowance:number;
    dateofjoining : string;
    employeeDepartment:EmployeeDepartment
    
    
    
   
}