import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { EmployeeDepartment } from '../models/employeeDepartment.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeDepartmentService {
  
  private apiUrl = 'http://localhost:5165/api/EmployeeDepartment/';

  constructor(private http:HttpClient) {}

  getAllDepartments():Observable<ApiResponse<EmployeeDepartment[]>>{
    return this.http.get<ApiResponse<EmployeeDepartment[]>>(this.apiUrl+"GetAllEmployeeDepartments");
  }
}
