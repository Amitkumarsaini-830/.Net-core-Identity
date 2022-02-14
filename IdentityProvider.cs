
using com.aadviktech.IMS.Constant.AllEnum;
using com.aadviktech.IMS.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.aadviktech.IMS.Filter
{
    public class IdentityProvider : IIdentityProvider
    {
        private UserManager<MyAppUser> userManager { get; }
        private SignInManager<MyAppUser> signInManager { get; }
        private RoleManager<AppRoles> roleManager { get; }
        MyDBContext db;
       

        public IdentityProvider(UserManager<MyAppUser> userManager, SignInManager<MyAppUser> signInManager, RoleManager<AppRoles> roleManager, MyDBContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.db = db;
            //RoleInitializer.Initialize(roleManager).Wait();
        }


        public async Task<Dictionary<string, object>> LoginUser(string userName, string password)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                #region ToCheckAllowWebAndActiveCustomeTag
                MyAppUser user = await userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Invalid UserId or Password");
                    return dict;
                }
                #endregion

                SignInResult result = await signInManager.PasswordSignInAsync(userName, password, false, false);
                if (result.Succeeded)  //0=success,3=fail
                {
                    //MyAppUser myuser = await userManager.FindByEmailAsync(userName);

                    //HttpContext.Session.["RoleId"] = member.RoleId;
                    //HttpContext.Current.Session["Role"] = member.Role.Name;
                    dict.Add("Status", true);
                    dict.Add("Message", "Success");
                    //dict.Add("Member", myuser);
                    return dict;
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Invalid UserId or Password");
                    return dict;
                }
            }
            catch (Exception e)
            {
               // ErrorLogRepo.AddException(e, "Login User");
                dict.Add("Status", false);
                dict.Add("Message", "Something went wrong");
                return dict;
            }
        }

        public void LogoutUser()
        {
            try
            {
                signInManager.SignOutAsync();
            }
            catch (Exception e) { }
        }

        public async Task<MyAppUser> CheckEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }
        public async Task<MyAppUser> CheckUserNameAsync(string username)
        {
            return await userManager.FindByNameAsync(username);
        }
         
        public IdentityResult ChangePassword(MyAppUser user, string newPassword, string oldPwd)
        {
            return userManager.ChangePasswordAsync(user, oldPwd, newPassword).Result;
        }

        public async Task<IdentityResult> RegisterUser(MyAppUser register)
        {
            //MyAppUser myAppUser = new MyAppUser();

            //myAppUser.UserName = register.Email;
            //myAppUser.Email = register.Email;
            //myAppUser.FullName = register.FullName;
            //myAppUser.PasswordHash = register.PasswordHash;
            //myAppUser.PhoneNumber = register.PhoneNumber;
            IdentityResult result = await userManager.CreateAsync(register, register.PasswordHash);
            if (result.Succeeded)
            {
                if (register.RoleId == (int)UserRole.admin)
                {
                    await userManager.AddToRoleAsync(register, UserRole.admin.ToString());
                }
                if (register.RoleId == (int)UserRole.accountant)
                {
                    await userManager.AddToRoleAsync(register, UserRole.accountant.ToString());
                }
                if (register.RoleId == (int)UserRole.client)
                {
                    await userManager.AddToRoleAsync(register, UserRole.client.ToString());
                }
                if (register.RoleId == (int)UserRole.party)
                {
                    await userManager.AddToRoleAsync(register, UserRole.party.ToString());
                }
            }
            return result;
        }
        public async Task<MyAppUser> GetCurrentUser(string username)
        {
            MyAppUser user = (await userManager.FindByNameAsync(username));
            return user;
        }

        public async Task<IList<string>> GetUserRolesAsync(MyAppUser user)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            return roles;
        }

  
    }
}
