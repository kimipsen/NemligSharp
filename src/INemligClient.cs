using System.Threading.Tasks;

namespace NemligSharp;

public interface INemligClient
{
    // search (GET, QueryString, "sæbe"/"s%C3%A6be"): search?query=s%C3%A6be&take=20&skip=0&recipeCount=2&search=s%C3%A6be&sortorder=default
    //ISearchResponse SearchAsync(string searchString);

    // quick (GET, QueryString, "sæbe"): quick?query=s%C3%A6be&take=20&skip=0&recipeCount=2&

    Task<ILoginResponse> LoginAsync(string userName, string password);

    Task<ICurrentUserResponse> GetCurrentUserAsync();

    Task<IOrderHistoryResponse> GetOrderHistoryAsync(int skip, int take);

    Task<IOrderResponse> GetOrderAsync(int orderId);

    Task<IShoppingListsResponse> GetShoppingListsAsync(int skip, int take);

    Task<IShoppingListResponse> GetShoppingListAsync(int shoppingListId);

    Task<ICurrentBasketResponse> GetCurrentBasketAsync();
}