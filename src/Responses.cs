using System;

namespace NemligSharp;

internal record Response() : IResponse;
internal record LoginResponse(string RedirectUrl, bool MergeSuccessful, bool ZipCodeFiffers, int DeliveryZoneId, GdprSettings GdprSettings, bool IsExternalLogin, bool IsFirstLogin) : Response(), ILoginResponse;
internal record CurrentUserResponse(string DebitorId, string Email, int MemberType, string MessageToDriver, Address DeliveryAddress, Address InvoiceAddress, DriverInformation DriverInformation, bool IsUnattendedTutorialShown, int DefaultDeliveryType, bool AddressesAreEqual, string EAN, string CVR) : Response(), ICurrentUserResponse;
internal record OrderHistoryResponse(int NumberOfPages, Order[] Orders) : Response(), IOrderHistoryResponse;
internal record OrderResponse(OrderLine[] Lines, string OrderNumber, decimal SubTotal, decimal DepositPrice, decimal ShippingPrice, decimal PackagingPrice, decimal TransactionFee, decimal TotalVatAmount, decimal Bonus, decimal AddedToAccount, decimal CouponDiscount, decimal TotalProductDiscountPrice, decimal TotalProductDiscount, decimal Total, CouponLine[] CouponLines, string Notes, string UnattendedNotes, string PlacementMessage, string DoorCode, int TimeslotDuration, int Id, string Email, int NumberOfProducts, int NumberOfPacks, int NumberOfDeposits) : Response(), IOrderResponse;
internal record ShoppingListsResponse(ShoppingList[] ShoppingListOverViewViewModels, int NumberOfPages) : Response(), IShoppingListsResponse;
internal record ShoppingListResponse(int Id, string Name, string Url, bool ContainsDeactivatedData, int ProductsCount, decimal TotalAmount, object[] ValidationFailures, Product[] Lines) : Response(), IShoppingListResponse;
internal record CurrentBasketResponse(string Id, Guid BasketGuid, Address InvoiceAddress, Address DeliveryAddress) : Response(), ICurrentBasketResponse;