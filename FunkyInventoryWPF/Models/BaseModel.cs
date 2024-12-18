using System.Data;

namespace FunkyInventoryWPF.Models;

public class BaseModel
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}
