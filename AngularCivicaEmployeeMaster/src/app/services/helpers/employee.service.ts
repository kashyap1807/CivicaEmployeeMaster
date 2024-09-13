import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddEmployee } from 'src/app/models/addEmployee.model';
import { Employee } from 'src/app/models/employee.model';
import { SalaryHeadTotal } from 'src/app/models/salaryHeadTotal';
import { UpdateEmployee } from 'src/app/models/update-employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl='http://localhost:5165/api/Employee/';

  constructor(private http:HttpClient) { }

  getAllEmployeeWithPagination(pageNumber: number,pageSize:number,sort:string,search:string) : Observable<ApiResponse<Employee[]>>{
    return this.http.get<ApiResponse<Employee[]>>(this.apiUrl+'GetAllEmployeesByPagination?search='+search+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort);
  }

  getAllEmployeesCount(search:string) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'GetEmployeeCount?search='+search);
  }

  addEmployee(employee: AddEmployee): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl + 'Create', employee);
  }


  getEmployeeById(employeeId:number|undefined):Observable<ApiResponse<Employee>>{
    return this.http.get<ApiResponse<Employee>>(this.apiUrl+"GetEmployeeById/"+employeeId)
  }

  UpdateEmployee(updateEmployee:UpdateEmployee):Observable<ApiResponse<string>>{
    return this.http.put<ApiResponse<string>>(this.apiUrl+"UpdateEmployee",updateEmployee);
  }

  deleteEmployeeById(employeeId: number | undefined): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(this.apiUrl + 'Delete/' + employeeId);
  }

  getTotalSalaryByMonth(month:number,year:number): Observable<ApiResponse<SalaryHeadTotal[]>>{
    return this.http.get<ApiResponse<SalaryHeadTotal[]>>(this.apiUrl+'GetTotalSalaryByMonth?month='+month+'&year='+year)
  }

  getTotalSalaryByYear(year:number): Observable<ApiResponse<SalaryHeadTotal[]>>{
    return this.http.get<ApiResponse<SalaryHeadTotal[]>>(this.apiUrl+'GetTotalSalaryByYear?year='+year)
  }

  getProftaxByMonth(month:number,year:number):Observable<ApiResponse<SalaryHeadTotal[]>>{
    return this.http.get<ApiResponse<SalaryHeadTotal[]>>(this.apiUrl+'GetTotalProfTaxByMonth?month='+month+'&year='+year)
  }

  getYearlySalaryOfEachEmployee(employeeId:number,year:number): Observable<ApiResponse<SalaryHeadTotal[]>>{
    return this.http.get<ApiResponse<SalaryHeadTotal[]>>(this.apiUrl+'GetTotalSalaryByYearAndId/'+employeeId+','+year)
  }
  getMonthlyProfTaxOfEachEmployee(employeeId:number|undefined,year:number|undefined,month:number|undefined): Observable<ApiResponse<SalaryHeadTotal[]>>{
    return this.http.get<ApiResponse<SalaryHeadTotal[]>>(this.apiUrl+'GetTotalSalaryByMonthYearAndId/'+employeeId+','+month+','+year)
  }



}
