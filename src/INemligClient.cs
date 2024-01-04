using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace NemligSharp;

public interface INemligClient
{
    // search (GET, QueryString, "sæbe"/"s%C3%A6be"): search?query=s%C3%A6be&take=20&skip=0&recipeCount=2&search=s%C3%A6be&sortorder=default
    //ISearchResponse SearchAsync(string searchString);

    // quick (GET, QueryString, "sæbe"): quick?query=s%C3%A6be&take=20&skip=0&recipeCount=2&

    bool IsReady { get; }

    Task<OneOf<ILoginResponse, ErrorResponse>> LoginAsync(string userName, string password, CancellationToken cancellationToken);

    Task<OneOf<ICurrentUserResponse, ErrorResponse>> GetCurrentUserAsync(CancellationToken cancellationToken);

    Task<OneOf<IOrderHistoryResponse, ErrorResponse>> GetOrderHistoryAsync(int skip, int take, CancellationToken cancellationToken);

    Task<OneOf<IOrderResponse, ErrorResponse>> GetOrderAsync(int orderId, CancellationToken cancellationToken);

    Task<OneOf<IShoppingListsResponse, ErrorResponse>> GetShoppingListsAsync(int skip, int take, CancellationToken cancellationToken);

    Task<OneOf<IShoppingListResponse, ErrorResponse>> GetShoppingListAsync(int shoppingListId, CancellationToken cancellationToken);

    Task<OneOf<ICurrentBasketResponse, ErrorResponse>> GetCurrentBasketAsync(CancellationToken cancellationToken);
}