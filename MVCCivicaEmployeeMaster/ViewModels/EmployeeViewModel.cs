namespace MVCCivicaEmployeeMaster.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public int DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal Allowance { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal PfDeduction { get; set; }
        public decimal ProfTax { get; set; }
        public decimal GrossDeductions { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime DateOfJoining { get; set; }

        public EmployeeDepartmentViewModel EmployeeDepartment { get; set; }
    }
}
