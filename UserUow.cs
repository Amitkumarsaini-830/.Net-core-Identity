using com.aadviktech.IMS.Constant;
using com.aadviktech.IMS.Constant.AllEnum;
using com.aadviktech.IMS.Contract.Repository_Interfaces;
using com.aadviktech.IMS.Contract.UOW_Interfaces;
using com.aadviktech.IMS.DB;
using com.aadviktech.IMS.Filter;
using com.aadviktech.IMS.ViewModel;
using Insight.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace com.aadviktech.IMS.Uow
{
    public class UserUow : IUserUow
    {
        private IHostingEnvironment Environment;
        private IIdentityProvider AuthProvider;
        private IUserRepository UserRepo;
        private IErrorLogRepository ErrorRepo;
        private IIdentityProvider IdentityProviderObj;
        private IConfiguration ConfigurationObj;
        private IStateRepository stateRepository;
        private ITaxRateRepository taxRateRepository;
        private ITaxGroupRepository groupRepository;
        private IUintRepository uintRepository;
        private IUCRatesRepository uCRatesRepository;
        private IBankRepository bankRepository;
        private ISpRepository spRepository;
        private ICityRepository cityRepository;
        private ILedgerGroupRepository ledgerGroupRepo;
        private ILedgerSubGroupRepository ledgerSubGroupRepo;
        private IInvoiceNoRepository invoiceNoRepo;
        private object webHostEnvironment;
        private ITransactionsRepository transactionsRepo;
        private IShopeRepo shopeRepo;
        private IPayemtModeOptionRepository payemtModeRepo;
        private IDescriptionRepository DescriptionRepo;
        private IExtraChargeRepository extraChargeRepo;
        private ICashTransactionRepository cashTransactionRepo;


        public UserUow(IHostingEnvironment Environment,
        IIdentityProvider AuthProvider,
            ISpRepository spRepository,
            IUintRepository uintRepository,
            ITaxGroupRepository groupRepository,
            IBankRepository bankRepository,
            ITaxRateRepository taxRateRepository,
            IUserRepository UserRepo,
            IStateRepository stateRepository,
            IErrorLogRepository ErrorRepo,
            IIdentityProvider IdentityProviderObj,
            IConfiguration ConfigurationObj,
            IUCRatesRepository uCRatesRepository,
            ICityRepository cityRepository,
            ILedgerGroupRepository ledgerGroupRepo,
            ILedgerSubGroupRepository ledgerSubGroupRepo,
            IInvoiceNoRepository invoiceNoRepo,
            ITransactionsRepository transactionsRepo,
            IShopeRepo shopeRepo,
            IPayemtModeOptionRepository payemtModeRepo,
            IDescriptionRepository DescriptionRepo,
            IExtraChargeRepository extraChargeRepo,
            ICashTransactionRepository cashTransactionRepo
            )
        {
            this.Environment = Environment;
            this.AuthProvider = AuthProvider;
            this.UserRepo = UserRepo;
            this.ErrorRepo = ErrorRepo;
            this.IdentityProviderObj = IdentityProviderObj;
            this.ConfigurationObj = ConfigurationObj;
            this.stateRepository = stateRepository;
            this.taxRateRepository = taxRateRepository;
            this.groupRepository = groupRepository;
            this.uintRepository = uintRepository;
            this.uCRatesRepository = uCRatesRepository;
            this.bankRepository = bankRepository;
            this.spRepository = spRepository;
            this.cityRepository = cityRepository;
            this.ledgerGroupRepo = ledgerGroupRepo;
            this.ledgerSubGroupRepo = ledgerSubGroupRepo;
            this.invoiceNoRepo = invoiceNoRepo;
            this.transactionsRepo = transactionsRepo;
            this.shopeRepo = shopeRepo;
            this.payemtModeRepo = payemtModeRepo;
            this.DescriptionRepo = DescriptionRepo;
            this.extraChargeRepo = extraChargeRepo;
            this.cashTransactionRepo = cashTransactionRepo;

        }

        public MyAppUser GetById(int id)
        {
            try
            {
                return UserRepo.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "GetById", id);
                return null;
            }
        }
        public List<City> GetCity(int StateId)
        {
            List<City> cities = cityRepository.GetCityList().Where(a => a.StateId == StateId).ToList();
            return cities;
        }
        public List<City> GetCityList()
        {
            List<City> cities = cityRepository.GetCityList();
            return cities;
        }

        public async Task<Dictionary<string, object>> RegisterUser(MyAppUser user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                if (user.PasswordHash != user.CnfPassword)
                {
                    result.Add("Status", false);
                    result.Add("Message", "Password and Confirm Password do not match");
                    return result;
                }
                MyAppUser myAppUser = await AuthProvider.CheckEmailAsync(user.Email);
                if (myAppUser == null)
                {
                    user.UserName = user.Email;
                    user.Active = true;
                    user.Deleted = false;
                    IdentityResult res = await AuthProvider.RegisterUser(user);
                    if (res.Succeeded)
                    {
                        result.Add("Status", true);
                        result.Add("Message", "User Registered Successfully !!");
                    }
                    else
                    {
                        result.Add("Status", false);
                        result.Add("Message", "Something went wrong, try after some time");
                    }
                }
                else
                {
                    result.Add("Status", false);
                    result.Add("Message", "User with this mail id is already registered!!");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "RegisterUser");
                result.Add("Status", false);
                result.Add("Message", "An Error Occured");
            }
            return result;
        }


        public Dictionary<string, object> DeleteUser(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                bool IfAny = transactionsRepo.TransactionByParty(id).Any();
                if (IfAny == true)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Party contains Recorde Can't Deleted");
                }
                else
                {
                    MyAppUser user = UserRepo.GetById(id);
                    if (user != null)
                    {
                        user.Active = false;
                        user.Deleted = true;
                        user.ModifiedOn = DateUtility.GetNowTime();
                        user.UserName = user.UserName + "_" + DateUtility.GetNowTime().ToString();
                        user.NormalizedUserName = user.UserName;
                        UserRepo.Save();
                        dict.Add("Status", true);
                        dict.Add("Message", "User Deleted Successfully");
                    }
                    else
                    {
                        dict.Add("Status", false);
                        dict.Add("Message", "User Not Found");
                    }
                }

            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "DeleteUser", id);
            }
            return dict;
        }

        public async Task<Dictionary<string, object>> EditUser(MyAppUser user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                MyAppUser myAppUser = UserRepo.GetById(user.Id);
                if (myAppUser != null)
                {
                    myAppUser.Active = user.Active;
                    myAppUser.FullName = user.FullName;
                    myAppUser.CompanyName = user.CompanyName;
                    myAppUser.Email = user.Email;
                    myAppUser.NormalizedEmail = myAppUser.Email.ToUpper();
                    myAppUser.Address = user.Address;
                    myAppUser.ModifiedOn = DateUtility.GetNowTime();
                    UserRepo.Save();

                    dict.Add("Status", true);
                    dict.Add("Message", "User info updated successfully");
                    return dict;
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "User not found");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "EditUser", user.Id);
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
            }
            return dict;
        }


        public async Task<Dictionary<string, object>> EditClient(MyAppUser user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                // string uniqueFileName =await UploadedFile(user);

                MyAppUser myAppUser = UserRepo.GetById(user.Id);
                if (myAppUser != null)
                {
                    if (user.ProfileImage != null)
                    {
                        if (myAppUser.ProfilePicture != null)
                        {
                            Dictionary<string, object> res = await DeleteFile(myAppUser.ProfilePicture);

                        }
                        string uniqueFileName = await UploadedFile(user);
                        myAppUser.ProfilePicture = uniqueFileName;
                    }
                    myAppUser.Active = user.Active;
                    myAppUser.BusinessCategory = user.BusinessCategory;
                    myAppUser.BusinessType = user.BusinessType;
                    myAppUser.Descriptions = user.Descriptions;
                    myAppUser.GSTINNO = user.GSTINNO;
                    myAppUser.GSTType = user.GSTType;
                    myAppUser.CompanyName = user.CompanyName;
                    myAppUser.StateId = user.StateId;
                    myAppUser.Email = user.Email;
                    myAppUser.NormalizedEmail = myAppUser.Email.ToUpper();
                    myAppUser.Address = user.Address;
                    myAppUser.PinCode = user.PinCode;
                    myAppUser.AccountantId = user.AccountantId;
                    myAppUser.ModifiedOn = DateUtility.GetNowTime();
                    // myAppUser.finacialTimeFromDate = user.finacialTimeFromDate;
                    //myAppUser.finacialTimeToDate = user.finacialTimeToDate;
                    UserRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Client info updated successfully");
                    return dict;
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Client not found");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "EditClient", user.Id);
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
            }
            return dict;
        }

        public async Task<Dictionary<string, object>> AddClient(MyAppUser user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var existed = UserRepo.GetUserByMobile(user.PhoneNumber);
                if (existed != null)
                {
                    dict.Add("Status", false);
                    if (existed.Active)
                        dict.Add("Message", "This Mobile Number already existed.");
                    else dict.Add("Message", "Given mobile number deactivated by administrator");
                    //string conflict = distributor.Mobile == existed.Mobile ? "Mobile Number" : "Email Id";
                    return dict;
                }
                else
                {
                    user.Active = true;
                    user.Deleted = false;
                    user.ClientId = 0;
                    //  user.AccountantId = 0;
                    user.RoleId = (int)UserRole.client;


                    user.Balance = 0;
                    user.UserName = user.PhoneNumber;
                    user.PasswordHash = ApplicationGlobal.GetRandomPassword(8);     //new
                    user.CreatedOn = DateUtility.GetNowTime();
                    user.DateIngoreTill = DateUtility.GetNowTime();

                    IdentityResult res = await IdentityProviderObj.RegisterUser(user);
                    if (res.Succeeded)
                    {
                        //UserRepo.Insert(distributor);     this has been done inside createuser()

                        dict.Add("Status", true);
                        dict.Add("Message", "Client added successfully");
                        //Task.Run(() => CommunicationUow.SendAddUserSMS(distributor));
                        //Task.Run(() => CommunicationUow.SendAddUserMail(distributor));
                    }
                    else
                    {
                        dict.Add("Status", false);
                        dict.Add("Message", res.Errors.FirstOrDefault().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Add Client Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }







        public async Task<Dictionary<string, object>> AddAccountant(MyAppUser user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var existed = UserRepo.GetUserByMobile(user.PhoneNumber);
                if (existed != null)
                {
                    dict.Add("Status", false);
                    if (existed.Active)
                        dict.Add("Message", "This Mobile Number already existed.");
                    else dict.Add("Message", "Given mobile number deactivated by administrator");
                    //string conflict = distributor.Mobile == existed.Mobile ? "Mobile Number" : "Email Id";
                    return dict;
                }
                else
                {
                    user.Active = true;
                    user.Deleted = false;
                    user.ClientId = 0;
                    user.AccountantId = 0;
                    user.RoleId = (int)UserRole.accountant;


                    user.Balance = 0;
                    user.UserName = user.PhoneNumber;
                    user.PasswordHash = ApplicationGlobal.GetRandomPassword(8);     //new
                    user.CreatedOn = DateUtility.GetNowTime();
                    user.CreatedOn = DateUtility.GetNowTime();
                    user.DateIngoreTill = DateUtility.GetNowTime();

                    IdentityResult res = await IdentityProviderObj.RegisterUser(user);
                    if (res.Succeeded)
                    {
                        //UserRepo.Insert(distributor);     this has been done inside createuser()

                        dict.Add("Status", true);
                        dict.Add("Message", "Accountant added successfully");
                        //Task.Run(() => CommunicationUow.SendAddUserSMS(distributor));
                        //Task.Run(() => CommunicationUow.SendAddUserMail(distributor));
                    }
                    else
                    {
                        dict.Add("Status", false);
                        dict.Add("Message", res.Errors.FirstOrDefault().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Add Accountant Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }


        public List<MyAppUser> GetAccountants()
        {
            List<MyAppUser> users = UserRepo.AccountantList().ToList();

            return users;
        }
        public List<MyAppUser> GetAccountants(ref DataPagingModel TablePaging)
        {
            try
            {
                using (SqlConnection DB = new SqlConnection(ConfigurationObj.GetSection("ConnectionStrings").GetSection("MyConnection").Value.ToString()))
                {
                    string SortingFilter = " order by AspNetUsers.FullName ";
                    #region Search Filter
                    string SearchString = string.Empty;
                    string mobile = string.Empty;
                    string searchtxt = string.Empty;

                    foreach (var item in TablePaging.SearchParameter)
                    {
                        string value = item.Value.Trim();
                        if (item.Key.ToLower() == "mobile" && !String.IsNullOrEmpty(value))
                        {
                            mobile = "%" + value + "%";
                            SearchString = SearchString + " AND AspNetUsers.PhoneNumber like @Mobile ";
                        }
                        else if (item.Key.ToLower() == "name" && !String.IsNullOrEmpty(value))
                        {
                            searchtxt = "%" + value + "%";
                            SearchString = SearchString + " AND AspNetUsers.FullName like @SearchTxt ";
                        }
                    }

                    #endregion
                    #region Query :GetList
                    string CommandText = "SELECT * FROM(Select row_number() over(" + SortingFilter + ") as RowNum, AspNetUsers.CreatedOn, AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.PhoneNumber, AspNetUsers.Email, AspNetUsers.Active, AspNetUsers.FullName, AspNetUsers.CompanyName, AspNetUsers.Balance, AspNetUsers.Address, AspNetUsers.StateId from AspNetUsers where AspNetUsers.Deleted = 0 AND AspNetUsers.RoleId =" + (int)Constant.AllEnum.UserRole.accountant + " " + SearchString + ") as tbl WHERE RowNum BETWEEN @RecordFrom AND @RecordTo";


                    #endregion

                    var parameters = new
                    {
                        RecordFrom = TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize) + 1,
                        RecordTo = TablePaging.PageSize + TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize),
                        Mobile = mobile,
                        SearchTxt = searchtxt
                    };

                    List<MyAppUser> ModelList = DB.QuerySql<MyAppUser>(CommandText, parameters).ToList();

                    CommandText = @"Select  COUNT(AspNetUsers.Id)
                                                     from AspNetUsers
                                                         where AspNetUsers.Deleted = 0  AND AspNetUsers.RoleId = " + (int)Constant.AllEnum.UserRole.accountant + " " + SearchString;

                    TablePaging.TotalRecords = DB.ExecuteScalarSql<int>(CommandText, parameters);
                    return ModelList;
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Get Accountants list");
                return new List<MyAppUser>();
            }

        }



        public List<MyAppUser> GetClients(ref DataPagingModel TablePaging)
        {
            try
            {
                using (SqlConnection DB = new SqlConnection(ConfigurationObj.GetSection("ConnectionStrings").GetSection("MyConnection").Value.ToString()))
                {
                    string SortingFilter = " order by AspNetUsers.FullName ";
                    #region Search Filter
                    string SearchString = string.Empty;
                    string mobile = string.Empty;
                    string searchtxt = string.Empty;

                    foreach (var item in TablePaging.SearchParameter)
                    {
                        string value = item.Value.Trim();
                        if (item.Key.ToLower() == "mobile" && !String.IsNullOrEmpty(value))
                        {
                            mobile = "%" + value + "%";
                            SearchString = SearchString + " AND AspNetUsers.PhoneNumber like @Mobile ";
                        }
                        else if (item.Key.ToLower() == "name" && !String.IsNullOrEmpty(value))
                        {
                            searchtxt = "%" + value + "%";
                            SearchString = SearchString + " AND AspNetUsers.FullName like @SearchTxt ";
                        }
                    }

                    #endregion
                    #region Query :GetList
                    string CommandText = "SELECT * FROM(Select row_number() over(" + SortingFilter + ") as RowNum, AspNetUsers.CreatedOn, AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.PhoneNumber, AspNetUsers.Email, AspNetUsers.Active, AspNetUsers.FullName, AspNetUsers.CompanyName, AspNetUsers.Balance, AspNetUsers.Address, AspNetUsers.StateId,(select u.FullName  from AspNetUsers as u where u.Id=AspNetUsers.AccountantId) as accountantName from AspNetUsers  where AspNetUsers.Deleted = 0 AND AspNetUsers.RoleId =" + (int)Constant.AllEnum.UserRole.client + " " + SearchString + ") as tbl WHERE RowNum BETWEEN @RecordFrom AND @RecordTo";

                    #endregion

                    var parameters = new
                    {
                        RecordFrom = TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize) + 1,
                        RecordTo = TablePaging.PageSize + TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize),
                        Mobile = mobile,
                        SearchTxt = searchtxt
                    };



                    List<MyAppUser> ModelList = DB.QuerySql<MyAppUser>(CommandText, parameters).ToList();

                    CommandText = @"Select  COUNT(AspNetUsers.Id)
                                                     from AspNetUsers
                                                         where AspNetUsers.Deleted = 0  AND AspNetUsers.RoleId = " + (int)Constant.AllEnum.UserRole.client + " " + SearchString;

                    TablePaging.TotalRecords = DB.ExecuteScalarSql<int>(CommandText, parameters);
                    return ModelList;
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Get Accountants list");
                return new List<MyAppUser>();
            }

        }



        public List<MyAppUser> GetClient()
        {
            List<MyAppUser> users = UserRepo.ClientsList().ToList();

            return users;
        }

        public List<State> GetState()
        {
            List<State> states = stateRepository.GetStates().ToList();
            return states;
        }

        public List<MyAppUser> GetPartyList(ref DataPagingModel TablePaging)
        {
            try
            {
                using (SqlConnection DB = new SqlConnection(ConfigurationObj.GetSection("ConnectionStrings").GetSection("MyConnection").Value.ToString()))
                {
                    string SortingFilter = " order by AspNetUsers.FullName ";
                    #region Search Filter
                    string SearchString = string.Empty;
                    string mobile = string.Empty;
                    string searchtxt = string.Empty;
                    int UserId = 0;
                    int userId = 0;

                    userId = TablePaging.CurrentUserId;
                    SearchString = SearchString + " AND AspNetUsers.CreatedBy= @UserId ";

                    foreach (var item in TablePaging.SearchParameter)
                    {
                        string value = item.Value.Trim();
                        if (item.Key.ToLower() == "mobile" && !String.IsNullOrEmpty(value))
                        {
                            mobile = "%" + value + "%";
                            SearchString = SearchString + " AND AspNetUsers.PhoneNumber like @Mobile ";
                        }
                        else if (item.Key.ToLower() == "name" && !String.IsNullOrEmpty(value))
                        {
                            searchtxt = "%" + value + "%";
                            SearchString = SearchString + " AND AspNetUsers.FullName like @SearchTxt ";
                        }
                    }

                    #endregion
                    #region Query :GetList
                    string CommandText = "SELECT * FROM(Select row_number() over(" + SortingFilter + ") as RowNum, AspNetUsers.CreatedOn, AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.PhoneNumber, AspNetUsers.Email, AspNetUsers.Active, AspNetUsers.FullName, AspNetUsers.CompanyName, AspNetUsers.Balance, AspNetUsers.Address, AspNetUsers.StateId,(select u.FullName  from AspNetUsers as u where u.Id=AspNetUsers.ClientId) as ClientName from[dbo].AspNetUsers  where AspNetUsers.Deleted = 0 AND AspNetUsers.RoleId =" + (int)Constant.AllEnum.UserRole.party + " " + SearchString + ") as tbl WHERE RowNum BETWEEN @RecordFrom AND @RecordTo";

                    #endregion

                    var parameters = new
                    {
                        UserId = userId,
                        RecordFrom = TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize) + 1,
                        RecordTo = TablePaging.PageSize + TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize),
                        Mobile = mobile,
                        SearchTxt = searchtxt
                    };



                    List<MyAppUser> ModelList = DB.QuerySql<MyAppUser>(CommandText, parameters).ToList();

                    CommandText = @"Select  COUNT(AspNetUsers.Id)
                                                     from AspNetUsers
                                                         where AspNetUsers.Deleted = 0  AND AspNetUsers.RoleId = " + (int)Constant.AllEnum.UserRole.party + " " + SearchString;

                    TablePaging.TotalRecords = DB.ExecuteScalarSql<int>(CommandText, parameters);
                    return ModelList;
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Get Accountants list");
                return new List<MyAppUser>();
            }

        }


        public List<Sp_ClientTransaction> GetClienTransactions(DataPagingModel TablePaging)
        {

            return spRepository.GetClientTransactionsBal(TablePaging);
        }



        public List<MyAppUser> GetPartyTrnx(ref DataPagingModel TablePaging)
        {
            try
            {
                using (SqlConnection DB = new SqlConnection(ConfigurationObj.GetSection("ConnectionStrings").GetSection("MyConnection").Value.ToString()))
                {
                    string SortingFilter = " order by AspNetUsers.CompanyName ";
                    #region Search Filter
                    string SearchString = string.Empty;
                    string mobile = string.Empty;
                    string searchtxt = string.Empty;
                    int UserId = 0;
                    int userId = 0;


                    userId = TablePaging.CurrentUserId;
                    SearchString = SearchString + "AND Transactions.CreatedBy= @UserId ";

                    foreach (var item in TablePaging.SearchParameter)
                    {
                        string value = item.Value.Trim();

                        if (item.Key.ToLower() == "name" && !String.IsNullOrEmpty(value))
                        {
                            searchtxt = "%" + value + "%";
                            SearchString = SearchString + " AND AspNetUsers.CompanyName like @SearchTxt ";
                        }
                    }

                    #endregion
                    #region Query :GetList
                    string CommandText = "SELECT * FROM(Select row_number() over(" + SortingFilter + ") as RowNum, AspNetUsers.CreatedOn, AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.PhoneNumber, AspNetUsers.Email, AspNetUsers.Active, AspNetUsers.FullName, AspNetUsers.CompanyName, AspNetUsers.Balance, AspNetUsers.Address, AspNetUsers.StateId,Transactions.TotalAmount as TatalAmount from[dbo].AspNetUsers Left join Transactions on AspNetUsers.Id=Transactions.PartyId  where AspNetUsers.Deleted = 0 AND Transactions.Deleted = 0 AND AspNetUsers.RoleId =" + (int)Constant.AllEnum.UserRole.party + " " + SearchString + ") as tbl WHERE RowNum BETWEEN @RecordFrom AND @RecordTo";

                    #endregion

                    var parameters = new
                    {
                        UserId = userId,
                        RecordFrom = TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize) + 1,
                        RecordTo = TablePaging.PageSize + TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize),
                        Mobile = mobile,
                        SearchTxt = searchtxt
                    };



                    List<MyAppUser> ModelList = DB.QuerySql<MyAppUser>(CommandText, parameters).ToList();

                    CommandText = @"Select  COUNT(AspNetUsers.Id)
                                                     from AspNetUsers
                                                         where AspNetUsers.Deleted = 0  AND AspNetUsers.RoleId = " + (int)Constant.AllEnum.UserRole.party + " " + SearchString;

                    TablePaging.TotalRecords = DB.ExecuteScalarSql<int>(CommandText, parameters);
                    return ModelList;
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Get Accountants list");
                return new List<MyAppUser>();
            }
        }


        public async Task<Dictionary<string, object>> AddParty(MyAppUser user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var existed = UserRepo.GetUserByMobile(user.PhoneNumber);
                var existedName = UserRepo.GetComapnyByName(user.CompanyName);
                MyAppUser myAppUser = UserRepo.GetById(user.ClientId);
                if (existed != null)
                {
                    dict.Add("Status", false);
                    if (existed.Active)
                        dict.Add("Message", "This Mobile Number already existed.");
                    else dict.Add("Message", "Given mobile number deactivated by administrator");
                    //string conflict = distributor.Mobile == existed.Mobile ? "Mobile Number" : "Email Id";
                    return dict;
                }
                else if (existedName != null)
                {
                    dict.Add("Status", false);
                    if (existedName.Active)
                        dict.Add("Message", "This Company Name already existed.");
                    else dict.Add("Message", "Given Company Name deactivated by administrator");
                    //string conflict = distributor.Mobile == existed.Mobile ? "Mobile Number" : "Email Id";
                    return dict;
                }
                else
                {
                    user.Active = true;
                    user.Deleted = false;
                    //  user.ClientId = 0; 
                    //  user.AccountantId = 0;
                    user.RoleId = (int)UserRole.party;
                    user.AccountantId = myAppUser.AccountantId;
                    if(user.ToPayToRecive==(int)BalanceType.ToPay)
                    {
                        user.ClosingBalance = (user.Balance * (-1));
                    }
                    else
                    {
                        user.ClosingBalance = user.Balance;
                    }
                    //user.ClosingBalance = 0;
                    user.UserName = user.PhoneNumber;
                    user.PasswordHash = ApplicationGlobal.GetRandomPassword(8);     //new
                    user.CreatedOn = DateUtility.GetNowTime();
                    user.DateIngoreTill = DateUtility.GetNowTime();
                    IdentityResult res = await IdentityProviderObj.RegisterUser(user);
                    if (res.Succeeded)
                    {
                        //if(user.Balance>0)
                        //{
                        //    Transaction transaction = new Transaction();
                        //    CashTransaction cash = new CashTransaction();

                        //    transaction.ExtraChargesDada = null;
                        //    transaction.TransactionItemsDetails = null;
                        //    transaction.BankTrnxInfos = null;
                        //    transaction.FinanceInfos = null;
                        //    transaction.WalletInfos = null;
                        //    transaction.CashTransactions = null;

                        //    transaction.TotalAmount = user.Balance;
                        //    transaction.Invoice_Date = DateTime.Now;
                        //    transaction.CreatedOn = DateTime.Now;
                        //    transaction.PartyId = user.Id;
                        //    transaction.PartyName = user.CompanyName;
                        //    transaction.ShipongPartyEmail = user.Email;
                        //    transaction.CreatedBy = user.ClientId;
                        //    transaction.Deleted = false;

                        //    if (user.ToPayToRecive==(int)BalanceType.ToPay)
                        //    {
                        //        transaction.TransactionType = (int)TrsnsacctionsType.PaymentOut;
                        //        cash.TranxType = (int)TrsnsacctionsType.PaymentOut;
                        //    }
                        //    else if (user.ToPayToRecive == (int)BalanceType.ToReceive)
                        //    {
                        //        transaction.TransactionType = (int)TrsnsacctionsType.PaymentIn;
                        //        cash.TranxType = (int)TrsnsacctionsType.PaymentIn;
                        //    }
                        //    transactionsRepo.Insert(transaction);
                        //    transactionsRepo.Save();
                        //    cash.TrnxId = transaction.Id;
                        //    cash.cashAmt = user.Balance;
                        //    cash.CreatedBy = user.ClientId;
                        //    cash.Deleted = false;
                        //    cash.Discription = "Opening Balance";
                        //    cashTransactionRepo.Insert(cash);
                        //    cashTransactionRepo.Save();
                        //}

                        dict.Add("Status", true);
                        dict.Add("Message", "Party added successfully");
                        //Task.Run(() => CommunicationUow.SendAddUserSMS(distributor));
                        //Task.Run(() => CommunicationUow.SendAddUserMail(distributor));
                    }
                    else
                    {
                        dict.Add("Status", false);
                        dict.Add("Message", res.Errors.FirstOrDefault().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                
                ErrorRepo.AddException(ex, "Add Distributor Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }




        public TaxRate GetTaxById(int id)
        {
            try
            {
                return taxRateRepository.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "GetById", id);
                return null;
            }
        }

        public async Task<Dictionary<string, object>> EditParty(MyAppUser user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                MyAppUser myAppUser = UserRepo.GetById(user.Id);
                decimal CBalance;
                if (myAppUser != null)
                {
                    myAppUser.Active = user.Active;
                    myAppUser.FullName = user.FullName;
                    myAppUser.CompanyName = user.CompanyName;
                    myAppUser.Email = user.Email;
                    myAppUser.NormalizedEmail = myAppUser.Email.ToUpper();
                    myAppUser.Address = user.Address;
                    myAppUser.ShippingAddress = user.ShippingAddress;
                    myAppUser.Balance = user.Balance;
                    myAppUser.ToPayToRecive = user.ToPayToRecive;
                    if (myAppUser.ToPayToRecive == (int)BalanceType.ToPay)
                    {
                        CBalance = (myAppUser.Balance * (-1));
                    }
                    else
                    {
                        CBalance = myAppUser.Balance;
                    }
                    myAppUser.ClosingBalance = await spRepository.UpdateTransaction(myAppUser.Id,CBalance);
                    myAppUser.StateId = user.StateId;
                    myAppUser.CityId = user.CityId;
                    myAppUser.PinCode = user.PinCode;
                    myAppUser.GSTType = user.GSTType;
                    myAppUser.GSTINNO = user.GSTINNO;
                    myAppUser.LedegerGruop = user.LedegerGruop;
                    myAppUser.LedgerSubGroup = user.LedgerSubGroup;
                    myAppUser.ModifiedOn = DateUtility.GetNowTime();
                    UserRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Party info updated successfully");
                    return dict;
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Party not found");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "EditUser", user.Id);
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
            }
            return dict;
        }
        public List<MyAppUser> GetParty()
        {
            List<MyAppUser> users = UserRepo.ClientsList().ToList();

            return users;
        }


        public List<MyAppUser> GetPartys(int UserId)
        {
            List<MyAppUser> users = UserRepo.PartyList(UserId).ToList();

            return users;
        }
        public MyAppUser GetUserByMobile(string name)
        {
            return UserRepo.GetUserByMobile(name);
        }

        public List<TaxRate> GetTaxList(int UserId)
        {
            List<TaxRate> taxRates = taxRateRepository.GetTaxRates(UserId).ToList();
            return taxRates;
        }

        public async Task<Dictionary<string, object>> ReateTaxRate(TaxRate tax)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                TaxRate taxRate = new TaxRate();
                taxRate.TaxName = tax.TaxName;
                taxRate.TaxType = tax.TaxType;
                taxRate.Rate = tax.Rate;
                taxRate.CreatedBy = tax.CreatedBy;
                taxRate.Deleted = false;
                taxRateRepository.Insert(taxRate);
                taxRateRepository.Save();
                dict.Add("Status", true);
                dict.Add("Message", "tax added successfully");

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Add tax Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }


        public async Task<Dictionary<string, object>> EditTaxRate(TaxRate tax)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                TaxRate taxRate = taxRateRepository.GetById(tax.Id);
                if (taxRate != null)
                {
                    taxRate.Deleted = tax.Deleted;
                    taxRate.TaxName = tax.TaxName;
                    taxRate.TaxType = tax.TaxType;
                    taxRate.Rate = tax.Rate;
                    taxRateRepository.Save();

                    dict.Add("Status", true);
                    dict.Add("Message", "TaxRate info updated successfully");
                    return dict;
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "TaxRate not found");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "EditTax", tax.Id);
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
            }
            return dict;
        }


        public Dictionary<string, object> DeleteTaxRate(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                TaxRate tax = taxRateRepository.GetById(id);
                if (tax != null)
                {

                    tax.Deleted = true;
                    taxRateRepository.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Tax Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "tax Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "DeleteUser", id);
            }
            return dict;
        }

        public List<TaxGroup> GetTaxIdsforGrp(int UserId)
        {
            List<TaxGroup> taxGroups = groupRepository.GetTaxGoup(UserId).ToList();

            foreach (var taxgrpid in taxGroups)
            {
                // List<string> TagIdsa = taxgrpid.TaxRateIds.Split(',').ToList();

                List<string> taxRates = taxRateRepository.GetTaxRates(UserId).Where(a => taxgrpid.TaxRateIds.Contains(a.Id.ToString())).Select(d => d.TaxName).ToList();

                taxgrpid.TaxRatName = String.Join(",", taxRates);

            }

            return taxGroups;
        }


        public List<TaxGroup> GetTaxGrpList(int UserId)
        {
            List<TaxGroup> taxGroups = groupRepository.GetTaxGoup(UserId).ToList();

            return taxGroups;
        }


        public async Task<Dictionary<string, object>> AddTaxgrp(TaxGroup taxgrp)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                TaxGroup taxGroup = new TaxGroup();
                taxGroup.Name = taxgrp.Name;
                taxGroup.Deleted = false;
                taxGroup.CreatedBy = taxgrp.CreatedBy;
                taxGroup.TaxRateIds = taxgrp.TaxRateIds;
                groupRepository.Insert(taxGroup);
                groupRepository.Save();
                dict.Add("Status", true);
                dict.Add("Message", "tax added successfully");

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Add tax  Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }



        public TaxGroup GetTaxgrpById(int id)
        {
            try
            {
                return groupRepository.GettaxGrpById(id);
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "GetById", id);
                return null;
            }
        }


        public async Task<Dictionary<string, object>> EditTaxGrp(TaxGroup Grp, List<int> TaxId)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                TaxGroup taxGroup = groupRepository.GettaxGrpById(Grp.Id);
                if (taxGroup != null)
                {
                    taxGroup.Deleted = Grp.Deleted;
                    taxGroup.Name = Grp.Name;
                    string str = String.Join(",", TaxId);
                    taxGroup.TaxRateIds = str;
                    groupRepository.Save();

                    dict.Add("Status", true);
                    dict.Add("Message", "TaxRate info updated successfully");
                    return dict;
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "TaxRate not found");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "EditTax", Grp.Id);
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
            }
            return dict;
        }



        public Dictionary<string, object> DeleteTaxGrp(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                TaxGroup Grp = groupRepository.GettaxGrpById(id);
                if (Grp != null)
                {

                    Grp.Deleted = true;
                    taxRateRepository.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Tax Group Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "TaxGroupr Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "TaxGroupr", id);
            }
            return dict;
        }

        public async Task<Dictionary<string, object>> AddUnit(UnitName unit)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                UnitName unitName = new UnitName();
                unitName.Name = unit.Name;
                unitName.Deleted = false;
                unitName.CreatedBy = unit.CreatedBy;
                unitName.ShortName = unit.ShortName;
                uintRepository.Insert(unitName);
                groupRepository.Save();
                dict.Add("Status", true);
                dict.Add("Message", "Unit added successfully");

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Unit tax  Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }

        public List<UnitName> GetUnitList(int UserId)
        {
            List<UnitName> unit = uintRepository.GetUnits(UserId);

            return unit;
        }


        public UnitName GetUnitById(int id)
        {
            try
            {
                return uintRepository.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "GetById", id);
                return null;
            }

        }

        public async Task<Dictionary<string, object>> EditUnit(UnitName unit)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                UnitName unitName = uintRepository.GetById(unit.Id);
                if (unitName != null)
                {
                    unitName.Deleted = unit.Deleted;
                    unitName.Name = unit.Name;
                    unitName.ShortName = unit.ShortName;
                    uintRepository.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Unit info updated successfully");
                    return dict;
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "TaxRate not found");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "UnitEdit", unit.Id);
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
            }
            return dict;
        }
        public Dictionary<string, object> DeleteUnit(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                UnitName unit = uintRepository.GetById(id);
                if (unit != null)
                {

                    unit.Deleted = true;
                    uintRepository.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Unit Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Unit Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "Unit", id);
            }
            return dict;
        }

        public async Task<Dictionary<string, object>> AddUCRate(int Id, int FirstUnit, int SecondUnit, int Quantity, int CreatedBy, int AlternateUnitIsApplicables)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                UCRate uCRate = uCRatesRepository.GetRateById(Id);
                if (uCRate != null)
                {
                    uCRate.BaseUnitId = FirstUnit;
                    uCRate.SecondaryUnitId = SecondUnit;
                    uCRate.SUQuantity = Quantity;
                    // uCRate.CreatedBy = CreatedBy;
                    uCRate.AlternateUnitIsApplicables = AlternateUnitIsApplicables;
                    // uCRate.Deleted = false;
                    dict.Add("Ratedata", uCRate);
                }
                else
                {
                    UCRate uCRates = new UCRate();
                    uCRates.BaseUnitId = FirstUnit;
                    uCRates.SecondaryUnitId = SecondUnit;
                    uCRates.SUQuantity = Quantity;
                    uCRates.CreatedBy = CreatedBy;
                    uCRates.AlternateUnitIsApplicables = AlternateUnitIsApplicables;
                    uCRates.Deleted = false;
                    uCRatesRepository.Insert(uCRates);
                    dict.Add("Ratedata", uCRates);
                }

                uCRatesRepository.Save();
                dict.Add("Status", true);
                dict.Add("Message", "Unit added successfully");

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Unit tax  Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }

        public UCRate GetUcRate(int id)
        {
            UCRate uCRate = uCRatesRepository.GetRateById(id);
            return uCRate;

        }


        public async Task<Dictionary<string, object>> AddBanks(BankAccount bank)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                if (bank.Name !=null && bank.AccountNo != null )
                {
                    bank.CreatedOn = DateUtility.GetNowTime();
                    bank.Deleted = false;
                    bankRepository.Insert(bank);
                    bankRepository.Save();
                    dict.Add("Data", bank);
                    dict.Add("Status", true);
                    dict.Add("Message", "Bank added successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Bank not added");
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Add AddBanks  Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }
        public List<BankAccount> GetBankList(int UserId)
        {
            List<BankAccount> banks = bankRepository.BankList(UserId);
            return banks;
        }

        public BankAccount BankDetail(int id)
        {
            BankAccount bank = bankRepository.GetById(id);
            return bank;
        }
        public Dictionary<string, object> DeleteBank(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                BankAccount bank = bankRepository.GetById(id);
                if (bank != null)
                {

                    bank.Deleted = true;
                    bankRepository.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Bank Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Bank Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "Bank", id);
            }
            return dict;
        }
       
       
        public Sp_ProfitLoss GetProfitLost(int UserId, string FromDate, string ToDate)
        {
            Sp_ProfitLoss expenses = spRepository.GetSp_ProfitLoss(UserId, FromDate, ToDate);
            return expenses;
        }

        public SP_BalanceSheet GetBalanceSheet(int UserId, string FromDate, string ToDate)
        {
            SP_BalanceSheet expenses = spRepository.GetBalanceSheetDate(UserId, FromDate, ToDate);
            return expenses;
        }

        public Sp_TrialBalance GetSp_TrialBalance(int UserId, string FromDate, string ToDate)
        {
            Sp_TrialBalance sp_TrialBalance = spRepository.GetTrialBalance(UserId, FromDate, ToDate);
            return sp_TrialBalance;

        }

     
      

        // LedgerGroup

        public async Task<Dictionary<string, object>> AddLedgerGroup(LedgerGroup ledger)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                bool existed = ledgerGroupRepo.ledgerGroups(ledger.CreatedBy).Where(a => a.Name.ToLower() == ledger.Name.ToLower()).Any();

                if (existed != false)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Ledger group name already, please create with new name.");
                }
                else
                {
                    ledger.Deleted = false;
                    ledgerGroupRepo.Insert(ledger);
                    ledgerGroupRepo.Save();
                    dict.Add("Data", ledger);
                    dict.Add("Status", true);
                    dict.Add("Message", "Ledger group added successfully");
                }

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Add AddLedgerGroup Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }

        public LedgerGroup GetLedger(int id)
        {
            LedgerGroup ledgerGroups = ledgerGroupRepo.GetById(id);
            return ledgerGroups;
        }
        public async Task<Dictionary<string, object>> EditLedgerGroup(LedgerGroup ledger)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                bool existed = ledgerGroupRepo.ledgerGroups(ledger.CreatedBy).Where(a => a.Name.ToLower() == ledger.Name.ToLower() && a.Id != ledger.Id).Any();

                if (existed != false)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Ledger group name already please Edit with new name.");
                }
                else
                {
                    LedgerGroup ledgerGroup = ledgerGroupRepo.GetById(ledger.Id);

                    ledgerGroup.Name = ledger.Name;
                    ledgerGroupRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Ledger group Edit successfully");
                }

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Add EditLedgerGroup Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }

        public Dictionary<string, object> DeleteLedgerGroup(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                LedgerGroup ledger = ledgerGroupRepo.GetById(id);
                if (ledger != null)
                {
                    bool SubCat = ledgerSubGroupRepo.ledgerSubGroups(ledger.CreatedBy).Where(a => !a.Deleted && a.Id == id).Any();
                    if (SubCat != true)
                    {
                        ledger.Deleted = true;
                        ledgerGroupRepo.Save();
                        dict.Add("Status", true);
                        dict.Add("Message", "Ledger group Deleted Successfully");
                    }
                    else
                    {
                        dict.Add("Status", false);
                        dict.Add("Message", "Ledger group Useing Somewhere Can't delete");
                    }
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Ledger group Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "DeleteItemCategoty", id);
            }
            return dict;
        }

        public List<LedgerGroup> ledgerGroups(int UserId)
        {
            List<LedgerGroup> ledgerGroups = ledgerGroupRepo.ledgerGroups(UserId).ToList();
            return ledgerGroups;
        }


        // LedgerSubGroup

        public List<LedgerSubGroup> ledgerSubGroups(int UserId)
        {
            List<LedgerSubGroup> ledgerSubGroups = ledgerSubGroupRepo.ledgerSubGroups(UserId).ToList();
            return ledgerSubGroups;
        }

        public async Task<Dictionary<string, object>> AddLedgerSubGroup(LedgerSubGroup ledgerSub)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                bool existed = ledgerSubGroupRepo.ledgerSubGroups(ledgerSub.CreatedBy).Where(a => a.NameSubCat.ToLower() == ledgerSub.NameSubCat.ToLower() && a.ledgerCategoryId == ledgerSub.ledgerCategoryId).Any();
                if (existed != false)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Ledger SubGroup name already please create with new name.");
                }
                else
                {
                    ledgerSub.LedgerNameCat = ledgerGroupRepo.GetById(ledgerSub.ledgerCategoryId).Name;
                    ledgerSub.Deleted = false;
                    ledgerSubGroupRepo.Insert(ledgerSub);
                    ledgerSubGroupRepo.Save();
                    dict.Add("Deta", ledgerSub);
                    dict.Add("Status", true);
                    dict.Add("Message", "Ledger SubGroup added successfully");
                }

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "AddLedgerSubGroup Error");
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }

        public LedgerSubGroup GetSubLedger(int id)
        {
            LedgerSubGroup ledgerSubGroups = ledgerSubGroupRepo.GetById(id);
            return ledgerSubGroups;
        }
        public async Task<Dictionary<string, object>> EditLedgerSubGroup(LedgerSubGroup ledgerSub)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                bool existed = ledgerSubGroupRepo.ledgerSubGroups(ledgerSub.CreatedBy).Where(a => a.NameSubCat.ToLower() == ledgerSub.NameSubCat.ToLower() && a.Id != ledgerSub.Id && a.ledgerCategoryId == ledgerSub.ledgerCategoryId).Any();

                if (existed != false)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Ledger SubGroup name already please Edit with new name.");
                }
                else
                {
                    LedgerSubGroup ledger = ledgerSubGroupRepo.GetById(ledgerSub.Id);
                    ledger.LedgerNameCat = ledgerGroupRepo.GetById(ledgerSub.ledgerCategoryId).Name;
                    ledger.NameSubCat = ledgerSub.NameSubCat;
                    ledger.ledgerCategoryId = ledgerSub.ledgerCategoryId;
                    ledgerSubGroupRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Ledger SubGroup Category Edit successfully");
                }

            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "EditLedgerSubGroup Error", ledgerSub.Id);
                dict.Add("Status", false);
                dict.Add("Message", "Temperory server error occured. Please try after some time.");
            }
            return dict;
        }

        public Dictionary<string, object> DeleteLedgerSubGroup(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                LedgerSubGroup ledgerSub = ledgerSubGroupRepo.GetById(id);
                if (ledgerSub != null)
                {
                    ledgerSub.Deleted = true;
                    ledgerSubGroupRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Ledger SubGroup Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Ledger SubGroup Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "DeleteLedgerSubGroup", id);
            }
            return dict;
        }
        public async Task<Dictionary<string, object>> InvoiceNumber(Invoice_no invoice_No)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Invoice_no invoice1 = invoiceNoRepo.GetInvoice(invoice_No.CreatedBy,invoice_No.TransactionType);
            if (invoice1 != null)
            {
                try
                {
                    invoice1.AutoManula = invoice_No.AutoManula;
                    invoice1.InOutWord = invoice_No.InOutWord;
                    invoice1.IsIncludeMonth = invoice_No.IsIncludeMonth;
                    invoice1.IsIncludeYear = invoice_No.IsIncludeYear;
                    invoice1.StringSerialNo = invoice_No.StringSerialNo;
                    invoice1.WhenRepeatSerialNo = invoice_No.WhenRepeatSerialNo;
                    invoice1.Prefix = invoice_No.Prefix;
                    invoice1.IsMultipleShope = invoice_No.IsMultipleShope;
                    invoiceNoRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Invoice Update succsess full");
                }
                catch (Exception ex)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "An error occured on server. Please try again");
                    ErrorRepo.AddException(ex, "InvoiceNumber");
                }
            }
            else
            {
                try
                {
                    invoice_No.Deleted = false;
                    invoiceNoRepo.Insert(invoice_No);
                    invoiceNoRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Invoice set succsess full");
                }
                catch (Exception ex)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "An error occured on server. Please try again");
                    ErrorRepo.AddException(ex, "InvoiceNumber");
                }
            }

            return dict;

        }
       public Invoice_no GetInvoice_No (int createBy,int TrnxType)
        {
            Invoice_no invoice_No = invoiceNoRepo.GetInvoice(createBy, TrnxType);
            return invoice_No; 
        }
        public Invoice_no GetInvoiceDetail(int UserId)
        {
            Invoice_no invoice_No = invoiceNoRepo.GetById(UserId);
            return invoice_No;
        }
        public PayemtModeOption GetPaymentMode(int UserId)
        {
            PayemtModeOption payemtMode = payemtModeRepo.GetByUser(UserId);
            return payemtMode;
        }

        public async Task<Dictionary<string, object>> PaymentMode(PayemtModeOption payemt)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            PayemtModeOption payemtMode = payemtModeRepo.GetByUser(payemt.createdBy);
            if (payemtMode != null)
            {
                try
                {
                    payemtMode.IsCash = payemt.IsCash;
                    payemtMode.IsBank = payemt.IsBank;
                    payemtMode.IsWallet = payemt.IsWallet;
                    payemtMode.IsFinance = payemt.IsFinance;

                    payemtModeRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Payment Mode Update succsess full");
                }
                catch (Exception ex)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "An error occured on server. Please try again");
                    ErrorRepo.AddException(ex, "PaymentMode");
                }
            }
            else
            {
                try
                {
                    // payemt.Deleted = false;
                    payemtModeRepo.Insert(payemt);
                    payemtModeRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Payment Mode set succsess full");
                }
                catch (Exception ex)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "An error occured on server. Please try again");
                    ErrorRepo.AddException(ex, "PaymentMode");
                }
            }

            return dict;

        }
        public async Task<Dictionary<string, object>> AddShopes(Shopes shopes)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                shopes.Deleted = false;
                shopeRepo.Insert(shopes);
                shopeRepo.Save();
                dict.Add("Status", true);
                dict.Add("Message", "shopes create succsess full");
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "AddShopes");
            }
            return dict;

        }
        public List<Shopes> GetShopes(int UserId)
        {
            List<Shopes> shopes = shopeRepo.ShopList(UserId);
            return shopes;
        }
        public Dictionary<string, object> DeleteShope(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                Shopes shopes = shopeRepo.GetById(id);
                if (shopes != null)
                {
                    shopes.Deleted = true;
                    shopeRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", " Shopes Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Shopes Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "DeleteShope", id);
            }
            return dict;
        }
        // Descriptions
        public async Task<Dictionary<string, object>> AddDescription(Description description)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                if (description.Descriptions != null && description.Descriptions!= "")
                {
                    description.Deleted = false;
                    DescriptionRepo.Insert(description);
                    DescriptionRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", "Description saved successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Description con't be null ");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "AddDescription");
            }
            return dict;

        }
        public List<Description> GetDescription(int UserId)
        {
            List<Description> descriptions = DescriptionRepo.DescriptionList(UserId);
            return descriptions;
        }
        public Dictionary<string, object> DeleteDescription(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                Description description = DescriptionRepo.GetById(id);
                if (description != null)
                {
                    description.Deleted = true;
                    DescriptionRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", " Description Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "Description Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "DeleteDescription", id);
            }
            return dict;
        }
        private async Task<string> UploadedFile(MyAppUser model)
        {
            string uniqueFileName = null;
            if (model.ProfileImage != null)
            {

                string uploadsFolder = Path.Combine(Environment.WebRootPath, "Profileimages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public async Task<Dictionary<string, object>> DeleteFile(string file)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            {
                try
                {
                    string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Profileimages/");
                    var fileList = Directory.EnumerateFiles(fileDirectory, "*", SearchOption.AllDirectories).Select(Path.GetFileName);
                    // string fileDirectory = fileDirectory;
                    string webRootPath = Environment.WebRootPath;
                    var fileName = "";
                    fileName = file;
                    string fullPath = webRootPath + "/Profileimages/" + file;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        dict.Add("Status", true);
                        dict.Add("Message", "File deleted succsessfull");
                    }
                }
                catch (Exception ex)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "An error occured during File deleting");
                    ErrorRepo.AddException(ex, "DeleteFile");
                }
            }
            return dict;
        }

        public async Task<Dictionary<string,object>> CreateExtraCharge(ExtraCharge extraCharge)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            {
                try
                {
                    if(extraCharge.Name !=null ||extraCharge.Name!="")
                    {
                        extraCharge.Deleted = false;
                        extraChargeRepo.Insert(extraCharge);
                        extraChargeRepo.Save();
                        dict.Add("Status", true);
                        dict.Add("Message","ExtraCharge create succsessfull");
                    }
                }
                catch(Exception ex)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "An error occured during ExtraCharge create");
                    ErrorRepo.AddException(ex, "CreateExtraCharge");
                }
            }
            return dict;
        }


        public List<ExtraCharge> ExtraChargeList(int UserId)
        {
            List<ExtraCharge> extras = extraChargeRepo.ExtraChargeList(UserId);
            return extras;
        }
      public ExtraCharge GetExtraxchargeById(int id)
        {
            return extraChargeRepo.GetById(id);
        }

        public Dictionary<string, object> DeleteExtraCharge(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                ExtraCharge extra = extraChargeRepo.GetById(id);
                if (extra != null)
                {
                    extra.Deleted = true;
                    extraChargeRepo.Save();
                    dict.Add("Status", true);
                    dict.Add("Message", " ExtraCharge Deleted Successfully");
                }
                else
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "ExtraCharge Not Found");
                }
            }
            catch (Exception ex)
            {
                dict.Add("Status", false);
                dict.Add("Message", "An error occured on server. Please try again");
                ErrorRepo.AddException(ex, "DeleteExtraCharge", id);
            }
            return dict;
        }
        public List<ExtraCharge> ExtraChargeList(ref DataPagingModel TablePaging)
        {
            try
            {
                using (SqlConnection DB = new SqlConnection(ConfigurationObj.GetSection("ConnectionStrings").GetSection("MyConnection").Value.ToString()))
                {
                    string SortingFilter = " order by ExtraCharges.Id ";
                    #region Search Filter
                    string SearchString = string.Empty;
                    string mobile = string.Empty;
                    string searchtxt = string.Empty;
                    int UserId = 0;
                    int userId = 0;
                    string searchtxtyp = string.Empty;


                    userId = TablePaging.CurrentUserId;
                    SearchString = SearchString + "AND ExtraCharges.CreateBy= @UserId ";

                    foreach (var item in TablePaging.SearchParameter)
                    {
                        string value = item.Value.Trim();

                        if (item.Key.ToLower() == "name" && !String.IsNullOrEmpty(value))
                        {
                            searchtxt = "%" + value + "%";
                            SearchString = SearchString + " AND ExtraCharges.Name like @SearchTxt ";
                        }
                        else if (item.Key.ToLower() == "trnxtype" && !String.IsNullOrEmpty(value))
                        {
                            if (value == "All")
                            {
                                //searchtxt = "%" + value + "%";
                                SearchString = SearchString + "";
                            }
                            else
                            {

                                searchtxtyp = value;
                                SearchString = SearchString + " AND ExtraCharges.IncomeExpenses =  @Searchtxtyp ";

                            }
                        }
                    }

                    #endregion
                    #region Query :GetList
                    string CommandText = "SELECT * FROM(Select row_number() over(" + SortingFilter + ") as RowNum, ExtraCharges.CreateBy, ExtraCharges.Id, ExtraCharges.Name, ExtraCharges.Deleted, ExtraCharges.IncomeExpenses, ExtraCharges.HNS_Code,TaxRates.Rate as Rates from ExtraCharges left join TaxRates on ExtraCharges.GST=TaxRates.Id where ExtraCharges.Deleted = 0" + SearchString + " ) as tbl WHERE RowNum BETWEEN @RecordFrom AND @RecordTo";

                    #endregion

                    var parameters = new
                    {
                        UserId = userId,
                        RecordFrom = TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize) + 1,
                        RecordTo = TablePaging.PageSize + TablePaging.CurrentPageID.TableSkipRecord(TablePaging.PageSize),
                        Mobile = mobile,
                        SearchTxt = searchtxt,
                        Searchtxtyp= searchtxtyp
                    };



                    List<ExtraCharge> ModelList = DB.QuerySql<ExtraCharge>(CommandText, parameters).ToList();

                    CommandText = @"Select  COUNT(ExtraCharges.Id)
                                                     from ExtraCharges
                                                         where ExtraCharges.Deleted = 0 " + SearchString;

                    TablePaging.TotalRecords = DB.ExecuteScalarSql<int>(CommandText, parameters);
                    return ModelList;
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddException(ex, "Get Accountants list");
                return new List<ExtraCharge>();
            }

        }
    }
}
