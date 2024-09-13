using CivicaEmployeeMaster.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivicaEmployeeMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDepartmentController : ControllerBase
    {
        private readonly IEmployeeDepartmentService _employeeDepartmentService;
        public EmployeeDepartmentController(IEmployeeDepartmentService employeeDepartmentService)
        {
            _employeeDepartmentService = employeeDepartmentService;
        }
        [HttpGet("GetAllEmployeeDepartments")]
        public IActionResult GetAllEmployeeDepartments()
        {
            var response = _employeeDepartmentService.GetEmployeeDepartment();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
