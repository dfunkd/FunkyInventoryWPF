namespace FunkyInventoryWPF.Models.UserModels;

public class AddUserRequest
{
    public Guid RoleId { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string EncryptedPassword { get; set; }
}
