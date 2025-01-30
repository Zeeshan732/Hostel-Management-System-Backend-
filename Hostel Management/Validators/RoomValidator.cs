using FluentValidation;
using Hostel_Management.Models;

namespace Hostel_Management.Validators
{
    public class RoomValidator : AbstractValidator<Room>
    {
        public RoomValidator()
        {
            //RuleFor(r => r.RoomId).GreaterThan(0).WithMessage("RoomId must be greater than 0");

            RuleFor(r => r.RoomNumber).GreaterThan(0).WithMessage("Room number must be a positive integer");

            RuleFor(r => r.RoomPrice).GreaterThan(0).WithMessage("Room price must be greater than 0");

            RuleFor(r => r.Type).NotEmpty().WithMessage("Room type is required");

            RuleFor(r => r.Status).NotEmpty().WithMessage("Room status is required")
                .Must(status => status == "Available" || status == "Partially Available" || status == "Occupied")
                .WithMessage("Room status must be one of the following: 'Available', 'Partially Available', 'Occupied'");

            RuleFor(r => r.TotalBeds).GreaterThan(0).WithMessage("Total beds must be greater than 0");

            RuleFor(r => r.OccupiedBeds).Must((r, occupiedBeds) => occupiedBeds >= 0 && occupiedBeds <= r.TotalBeds)
                .WithMessage("Occupied beds must be between 0 and total beds");


            RuleFor(r => r.RemainingBeds)
                .Equal(r => r.TotalBeds - r.OccupiedBeds).WithMessage("Remaining beds must equal total beds minus occupied beds");
        }
    }
}
