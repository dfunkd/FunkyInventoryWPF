using FunkyInventoryWPF.Data.Repositories;
using FunkyInventoryWPF.Models.UserModels;

namespace FunkyInventoryWPF.Services;

public interface IRoleService
{
    Task<Role?> AddRole(AddRoleRequest role, CancellationToken cancellationToken = default);
    Task<bool> DeleteRole(Guid roleId, CancellationToken cancellationToken = default);
    Task<List<Role>> GetAllRoles(CancellationToken cancellationToken = default);
    Task<Role?> GetRoleById(Guid roleId, CancellationToken cancellationToken = default);
    Task<Role?> UpdateRole(Guid roleId, UpdateRoleRequest role, CancellationToken cancellationToken = default);
}

public class RoleService(IRoleRepository repo) : IRoleService
{
    public async Task<Role?> AddRole(AddRoleRequest role, CancellationToken cancellationToken = default)
        => await repo.AddRole(role, cancellationToken);

    public async Task<bool> DeleteRole(Guid roleId, CancellationToken cancellationToken = default)
        => await repo.DeleteRole(roleId, cancellationToken);

    public async Task<List<Role>> GetAllRoles(CancellationToken cancellationToken = default)
        => await repo.GetAllRoles(cancellationToken);

    public async Task<Role?> GetRoleById(Guid roleId, CancellationToken cancellationToken = default)
        => await repo.GetRoleById(roleId, cancellationToken);

    public async Task<Role?> UpdateRole(Guid roleId, UpdateRoleRequest role, CancellationToken cancellationToken = default)
        => await repo.UpdateRole(roleId, role, cancellationToken);
}
