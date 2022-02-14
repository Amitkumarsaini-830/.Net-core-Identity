using com.aadviktech.IMS.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MLM.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.aadviktech.IMS.Filter
{
    public interface IIdentityProvider
    {
        Task<Dictionary<string, object>> LoginUser(string userName, string password);
        void LogoutUser();
        Task<MyAppUser> CheckEmailAsync(string email);
        IdentityResult ChangePassword(MyAppUser user, string newPassword, string oldPwd);
        Task<IdentityResult> RegisterUser(MyAppUser register);
        Task<MyAppUser> GetCurrentUser(string username);
        Task<IList<string>> GetUserRolesAsync(MyAppUser user);
        Task<MyAppUser> CheckUserNameAsync(string username);
    }
}
