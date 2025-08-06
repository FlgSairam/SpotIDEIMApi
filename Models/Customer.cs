namespace DapperAuthApi.Models;

public class Customer
{
    public long Pid { get; set; }
    public string? Consumer_Name { get; set; } 
    public string? Short_Name { get; set; }
    public string? Consumer_Address { get; set; }
    public string? Contact_Person_Name { get; set; }
    public string? Contact_No { get; set; }
    public string? Email { get; set; }
     public string? Website { get; set; }
    public string? Contract_Period { get; set; }
    public int Record_Status { get; set; }
    public string? Created_By { get; set; }
    public DateTime Created_Date { get; set; }
    public string? Updated_By { get; set; }
    public DateTime? Updated_Date { get; set; }
}
 