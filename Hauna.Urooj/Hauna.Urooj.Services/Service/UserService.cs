using Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository;
using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Interface;
using System.Text;

namespace Hauna.Urooj.Hauna.Urooj.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly IMailService _mailService;
        private readonly string Salt;
        public UserService(IUserRepository userRepository, TokenManager tokenManager,IMailService mailService)
        {
            _userRepository = userRepository;
            _tokenManager = tokenManager;
            _mailService = mailService;
        }

        public Loginresponse ValidateUser(LoginModel loginModel)
        {
            string userAccess;
            loginModel.password = PasswordHelper.HashPassword(loginModel.password);
            var user = _userRepository.GetUser(loginModel);
            if (user != null)
            {
                userAccess = user.IsAdmin ? "1" : "0";
                var token = _tokenManager.GenerateWebToken(user.UserId, userAccess);

                var response = new Loginresponse
                {
                    Token = token,
                    UserAccess = userAccess,
                };
                return response;
            }
            return null;
        }

        public TokenExtractedClass VerifyToken(string token)
        {
            var extractedValues = _tokenManager.GetUserFromToken(token);
            if (extractedValues != null)
            {
                return extractedValues;
            }
            return null;
        }

        public async Task<string> AddAdmin(UserInfoModel userInfoModel,int isAdmin = 0)
        {
            var personalEmail = _userRepository.CheckForDuplicateEmail(userInfoModel.PersonalEmail);
            if (personalEmail != null)
            {
                return personalEmail;
            }
            _userRepository.InsertUserInfo(userInfoModel);
            var InfoId = _userRepository.fetchInfoId(userInfoModel.PersonalEmail);
            UserModel userModel = new UserModel();
            userModel.InfoId = InfoId;
            userModel.ModifiedDate = DateTime.UtcNow;
            userModel.UserId = $"Urooj@{InfoId}";
            userModel.IsAdmin = isAdmin==1?true:false;
            var Password =await GenerateRandomPassword(8);
            userModel.EncryptedPassword = PasswordHelper.HashPassword(Password);
            _userRepository.InsertUser(userModel);
            var mailDetails = new MailRequest();
            mailDetails.ToEmail = userInfoModel.PersonalEmail;
            mailDetails.Subject = "Your Login Credentials for Urooj webSite";
            mailDetails.UserName = userModel.UserId;
            mailDetails.Password = Password;
            await _mailService.SendEmailAsync(mailDetails);
            return null;
        }

        public async Task Subscription(SubscriptionModel subscriptionModel)
        {
            var mailDetails = new MailRequest();
            mailDetails.Subject = "A Query Has Been Requested";
            var body = $"<div>Name : {subscriptionModel.Name}</div><div>Organization Name : {subscriptionModel.OrganizationName}</div><div>Address : {subscriptionModel.Address}</div><div>Query : {subscriptionModel.Query}</div>";
            mailDetails.Body = body ;
            await _mailService.SendEmailForSubAsync(mailDetails);
        }

        public async Task<string> GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                char randomChar = validChars[random.Next(validChars.Length)];
                password.Append(randomChar);
            }

            return password.ToString();
        }

        public IEnumerable<UsersDisplayModel> GetUsers()
        {
            var users = _userRepository.GetAllActiveUsers();
            return users;
        }

        public string ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var loginModel = new LoginModel()
            {
                userName = changePasswordModel.UserName,
                password = PasswordHelper.HashPassword(changePasswordModel.OldPassword)
            };
            var user = _userRepository.GetUser(loginModel);
            if(user == null)
            {
                return "Invalid OldPassword";
            }
            _userRepository.ChangePassword(changePasswordModel.UserName, PasswordHelper.HashPassword(changePasswordModel.NewPassword));
            return null;
        }

        public void RemoveUser(string username)
        {
            _userRepository.SetIsActiveInUsers(username);
        }

        public UserDetail GetUserDetails(string userId)
        {
            var userDetails = _userRepository.GetUserData(userId);
            return userDetails;
        }
    }
}
