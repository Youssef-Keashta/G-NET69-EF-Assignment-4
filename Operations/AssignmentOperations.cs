using System.Globalization;
using System.Net.Mail;
using G_NET69_EF_Assignment_4.DbContexts;
using G_NET69_EF_Assignment_4.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET69_EF_Assignment_4.Operations;

public sealed class AssignmentOperations
{
    private readonly NationalBankDbContext _context;

    public AssignmentOperations(NationalBankDbContext context)
    {
        _context = context;
    }

    public void RunMenu()
    {
        while (true)
        {
            try
            {
                ShowMenu();
                int choice = ReadInt("  Enter choice: ", 0, 5);
                if (choice == 0)
                {
                    return;
                }

                switch (choice)
                {
                    case 1:
                        AddCustomer();
                        break;
                    case 2:
                        OpenAccount();
                        break;
                    case 3:
                        UpdateAccountStatus();
                        break;
                    case 4:
                        RemoveAccountFromCustomer();
                        break;
                    case 5:
                        ListCustomersWithAccounts();
                        break;
                }

                PauseForMenu();
            }
            catch (EndOfStreamException)
            {
                return;
            }
            catch (Exception exception)
            {
                WriteColored($"\nThe operation could not be completed: {exception.Message}", ConsoleColor.Red);
                try
                {
                    PauseForMenu();
                }
                catch (EndOfStreamException)
                {
                    return;
                }
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("    National Bank - Management");
        Console.WriteLine("========================================");
        Console.WriteLine("  1) Add a new Customer");
        Console.WriteLine("  2) Open a new Account for a Customer");
        Console.WriteLine("  3) Update Account Status (Active / Closed)");
        Console.WriteLine("  4) Remove an Account from a Customer");
        Console.WriteLine("  5) List all Customers (with accounts)");
        Console.WriteLine("  0) Exit");
        Console.WriteLine("----------------------------------------");
    }

    private void AddCustomer()
    {
        Console.WriteLine("\n--- Add New Customer ---");
        string fullName = ReadNonEmpty("Full Name      : ");
        string nationalId = ReadNationalId();
        DateTime dateOfBirth = ReadDateOfBirth();
        string email = ReadEmail();
        string phoneNumber = ReadPhoneNumber();
        string address = ReadNonEmpty("Address        : ");

        Console.WriteLine("Customer Type:");
        Console.WriteLine("    1) Individual");
        Console.WriteLine("    2) Business");
        CustomerType customerType = ReadInt("  Choice: ", 1, 2) == 1
            ? CustomerType.Individual
            : CustomerType.Business;

        if (_context.Customers.Any(customer => customer.NationalId == nationalId))
        {
            WriteColored("A customer with that National ID already exists.", ConsoleColor.Red);
            return;
        }

        var customer = new Customer
        {
            FullName = fullName,
            NationalId = nationalId,
            DateOfBirth = dateOfBirth,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address,
            CustomerType = customerType
        };

        _context.Customers.Add(customer);
        _context.SaveChanges();
        WriteColored($"\nCustomer created successfully. CustomerId = {customer.Id}", ConsoleColor.Green);
    }

    private void OpenAccount()
    {
        Console.WriteLine("\n--- Open New Account ---");
        string accountNumber = ReadNonEmpty("Account Number : ");
        if (accountNumber.Length > 30)
        {
            WriteColored("Account numbers must be 30 characters or fewer.", ConsoleColor.Red);
            return;
        }

        Console.WriteLine("Account Type:");
        Console.WriteLine("    1) Savings");
        Console.WriteLine("    2) Current");
        Console.WriteLine("    3) Business");
        AccountType accountType = ReadInt("  Choice: ", 1, 3) switch
        {
            1 => AccountType.Savings,
            2 => AccountType.Current,
            _ => AccountType.Business
        };

        string branchCode = ReadNonEmpty("Branch Code    : ");
        int customerId = ReadInt("Customer Id    : ", 1, int.MaxValue);

        Console.WriteLine("Ownership Role:");
        Console.WriteLine("    1) Primary");
        Console.WriteLine("    2) CoHolder");
        OwnershipType ownershipType = ReadInt("  Choice: ", 1, 2) == 1
            ? OwnershipType.Primary
            : OwnershipType.CoHolder;

        WriteColored($"Validating branch '{branchCode}' and customer #{customerId}...", ConsoleColor.Cyan);
        bool branchExists = _context.Branches.Any(branch => branch.Code == branchCode);
        bool customerExists = _context.Customers.Any(customer => customer.Id == customerId);
        if (!branchExists || !customerExists)
        {
            WriteColored(!branchExists
                ? $"Branch '{branchCode}' was not found."
                : $"Customer #{customerId} was not found.", ConsoleColor.Red);
            return;
        }

        if (_context.Accounts.Any(account => account.AccountNumber == accountNumber))
        {
            WriteColored($"Account '{accountNumber}' already exists.", ConsoleColor.Red);
            return;
        }

        var account = new Account
        {
            AccountNumber = accountNumber,
            AccountType = accountType,
            CurrentBalance = 0m,
            OpeningDate = DateTime.Today,
            BranchCode = branchCode,
            CustomerAccounts =
            {
                new CustomerAccount
                {
                    CustomerId = customerId,
                    OwnershipType = ownershipType,
                    OwnershipStartDate = DateTime.Today,
                    AccountStatus = AccountStatus.Active
                }
            }
        };

        _context.Accounts.Add(account);
        _context.SaveChanges();
        WriteColored(
            $"Account '{accountNumber}' created and linked to customer {customerId} as {ownershipType} owner.",
            ConsoleColor.Green);
    }

    private void UpdateAccountStatus()
    {
        Console.WriteLine("\n--- Update Account Status ---");
        string accountNumber = ReadNonEmpty("Account Number : ");
        int customerId = ReadInt("Customer Id    : ", 1, int.MaxValue);

        Console.WriteLine("New Status:");
        Console.WriteLine("    1) Active");
        Console.WriteLine("    2) Closed");
        AccountStatus newStatus = ReadInt("  Choice: ", 1, 2) == 1
            ? AccountStatus.Active
            : AccountStatus.Closed;

        var customerAccount = _context.CustomerAccounts.Find(customerId, accountNumber);
        if (customerAccount is null)
        {
            WriteColored("The customer-account link was not found.", ConsoleColor.Red);
            return;
        }

        customerAccount.AccountStatus = newStatus;
        _context.SaveChanges();
        WriteColored($"Status updated to {newStatus}.", ConsoleColor.Green);
    }

    private void RemoveAccountFromCustomer()
    {
        Console.WriteLine("\n--- Remove Account From Customer ---");
        string accountNumber = ReadNonEmpty("Account Number : ");
        int customerId = ReadInt("Customer Id    : ", 1, int.MaxValue);

        var customerAccount = _context.CustomerAccounts
            .Include(link => link.Account)
            .FirstOrDefault(link => link.AccountNumber == accountNumber && link.CustomerId == customerId);
        if (customerAccount is null)
        {
            WriteColored("The customer-account link was not found.", ConsoleColor.Red);
            return;
        }

        var account = customerAccount.Account;
        _context.CustomerAccounts.Remove(customerAccount);
        _context.SaveChanges();
        WriteColored("  Ownership link deleted.", ConsoleColor.Green);

        bool hasOtherOwners = _context.CustomerAccounts.Any(link => link.AccountNumber == accountNumber);
        if (!hasOtherOwners)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
            WriteColored($"      That was the last owner - account '{accountNumber}' was also removed.", ConsoleColor.Cyan);
        }
    }

    private void ListCustomersWithAccounts()
    {
        Console.WriteLine("\n--- All Customers ---\n");
        var customers = _context.Customers
            .Include(customer => customer.CustomerAccounts)
                .ThenInclude(link => link.Account)
                    .ThenInclude(account => account.Branch)
            .OrderBy(customer => customer.Id)
            .ToList();

        foreach (var customer in customers)
        {
            Console.WriteLine($"  #{customer.Id} {customer.FullName} ({customer.CustomerType})");
            if (customer.CustomerAccounts.Count == 0)
            {
                Console.WriteLine("      (no accounts)");
                continue;
            }

            foreach (var link in customer.CustomerAccounts.OrderBy(item => item.AccountNumber))
            {
                Account account = link.Account;
                Console.WriteLine(
                    $"      {account.AccountNumber,-12} {account.AccountType,-8} " +
                    $"Balance: {account.CurrentBalance,12:N2}   {link.OwnershipType,-8} " +
                    $"{link.AccountStatus,-6}   @ {account.Branch.Name}");
            }
        }
    }

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = ReadLine();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Please enter a value.");
        }
    }

    private static string ReadNationalId()
    {
        while (true)
        {
            string nationalId = ReadNonEmpty("National ID    : ");
            bool validLength = nationalId.Length == 11 || nationalId.Length == 14;
            if (validLength && nationalId.All(char.IsDigit))
            {
                return nationalId;
            }

            Console.WriteLine("National ID must contain 11 or 14 digits.");
        }
    }

    private static DateTime ReadDateOfBirth()
    {
        while (true)
        {
            Console.Write("Date of Birth  : (yyyy-MM-dd) ");
            string value = ReadLine();
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dateOfBirth) && dateOfBirth.Date <= DateTime.Today)
            {
                return dateOfBirth;
            }

            Console.WriteLine("Enter a valid date in yyyy-MM-dd format that is not in the future.");
        }
    }

    private static string ReadEmail()
    {
        while (true)
        {
            string email = ReadNonEmpty("Email          : ");
            if (MailAddress.TryCreate(email, out MailAddress? address) && address.Address == email)
            {
                return email;
            }

            Console.WriteLine("Please enter a valid email address.");
        }
    }

    private static string ReadPhoneNumber()
    {
        while (true)
        {
            string phoneNumber = ReadNonEmpty("Phone          : ");
            string digits = new(phoneNumber.Where(char.IsDigit).ToArray());
            if (digits.Length >= 7 && digits.Length <= 15 && digits.Length == phoneNumber.Length)
            {
                return phoneNumber;
            }

            Console.WriteLine("Phone number must contain 7 to 15 digits.");
        }
    }

    private static int ReadInt(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = ReadLine();
            if (int.TryParse(value, out int number) && number >= minimum && number <= maximum)
            {
                return number;
            }

            Console.WriteLine($"Please enter a number from {minimum} to {maximum}.");
        }
    }

    private static string ReadLine()
    {
        return Console.ReadLine() ?? throw new EndOfStreamException();
    }

    private static void PauseForMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to return to the menu...");
        if (Console.IsInputRedirected)
        {
            _ = Console.ReadLine();
        }
        else
        {
            Console.ReadKey(intercept: true);
        }

        Console.WriteLine();
    }

    private static void WriteColored(string message, ConsoleColor color)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }
        finally
        {
            Console.ForegroundColor = originalColor;
        }
    }
}
