using FitnessOperationsApi.Models;

namespace FitnessOperationsApi.Data;

public static class DbSeeder
{
    public static async Task Seed(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                FullName = "System Admin",
                Email = "admin@gym.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin"
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}