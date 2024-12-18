using Dapper;
using FunkyInventoryWPF.Models.UserModels;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FunkyInventoryWPF.Data.Repositories;

public interface ILoginRepository
{
    Task<User?> Login(string username, string password, CancellationToken cancellationToken = default);
    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default);
}

public class LoginRepository : ILoginRepository
{
    public async Task<User?> Login(string username, string password, CancellationToken cancellationToken = default)
    {
        User? ret = null;

        const string sSql = @"
SELECT u.RoleId, u.UserId, u.CreatedBy, u.CreatedDate, u.ModifiedBy, u.ModifiedDate, u.FirstName, u.LastName, u.LastLogin, u.Username, u.Email, u.Password, u.EncryptedPassword,
    r.RoleId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.RoleName, r.NormalizedName
FROM dbo.[User] AS u
    INNER JOIN dbo.[Role] AS r ON r.RoleId = u.RoleId
WHERE UserName = @userName
	AND [Password] = @password
";

        var parms = new { username, password };
        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        try
        {
            IEnumerable<User> res = await conn.QueryAsync<User, Role, User>(sCmd, (user, role) =>
            {
                user.Role = role;
                return user;
            },
            splitOn: "RoleId");

            if (res is not null && res.Count() > 0)
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

    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default)
    {
        User? ret = null;

        const string sSql = @"
SELECT u.RoleId, u.UserId, u.CreatedBy, u.CreatedDate, u.ModifiedBy, u.ModifiedDate, u.FirstName, u.LastName, u.LastLogin, u.Username, u.Email, u.Password, u.EncryptedPassword,
    r.RoleId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.RoleName, r.NormalizedName
FROM dbo.[User] AS u
    INNER JOIN dbo.[Role] AS r ON r.RoleId = u.RoleId
WHERE UserId = @userId
";

        var parms = new { userId };
        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        IEnumerable<User> res = await conn.QueryAsync<User, Role, User>(sCmd, (user, role) =>
        {
            user.Role = role;
            return user;
        },
        splitOn: "RoleId");

        if (res is not null)
            ret = res.FirstOrDefault();

        return ret;
    }
}
