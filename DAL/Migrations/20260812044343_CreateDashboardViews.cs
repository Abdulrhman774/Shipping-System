using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class CreateDashboardViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Dashboard Summary
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_DashboardSummary AS
                WITH ShipmentCounts AS (
                    SELECT COUNT(*) AS TotalShipments, 
                           SUM(CASE WHEN Status = 4 THEN 1 ELSE 0 END) AS ActiveDeliveries, 
                           SUM(CASE WHEN Status IN (1, 2) THEN 1 ELSE 0 END) AS PendingApprovals, 
                           SUM(CASE WHEN Status != 5 AND ShippingDate < DATEADD(day, - 3, GETDATE()) THEN 1 ELSE 0 END) AS DelayedShipments
                    FROM dbo.TbShipment
                    WHERE (CurrentState <> 3)
                )
                SELECT TotalShipments, ActiveDeliveries, PendingApprovals, DelayedShipments, 
                       CASE WHEN TotalShipments = 0 THEN 0 ELSE 
                           (SELECT COUNT(*) FROM TbShipment 
                            WHERE DeliveryDate IS NOT NULL AND DeliveryDate <= DATEADD(day, 3, ShippingDate) AND CurrentState != 3) * 100.0 / TotalShipments 
                       END AS EfficiencyRate
                FROM ShipmentCounts;
            ");

            // 2. Financials
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_Financials AS
                SELECT YEAR(ShippingDate) AS Year, MONTH(ShippingDate) AS Month, DATENAME(month, ShippingDate) AS MonthName, 
                       SUM(ShippingRate) AS TotalRevenue, SUM(PackageValue) AS TotalCost
                FROM dbo.TbShipment
                WHERE (CurrentState <> 3)
                GROUP BY YEAR(ShippingDate), MONTH(ShippingDate), DATENAME(month, ShippingDate);
            ");

            // 3. Monthly Shipments
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_MonthlyShipments AS
                SELECT YEAR(CreatedDate) AS ShipmentYear, MONTH(CreatedDate) AS ShipmentMonth, DATENAME(month, CreatedDate) AS MonthName, Status, 
                       CASE Status WHEN 1 THEN 'Confirmed' WHEN 2 THEN 'Processing' WHEN 3 THEN 'Quality Check' WHEN 4 THEN 'Dispatched' WHEN 5 THEN 'Delivered' ELSE 'Unknown' END AS StatusName, 
                       COUNT(*) AS TotalShipments, SUM(PackageValue) AS TotalValue, SUM(ShippingRate) AS TotalRevenue, AVG(ShippingRate) AS AverageRate
                FROM dbo.TbShipment
                WHERE (CurrentState <> 3)
                GROUP BY YEAR(CreatedDate), MONTH(CreatedDate), DATENAME(month, CreatedDate), Status;
            ");

            // 4. Monthly Volume
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_MonthlyVolume AS
                SELECT YEAR(ShippingDate) AS Year, MONTH(ShippingDate) AS Month, DATENAME(month, ShippingDate) AS MonthName, COUNT(*) AS ShipmentCount
                FROM dbo.TbShipment
                WHERE (CurrentState <> 3)
                GROUP BY YEAR(ShippingDate), MONTH(ShippingDate), DATENAME(month, ShippingDate);
            ");

            // 5. Recent Shipments
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_RecentShipments AS
                SELECT TOP (20) s.Id AS ShipmentId, s.TrackingNumber, COALESCE (rc.CityEName, rc.CityAName, 'Unknown') AS Destination, s.Status, 
                       CASE WHEN s.PackageValue > 1000 THEN 'High' WHEN s.PackageValue > 500 THEN 'Medium' ELSE 'Low' END AS Priority, 
                       COALESCE (s.StatusLastUpdatedAt, s.UpdatedDate, s.CreatedDate) AS LastUpdate, s.CreatedDate
                FROM dbo.TbShipment AS s 
                LEFT OUTER JOIN dbo.TbUserReceiver AS r ON s.ReceiverId = r.Id 
                LEFT OUTER JOIN dbo.TbCity AS rc ON r.CityId = rc.Id
                WHERE (s.CurrentState <> 3)
                ORDER BY s.CreatedDate DESC;
            ");

            // 6. Shipment Details
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_ShipmentDetails AS
                SELECT s.Id AS ShipmentId, s.TrackingNumber, s.ShippingDate, s.DeliveryDate, s.Weight, s.Width, s.Height, s.Length, s.PackageValue, s.ShippingRate, s.PaymentMethodId, s.UserSubscriptionId, s.ShippingPackagingId, s.ReferenceId, 
                       s.SenderId, u_s.Name AS SenderName, u_s.Email AS SenderEmail, u_s.Phone AS SenderPhone, u_s.Address AS SenderAddress, u_s.PostalCode AS SenderPostalCode, u_s.Contact AS SenderContact, 
                       u_s.OtherAddress AS SenderOtherAddress, u_s.IsDefaultAddress AS SenderIsDefault, c_s.Id AS SenderCityId, c_s.CityEName AS SenderCityName, c_s.CityAName AS SenderCityAName, co_s.Id AS SenderCountryId, 
                       co_s.CountryEName AS SenderCountryName, co_s.CountryAName AS SenderCountryAName, s.ReceiverId, u_r.Name AS ReceiverName, u_r.Email AS ReceiverEmail, u_r.Phone AS ReceiverPhone, 
                       u_r.Address AS ReceiverAddress, u_r.PostalCode AS ReceiverPostalCode, u_r.Contact AS ReceiverContact, u_r.OtherAddress AS ReceiverOtherAddress, u_r.IsDefaultAddress AS ReceiverIsDefault, c_r.Id AS ReceiverCityId, 
                       c_r.CityEName AS ReceiverCityName, c_r.CityAName AS ReceiverCityAName, co_r.Id AS ReceiverCountryId, co_r.CountryEName AS ReceiverCountryName, co_r.CountryAName AS ReceiverCountryAName, s.ShippingTypeId, 
                       st.ShippingTypeEName AS ShippingTypeName, st.ShippingTypeAName, st.ShippingFactor, pm.MethdAName AS PaymentMethodAName, pm.MethodEName AS PaymentMethodEName, pm.Commission AS PaymentCommission, 
                       sp.ShippingPackagingAname AS PackagingAName, sp.ShippingPackagingEname AS PackagingEName, s.CurrentState, s.CreatedDate, s.CreatedBy, s.UpdatedDate, s.UpdatedBy, s.Status, s.StatusLastUpdatedAt
                FROM dbo.TbShipment AS s 
                LEFT OUTER JOIN dbo.TbUserSender AS u_s ON s.SenderId = u_s.Id 
                LEFT OUTER JOIN dbo.TbCity AS c_s ON u_s.CityId = c_s.Id 
                LEFT OUTER JOIN dbo.TbCountry AS co_s ON c_s.CountryId = co_s.Id 
                LEFT OUTER JOIN dbo.TbUserReceiver AS u_r ON s.ReceiverId = u_r.Id 
                LEFT OUTER JOIN dbo.TbCity AS c_r ON u_r.CityId = c_r.Id 
                LEFT OUTER JOIN dbo.TbCountry AS co_r ON c_r.CountryId = co_r.Id 
                LEFT OUTER JOIN dbo.TbShippingType AS st ON s.ShippingTypeId = st.Id 
                LEFT OUTER JOIN dbo.TbPaymentMethod AS pm ON s.PaymentMethodId = pm.Id 
                LEFT OUTER JOIN dbo.TbShippingPackaging AS sp ON s.ShippingPackagingId = sp.Id;
            ");

            // 7. Shipments By Type
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_ShipmentsByType AS
                SELECT st.Id AS ShippingTypeId, st.ShippingTypeEName AS ShippingTypeName, s.Status, 
                       CASE s.Status WHEN 1 THEN 'Confirmed' WHEN 2 THEN 'Processing' WHEN 3 THEN 'Quality Check' WHEN 4 THEN 'Dispatched' WHEN 5 THEN 'Delivered' ELSE 'Unknown' END AS StatusName, 
                       COUNT(*) AS ShipmentCount, SUM(s.PackageValue) AS TotalPackageValue, SUM(s.ShippingRate) AS TotalShippingRate, AVG(s.ShippingRate) AS AverageShippingRate
                FROM dbo.TbShipment AS s 
                INNER JOIN dbo.TbShippingType AS st ON s.ShippingTypeId = st.Id
                WHERE (s.CurrentState <> 3)
                GROUP BY st.Id, st.ShippingTypeEName, s.Status;
            ");

            // 8. Shipment Stats
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_ShipmentStats AS
                SELECT Status, CASE Status WHEN 1 THEN 'Confirmed' WHEN 2 THEN 'Processing' WHEN 3 THEN 'Quality Check' WHEN 4 THEN 'Dispatched' WHEN 5 THEN 'Delivered' ELSE 'Unknown' END AS StatusName, 
                       COUNT(*) AS ShipmentCount, SUM(PackageValue) AS TotalPackageValue, SUM(ShippingRate) AS TotalShippingRate, AVG(ShippingRate) AS AverageShippingRate, MIN(CreatedDate) AS OldestShipment, MAX(CreatedDate) AS LatestShipment
                FROM dbo.TbShipment
                WHERE (CurrentState <> 3)
                GROUP BY Status;
            ");

            // 9. Shipment Status Distribution
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_ShipmentStatusDistribution AS
                SELECT Status, COUNT(*) AS Count
                FROM dbo.TbShipment
                WHERE (CurrentState <> 3)
                GROUP BY Status;
            ");

            // 10. Top Shippers
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.vw_TopShippers AS
                SELECT TOP (5) s.SenderId, u.Name AS SenderName, COUNT(s.Id) AS ShipmentCount, SUM(s.ShippingRate) AS TotalRevenue
                FROM dbo.TbShipment AS s 
                INNER JOIN dbo.TbUserSender AS u ON s.SenderId = u.Id
                WHERE (s.CurrentState <> 3)
                GROUP BY s.SenderId, u.Name
                ORDER BY TotalRevenue DESC;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_DashboardSummary;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_Financials;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_MonthlyShipments;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_MonthlyVolume;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_RecentShipments;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_ShipmentDetails;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_ShipmentsByType;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_ShipmentStats;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_ShipmentStatusDistribution;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_TopShippers;");
        }
    }
}