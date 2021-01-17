using System;

namespace NemligSharp
{
	internal record Response(int StatusCode) : IResponse;
	internal record LoginResponse(int StatusCode, string RedirectUrl, bool MergeSuccessful, bool ZipCodeFiffers, int DeliveryZoneId, GdprSettings GdprSettings, bool IsExternalLogin, bool IsFirstLogin) : Response(StatusCode), ILoginResponse;
	internal record CurrentUserResponse(int StatusCode, string DebitorId, string Email, int MemberType, string MessageToDriver, Address DeliveryAddress, Address InvoiceAddress, DriverInformation DriverInformation, bool IsUnattendedTutorialShown, int DefaultDeliveryType, bool AddressesAreEqual, string EAN, string CVR) : Response(StatusCode), ICurrentUserResponse;
	internal record OrderHistoryResponse(int StatusCode, int NumberOfPages, Order[] Orders) : Response(StatusCode), IOrderHistoryResponse;
	internal record OrderResponse(int StatusCode, OrderLine[] Lines, string OrderNumber, decimal SubTotal, decimal DepositPrice, decimal ShippingPrice, decimal PackagingPrice, decimal TransactionFee, decimal TotalVatAmount, decimal Bonus, decimal AddedToAccount, decimal CouponDiscount, decimal TotalProductDiscountPrice, decimal TotalProductDiscount, decimal Total, CouponLine[] CouponLines, string Notes, string UnattendedNotes, string PlacementMessage, string DoorCode, int TimeslotDuration, int Id, string Email, int NumberOfProducts, int NumberOfPacks, int NumberOfDeposits) : Response(StatusCode), IOrderResponse;
	internal record ShoppingListsResponse(int StatusCode, ShoppingList[] ShoppingListOverViewViewModels, int NumberOfPages) : Response(StatusCode), IShoppingListsResponse;
	internal record ShoppingListResponse(int StatusCode, int Id, string Name, string Url, bool ContainsDeactivatedData, int ProductsCount, decimal TotalAmount, object[] ValidationFailures, Product[] Lines) : Response(StatusCode), IShoppingListResponse;
	internal record CurrentBasketResponse(int StatusCode, string Id, Guid BasketGuid, Address InvoiceAddress, Address DeliveryAddress) : Response(StatusCode), ICurrentBasketResponse;
}