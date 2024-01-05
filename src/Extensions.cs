using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace NemligSharp;

public static class Extensions
{
    private const int PageSize = 25;
    public static async Task<OneOf<IList<IOrderResponse>, ErrorResponse>> GetAllOrderDetailsAsync(this INemligClient client, CancellationToken cancellationToken)
    {
        if (!client.IsReady) return new List<IOrderResponse>().AsReadOnly();

        List<IOrderResponse> responses = [];
        int currentPage = 0;
        int numberOfPages = 0;
        do
        {
            var orderHistory = await client.GetOrderHistoryAsync(currentPage * PageSize, PageSize, cancellationToken).ConfigureAwait(false);
            if (orderHistory.TryPickT0(out var orderHistoryResponse, out var orderHistoryErrorResponse))
            {
                numberOfPages = orderHistoryResponse.NumberOfPages;
                var orderDetails = await client.GetAllOrderDetailsAsync(orderHistoryResponse.Orders.Select(o => o.Id), cancellationToken).ConfigureAwait(false);

                if (orderDetails.TryPickT0(out var orderDetailsResponse, out var orderDetailsErrorResponse))
                    responses.AddRange(orderDetailsResponse);
                else
                    return orderDetailsErrorResponse;
            }
            else
                return orderHistoryErrorResponse;
            currentPage++;
        } while (currentPage < numberOfPages);

        return responses.AsReadOnly();
    }

    public static async Task<OneOf<IList<IOrderResponse>, ErrorResponse>> GetAllOrderDetailsAsync(this INemligClient client, IEnumerable<int> orderIds, CancellationToken cancellationToken)
    {
        if (!client.IsReady) return new List<IOrderResponse>().AsReadOnly();

        List<IOrderResponse> responses = [];

        foreach (var orderId in orderIds)
        {
            var orderDetails = await client.GetOrderAsync(orderId, cancellationToken).ConfigureAwait(false);
            if (orderDetails.TryPickT0(out var orderDetailsResponse, out var orderDetailsErrorResponse))
                responses.Add(orderDetailsResponse);
            else
                return orderDetailsErrorResponse;
        }

        return responses.AsReadOnly();
    }
}
