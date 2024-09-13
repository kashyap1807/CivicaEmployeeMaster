import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EmployeeService } from './employee.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddEmployee } from 'src/app/models/addEmployee.model';
import { Employee } from 'src/app/models/employee.model';
import { UpdateEmployee } from 'src/app/models/update-employee.model';


describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;
  const mockApiResponse: ApiResponse<Employee[]> = {
    success: true,
    data: [
      {
        id: 1,
        firstName: 'Employee 1',
        lastName: 'emp1',
        employeeEmail:'emp@gmail.com',
        departmentId:1,
        
        gender:'M',
        basicSalary:1000000,
        allowance:1000,
        hra:20,
        pfDeduction:29,
        profTax:32,
        grossDeductions:200,
        totalSalary:198789,
        grossSalary: 678,
        dateOfJoining:'',
        employeeDepartment:{
          departmentId: 1,
          departmentName: 'Software'
        }

      },
      {
        id: 1,
        firstName: 'Employee 1',
        lastName: 'emp1',
        employeeEmail:'emp@gmail.com',
        departmentId:1,
        dateOfJoining:'',
        gender:'M',
        basicSalary:1000000,
        allowance:1000,
        hra:20,
        pfDeduction:29,
        profTax:32,
        grossDeductions:200,
        totalSalary:198789,
        grossSalary: 678,
        employeeDepartment:{
          departmentId: 1,
          departmentName: 'Software'
        }
      }
    ],
    message: ''
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EmployeeService]});

    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all paginated employee  and search',()=>{
    //Arrange
    const page = 1;
    const pageSize = 2;
    const sortOrder = "asc";
   
    const search = "yes"
   

     //act
     service.getAllEmployeeWithPagination(page,pageSize,sortOrder,search).subscribe(response =>{
      //assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);

    });
    const req =httpMock.expectOne('http://localhost:5165/api/Employee/GetAllEmployeesByPagination?search='+search+'&page='+page+'&pageSize='+pageSize+'&sortOrder='+sortOrder);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });

  it('should handle empty list of paginated employee and search',()=>{
    //Arrange
    const page = 1;
    const pageSize = 2;
    const sortOrder = "asc";
    
    const search = "yes"

    const emptyResponse: ApiResponse<Employee[]> = {
      success: true,
      data: [],
      message: ''
    }
   

     //act
     service.getAllEmployeeWithPagination(page,pageSize,sortOrder,search).subscribe(response =>{
      //assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);

    });
    const req =httpMock.expectOne('http://localhost:5165/api/Employee/GetAllEmployeesByPagination?search='+search+'&page='+page+'&pageSize='+pageSize+'&sortOrder='+sortOrder);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);

  });

  it('should handle HTTP error while fetching paginated employee and search',()=>{
    //Arrange
    const page = 1;
    const pageSize = 2;
    const sortOrder = "asc";
   
    const search = "yes"
    const mockHttpError ={
      statusText: "Internal Server Error",
      status: 500
      };
    const ApiUrl = 'http://localhost:5165/api/Employee/GetAllEmployeesByPagination?search='+search+'&page='+page+'&pageSize='+pageSize+'&sortOrder='+sortOrder
    
    //act
    service.getAllEmployeeWithPagination(page,pageSize,sortOrder,search).subscribe({
      next:()=> fail('should have failed with the 500 error'),
      error: (error=> {
       expect(error.status).toEqual(500);
       expect(error.statusText).toEqual("Internal Server Error");
       
      })
   });
    const req =httpMock.expectOne(ApiUrl);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);

  });

  it('should get employee count with search',()=>{
    //Arrange
    
    const mockApiResponse = { data: 2 }; 
    const search = 'employee1'
    
     //act
     service.getAllEmployeesCount(search).subscribe(response =>{
      //assert
      expect(response.data).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);

    });
    const req =httpMock.expectOne('http://localhost:5165/api/Employee/GetEmployeeCount?search='+search);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });

  it('should handle when  employee count is zero ', () => {
    //Arrange
    const search = 'employee1'
    const apiUrl = 'http://localhost:5165/api/Employee/GetEmployeeCount?search='+search
     const mockApiResponse = { data: 0 }; 
    const emptyResponse: ApiResponse<number> = {
      success: true,
      data: 0,
      message: ''
    }
    //Act
    service.getAllEmployeesCount(search).subscribe((response) => {
      //Assert
      expect(response.data).toBe(0);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully while fetching employee count', () => {
    //Arrange
    const search = 'employee1'
    const apiUrl = 'http://localhost:5165/api/Employee/GetEmployeeCount?search='+search;
     const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllEmployeesCount(search).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });

  it('should add employee',()=>{
    const employee : AddEmployee ={
      firstName:'EMployee 1',
      lastName: 'employee1',
      employeeEmail: 'emp@gmail.com',
      employeeDepartment : {
        departmentId : 1,
        departmentName:'Software'
      },
      gender: 'M',
      basicSalary: 100000,
      allowance: 99999,
      dateofjoining:'',
      departmentId: 1
    }
     //Act
     const mockSuccessResponse :ApiResponse<string> ={
      success : true,
      message : "Employee saved successfully",
      data : ''
    };
    //act
    service.addEmployee(employee).subscribe(response=>{
      expect(response).toBe(mockSuccessResponse);
  });
  const req = httpMock.expectOne('http://localhost:5165/api/Employee/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  })

  it('should handle error while adding  employee',()=>{
    const employee : AddEmployee ={
      firstName:'EMployee 1',
      lastName: 'employee1',
      employeeEmail: 'emp@gmail.com',
      employeeDepartment : {
        departmentId : 1,
        departmentName:'Software'
      },
      gender: 'M',
      basicSalary: 100000,
      allowance: 99999,
      dateofjoining:'',
      departmentId: 1
    }
     //Act
     const mockErrorResponse : ApiResponse<string> ={
      success :false,
      message : 'Employee already exist',
      data : " "
    };
    //act
    service.addEmployee(employee).subscribe(response=>{
      expect(response).toBe(mockErrorResponse);
  });
  const req = httpMock.expectOne('http://localhost:5165/api/Employee/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  })


  it('should handle error while adding  employee',()=>{
    const employee : AddEmployee ={
      firstName:'EMployee 1',
      lastName: 'employee1',
      employeeEmail: 'emp@gmail.com',
      employeeDepartment : {
        departmentId : 1,
        departmentName:'Software'
      },
      gender: 'M',
      basicSalary: 100000,
      allowance: 99999,
      dateofjoining:'',
      departmentId: 1
    }
     //Act
     const mockHttpError ={
      statusText: "Internal Server Error",
      status: 500
      };
    //act
    service.addEmployee(employee).subscribe({
      next:()=> fail('should have failed with the 500 error'),
      error: (error=> {
       expect(error.status).toEqual(500);
       expect(error.statusText).toEqual("Internal Server Error");
      })
  });
  const req = httpMock.expectOne('http://localhost:5165/api/Employee/Create');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  })

  it('should fetch employee by id',()=>{
    const id = 1;
    const mockSuccessResponse :ApiResponse<Employee>={
      success :true,
      data :{
        id: 1,
        firstName: 'Employee 1',
        lastName: 'emp1',
        employeeEmail:'emp@gmail.com',
        departmentId:1,
        dateOfJoining:'',
        gender:'M',
        basicSalary:1000000,
        allowance:1000,
        hra:20,
        pfDeduction:29,
        profTax:32,
        grossDeductions:200,
        totalSalary:198789,
        grossSalary: 678,
        employeeDepartment:{
          departmentId: 1,
          departmentName: 'Software'
        }
      },
      message:''
    };
    //act
    service.getEmployeeById(id).subscribe(response =>{
      //assert
      expect(response).toBe(mockSuccessResponse);
      expect(response.data.id).toEqual(id);

    });
    const req =httpMock.expectOne('http://localhost:5165/api/Employee/GetEmployeeById/' +id);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);
  });


  it('should handle failed contact retrival',()=>{
    //arrange
    const id =1;
    const mockErrorResponse : ApiResponse<Employee>={
      success : false,
      data: {} as Employee,
      message : "No recoprd found"
    };
    //act
    service.getEmployeeById(id).subscribe(response => {
      //assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toEqual("No recoprd found");
      expect(response.success).toBeFalse();

    });
    const req =httpMock.expectOne('http://localhost:5165/api/Employee/GetEmployeeById/' +id);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

 });

 it('should handle http error',()=>{
  const id =1;
  const mockHttpError ={
    status: 500,
    statusText :'Internal server error'
  };
  //act
  service.getEmployeeById(id).subscribe({
    next : ()=> fail('should have faild with 500 error'),
    error:(error)=>{
      //assert
      expect(error.status).toBe(500);
      expect(error.statusText).toBe('Internal server error');
    }
  });
  const req =httpMock.expectOne('http://localhost:5165/api/Employee/GetEmployeeById/' +id);
  expect(req.request.method).toBe('GET');
  req.flush({},mockHttpError);


 });

 it('should delete the employee by id successfully',()=>{
  //arrange
   const id =1;
    const mockSuccessResponse :ApiResponse<string>={
      success : false,
      data: '',
      message : "deleted successfully"

    };
    service.deleteEmployeeById(id).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockSuccessResponse);
      expect(response.message).toBe("deleted successfully");
      expect(response.data).toEqual;(mockSuccessResponse.data);

    });
    const req = httpMock.expectOne("http://localhost:5165/api/Employee/Delete/"+ id);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);
 })

 it('should not delete the employee by id successfully',()=>{
  //arrange
   const id =1;
    const mockErrorResponse :ApiResponse<string>={
      success : false,
      data: '',
      message : " not deleted successfully"

    };
    service.deleteEmployeeById(id).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe(" not deleted successfully");
      expect(response.data).toEqual;(mockErrorResponse.data);

    });
    const req = httpMock.expectOne("http://localhost:5165/api/Employee/Delete/"+ id);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockErrorResponse);
 });

 it('should handle http error while deleting',()=>{
  const id =1;
  const mockHttpError ={
    status: 500,
    statusText :'Internal server error'
  };
  //act
  service.deleteEmployeeById(id).subscribe({
    next : ()=> fail('should have faild with 500 error'),
    error:(error)=>{
      //assert
      expect(error.status).toBe(500);
      expect(error.statusText).toBe('Internal server error');
    }
  });
  const req = httpMock.expectOne("http://localhost:5165/api/Employee/Delete/"+ id);
    expect(req.request.method).toBe('DELETE');
    req.flush({},mockHttpError);


 });

 it('should update a employee successfully', ()=>{
  const editContact : UpdateEmployee={
    id:1,
    firstName:'EMployee 1',
    lastName: 'employee1',
    employeeEmail: 'emp@gmail.com',
    dateOfJoining:'',
    gender: 'M',
    basicSalary: 100000,
    allowance: 99999,
    
    departmentId: 1
  }
 
  const mockSuccessResponse : ApiResponse<string> ={
    success: true,
    message: "Contact updated successfully.",
    data: ''
        }
 
  //Act
  service.UpdateEmployee(editContact).subscribe(response => {
    expect(response).toEqual(mockSuccessResponse);
  });
 
  const req = httpMock.expectOne( 'http://localhost:5165/api/Employee/UpdateEmployee');
  expect(req.request.method).toBe('PUT');
  req.flush(mockSuccessResponse);
 
  });

  it('should handle failed update of cotact', ()=>{
    const editContact : UpdateEmployee={
      id:1,
    firstName:'EMployee 1',
    lastName: 'employee1',
    employeeEmail: 'emp@gmail.com',
    dateOfJoining:'',
    gender: 'M',
    basicSalary: 100000,
    allowance: 99999,
    
    departmentId: 1
    }
   
    const mockErrorResponse : ApiResponse<string> ={
      success: false,
      message: "Contact already exists.",
      data:''
          }
   
    //Act
    service.UpdateEmployee(editContact).subscribe(response => {
      expect(response).toEqual(mockErrorResponse);
    });
   
    const req = httpMock.expectOne('http://localhost:5165/api/Employee/UpdateEmployee');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);
   
    });
    it('should handle http error for update', ()=>{
      const editContact : UpdateEmployee={
        id:1,
        firstName:'EMployee 1',
        lastName: 'employee1',
        employeeEmail: 'emp@gmail.com',
        dateOfJoining:'',
        gender: 'M',
        basicSalary: 100000,
        allowance: 99999,
        
        departmentId: 1
      }
     
      const mockHttpError ={
        statusText: "Internal Server Error",
        status: 500
        };
     
      //Act
      service.UpdateEmployee(editContact).subscribe({
       next:()=> fail('should have failed with the 500 error'),
       error: (error=> {
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual("Internal Server Error");
       })
    });
     
      const req = httpMock.expectOne('http://localhost:5165/api/Employee/UpdateEmployee');
      expect(req.request.method).toBe('PUT');
      req.flush({},mockHttpError);
     
      });









  
});
