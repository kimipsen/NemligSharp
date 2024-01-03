using System.Threading.Tasks;
using OneOf;

namespace NemligSharp;

public interface INemligClient
{
    // search (GET, QueryString, "sæbe"/"s%C3%A6be"): search?query=s%C3%A6be&take=20&skip=0&recipeCount=2&search=s%C3%A6be&sortorder=default
    //ISearchResponse SearchAsync(string searchString);

    // quick (GET, QueryString, "sæbe"): quick?query=s%C3%A6be&take=20&skip=0&recipeCount=2&

    bool IsReady { get; }

    Task<OneOf<ILoginResponse, ErrorResponse>> LoginAsync(string userName, string password);

    Task<OneOf<ICurrentUserResponse, ErrorResponse>> GetCurrentUserAsync();

    Task<OneOf<IOrderHistoryResponse, ErrorResponse>> GetOrderHistoryAsync(int skip, int take);

    Task<OneOf<IOrderResponse, ErrorResponse>> GetOrderAsync(int orderId);

    Task<OneOf<IShoppingListsResponse, ErrorResponse>> GetShoppingListsAsync(int skip, int take);

    Task<OneOf<IShoppingListResponse, ErrorResponse>> GetShoppingListAsync(int shoppingListId);

    Task<OneOf<ICurrentBasketResponse, ErrorResponse>> GetCurrentBasketAsync();
}