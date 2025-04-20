using Api.Models;

namespace Api.Dtos.Paycheck
{
    public class GetPaycheckDto
    {
        public DateTime Date { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmmount {  get; set; }
        public ICollection<Deduction> Deductions { get; set; } = new List<Deduction>();
        public decimal DeductedAmount {  get; set; }
    }
}
