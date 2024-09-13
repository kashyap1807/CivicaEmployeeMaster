import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EmployeeDepartmentService } from './employee-department.service';
import { EmployeeDepartment } from '../models/employeeDepartment.model';
import { ApiResponse } from '../models/ApiResponse{T}'; // Ensure this path is correct based on your application

describe('EmployeeDepartmentService', () => {
  let service: EmployeeDepartmentService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EmployeeDepartmentService]
    });

    // Inject the service (subject under test) and the Mock backend
    service = TestBed.inject(EmployeeDepartmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // After each test, verify that there are no outstanding HTTP requests
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  
    it('should return an Observable<ApiResponse<EmployeeDepartment[]>>', () => {
      const dummyDepartments: EmployeeDepartment[] = [
        { departmentId: 1, departmentName: 'Department 1' },
        { departmentId: 2, departmentName: 'Department 2' }
      ];

      const dummyApiResponse: ApiResponse<EmployeeDepartment[]> = {
        data: dummyDepartments,
        message: 'Departments fetched successfully',
        success: true
      };

      service.getAllDepartments().subscribe(response => {
        expect(response).toEqual(dummyApiResponse);
      });

      const req = httpMock.expectOne('http://localhost:5165/api/EmployeeDepartment/GetAllEmployeeDepartments');
      expect(req.request.method).toBe('GET');
      req.flush(dummyApiResponse);
    });

});
