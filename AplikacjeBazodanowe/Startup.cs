using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AplikacjeBazodanowe.Data;
using AplikacjeBazodanowe.Data.FileManager;
using AplikacjeBazodanowe.Data.Repository;
using AplikacjeBazodanowe.Options;


namespace AplikacjeBazodanowe;
public class Startup
{
    private IConfiguration _config;
    public Startup(IConfiguration config)
    {
        _config = config;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(_config["DefaultConnection"]);
            });

        services.Configure<DefaultAdminOptions>(
            _config.GetSection(DefaultAdminOptions.DefaultAdmin)
        );

        services.AddTransient<IRepository, Repository>();
        services.AddTransient<IFileManager, FileManager>();

        services.AddIdentity<IdentityUser, IdentityRole>(options => {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
        })
            //.AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
        });
        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false;
            options.CacheProfiles.Add("Monthly", new CacheProfile { Duration = 30 * 24 * 60 * 60 }); // 30 * 24 * 60 * 60 = 30 Days
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseMvcWithDefaultRoute();
    }
}
