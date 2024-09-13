import { EmployeeDepartment } from "./employeeDepartment.model";

export interface Employee{
    id:number;
    employeeEmail:string;
    departmentId:number;
    firstName:string;
    lastName:string;
    gender:string;
    basicSalary:number;
    hra:number;
    allowance:number;
    grossSalary:number;
    pfDeduction:number;
    profTax:number;
    grossDeductions:number;
    totalSalary:number;
    dateOfJoining:string;
    employeeDepartment:EmployeeDepartment
}