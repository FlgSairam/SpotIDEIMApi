namespace DapperAuthApi.Models
{
    public class AppUser
    {
        public long Pid { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public bool Is_Active { get; set; }
        public int Record_Status { get; set; } = 1;
        public string Created_By { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public string? Updated_By { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
