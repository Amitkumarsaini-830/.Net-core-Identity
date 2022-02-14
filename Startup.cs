
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using com.aadviktech.IMS.Constant;
using com.aadviktech.IMS.Contract.Repository_Interfaces;
using com.aadviktech.IMS.Contract.UOW_Interfaces;
using com.aadviktech.IMS.DB;
using com.aadviktech.IMS.Filter;
using com.aadviktech.IMS.Repository.Repositories;
using com.aadviktech.IMS.Uow;
using com.aadviktech.IMS.web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace MLM.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configurationst = configuration;

            ApplicationGlobal.AppWebsiteUrl = Configurationst.GetSection("Appurl").Value.ToString();
        }
        public static IConfiguration Configurationst { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<MyAppUser, AppRoles>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<MyDBContext>().AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromSeconds(300); // .FromDays(1) ...
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(5);
            });

            services.AddDbContext<MyDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MyConnection"));
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
            });
            services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddMvcOptions(options =>
            //{
            //    options.EnableEndpointRouting = false;
            //});

            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
            });
            services.AddScoped<IIdentityProvider, IdentityProvider>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPasswordHasher<MyAppUser>, CustomPasswordHasher>();



            #region UOW_Resolver

            services.AddScoped<IUserUow, UserUow>();
            services.AddScoped<IAdminUOW, AdminUOW>();
            services.AddScoped<IItemUOw, ItemUow>();
            services.AddScoped<ITransactionsUOW, TransactionsUOW>();
            services.AddScoped<ILedgerUOW, LedgerUOW>();

            #endregion

            #region Repository_Resolver

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ITaxRateRepository, TaxRateRepository>();
            services.AddScoped<ITaxGroupRepository, TaxGroupRepository>();
            services.AddScoped<IUintRepository, UnitRepository>();
            services.AddScoped<IUCRatesRepository,UCRatesRepository>();
            services.AddScoped<ITransactionsRepository,TransactionsRepository>();
            services.AddScoped<ITransactionsLogsRepository, TransactionsLogsRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
           // services.AddScoped<IExpensesRepository, ExpensesRepository>();
            services.AddScoped<IExpensesCategoryRepository, ExpensesCategoryRepository>();
            services.AddScoped<ICategotyRepository, CategoryRepository>();
            services.AddScoped<ISpRepository, SpRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddScoped<IItemSubCategoryRepository, ItemSubCategoryRepository>();
            services.AddScoped<ILedgerGroupRepository, LedgerGroupRepository>();
            services.AddScoped<ILedgerSubGroupRepository, LedgerSubGroupRepositorycs>();
            services.AddScoped<IInvoiceNoRepository, InvoiceNoRepository>();
            services.AddScoped<ITransactionItemDetailRepository, TransactionItemDetailRepository>();
            services.AddScoped<IShopeRepo, ShopRepo>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            services.AddScoped<ICashRepository,CashRepository>();
            services.AddScoped<IPayemtModeOptionRepository, PayemtModeOptionRepository>();
            services.AddScoped<IDescriptionRepository, DescriptionRepository>();
            services.AddScoped<IWalletInfoRepository, WalletInfoRepository>();
            services.AddScoped<IBankTrnxInfosRepository, BankTrnxInfosRepository>();
            services.AddScoped<ICashTransactionRepository, CashTransactionRepository>();
            services.AddScoped<IFinanceInfoRepository, FinanceRepository>();
            services.AddScoped<IExtraChargeRepository,ExtraChargeRepository>();
            services.AddScoped<IExtraChargeDataRepository, ExtraChargeDataRepository>();
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpacc)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            //app.UseStaticFiles(new StaticFileOptions() { FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")) });
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });

            //here is where you set you accessor
            SessionUtility.SetHttpContextAccessor(httpacc);

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Account}/{action=Login}/{id?}");
            //});

            // so TempData is functional when tracking is disabled.
            
        }
    }
}
