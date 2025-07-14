namespace DapperAuthApi.Models
{
    public class SaveResult
    {
        public int Status { get; set; } 
        public string? Message { get; set; }

        public static SaveResult FromStatus(int resultCode)
        {
            return new SaveResult
            {
                Status = resultCode,
                Message = resultCode switch
                {
                    < 0 => "An error occurred during the operation.",
                    0 => "Failed to Save",
                    >= 1 =>"Saved successfully",
                }
            };
        }
    }
}
