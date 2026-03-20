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


    public class ExternalApiSettings
    {
        public string WorkflowCallApi { get; set; } = string.Empty;
       
    }
    public class FormDetails
    {
        public string? ReferenceKey { get; set; }
        public string? FormId { get; set; }
        public Dictionary<string, object?> Context { get; set; } = new();
        public string? UsedTypes  {  get; set; }

    }
}

