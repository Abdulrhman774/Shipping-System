using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.ShipmentStatusHistory;

public class UpdateShipmentStatusHistoryDtoValidator : AbstractValidator<UpdateShipmentStatusHistoryDto>
{
    public UpdateShipmentStatusHistoryDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("History record ID is required.");

        RuleFor(x => x.ShipmentId)
            .NotEmpty().WithMessage("Shipment ID is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .IsInEnum().WithMessage("Invalid status value.");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters.");
    }
}
