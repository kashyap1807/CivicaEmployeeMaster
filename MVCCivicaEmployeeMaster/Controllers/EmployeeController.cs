using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCivicaEmployeeMaster.Infrastructure;
using MVCCivicaEmployeeMaster.ViewModels;
using Newtonsoft.Json;

namespace MVCCivicaEmployeeMaster.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpClientService _httpClientService;

        private readonly IConfiguration _configuration;

        private string endPoint;

        public EmployeeController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }




        public IActionResult Index(string? search = null, int page = 1 , int pageSize = 4, string sortOrder="asc" )
        {
            if (!string.IsNullOrEmpty(search) && search.Length < 3)
            {
                ModelState.AddModelError("", "Search query must be at least 3 characters long.");

                // Prepare view data
                ViewBag.search = search;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.sortOrder = sortOrder;
                ViewBag.TotalPages = 0; // or set based on your logic
                ViewData["SearchError"] = "Search query must be at least 3 characters long.";

                return View(new List<EmployeeViewModel>()); // Return an empty list or appropriate model
            }
            var apiUrl = $"{endPoint}Employee/GetAllEmployeesByPagination"
                + "?search=" + search
                + "&page=" + page
                + "&pageSize=" + pageSize
                + "&sortOrder=" + sortOrder;

            var totalCountApiUrl = $"{endPoint}Employee/GetEmployeeCount"
                + "?search=" + search;

            ServiceResponse<int> countResponse = new ServiceResponse<int>();
            ServiceResponse<IEnumerable<EmployeeViewModel>> response = new ServiceResponse<IEnumerable<EmployeeViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            countResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (totalCountApiUrl, HttpMethod.Get, HttpContext.Request);

            var totalCount = countResponse.Data;

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.search = search;
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            ViewBag.sortOrder = sortOrder;
            ViewBag.TotalPages = totalPages;

            if (response.Success)
            {

                return View(response.Data);
            }

            return View(new List<EmployeeViewModel>());
        }

        private IEnumerable<EmployeeDepartmentViewModel> GetDepartments()
        {
            var apiUrl = $"{endPoint}EmployeeDepartment/GetAllEmployeeDepartments";

            ServiceResponse<IEnumerable<EmployeeDepartmentViewModel>> response = new ServiceResponse<IEnumerable<EmployeeDepartmentViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeDepartmentViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Employee/GetEmployeeById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<EmployeeViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<EmployeeViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {

                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<EmployeeViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }

        }

        [Authorize]
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            IEnumerable<EmployeeDepartmentViewModel> departments = GetDepartments();
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AddEmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {


                var apiUrl = $"{endPoint}Employee/Create";
                var response = _httpClientService.PostHttpResponseMessage<AddEmployeeViewModel>(apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<AddEmployeeViewModel>>(data);

                    if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                    {
                        return View(serviceResponse.Data);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = serviceResponse?.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<AddEmployeeViewModel>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                    return RedirectToAction("Create");
                }
            }

            IEnumerable<EmployeeDepartmentViewModel> departments = GetDepartments();
            ViewBag.Departments = departments;
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Employee/GetEmployeeById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateEmployeeViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateEmployeeViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    IEnumerable<EmployeeDepartmentViewModel> departments = GetDepartments();
                    ViewBag.Departments = departments;
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateEmployeeViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Edit");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(UpdateEmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
               
                var apiUrl = $"{endPoint}Employee/UpdateEmployee";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                }
            }

            IEnumerable<EmployeeDepartmentViewModel> departments = GetDepartments();
            ViewBag.Departments = departments;
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Employee/Delete/" + id;
            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);
            if (response.Success)
            {
                TempData["successMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["errorMessage"] = response.Message;
                return RedirectToAction("Index");
            }

        }



    }
}
