using System;

namespace NemligSharp;

public interface IResponse
{
}

public interface ISearchResponse : IResponse
{
}

public interface ILoginResponse : IResponse
{
    // "TimeslotUtc": "2021011715-60-960",

    string RedirectUrl { get; }
    bool MergeSuccessful { get; }
    bool ZipCodeFiffers { get; }
    int DeliveryZoneId { get; }
    GdprSettings GdprSettings { get; }
    bool IsExternalLogin { get; }
    bool IsFirstLogin { get; }
}

public interface ICurrentUserResponse : IResponse
{
    string DebitorId { get; }
    string Email { get; }
    int MemberType { get; }
    string MessageToDriver { get; }
    Address DeliveryAddress { get; }
    Address InvoiceAddress { get; }
    DriverInformation DriverInformation { get; }
    bool IsUnattendedTutorialShown { get; }
    int DefaultDeliveryType { get; }
    bool AddressesAreEqual { get; }
    string EAN { get; }
    string CVR { get; }
}

public interface IOrderHistoryResponse : IResponse
{
    int NumberOfPages { get; }
    Order[] Orders { get; }
}

public interface IOrderResponse : IResponse
{
    OrderLine[] Lines { get; }
    string OrderNumber { get; }
    decimal SubTotal { get; }
    decimal DepositPrice { get; }
    decimal ShippingPrice { get; }
    decimal PackagingPrice { get; }
    decimal TransactionFee { get; }
    decimal TotalVatAmount { get; }
    decimal Bonus { get; }
    decimal AddedToAccount { get; }
    decimal CouponDiscount { get; }
    decimal TotalProductDiscountPrice { get; }
    decimal TotalProductDiscount { get; }
    decimal Total { get; }
    CouponLine[] CouponLines { get; }
    string Notes { get; }
    string UnattendedNotes { get; }
    string PlacementMessage { get; }
    string DoorCode { get; }
    int TimeslotDuration { get; }
    int Id { get; }
    string Email { get; }
    int NumberOfProducts { get; }
    int NumberOfPacks { get; }
    int NumberOfDeposits { get; }
}

public interface IShoppingListsResponse : IResponse
{
    int NumberOfPages { get; }
    ShoppingList[] ShoppingListOverViewViewModels { get; }
}

public interface IShoppingListResponse : IResponse
{
    int Id { get; }
    string Name { get; }
    string Url { get; }
    bool ContainsDeactivatedData { get; }
    int ProductsCount { get; }
    decimal TotalAmount { get; }
    object[] ValidationFailures { get; }
    Product[] Lines { get; }
}

public interface ICurrentBasketResponse : IResponse
{
    string Id { get; }
    Guid BasketGuid { get; }
    Address InvoiceAddress { get; }
    Address DeliveryAddress { get; }
}