using FunkyInventoryWPF.Models.UserModels;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FunkyInventoryWPF.Data.Repositories;

public interface IUserRepository
{
    Task<User?> AddUser(AddUserRequest user, CancellationToken cancellationToken = default);
    Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> UpdatePassword(Guid userId, string password, string encrypted, CancellationToken cancellationToken = default);
    Task<User?> UpdateUser(Guid userId, UpdateUserRequest user, CancellationToken cancellationToken = default);
    Task<bool> UserNameOrEmailExists(string email, string userName, CancellationToken cancellationToken = default);
}

public class UserRepository : IUserRepository
{
    public async Task<User?> AddUser(AddUserRequest user, CancellationToken cancellationToken = default)
    {
        User? ret = null;
        Guid? res = null;
        Role? role = null;

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        if (user.RoleId == Guid.Empty)
        {
            const string sSql = @"
SELECT RoleId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, RoleName, NormalizedName
FROM dbo.[Role]
WHERE RoleName = 'Guest'
";

            CommandDefinition sCmd = new(sSql, null, trx, 150, cancellationToken: cancellationToken);
            role = await conn.QueryFirstOrDefaultAsync<Role>(sCmd);
            if (role is null)
                return null;

            user.RoleId = role.RoleId;
        }

        const string iSql = @"
INSERT INTO dbo.[User] (RoleId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, FirstName, LastName, Username, Email, Password, EncryptedPassword)
OUTPUT INSERTED.UserId
VALUES (@roleId, @by, @now, @by, @now, @firstName, @lastName, @userName, @email, @password, @encryptedPassword)
";

        var iParms = new
        {
            user.RoleId,
            DateTime.Now,
            By = "System",
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Email,
            user.Password,
            user.EncryptedPassword
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
            ret = await GetUserById(res.Value, cancellationToken);

        return ret;
    }

    public async Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default)
    {
        int res = -1;

        const string dSql = @"
DELETE dbo.[User]
WHERE UserId = @userId
";

        var parms = new { userId };

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

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        List<User> ret = [];

        const string sSql = @"
SELECT u.RoleId, u.UserId, u.CreatedBy, u.CreatedDate, u.ModifiedBy, u.ModifiedDate, u.FirstName, u.LastName, u.LastLogin, u.Username, u.Email, u.Password, u.EncryptedPassword,
    r.RoleId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.RoleName, r.NormalizedName
FROM dbo.[User] AS u
    INNER JOIN dbo.[Role] AS r ON r.RoleId = u.RoleId
";

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        CommandDefinition sCmd = new(sSql, null, null, 150, cancellationToken: cancellationToken);

        try
        {
            IEnumerable<User>? res = await conn.QueryAsync<User, Role, User>(sCmd, (user, role) =>
            {
                user.Role = role;
                return user;
            }, splitOn: "UserId, RoleId");

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

    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default)
    {
        User? ret = null;

        const string sSql = @"
SELECT u.RoleId, u.UserId, u.CreatedBy, u.CreatedDate, u.ModifiedBy, u.ModifiedDate, u.FirstName, u.LastName, u.LastLogin, u.Username, u.Email, u.Password, u.EncryptedPassword,
    r.RoleId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.RoleName, r.NormalizedName
FROM dbo.[User] AS u
    INNER JOIN dbo.[Role] AS r ON r.RoleId = u.RoleId
WHERE u.UserId = @userId
";

        var parms = new { userId };

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

        try
        {
            IEnumerable<User>? res = await conn.QueryAsync<User, Role, User>(sCmd, (user, role) =>
            {
                user.Role = role;
                return user;
            }, splitOn: "UserId, RoleId");

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

    public async Task<User?> UpdatePassword(Guid userId, string password, string encrypted, CancellationToken cancellationToken = default)
    {
        User? ret = null;
        int res = 0;

        const string iSql = @"
UPDATE dbo.[User]
SET ModifiedBy = @by
    , ModifiedDate = @now
    , [Password] = @password
    , EncryptedPassword = @encrypted
WHERE UserId = @userId
";

        var iParms = new
        {
            DateTime.Now,
            By = "System",
            password,
            encrypted,
            userId
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
            ret = await GetUserById(userId, cancellationToken);

        return ret;
    }

    public async Task<User?> UpdateUser(Guid userId, UpdateUserRequest user, CancellationToken cancellationToken = default)
    {
        User? ret = null;
        int res = 0;

        const string iSql = @"
UPDATE dbo.[User]
SET ModifiedBy = @by
    , ModifiedDate = @now
    , FirstName = @firstName
    , Email = @email
    , LastName = @lastName
    , LastLogin = @lastLogin
    , Username = @userName
    , RoleId = @roleId
WHERE UserId = @userId
";

        var iParms = new
        {
            DateTime.Now,
            By = "System",
            user.FirstName,
            user.LastName,
            user.LastLogin,
            user.UserName,
            user.Email,
            user.RoleId,
            userId
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
            ret = await GetUserById(userId, cancellationToken);

        return ret;
    }

    public async Task<bool> UserNameOrEmailExists(string email, string userName, CancellationToken cancellationToken)
    {
        IEnumerable<User>? res = null;

        const string sSql = @"
SELECT u.RoleId, u.UserId, u.CreatedBy, u.CreatedDate, u.ModifiedBy, u.ModifiedDate, u.FirstName, u.LastName, u.LastLogin, u.Username, u.Email, u.Password, u.EncryptedPassword
FROM dbo.[User] AS u
WHERE u.Email = @email
    OR u.UserName = @userName
";

        var parms = new { email, userName };

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

        try
        {
            res = await conn.QueryAsync<User>(sCmd);
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return res is not null && res.Count() > 0;
    }
}
