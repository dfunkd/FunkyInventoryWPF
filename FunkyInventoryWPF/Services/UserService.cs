using FunkyInventoryWPF.Data.Repositories;
using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.Models.UserModels.Requests;

namespace FunkyInventoryWPF.Services;

public interface IUserService
{
    Task<User?> AddUser(AddUserRequest req, CancellationToken cancellationToken = default);
    Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> UpdatePassword(Guid userId, string password, string encrypted, CancellationToken cancellationToken = default);
    Task<User?> UpdateUser(Guid userId, UpdateUserRequest req, CancellationToken cancellationToken = default);
    Task<bool> UserNameOrEmailExists(string email, string username, CancellationToken cancellationToken = default);
}

public class UserService(IUserRepository repo) : IUserService
{
    public async Task<User?> AddUser(AddUserRequest req, CancellationToken cancellationToken = default)
        => await repo.AddUser(req, cancellationToken);

    public async Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default)
        => await repo.DeleteUser(userId, cancellationToken);

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
        => await repo.GetAllUsers(cancellationToken);

    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default)
        => await repo.GetUserById(userId, cancellationToken);

    public async Task<User?> UpdatePassword(Guid userId, string password, string encrypted, CancellationToken cancellationToken = default)
        => await repo.UpdatePassword(userId, password, encrypted, cancellationToken);

    public async Task<User?> UpdateUser(Guid userId, UpdateUserRequest req, CancellationToken cancellationToken = default)
        => await repo.UpdateUser(userId, req, cancellationToken);

    public async Task<bool> UserNameOrEmailExists(string email, string username, CancellationToken cancellationToken = default)
        => await repo.UserNameOrEmailExists(email, username, cancellationToken);
}
