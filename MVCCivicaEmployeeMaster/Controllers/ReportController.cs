using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCCivicaEmployeeMaster.Infrastructure;
using MVCCivicaEmployeeMaster.ViewModel;
using MVCCivicaEmployeeMaster.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Reflection;
namespace MVCCivicaEmployeeMaster.Controllers
{
    public class ReportController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;


        private string endPoint;

        public ReportController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];

        }

        public IActionResult YearlySalaryReport()
        {
            var model = new SalaryHeadTotal();
            model.Year = DateTime.Now.Year; // Set default year to current year

            // Adjust the range for the dropdown to include years from 2001 to 2024
            ViewBag.YearList = Enumerable.Range(2001, 2024).ToList();

            return View(model);
        }


        // POST: /Salary/YearlySalaryReport
        [HttpPost]
        public IActionResult YearlySalaryReport(int year)
        {
          
            var apiUrl = $"{endPoint}Employee/GetTotalSalaryByYear?year={year}";
            var response = _httpClientService.GetHttpResponseMessage<SalaryHeadTotal>(apiUrl, HttpContext.Request);


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(data);

                // Extract the 'data' array from the JSON object
                var dataArray = jsonObject["data"].ToObject<JArray>();

                // Deserialize 'data' array into a list of SalaryHeadTotal objects

                var serviceResponse = dataArray.ToObject<List<SalaryHeadTotal>>();

                if (serviceResponse != null && serviceResponse.Count > 0)
                {
                    decimal totalBasicSalary = serviceResponse.Sum(s => s.BasicSalary);
                    decimal totalHRA = serviceResponse.Sum(s => s.HRA);
                    decimal totalAllowance = serviceResponse.Sum(s => s.Allowance);
                    decimal totalGrossSalary = serviceResponse.Sum(s => s.GrossSalary);
                    decimal totalPfDeduction = serviceResponse.Sum(s => s.PfDeduction);
                    decimal totalProfTax = serviceResponse.Sum(s => s.ProfTax);
                    decimal totalGrossDeductions = serviceResponse.Sum(s => s.GrossDeductions);
                    decimal totalTotalSalary = serviceResponse.Sum(s => s.TotalSalary);


                    var model = new SalaryHeadTotal
                    {
                        BasicSalary = totalBasicSalary,
                        HRA = totalHRA,
                        Allowance = totalAllowance,
                        GrossSalary = totalGrossSalary,
                        PfDeduction = totalPfDeduction,
                        ProfTax = totalProfTax,
                        GrossDeductions = totalGrossDeductions,
                        TotalSalary = totalTotalSalary
                    };
                    return View("YearlySalaryReport", model);
                }
                return View("YearlySalaryReport");


            }
            else
            {
                TempData["ErrorMessage"] = "No data available for this year.";
                return RedirectToAction("YearlySalaryReport");
            }

        }


        public IActionResult MonthalyProfTaxReport()
        {
            var model = new SalaryHeadTotal();
            model.Year = DateTime.Now.Year; // Set default year to current year

            // Prepare a list to hold year and month combinations
            List<SelectListItem> yearMonthList = new List<SelectListItem>();

            // Loop through each year from 2001 to the current year
            for (int year = 2001; year <= DateTime.Now.Year; year++)
            {
                // Loop through each month from 1 to 12
                for (int month = 1; month <= 12; month++)
                {
                    // Create a SelectListItem for each month in each year
                    SelectListItem item = new SelectListItem
                    {
                        Value = $"{year}-{month.ToString("00")}", // Format: "yyyy-MM" (e.g., "2024-07")
                        Text = $"{year} - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}"
                    };
                    yearMonthList.Add(item);
                }
            }

            // Assign the list to ViewBag
            ViewBag.YearMonthList = yearMonthList;

            return View(model);
        }

        // POST: /Salary/YearlySalaryReport
        [HttpPost]
        public IActionResult MonthalyProfTaxReport(int year, int month)
        {
           
            if(year==DateTime.Now.Year && month > DateTime.Now.Month)
            {
                TempData["ErrorMessage"] = "Please select past month.";
                var emptyModel = new SalaryHeadTotal(); // Or fetch an appropriate empty model
                return View(emptyModel);
            }
            if (year < 2000 || month < 0)
            {
                TempData["ErrorMessage"] = "Please select both year and month.";
                var emptyModel = new SalaryHeadTotal(); // Or fetch an appropriate empty model
                return View(emptyModel);
            }
            var apiUrl = $"{endPoint}Employee/GetTotalSalaryByMonth?month={month}&year={year}";
            var response = _httpClientService.GetHttpResponseMessage<SalaryHeadTotal>(apiUrl, HttpContext.Request);


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(data);

                // Extract the 'data' array from the JSON object
                var dataArray = jsonObject["data"].ToObject<JArray>();

                // Deserialize 'data' array into a list of SalaryHeadTotal objects

                var serviceResponse = dataArray.ToObject<List<SalaryHeadTotal>>();

                if (serviceResponse != null && serviceResponse.Count > 0)
                {

                    decimal totalProfTax = serviceResponse.Sum(s => s.ProfTax);



                    var model = new SalaryHeadTotal
                    {

                        ProfTax = totalProfTax,

                    };
                    return View("MonthalyProfTaxReport", model);
                }
                return View("MonthalyProfTaxReport");


            }
            else
            {
                TempData["ErrorMessage"] = "No data available for this year.";
                return RedirectToAction("MonthalyProfTaxReport");
            }

        }


        public IActionResult YearlyEachEmpSalaryReport(int id)
        {
            var model1 = new SalaryHeadTotal();
            model1.Year = DateTime.Now.Year; // Set default year to current year

            // Adjust the range for the dropdown to include years from 2001 to 2024
            ViewBag.YearList = Enumerable.Range(2001, 2024).ToList();
            ViewBag.empId = id;
            //model1.Id = id;

            return View(model1);
        }



        // POST: /Salary/YearlyEachEmpSalaryReport
        [HttpPost]
        public IActionResult YearlyEachEmpSalaryReport(int year, int id)
        {
           var apiUrl = $"{endPoint}Employee/GetTotalSalaryByYearAndId/{id},{year}";
                var response = _httpClientService.GetHttpResponseMessage<SalaryHeadTotal>(apiUrl, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(data);

                    // Extract the 'data' array from the JSON object
                    var dataArray = jsonObject["data"].ToObject<JArray>();

                    // Deserialize 'data' array into a list of SalaryHeadTotal objects
                    var serviceResponse = dataArray.ToObject<List<SalaryHeadTotal>>();

                    if (serviceResponse != null && serviceResponse.Count > 0)
                    {
                        decimal totalBasicSalary = serviceResponse.Sum(s => s.BasicSalary);
                        decimal totalHRA = serviceResponse.Sum(s => s.HRA);
                        decimal totalAllowance = serviceResponse.Sum(s => s.Allowance);
                        decimal totalGrossSalary = serviceResponse.Sum(s => s.GrossSalary);
                        decimal totalPfDeduction = serviceResponse.Sum(s => s.PfDeduction);
                        decimal totalProfTax = serviceResponse.Sum(s => s.ProfTax);
                        decimal totalGrossDeductions = serviceResponse.Sum(s => s.GrossDeductions);
                        decimal totalTotalSalary = serviceResponse.Sum(s => s.TotalSalary);

                        var model = new SalaryHeadTotal
                        {
                            BasicSalary = totalBasicSalary,
                            HRA = totalHRA,
                            Allowance = totalAllowance,
                            GrossSalary = totalGrossSalary,
                            PfDeduction = totalPfDeduction,
                            ProfTax = totalProfTax,
                            GrossDeductions = totalGrossDeductions,
                            TotalSalary = totalTotalSalary
                        };
                        return View("YearlyEachEmpSalaryReport", model);
                    }
                    return View("YearlyEachEmpSalaryReport");
                }
                else
                {
                    TempData["ErrorMessage"] = "No data available for this year and employee.";
                    return RedirectToAction("YearlyEachEmpSalaryReport");
                }
            
            
        }
        public IActionResult MonthlyEachEmpSalaryReport(int id)
        {
            var model1 = new SalaryHeadTotal();
            model1.Year = DateTime.Now.Year; // Set default year to current year
            List<SelectListItem> yearMonthList = new List<SelectListItem>();

            // Loop through each year from 2001 to the current year
            for (int year = 2001; year <= DateTime.Now.Year; year++)
            {
                // Loop through each month from 1 to 12
                for (int month = 1; month <= 12; month++)
                {
                    // Create a SelectListItem for each month in each year
                    SelectListItem item = new SelectListItem
                    {
                        Value = $"{year}-{month.ToString("00")}", // Format: "yyyy-MM" (e.g., "2024-07")
                        Text = $"{year} - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}"
                    };
                    yearMonthList.Add(item);
                }
            }

            // Adjust the range for the dropdown to include years from 2001 to 2024
            ViewBag.YearList = Enumerable.Range(2001, 2024).ToList();
            ViewBag.empId = id;
            //model1.Id = id;

            return View(model1);
        }



        // POST: /Salary/MonthlyEachEmpSalaryReport
        [HttpPost]
        public IActionResult MonthlyEachEmpSalaryReport(int month, int year, int id)
        {
            if (year > 2000 || month > 0)
            {
                if (year == DateTime.Now.Year && month > DateTime.Now.Month)
                {
                    TempData["ErrorMessage"] = "Please select past month.";
                    var emptyModel = new SalaryHeadTotal(); // Or fetch an appropriate empty model
                    return View(emptyModel);
                }
                if (year == null || month == null)
                {
                    TempData["ErrorMessage"] = "Please select both year and month.";
                    var emptyModel = new SalaryHeadTotal(); // Or fetch an appropriate empty model
                    return View(emptyModel);
                }
                var apiUrl = $"{endPoint}Employee/GetTotalSalaryByMonthYearAndId/{id},{month},{year}";
                var response = _httpClientService.GetHttpResponseMessage<SalaryHeadTotal>(apiUrl, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(data);

                    // Extract the 'data' array from the JSON object
                    var dataArray = jsonObject["data"].ToObject<JArray>();

                    // Deserialize 'data' array into a list of SalaryHeadTotal objects
                    var serviceResponse = dataArray.ToObject<List<SalaryHeadTotal>>();

                    if (serviceResponse != null && serviceResponse.Count > 0)
                    {
                        decimal totalBasicSalary = serviceResponse.Sum(s => s.BasicSalary);
                        decimal totalHRA = serviceResponse.Sum(s => s.HRA);
                        decimal totalAllowance = serviceResponse.Sum(s => s.Allowance);
                        decimal totalGrossSalary = serviceResponse.Sum(s => s.GrossSalary);
                        decimal totalPfDeduction = serviceResponse.Sum(s => s.PfDeduction);
                        decimal totalProfTax = serviceResponse.Sum(s => s.ProfTax);
                        decimal totalGrossDeductions = serviceResponse.Sum(s => s.GrossDeductions);
                        decimal totalTotalSalary = serviceResponse.Sum(s => s.TotalSalary);

                        var model = new SalaryHeadTotal
                        {
                            BasicSalary = totalBasicSalary,
                            HRA = totalHRA,
                            Allowance = totalAllowance,
                            GrossSalary = totalGrossSalary,
                            PfDeduction = totalPfDeduction,
                            ProfTax = totalProfTax,
                            GrossDeductions = totalGrossDeductions,
                            TotalSalary = totalTotalSalary
                        };
                        return View("MonthlyEachEmpSalaryReport", model);
                    }
                    return View("MonthlyEachEmpSalaryReport");
                }
                else
                {
                    TempData["ErrorMessage"] = "No data available for this year,month and employee.";
                    return RedirectToAction("MonthlyEachEmpSalaryReport");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please select month.";
                return RedirectToAction("MonthlyEachEmpSalaryReport");
            }
        }
        [HttpGet]
        public IActionResult MonthlySalaryReport()
        {
            var model = new SalaryHeadTotal();
            model.Year = DateTime.Now.Year; // Set default year to current year

            // Prepare a list to hold year and month combinations
            List<SelectListItem> yearMonthList = new List<SelectListItem>();

            // Loop through each year from 2001 to the current year
            for (int year = 2001; year <= DateTime.Now.Year; year++)
            {
                // Loop through each month from 1 to 12
                for (int month = 1; month <= 12; month++)
                {
                    // Create a SelectListItem for each month in each year
                    SelectListItem item = new SelectListItem
                    {
                        Value = $"{year}-{month.ToString("00")}", // Format: "yyyy-MM" (e.g., "2024-07")
                        Text = $"{year} - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}"
                    };
                    yearMonthList.Add(item);
                }
            }

            // Assign the list to ViewBag
            ViewBag.YearMonthList = yearMonthList;

            return View(model);
        }


        // POST: /Salary/MonthlySalaryReport
        [HttpPost]
        public IActionResult MonthlySalaryReport(int month, int year)
        {
            
            if (year == DateTime.Now.Year && month > DateTime.Now.Month)
            {
                TempData["ErrorMessage"] = "Please select past month.";
                var emptyModel = new SalaryHeadTotal(); // Or fetch an appropriate empty model
                return View(emptyModel);
            }
            if (year == null || month == null)
            {
                TempData["ErrorMessage"] = "Please select both year and month.";
                var emptyModel = new SalaryHeadTotal(); // Or fetch an appropriate empty model
                return View(emptyModel);
            }
            var apiUrl = $"{endPoint}Employee/GetTotalSalaryByMonth?month={month}&year={year}";
            var response = _httpClientService.GetHttpResponseMessage<SalaryHeadTotal>(apiUrl, HttpContext.Request);


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(data);

                // Extract the 'data' array from the JSON object
                var dataArray = jsonObject["data"].ToObject<JArray>();

                // Deserialize 'data' array into a list of SalaryHeadTotal objects

                var serviceResponse = dataArray.ToObject<List<SalaryHeadTotal>>();

                if (serviceResponse != null && serviceResponse.Count > 0)
                {
                    decimal totalBasicSalary = serviceResponse.Sum(s => s.BasicSalary);
                    decimal totalHRA = serviceResponse.Sum(s => s.HRA);
                    decimal totalAllowance = serviceResponse.Sum(s => s.Allowance);
                    decimal totalGrossSalary = serviceResponse.Sum(s => s.GrossSalary);
                    decimal totalPfDeduction = serviceResponse.Sum(s => s.PfDeduction);
                    decimal totalProfTax = serviceResponse.Sum(s => s.ProfTax);
                    decimal totalGrossDeductions = serviceResponse.Sum(s => s.GrossDeductions);
                    decimal totalTotalSalary = serviceResponse.Sum(s => s.TotalSalary);


                    var model = new SalaryHeadTotal
                    {
                        BasicSalary = totalBasicSalary,
                        HRA = totalHRA,
                        Allowance = totalAllowance,
                        GrossSalary = totalGrossSalary,
                        PfDeduction = totalPfDeduction,
                        ProfTax = totalProfTax,
                        GrossDeductions = totalGrossDeductions,
                        TotalSalary = totalTotalSalary
                    };
                    return View("MonthlySalaryReport", model);
                }
                return View("MonthlySalaryReport");


            }
            else
            {
                TempData["ErrorMessage"] = "No data available for this year.";
                return RedirectToAction("MonthlySalaryReport");
            }

        }


        public IActionResult EachEmployeeGrid(string? search = null, int page = 1, int pageSize = 4, string sortOrder = "asc")
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
        public IActionResult Load(string contentType) // Change parameter name here as well
        {
            // Logic to load appropriate partial view based on 'contentType'
            if (contentType == "eachEmployee")
            {
                return PartialView("_EachEmployeeView");
            }
            else if (contentType == "allEmployee")
            {
                return PartialView("_AllEmployeesView");
            }
            else
            {
                return NotFound(); // Handle invalid contentType scenario
            }
        }
    }
}
