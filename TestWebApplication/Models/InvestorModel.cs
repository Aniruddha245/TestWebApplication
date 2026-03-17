namespace TestWebApplication.Models
{
    public class InvestorModel
    {
        public int InvestorId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public decimal? LandArea { get; set; }
        public string? LandUnit { get; set; }
        public string? District { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
