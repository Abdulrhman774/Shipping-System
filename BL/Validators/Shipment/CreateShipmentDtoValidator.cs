// BL/Validators/Shipment/CreateShipmentDtoValidator.cs

using FluentValidation;
using BL.DTOs.Shipment;
using System;

namespace BL.Validators.Shipment;

public class CreateShipmentDtoValidator : AbstractValidator<CreateShipmentDto>
{
    // Define reasonable limits
    private const decimal MaxPackageValue = 1000000m;        // 1 million
    private const decimal MaxShippingRate = 9999.99m;        // Database limit
    private const double MaxWidth = 500.0;                   // 500 cm
    private const double MaxHeight = 500.0;                  // 500 cm
    private const double MaxLength = 500.0;                  // 500 cm
    private const double MaxWeight = 1000.0;                 // 1000 kg
    private const double MinDimension = 0.01;                // 1mm minimum
    private const double MinWeight = 0.01;                   // 10g minimum
    private const decimal MinPackageValue = 0.01m;           // 1 cent minimum

    public CreateShipmentDtoValidator()
    {
        // ===== DATE VALIDATIONS =====

        RuleFor(x => x.ShippingDate)
            .NotEmpty().WithMessage("Shipping date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Shipping date cannot be in the past.");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.DeliveryDate.HasValue)
            .WithMessage("Delivery date cannot be in the past.");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(x => x.ShippingDate)
            .When(x => x.DeliveryDate.HasValue)
            .WithMessage("Delivery date must be after or on the shipping date.");

        // ===== REQUIRED ID VALIDATIONS =====

        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("Sender is required.")
            .Must(id => id != Guid.Empty).WithMessage("Invalid sender ID.");

        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("Receiver is required.")
            .Must(id => id != Guid.Empty).WithMessage("Invalid receiver ID.");

        RuleFor(x => x.ShippingTypeId)
            .NotEmpty().WithMessage("Shipping type is required.")
            .Must(id => id != Guid.Empty).WithMessage("Invalid shipping type ID.");

        // ===== DIMENSION VALIDATIONS =====

        RuleFor(x => x.Width)
            .NotEmpty().WithMessage("Width is required.")
            .GreaterThanOrEqualTo((double)MinDimension)
            .WithMessage($"Width must be at least {MinDimension} cm.")
            .LessThanOrEqualTo(MaxWidth)
            .WithMessage($"Width must not exceed {MaxWidth} cm.");

        RuleFor(x => x.Height)
            .NotEmpty().WithMessage("Height is required.")
            .GreaterThanOrEqualTo((double)MinDimension)
            .WithMessage($"Height must be at least {MinDimension} cm.")
            .LessThanOrEqualTo(MaxHeight)
            .WithMessage($"Height must not exceed {MaxHeight} cm.");

        RuleFor(x => x.Length)
            .NotEmpty().WithMessage("Length is required.")
            .GreaterThanOrEqualTo((double)MinDimension)
            .WithMessage($"Length must be at least {MinDimension} cm.")
            .LessThanOrEqualTo(MaxLength)
            .WithMessage($"Length must not exceed {MaxLength} cm.");

        // ===== WEIGHT VALIDATIONS =====

        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage("Weight is required.")
            .GreaterThanOrEqualTo(MinWeight)
            .WithMessage($"Weight must be at least {MinWeight} kg.")
            .LessThanOrEqualTo(MaxWeight)
            .WithMessage($"Weight must not exceed {MaxWeight} kg.");

        // ===== PACKAGE VALUE VALIDATIONS =====

        RuleFor(x => x.PackageValue)
            .NotEmpty().WithMessage("Package value is required.")
            .GreaterThanOrEqualTo(MinPackageValue)
            .WithMessage($"Package value must be at least {MinPackageValue:C}.")
            .LessThanOrEqualTo(MaxPackageValue)
            .WithMessage($"Package value must not exceed {MaxPackageValue:C}.");

