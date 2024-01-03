using NemligSharp;

public class NemligClientTests
{
    // Can successfully login with valid credentials
    [Fact]
    public async Task test_login_with_valid_credentials()
    {
        // Arrange
        var client = new NemligClient();
        var userName = "valid_username";
        var password = "valid_password";

        // Act
        var result = await client.LoginAsync(userName, password);

        // Assert
        Assert.IsAssignableFrom<ILoginResponse>(result.Value);
    }

    // Can retrieve current user information
    [Fact]
    public async Task test_retrieve_current_user_information()
    {
        // Arrange
        var client = new NemligClient();
        await client.LoginAsync("valid_username", "valid_password");

        // Act
        var result = await client.GetCurrentUserAsync();

        // Assert
        Assert.IsAssignableFrom<ICurrentUserResponse>(result.Value);
    }

    // Can retrieve order history with valid parameters
    [Fact]
    public async Task test_retrieve_order_history_with_valid_parameters()
    {
        // Arrange
        var client = new NemligClient();
        await client.LoginAsync("valid_username", "valid_password");
        var skip = 0;
        var take = 10;

        // Act
        var result = await client.GetOrderHistoryAsync(skip, take);

        // Assert
        Assert.IsAssignableFrom<IOrderHistoryResponse>(result.Value);
    }

    // Login fails with invalid credentials
    [Fact]
    public async Task test_login_fails_with_invalid_credentials()
    {
        // Arrange
        var client = new NemligClient();
        var userName = "invalid_username";
        var password = "invalid_password";

        // Act
        var result = await client.LoginAsync(userName, password);

        // Assert
        Assert.IsType<UnknownErrorResponse>(result.Value);
    }

    // GetCurrentUserAsync fails when not logged in
    [Fact]
    public async Task test_get_current_user_fails_when_not_logged_in()
    {
        // Arrange
        var client = new NemligClient();

        // Act
        var result = await client.GetCurrentUserAsync();

        // Assert
        Assert.IsType<NotLoggedInErrorResponse>(result.Value);
    }

    // GetOrderHistoryAsync fails with invalid parameters
    [Fact]
    public async Task test_get_order_history_fails_with_invalid_parameters()
    {
        // Arrange
        var client = new NemligClient();
        await client.LoginAsync("valid_username", "valid_password");
        var skip = -1;
        var take = 0;

        // Act
        var result = await client.GetOrderHistoryAsync(skip, take);

        // Assert
        Assert.IsType<UnknownErrorResponse>(result.Value);
    }
}