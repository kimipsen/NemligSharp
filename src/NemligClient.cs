using System.Text.Json;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace NemligSharp
{
	public class NemligClient : INemligClient
	{
		public const string NemligBaseUrl = "https://www.nemlig.com/";
		private readonly IFlurlClient _flurlClient;
		private CookieJar _cookieJar = null;

		public NemligClient(IFlurlClientFactory flurlClientFactory)
		{
			_flurlClient = flurlClientFactory.Get(NemligBaseUrl);
		}

		public async Task<ILoginResponse> LoginAsync(string userName, string password)
		{
			var payload = new LoginPayload(userName, password);
			var response = await _flurlClient.Request(new string[] { "webapi", "login" }).WithCookies(out var jar).PostJsonAsync(payload).ConfigureAwait(false);
			var jsonString = await response.GetStringAsync().ConfigureAwait(false);
			var deserialized = JsonSerializer.Deserialize<LoginResponse>(jsonString);

			_cookieJar = jar;

			return deserialized with { StatusCode = response.StatusCode };
		}

		public async Task<ICurrentUserResponse> GetCurrentUserAsync()
		{
			return await DoFlurlGet<CurrentUserResponse, ICurrentUserResponse>(new string[] { "webapi", "user", "GetCurrentUser" }).ConfigureAwait(false);
		}

		public async Task<IOrderHistoryResponse> GetOrderHistoryAsync(int skip, int take)
		{
			return await DoFlurlGet<OrderHistoryResponse, IOrderHistoryResponse>(new string[] { "webapi", "order", "GetBasicOrderHistory" }, new { skip, take });
		}

		public async Task<IOrderResponse> GetOrderAsync(int orderId)
		{
			return await DoFlurlGet<OrderResponse, IOrderResponse>(new string[] { "webapi", "order", "GetOrderHistory" }, new { orderNumber = orderId });
		}

		public async Task<IShoppingListsResponse> GetShoppingListsAsync(int skip, int take)
		{
			return await DoFlurlGet<ShoppingListsResponse, IShoppingListsResponse>(new string[] { "webapi", "ShoppingList", "GetShoppingLists" }, new { skip, take });
		}

		public async Task<IShoppingListResponse> GetShoppingListAsync(int shoppingListId)
		{
			return await DoFlurlGet<ShoppingListResponse, IShoppingListResponse>(new string[] { "webapi", "ShoppingList", "GetShoppingList" }, new { listId = shoppingListId });
		}

		public async Task<ICurrentBasketResponse> GetCurrentBasketAsync()
		{
			return await DoFlurlGet<CurrentBasketResponse, ICurrentBasketResponse>(new string[] { "webapi", "basket", "GetBasket" });
		}

		private async Task<TResponseInterface> DoFlurlGet<TResponseImplementation, TResponseInterface>(string[] pathSegments, object queryParameters = null)
			where TResponseImplementation : Response, TResponseInterface
			where TResponseInterface : IResponse
		{
			var response = await _flurlClient.Request(pathSegments).SetQueryParams(queryParameters).WithCookies(_cookieJar).GetAsync().ConfigureAwait(false);
			var jsonString = await response.GetStringAsync().ConfigureAwait(false);
			var deserialized = JsonSerializer.Deserialize<TResponseImplementation>(jsonString);

			return deserialized with { StatusCode = response.StatusCode };
		}
	}
}