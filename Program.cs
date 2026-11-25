using G_NET69_EF_Assignment_4.DbContexts;
using G_NET69_EF_Assignment_4.Operations;
using Microsoft.EntityFrameworkCore;

namespace G_NET69_EF_Assignment_4;

public static class Program
{
    public static void Main(string[] args)
    {
        using var context = new NationalBankDbContext();

        try
        {
            context.Database.Migrate();
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Could not connect to the database: {exception.Message}");
            Console.ResetColor();
            return;
        }

        var operations = new AssignmentOperations(context);
        operations.RunMenu();
    }
}
