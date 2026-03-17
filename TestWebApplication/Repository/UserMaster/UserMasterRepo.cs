using Azure;
using Dapper;

using System.Data;
using System.Reflection;
using TestWebApplication.Factory;
using TestWebApplication.Models;

namespace TestWebApplication.Repository.UserMaster
{
    public class UserMasterRepo :RepositoryBase, IUserMaster
    {

        public UserMasterRepo(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }
        public Task<int> deleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserMasterEntity> findById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserMasterEntity> GetAllUser()
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertUser(UserMasterEntity obj)
        {
            var param = new DynamicParameters();

            param.Add("@Action", "INSERT");
            param.Add("@UserName", obj.UserName);
            param.Add("@Password", obj.Password);
            param.Add("@Role", obj.Role);
            param.Add("@Designation", obj.Designation);
            param.Add("@IsActive", obj.IsActive);

            // OUTPUT parameter
            param.Add("@Msg", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await Connection.ExecuteAsync(
                "sp_WebApp_UserMaster",
                param,
                commandType: CommandType.StoredProcedure
            );

            int result = param.Get<int>("@Msg");

            return result;   // 1 = success, 0 = failure
        }

        public Task<int> updateUser(UserMasterEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<UserMasterEntity> Login(Login login)
        {
            var param = new DynamicParameters();
            param.Add("@Action", "LOGIN");
            param.Add("@UserName", login.UserName);
            param.Add("@Password", login.Password);

            // Stored procedure expects @Msg output parameter even for LOGIN
            param.Add("@Msg", dbType: DbType.Int32, direction: ParameterDirection.Output);

            return await Connection.QueryFirstOrDefaultAsync<UserMasterEntity>(
                "sp_WebApp_UserMaster",
                param,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
