// DAL/Configurations/Views/VwShipmentDetailsConfiguration.cs
using Domain.Entities.Views;
using Domain.Entities.Views.Shipment.Statistics_Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Views
{
    public class VwShipmentDetailsConfiguration : IEntityTypeConfiguration<vw_ShipmentDetails>
    {
        public void Configure(EntityTypeBuilder<vw_ShipmentDetails> builder)
        {
            builder.HasNoKey();
            builder.ToView("vw_ShipmentDetails");


            // Mapping الأعمدة
            builder.Property(e => e.ShipmentId).HasColumnName("ShipmentId");
            builder.Property(e => e.TrackingNumber).HasColumnName("TrackingNumber");
            builder.Property(e => e.ShippingDate).HasColumnName("ShippingDate");
            builder.Property(e => e.DeliveryDate).HasColumnName("DeliveryDate");
            builder.Property(e => e.Weight).HasColumnName("Weight");
            builder.Property(e => e.Width).HasColumnName("Width");
            builder.Property(e => e.Height).HasColumnName("Height");
            builder.Property(e => e.Length).HasColumnName("Length");
            builder.Property(e => e.PackageValue).HasColumnName("PackageValue");
            builder.Property(e => e.ShippingRate).HasColumnName("ShippingRate");
            builder.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodId");
            builder.Property(e => e.UserSubscriptionId).HasColumnName("UserSubscriptionId");
            builder.Property(e => e.ShippingPackagingId).HasColumnName("ShippingPackagingId");
            builder.Property(e => e.ReferenceId).HasColumnName("ReferenceId");

            // Sender
            builder.Property(e => e.SenderId).HasColumnName("SenderId");
            builder.Property(e => e.SenderName).HasColumnName("SenderName");
            builder.Property(e => e.SenderEmail).HasColumnName("SenderEmail");
            builder.Property(e => e.SenderPhone).HasColumnName("SenderPhone");
            builder.Property(e => e.SenderAddress).HasColumnName("SenderAddress");
            builder.Property(e => e.SenderPostalCode).HasColumnName("SenderPostalCode");
            builder.Property(e => e.SenderContact).HasColumnName("SenderContact");
            builder.Property(e => e.SenderOtherAddress).HasColumnName("SenderOtherAddress");
            builder.Property(e => e.SenderIsDefault).HasColumnName("SenderIsDefault");
            builder.Property(e => e.SenderCityId).HasColumnName("SenderCityId");
            builder.Property(e => e.SenderCityName).HasColumnName("SenderCityName");
            builder.Property(e => e.SenderCityAName).HasColumnName("SenderCityAName");
            builder.Property(e => e.SenderCountryId).HasColumnName("SenderCountryId");
            builder.Property(e => e.SenderCountryName).HasColumnName("SenderCountryName");
            builder.Property(e => e.SenderCountryAName).HasColumnName("SenderCountryAName");

            // Receiver
            builder.Property(e => e.ReceiverId).HasColumnName("ReceiverId");
            builder.Property(e => e.ReceiverName).HasColumnName("ReceiverName");
            builder.Property(e => e.ReceiverEmail).HasColumnName("ReceiverEmail");
            builder.Property(e => e.ReceiverPhone).HasColumnName("ReceiverPhone");
            builder.Property(e => e.ReceiverAddress).HasColumnName("ReceiverAddress");
            builder.Property(e => e.ReceiverPostalCode).HasColumnName("ReceiverPostalCode");
            builder.Property(e => e.ReceiverContact).HasColumnName("ReceiverContact");
            builder.Property(e => e.ReceiverOtherAddress).HasColumnName("ReceiverOtherAddress");
            builder.Property(e => e.ReceiverIsDefault).HasColumnName("ReceiverIsDefault");
            builder.Property(e => e.ReceiverCityId).HasColumnName("ReceiverCityId");
            builder.Property(e => e.ReceiverCityName).HasColumnName("ReceiverCityName");
            builder.Property(e => e.ReceiverCityAName).HasColumnName("ReceiverCityAName");
            builder.Property(e => e.ReceiverCountryId).HasColumnName("ReceiverCountryId");
            builder.Property(e => e.ReceiverCountryName).HasColumnName("ReceiverCountryName");
            builder.Property(e => e.ReceiverCountryAName).HasColumnName("ReceiverCountryAName");

            // Shipping Type
            builder.Property(e => e.ShippingTypeId).HasColumnName("ShippingTypeId");
            builder.Property(e => e.ShippingTypeName).HasColumnName("ShippingTypeName");
            builder.Property(e => e.ShippingTypeAName).HasColumnName("ShippingTypeAName");
            builder.Property(e => e.ShippingFactor).HasColumnName("ShippingFactor");

            // Payment Method
            builder.Property(e => e.PaymentMethodAName).HasColumnName("PaymentMethodAName");
            builder.Property(e => e.PaymentMethodEName).HasColumnName("PaymentMethodEName");
            builder.Property(e => e.PaymentCommission).HasColumnName("PaymentCommission");

            // Packaging
            builder.Property(e => e.PackagingAName).HasColumnName("PackagingAName");
            builder.Property(e => e.PackagingEName).HasColumnName("PackagingEName");

            // Status
            builder.Property(e => e.CurrentState).HasColumnName("CurrentState");
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
            builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        }
    }
}