        // ===== SHIPPING RATE VALIDATION =====
        // This should not be required in the DTO as it's calculated by the system
        // But we validate it if provided
        RuleFor(x => x.ShippingRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ShippingRate > 0)
            .WithMessage("Shipping rate cannot be negative.")
            .LessThanOrEqualTo(MaxShippingRate)
            .When(x => x.ShippingRate > 0)
            .WithMessage($"Shipping rate must not exceed {MaxShippingRate}.");

        // ===== OPTIONAL ID VALIDATIONS =====
        // These are optional, but if provided they must be valid GUIDs

        RuleFor(x => x.PaymentMethodId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("Invalid payment method ID.");

        RuleFor(x => x.UserSubscriptionId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("Invalid subscription ID.");

        RuleFor(x => x.ShippingPackagingId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("Invalid shipping packaging ID.");

        RuleFor(x => x.ReferenceId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("Invalid reference ID.");

        // ===== COMPLEX VALIDATIONS =====
        // Cross-field validations

        // Ensure sender and receiver are not the same
        RuleFor(x => x)
            .Must(x => x.SenderId != x.ReceiverId)
            .WithMessage("Sender and receiver cannot be the same person.");

        // Validate volumetric weight (optional - business rule)
        RuleFor(x => x)
            .Must(x => CalculateVolumetricWeight(x) < 1000) // Max 1000 kg volumetric
            .WithMessage("Package is too large (volumetric weight exceeds limit).");

        // Ensure shipping date is not too far in the future (e.g., max 30 days)
        RuleFor(x => x.ShippingDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30))
            .WithMessage("Shipping date cannot be more than 30 days in the future.");

        // Ensure delivery date is not too far from shipping date
        RuleFor(x => x)
            .Must(x => !x.DeliveryDate.HasValue ||
                       (x.DeliveryDate.Value - x.ShippingDate).TotalDays <= 30)
            .WithMessage("Delivery date cannot be more than 30 days after shipping date.");

        // ===== CUSTOM VALIDATION FOR CASE 12 =====
        // This catches the very large package scenario

        RuleFor(x => x)
            .Must(x => IsPackageWithinReasonableLimits(x))
            .WithMessage("Package dimensions or weight exceed reasonable limits. " +
                        "Maximum allowed: Width=500cm, Height=500cm, Length=500cm, Weight=1000kg.");

        // Validate against database constraints
        RuleFor(x => x)
            .Must(x => EstimateShippingRate(x) <= MaxShippingRate)
            .WithMessage($"Estimated shipping rate exceeds maximum allowed ({MaxShippingRate:C}). " +
                        "Please reduce package dimensions or weight.");
    }

    #region Private Helper Methods

    private double CalculateVolumetricWeight(CreateShipmentDto dto)
    {
        const double volumetricDivisor = 5000.0;
        return (dto.Width * dto.Height * dto.Length) / volumetricDivisor;
    }

    private bool IsPackageWithinReasonableLimits(CreateShipmentDto dto)
    {
        return dto.Width <= MaxWidth &&
               dto.Height <= MaxHeight &&
               dto.Length <= MaxLength &&
               dto.Weight <= MaxWeight;
    }

    private decimal EstimateShippingRate(CreateShipmentDto dto)
    {
        // Simple estimation for validation purposes
        // This is just to check if the rate will exceed the database limit
        const decimal baseRate = 10m;
        const decimal weightRate = 5m;
        const decimal dimensionRate = 2m;

        var volumetricWeight = (decimal)CalculateVolumetricWeight(dto);
        var billableWeight = Math.Max((decimal)dto.Weight, volumetricWeight);

        var estimatedRate = baseRate + (billableWeight * weightRate) +
                           ((decimal)(dto.Width + dto.Height + dto.Length) * dimensionRate);

        return Math.Round(estimatedRate, 2);
    }

    #endregion
}