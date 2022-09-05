using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AplikacjeBazodanowe.Data;
using AplikacjeBazodanowe.Options;

namespace AplikacjeBazodanowe;

public class Program
{
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();

    public static void Main(string[] args)
    {
        var host = CreateWebHostBuilder(args).Build();

        try
        {
            var scope = host.Services.CreateScope();

            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var defaultAdmin = scope.ServiceProvider.GetRequiredService<IOptions<DefaultAdminOptions>>().Value;

            ctx.Database.EnsureCreated();
            var adminRole = new IdentityRole(defaultAdmin.Role);

            if (!ctx.Roles.Any())
            {
                roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
            }

            if(!ctx.Users.Any(u => u.UserName == defaultAdmin.Username))
            {
                var adminUser = new IdentityUser
                {
                    UserName = defaultAdmin.Username,
                    Email = defaultAdmin.Email
                };

                var result = userMgr.CreateAsync(adminUser, defaultAdmin.Password).GetAwaiter().GetResult();
                userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        host.Run();
    }
}
