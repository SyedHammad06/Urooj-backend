using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Service;

namespace Hauna.Urooj.Hauna.Urooj.Services.Interface
{
    public interface IUserService 
    {
        Loginresponse ValidateUser(LoginModel loginModel);
        TokenExtractedClass VerifyToken(string token);
        Task<string> AddAdmin(UserInfoModel userInfoModel, int isAdmin);
        Task Subscription(SubscriptionModel subscriptionModel);
        IEnumerable<UsersDisplayModel> GetUsers();
        string ChangePassword(ChangePasswordModel changePassword);
        void RemoveUser(string username);
        UserDetail GetUserDetails(string userId);
    }
}
