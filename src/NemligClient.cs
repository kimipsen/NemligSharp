using System.Text.Json;
using System.Threading.Tasks;
using Flurl.Http;
using OneOf;

namespace NemligSharp;

public class NemligClient : INemligClient
{
    public const string NemligBaseUrl = "https://www.nemlig.com/";
    private readonly IFlurlClient _flurlClient = new FlurlClient(NemligBaseUrl);
    private CookieJar _cookieJar = null;

    public bool IsReady => _cookieJar != null;

    public async Task<OneOf<ILoginResponse, ErrorResponse>> LoginAsync(string userName, string password)
    {
        var payload = new LoginPayload(userName, password);
        var response = await _flurlClient.Request("webapi", "login").WithCookies(out var jar).PostJsonAsync(payload).ConfigureAwait(false);

        if (response.StatusCode == 404) return new NotFoundResponse();
        if (response.StatusCode != 200) return new UnknownErrorResponse();

        var jsonString = await response.GetStringAsync().ConfigureAwait(false);
        var deserialized = JsonSerializer.Deserialize<LoginResponse>(jsonString);

        _cookieJar = jar;

        return deserialized;
    }

    public async Task<OneOf<ICurrentUserResponse, ErrorResponse>> GetCurrentUserAsync() => await DoFlurlGetV2<CurrentUserResponse, ICurrentUserResponse>(["webapi", "user", "GetCurrentUser"]).ConfigureAwait(false);

    public async Task<OneOf<IOrderHistoryResponse, ErrorResponse>> GetOrderHistoryAsync(int skip, int take) => await DoFlurlGetV2<OrderHistoryResponse, IOrderHistoryResponse>(["webapi", "order", "GetBasicOrderHistory"], new { skip, take });

    public async Task<OneOf<IOrderResponse, ErrorResponse>> GetOrderAsync(int orderId) => await DoFlurlGetV2<OrderResponse, IOrderResponse>(["webapi", "order", "GetOrderHistory"], new { orderNumber = orderId });

    public async Task<OneOf<IShoppingListsResponse, ErrorResponse>> GetShoppingListsAsync(int skip, int take) => await DoFlurlGetV2<ShoppingListsResponse, IShoppingListsResponse>(["webapi", "ShoppingList", "GetShoppingLists"], new { skip, take });

    public async Task<OneOf<IShoppingListResponse, ErrorResponse>> GetShoppingListAsync(int shoppingListId) => await DoFlurlGetV2<ShoppingListResponse, IShoppingListResponse>(["webapi", "ShoppingList", "GetShoppingList"], new { listId = shoppingListId });

    public async Task<OneOf<ICurrentBasketResponse, ErrorResponse>> GetCurrentBasketAsync() => await DoFlurlGetV2<CurrentBasketResponse, ICurrentBasketResponse>(["webapi", "basket", "GetBasket"]);

    private async Task<OneOf<TResponseInterface, ErrorResponse>> DoFlurlGetV2<TResponseImplementation, TResponseInterface>(string[] pathSegments, object queryParameters = null)
        where TResponseImplementation : Response, TResponseInterface
        where TResponseInterface : IResponse
    {
        var response = await _flurlClient.Request(pathSegments).SetQueryParams(queryParameters).WithCookies(_cookieJar).GetAsync().ConfigureAwait(false);

        if (response.StatusCode == 404) return new NotFoundResponse();
        if (response.StatusCode != 200) return new UnknownErrorResponse();

        var jsonString = await response.GetStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<TResponseImplementation>(jsonString);
    }
}