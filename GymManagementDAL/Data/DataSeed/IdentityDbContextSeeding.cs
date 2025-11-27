using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {

                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return false;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
               {
                   new IdentityRole("SuperAdmin"),
                   new IdentityRole("Admin")
               };
                    foreach (var Role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(Role.Name!).Result)
                        {
                            roleManager.CreateAsync(Role).Wait();
                        }
                    }
                }  

                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Mahmoud",
                        LastName = "Gamal",
                        UserName = "MahmoudGamal",
                        Email = "MahmoudGamal@gmail.com",
                        PhoneNumber = "01123456789",

                    };
                    userManager.CreateAsync(MainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin01 = new ApplicationUser()
                    {
                        FirstName = "Ali",
                        LastName = "Ahmed",
                        UserName = "AliAhmed",
                        Email = "AliAhmed@gmail.com",
                        PhoneNumber = "01232589652"
                    };
                    userManager.CreateAsync(Admin01, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin01, "Admin").Wait();

                }
                return true;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
            }
        }

    }


}
