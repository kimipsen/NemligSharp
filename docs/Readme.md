## Sample

```C#
using NemligSharp;

NemligClient client = new();
await client.LoginAsync("ValidUsername", "ValidPassword");

var orders = await client.GetOrderHistoryAsync(0, 5);

orders.Switch(
    orderHistory => Console.WriteLine($"{orderHistory.Orders}"),
    error => Console.WriteLine($"{error.GetType()}")
);

Console.WriteLine("finished!");

```