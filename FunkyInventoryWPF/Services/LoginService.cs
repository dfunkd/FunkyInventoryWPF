using FunkyInventoryWPF.Data.Repositories;
using FunkyInventoryWPF.Models.UserModels;

namespace FunkyInventoryWPF.Services;

public interface ILoginService
{
    Task<User?> Login(string userName, string password, CancellationToken cancellationToken = default);
    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default);
}

public class LoginService(ILoginRepository repo) : ILoginService
{
    public async Task<User?> Login(string userName, string password, CancellationToken cancellationToken = default)
        => await repo.Login(userName, password, cancellationToken);

    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default)
        => await repo.GetUserById(userId, cancellationToken);
}
