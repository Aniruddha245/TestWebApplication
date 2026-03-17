namespace TestWebApplication.Models
{
    public class UserMasterEntity
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Designation { get; set; }

        public bool IsActive { get; set; } = true;
    }
    public class Login
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
