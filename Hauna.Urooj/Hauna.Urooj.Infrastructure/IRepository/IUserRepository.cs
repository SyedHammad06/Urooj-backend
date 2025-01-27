using Hauna.Urooj.Hauna.Urooj.Models;

namespace Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository
{
    public interface IUserRepository
    {
        UserModel GetUser(LoginModel loginModel);
        string CheckForDuplicateEmail(string email);
        void InsertUserInfo(UserInfoModel model);
        int fetchInfoId(string email);
        void InsertUser(UserModel userModel);
        IEnumerable<UsersDisplayModel> GetAllActiveUsers();
        void ChangePassword(string userId, string newEncryptedPassword);
        void SetIsActiveInUsers(string userId);
        UserDetail GetUserData(string userId);
    }
}
