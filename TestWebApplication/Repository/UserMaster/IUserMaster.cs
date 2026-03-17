using TestWebApplication.Models;

namespace TestWebApplication.Repository.UserMaster
{
    public interface IUserMaster
    {
        public Task<int> InsertUser(UserMasterEntity obj);
        Task<int> updateUser(UserMasterEntity obj);
        Task<int> deleteUser(int id);
        Task<UserMasterEntity> findById(int id);
        Task<UserMasterEntity> GetAllUser();
        Task<UserMasterEntity> Login(Login login);

    }
}
