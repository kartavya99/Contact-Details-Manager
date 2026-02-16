using ContactDetailsManager.Filters.ActionFilters;
using ContactsDetailsManager.Core.Domain.IdentityEntities;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepositoryContarcts;
using Respostiories;
using ServiceContracts;
using Services;

namespace ContactDetailsManager 
{ 
    public static class ConfigureServicesExtension
    {

        
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddTransient<ResponseHeaderActionFilter>();

            services.AddControllersWithViews(options =>
            {
                // options.Filters.Add<ResponseHeaderActionFilter>();

                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

                options.Filters.Add(new ResponseHeaderActionFilter(logger)
                {
                    Key = "My-Key-From-Global",
                    Value = "My-Value-From-Global",
                    Order = 2
                });
            });


            //add services into IoC container
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IpersonsRepository, PersonsRepository>();

            services.AddScoped<ICountriesGetterService, CountriesGetterService>();
            services.AddScoped<ICountriesAdderService, CountriesAdderService>();
            services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();

            services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();

            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();
            services.AddTransient<PersonsListActionFilter>();


            if (!env.IsEnvironment("Test"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }


            //Enable Identity in this project
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3; //Eg: AB12AB
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                // enforces authorization policy (user must be authenticated ) for all the action methods
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            // Data Source=(localdb)\ProjectModels;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

            services.AddHttpLogging(options => {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
                                        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
