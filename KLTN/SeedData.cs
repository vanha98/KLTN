using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KLTN
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<KLTNContext>();
            await context.Database.MigrateAsync();

            if (!context.Roles.Any())
            {
                await EnsureCreatedRole(services, "SinhVien");
                await EnsureCreatedRole(services, "GVHD");
                await EnsureCreatedRole(services, "QuanLy");
                await EnsureCreatedRole(services, "Administrators");
            }
           
            if (!context.Users.Any())
            {
                EnsureCreatedUser(services, "3116410022", "SinhVien", new Claim[]{
                    new Claim("Name", "Tạ Văn Hà"),
                });

                EnsureCreatedUser(services, "3116410024", "SinhVien", new Claim[]{
                    new Claim("Name", "Đinh Tuấn Hải"),
                });
                EnsureCreatedUser(services, "333641002", "GVHD", new Claim[]{
                    new Claim("Name", "Nguyễn Văn B"),
                });

                EnsureCreatedUser(services, "333641001", "GVHD", new Claim[]{
                    new Claim("Name", "Nguyễn Văn A"),
                });

                EnsureCreatedUser(services, "222641001", "QuanLy", new Claim[]{
                    new Claim("Name", "Quản lý 1"),
                });
            }
        }

        private static void EnsureCreatedUser(IServiceProvider services, string username, string role, Claim[] claims)
        {
            var userMgr = services.GetRequiredService<UserManager<AppUser>>();

            var user = userMgr.FindByNameAsync(username).Result;
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = username
                };
                var result = userMgr.CreateAsync(user, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddToRoleAsync(user, role).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(user, claims).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static async Task EnsureCreatedRole(IServiceProvider services, string role)
        {
            var roleMgr = services.GetRequiredService<RoleManager<AppRole>>();
            await roleMgr.CreateAsync(new AppRole() { Name=role});
        }
    }
}
