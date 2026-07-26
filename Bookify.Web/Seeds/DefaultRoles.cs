namespace Bookify.Web.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            
            if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));

            if (!await roleManager.RoleExistsAsync(AppRoles.Archive))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Archive));

            if (!await roleManager.RoleExistsAsync(AppRoles.Reception))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Reception));
        }
    }
}