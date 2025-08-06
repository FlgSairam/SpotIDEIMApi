// Model
namespace DapperAuthApi.Models
{
    public class UserCustomerMapping
    {
        public long pid { get; set; }
        public long UserId { get; set; }
        public long CustomerId { get; set; }
        public byte record_status { get; set; } = 1;
        public string? created_by { get; set; }
        public DateTime created_date { get; set; }
        public string? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }

    public class UserCustomerMapRequest
    {
        public long UserId { get; set; }
        public List<long> CustomerIds { get; set; } = new();
        public string? ModifiedBy { get; set; } // used for created_by or updated_by
    }
}