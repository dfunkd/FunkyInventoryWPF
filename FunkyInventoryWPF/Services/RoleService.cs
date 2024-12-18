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

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async Task<Role?> AddRole(AddRoleRequest role, CancellationToken cancellationToken = default)
        => await roleRepository.AddRole(role, cancellationToken);

    public async Task<bool> DeleteRole(Guid roleId, CancellationToken cancellationToken = default)
        => await roleRepository.DeleteRole(roleId, cancellationToken);

    public async Task<List<Role>> GetAllRoles(CancellationToken cancellationToken = default)
        => await roleRepository.GetAllRoles(cancellationToken);

    public async Task<Role?> GetRoleById(Guid roleId, CancellationToken cancellationToken = default)
        => await roleRepository.GetRoleById(roleId, cancellationToken);

    public async Task<Role?> UpdateRole(Guid roleId, UpdateRoleRequest role, CancellationToken cancellationToken = default)
        => await roleRepository.UpdateRole(roleId, role, cancellationToken);
}
