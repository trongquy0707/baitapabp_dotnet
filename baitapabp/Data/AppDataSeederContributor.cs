using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace baitapabp.Data.Seed
{
    public class AppDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;

        public AppDataSeederContributor(
            IdentityUserManager userManager,
            IdentityRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var adminRole = await _roleManager.FindByNameAsync("admin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole(Guid.NewGuid(), "admin");
                await _roleManager.CreateAsync(adminRole);
            }

            var employeeRole = await _roleManager.FindByNameAsync("employee");
            if (employeeRole == null)
            {
                employeeRole = new IdentityRole(Guid.NewGuid(), "employee");
                await _roleManager.CreateAsync(employeeRole);
            }

            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new IdentityUser(
                    Guid.NewGuid(),
                    "admin",
                    "admin@local.com"

                );
                adminUser.SetEmailConfirmed(true);
                adminUser.SetIsActive(true); 
                

                var result = await _userManager.CreateAsync(adminUser, "123456aA@");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "admin");
                }
                else
                {
                    Console.WriteLine(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            if (adminUser != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(adminUser);
                await _userManager.ResetPasswordAsync(adminUser, token, "123456aA@");
            }

            var employeeUser = await _userManager.FindByNameAsync("employee");
            if (employeeUser == null)
            {
                employeeUser = new IdentityUser(
                    Guid.NewGuid(),
                    "employee",
                    "employee@local.com"
                );

                employeeUser.SetEmailConfirmed(true);

                var result = await _userManager.CreateAsync(employeeUser, "123456aA@");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(employeeUser, "employee");
                }
            }
        }
    }
}
