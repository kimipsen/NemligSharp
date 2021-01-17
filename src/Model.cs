using System;

namespace NemligSharp
{
	public record Order(string OrderNumber, decimal Total, int Id, DeliveryTime DeliveryTime, string DeliveryAddress, bool IsDeliveryOnWay, int DeliveryType);
	public record DeliveryTime(DateTime Start, DateTime End);
	public record OrderLine(string GroupName, string ProductNumber, string ProductName, string RecipeId, int Quantity, string Description, decimal AverageItemPrice, decimal Amount);
	public record GdprSettings(int NewslettersIntegrationId, int RecipesIntegrationId, int SmsNotificationsIntegrationId, int NemligAdsOnSearchEnginesIntegrationId, int NemligAdsOnOtherSitesIntegrationId, int SurveysIntegrationId);
	public record Address(string FirstName, string MiddleName, string LastName, string StreetName, int HouseNumber, string HouseNumberLetter, string Floor, string Side, string Door, int PostalCode, string PostalDistrict, string CompanyName, string MobileNumber, string PhoneNumber, string ContactPerson, bool IsEmptyAddress, string Name);
	public record DriverInformation(string AttendedDeliveryNote, string UnattendedDeliveryDoorCode, string UnattendedDeliveryNote, string UnattendedPlacementMessage);
	public record ShoppingList(int Id, string Name, string Url, bool ContainsDeactivatedData, int ProductsCount, decimal TotalAmount, int ProductCountInList);
	public record Product(decimal ItemPrice, decimal DiscountSavings, int Quantity, string PrimaryImage, bool IsProductDeactivated, decimal ProductsTotalAmount, Availability Availability, int GroupSortOrder, string GroupName, string Id, string Name, string Url, string UnitPrice, decimal UnitPriceCalc, string UnitPriceLabel, bool DiscountItem, string Description, decimal Price, string Campaign, string[] Labels, string ProductSubGroupNumber, string ProductSubGroupName, string ProductCategoryGroupNumber, string ProductCategoryGroupName, string ProductMainGroupNumber, string ProductMainGroupName);
	public record Availability(bool IsDeliveryAvailable, bool IsAvailableInStock, string[] ReasonMessageKeys);
	public record CouponLine(string Type, string Name, string CouponNumber);
}