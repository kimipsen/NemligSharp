using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace NemligSharp;

public static class Extensions
{
    private const int PageSize = 25;
    public static async Task<OneOf<IList<Order>, ErrorResponse>> GetAllOrdersWithDetailsAsync(this INemligClient client, CancellationToken cancellationToken)
    {
        if (!client.IsReady) return new NotLoggedInErrorResponse();

        List<Order> responses = [];
        int currentPage = 0;
        int numberOfPages = 0;
        do
        {
            var orderHistory = await client.GetOrderHistoryAsync(currentPage * PageSize, PageSize, cancellationToken).ConfigureAwait(false);
            if (orderHistory.TryPickT0(out var orderHistoryResponse, out var orderHistoryErrorResponse))
            {
                numberOfPages = orderHistoryResponse.NumberOfPages;
                var orderDetails = await client.GetAllOrderDetailsAsync(orderHistoryResponse.Orders, cancellationToken).ConfigureAwait(false);

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

    public static async Task<OneOf<IList<Order>, ErrorResponse>> GetAllOrderDetailsAsync(this INemligClient client, IEnumerable<Order> orders, CancellationToken cancellationToken)
    {
        if (!client.IsReady) return new NotLoggedInErrorResponse();

        List<Order> responses = [];

        foreach (var order in orders)
        {
            var orderDetails = await client.GetOrderAsync(order.Id, cancellationToken).ConfigureAwait(false);
            if (orderDetails.TryPickT0(out var orderDetailsResponse, out var orderDetailsErrorResponse))
            {
                var orderWithDetails = order with { OrderDetails = orderDetailsResponse };
                responses.Add(orderWithDetails);
            }
            else
                return orderDetailsErrorResponse;
        }

        return responses.AsReadOnly();
    }
}
