using System;
using System.Threading.Tasks;
using RabbitWings.Orders;
using RabbitWings.Orders.Entities;
using RabbitWings.Catalog;
using RabbitWings.Account;
using RabbitWings.Core;

class Program
{
    static async Task Main(string[] args)
    {
        // Testando CreateTransaction
        Console.WriteLine("=== Testando CreateTransaction ===");
        var transactionRequest = new TransactionRequest
        {
            User = new UserDetails
            {
                MyObject = new MyObject
                {
                    Id = "f97916cb-af41-44d7-8956-68c5dd35d1e1",
                    Name = "Lizzy Powell"
                }
            },
            Sku = "2",
            ItemName = "AR Combat",
            Quantity = 3
        };

        await OrderService.CreateTransaction(
            transactionRequest,
            onSuccess: response => Console.WriteLine("CreateTransaction Success: " + response),
            onError: error => Console.WriteLine($"CreateTransaction Error {error.StatusCode}: {error.ErrorMessage}")
        );

        Console.WriteLine(); // Adding a blank line for separation

        // Testando GetAllCatalogs
        Console.WriteLine("=== Testando GetAllCatalogs ===");
        await CatalogService.GetAllCatalogs(
            onSuccess: response => Console.WriteLine("GetAllCatalogs Success: " + response),
            onError: error => Console.WriteLine($"GetAllCatalogs Error {error.StatusCode}: {error.ErrorMessage}")
        );

        Console.WriteLine(); // Adding a blank line for separation

        // Testando GetSpecificItem
        Console.WriteLine("=== Testando GetSpecificItem ===");
        string itemId = "668582e9-cb62-44e2-8dd3-9a20ace05380-storeItemCache";
        string itemType = "storeItemCache";
        int? itemSku = 2;
        string itemName = "AR Combat";

        await CatalogService.GetSpecificItem(
            itemId,
            itemType,
            itemSku,
            itemName,
            onSuccess: response => Console.WriteLine("GetSpecificItem Success: " + response),
            onError: error => Console.WriteLine($"GetSpecificItem Error {error.StatusCode}: {error.ErrorMessage}")
        );

        Console.WriteLine(); // Adding a blank line for separation

        // Testando RegisterAsync
        Console.WriteLine("=== Testando RegisterAsync ===");
        string firstName = "Lizzy";
        string lastName = "Powell";
        string registerEmail = "lizzy@example.com";
        string registerPassword = "securePassword123!";
        string gender = "Female";
        string nickname = "LizzyP";
        string picture = "profilePicUrl";
        string phone = "1234567890";
        string phoneAuth = "verified";
        string birthday = "01/01/1995";
        string country = "USA";

        await RegistrationService.RegisterAsync(
            firstName,
            lastName,
            registerEmail,
            registerPassword,
            gender,
            nickname,
            picture,
            phone,
            phoneAuth,
            birthday,
            country,
            onSuccess: response => Console.WriteLine("Register Success: " + response),
            onError: error => Console.WriteLine($"Register Error {error.StatusCode}: {error.ErrorMessage}")
        );

        Console.WriteLine(); // Adding a blank line for separation

        // Testando LoginAsync
        Console.WriteLine("=== Testando LoginAsync ===");
        string email = "lizzy@example.com";
        string password = "securePassword123!";

        await LoginService.LoginAsync(
            email,
            password,
            onSuccess: response => Console.WriteLine("Login Success: " + response),
            onError: error => Console.WriteLine($"Login Error {error.StatusCode}: {error.ErrorMessage}")
        );

        Console.WriteLine(); // Adding a blank line for separation
    }
}
