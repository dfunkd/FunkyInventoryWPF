using Dapper;
using FunkyInventoryWPF.Models.UserModels;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FunkyInventoryWPF.Data.Repositories;

public interface IRoleRepository
{
    Task<Role?> AddRole(AddRoleRequest role, CancellationToken cancellationToken = default);
    Task<bool> DeleteRole(Guid roleId, CancellationToken cancellationToken = default);
    Task<List<Role>> GetAllRoles(CancellationToken cancellationToken = default);
    Task<Role?> GetRoleById(Guid roleId, CancellationToken cancellationToken = default);
    Task<Role?> UpdateRole(Guid roleId, UpdateRoleRequest role, CancellationToken cancellationToken = default);
}

public class RoleRepository : IRoleRepository
{
    public async Task<Role?> AddRole(AddRoleRequest role, CancellationToken cancellationToken = default)
    {
        Role? ret = null;
        Guid? res = null;

        const string iSql = @"
INSERT INTO dbo.[Role] (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, RoleName, NormalizedName)
OUTPUT INSERTED.RoleId
VALUES (@by, @now, @by, @now, @roleName, @normalizedName)
";

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var iParms = new
        {
            DateTime.Now,
            By = "System",
            role.NormalizedName,
            role.RoleName
        };

        try
        {
            CommandDefinition iCmd = new(iSql, iParms, trx, 150, cancellationToken: cancellationToken);

            res = await conn.QuerySingleAsync<Guid>(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }

        if (res != null)
            ret = await GetRoleById(res.Value, cancellationToken);

        return ret;
    }

    public async Task<bool> DeleteRole(Guid roleId, CancellationToken cancellationToken = default)
    {
        int res = -1;

        const string dSql = @"
DELETE dbo.[Role]
WHERE RoleId = @roleId
";

        var parms = new { roleId };

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        try
        {
            CommandDefinition dCmd = new(dSql, parms, trx, 150, cancellationToken: cancellationToken);

            res = await conn.ExecuteAsync(dCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }

        return res > 0;
    }

    public async Task<List<Role>> GetAllRoles(CancellationToken cancellationToken = default)
    {
        List<Role> ret = [];

        const string sSql = @"
SELECT RoleId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, RoleName, NormalizedName
FROM dbo.[Role]
";

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);

        try
        {
            CommandDefinition sCmd = new(sSql, null, null, 150, cancellationToken: cancellationToken);

            IEnumerable<Role>? res = await conn.QueryAsync<Role>(sCmd);

            if (res is not null)
                ret.AddRange(res);
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return ret;
    }

    public async Task<Role?> GetRoleById(Guid roleId, CancellationToken cancellationToken = default)
    {
        Role? ret = null;

        const string sSql = @"
SELECT RoleId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, RoleName, NormalizedName
FROM dbo.[Role]
WHERE RoleId = @roleId
";

        var parms = new { roleId };

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);


        try
        {
            CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

            IEnumerable<Role>? res = await conn.QueryAsync<Role>(sCmd);

            if (res is not null)
                ret = res.FirstOrDefault();
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return ret;
    }

    public async Task<Role?> UpdateRole(Guid roleId, UpdateRoleRequest role, CancellationToken cancellationToken = default)
    {
        Role? ret = null;
        int res = 0;

        const string iSql = @"
UPDATE dbo.[Role]
SET ModifiedBy = @by
    , ModifiedDate = @now
    , RoleName = @roleName
    , NormalizedName = @normalizedName
WHERE RoleId = @roleId
";

        var iParms = new
        {
            DateTime.Now,
            By = "System",
            role.RoleName,
            role.NormalizedName,
            roleId
        };

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        try
        {
            CommandDefinition iCmd = new(iSql, iParms, trx, 150, cancellationToken: cancellationToken);

            res = await conn.ExecuteAsync(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }

        if (res > 0)
            ret = await GetRoleById(roleId, cancellationToken);

        return ret;
    }
}
