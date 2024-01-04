using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using OneOf;

namespace NemligSharp;

public class NemligClient : INemligClient
{
    public const string NemligBaseUrl = "https://www.nemlig.com/";
    private readonly FlurlClient _flurlClient = new(NemligBaseUrl);
    private CookieJar? _cookieJar = null;
    private LoginResponse? _loginResponse;

    [MemberNotNullWhen(true, nameof(_loginResponse))]
    public bool IsReady => _loginResponse != null;

    public async Task<OneOf<ILoginResponse, ErrorResponse>> LoginAsync(string userName, string password, CancellationToken cancellationToken)
    {
        if (IsReady) return _loginResponse;

        var payload = new LoginPayload(userName, password);
        var response = await _flurlClient
            .AllowAnyHttpStatus()
            .Request("webapi", "login")
            .WithCookies(out var jar)
            .PostJsonAsync(payload, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode == 404) return new NotFoundResponse();
        if (response.StatusCode != 200) return new UnknownErrorResponse();

        var jsonString = await response.GetStringAsync().ConfigureAwait(false);

        try {
            _loginResponse = JsonSerializer.Deserialize<LoginResponse>(jsonString);
        }
        catch (JsonException jexc)
        {
            return new JsonParseErrorResponse(jexc.Message);
        }

        _cookieJar = jar;

        return _loginResponse!;
    }

    public async Task<OneOf<ICurrentUserResponse, ErrorResponse>> GetCurrentUserAsync(CancellationToken cancellationToken) => await DoFlurlGetV2<CurrentUserResponse, ICurrentUserResponse>(["webapi", "user", "GetCurrentUser"], cancellationToken).ConfigureAwait(false);

    public async Task<OneOf<IOrderHistoryResponse, ErrorResponse>> GetOrderHistoryAsync(int skip, int take, CancellationToken cancellationToken) => await DoFlurlGetV2<OrderHistoryResponse, IOrderHistoryResponse>(["webapi", "order", "GetBasicOrderHistory"], cancellationToken, new { skip, take });

    public async Task<OneOf<IOrderResponse, ErrorResponse>> GetOrderAsync(int orderId, CancellationToken cancellationToken) => await DoFlurlGetV2<OrderResponse, IOrderResponse>(["webapi", "order", "GetOrderHistory"], cancellationToken, new { orderNumber = orderId });

    public async Task<OneOf<IShoppingListsResponse, ErrorResponse>> GetShoppingListsAsync(int skip, int take, CancellationToken cancellationToken) => await DoFlurlGetV2<ShoppingListsResponse, IShoppingListsResponse>(["webapi", "ShoppingList", "GetShoppingLists"], cancellationToken, new { skip, take });

    public async Task<OneOf<IShoppingListResponse, ErrorResponse>> GetShoppingListAsync(int shoppingListId, CancellationToken cancellationToken) => await DoFlurlGetV2<ShoppingListResponse, IShoppingListResponse>(["webapi", "ShoppingList", "GetShoppingList"], cancellationToken, new { listId = shoppingListId });

    public async Task<OneOf<ICurrentBasketResponse, ErrorResponse>> GetCurrentBasketAsync(CancellationToken cancellationToken) => await DoFlurlGetV2<CurrentBasketResponse, ICurrentBasketResponse>(["webapi", "basket", "GetBasket"], cancellationToken);

    private async Task<OneOf<TResponseInterface, ErrorResponse>> DoFlurlGetV2<TResponseImplementation, TResponseInterface>(string[] pathSegments, CancellationToken cancellationToken, object queryParameters = null)
        where TResponseImplementation : Response, TResponseInterface
        where TResponseInterface : IResponse
    {
        if (!IsReady) return new NotLoggedInErrorResponse();

        var response = await _flurlClient.Request(pathSegments).SetQueryParams(queryParameters).WithCookies(_cookieJar).GetAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        if (response.StatusCode == 404) return new NotFoundResponse();
        if (response.StatusCode != 200) return new UnknownErrorResponse();

        var jsonString = await response.GetStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<TResponseImplementation>(jsonString)!;
    }
}