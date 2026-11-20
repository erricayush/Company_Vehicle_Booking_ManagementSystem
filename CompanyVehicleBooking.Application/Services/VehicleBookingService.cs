using CompanyVehicleBooking.Application.Features.Booking.Command.ApproveBooking;
using CompanyVehicleBooking.Application.Features.Booking.Command.CreateBooking;
using CompanyVehicleBooking.Application.Features.Booking.Command.RejectBooking;
using CompanyVehicleBooking.Application.Features.Booking.Query.GetAllBooking;
using CompanyVehicleBooking.Application.Features.Booking.Query.GetBookingById;
using CompanyVehicleBooking.Application.Features.Booking.Query.ViewMyBookings;
using CompanyVehicleBooking.Application.Features.Tracking.Command;
using CompanyVehicleBooking.Application.Features.Tracking.Command.CompleteTrip;
using CompanyVehicleBooking.Application.ServicesContracts;
using CompanyVehicleBooking.Domain.CustomExceptions;
using CompanyVehicleBooking.Domain.Entities;
using CompanyVehicleBooking.Domain.Enums;
using CompanyVehicleBooking.Domain.RepositoryContracts;

namespace CompanyVehicleBooking.Application.Services
{
    public class VehicleBookingService(IVehicleBookingRepository vehicleBookingRepository, IVehicleRepository vehicleRepository, ICurrentUser currentUser, IBookingStatusHistoryRepository bookingStatusHistory) : IVehicleBookingService
    {
        private readonly IVehicleBookingRepository _vehicleBookingRepository = vehicleBookingRepository;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly ICurrentUser _currentUser = currentUser;
        private readonly IBookingStatusHistoryRepository _bookingStatusHistory = bookingStatusHistory;

        public async Task<AllocateVehicleCommandResponse> AllocateVehicleAsync(AllocateVehicleCommand request, CancellationToken cancellationToken)
        {

            var booking = await _vehicleBookingRepository.GetBookingByBookingIdAsync(request.BookingId, cancellationToken) ?? throw new NotFoundException("booking is not present");

            if (booking.BookingStatus != BookingStatus.Approved)
            {
                throw new BadRequestException("Vehicle allocation is only allowed after approval");
            }
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken) ?? throw new NotFoundException("Vehicle is not present");

            if (!vehicle.IsActive)
            {
                throw new BadRequestException("you can allocate only active vehicle");
            }

            booking.VehicleId = request.VehicleId;
            booking.BookingStatus = BookingStatus.Allocated;

            var statusHistory = new BookingStatusHistory
            {
                Id = Guid.NewGuid(),
                VehicleBookingId = booking.Id,
                Status = Status.Allocated,
                UpdatedDate = DateTime.UtcNow
            };

            await _vehicleBookingRepository.UpdateAsync(booking, cancellationToken);
            await _bookingStatusHistory.AddAsync(statusHistory, cancellationToken);

            return new AllocateVehicleCommandResponse
            {
                Message = "Vehicle is allocated successfully"
            };

        }

        public async Task<ApproveBookingCommandResponse> ApproveBookingAsync(ApproveBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _vehicleBookingRepository.GetBookingByBookingIdAsync(request.BookingId, cancellationToken) ?? throw new NotFoundException("booking is not available");

            if (booking.BookingStatus == BookingStatus.Completed)
            {
                throw new BadRequestException("Completed booking can not modified");
            }

            if(booking.BookingStatus != BookingStatus.Pending)
            {
                throw new BadRequestException("only pending booking can be approved");
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(booking.VehicleId, cancellationToken) ?? throw new BadRequestException("Vehicle is not present");
            if (!vehicle.IsActive)
            {
                throw new BadRequestException("only active vehicle can be approved");
            }

            var vehicleHasAlreadyApprovedBooking = await _vehicleBookingRepository.VehicleHasAlreadyApprovedBookingAsync(booking.VehicleId, booking.ReturnDate, booking.TravelDate, booking.Id, cancellationToken);
            if (vehicleHasAlreadyApprovedBooking)
            {
                throw new BadRequestException("Vehicle already has approved booking");
            }

            var employeeHasApprovedBooking = await _vehicleBookingRepository.EmployeeHasAlreadyApprovedBookingAsync(booking.EmployeeId, booking.ReturnDate, booking.TravelDate,booking.Id,cancellationToken);
            if (employeeHasApprovedBooking)
            {
                throw new BadRequestException("Employee already has approved booking");
            }

            booking.BookingStatus = BookingStatus.Approved;

            var statusHistory = new BookingStatusHistory
            {
                Id = Guid.NewGuid(),
                VehicleBookingId = booking.Id,
                Status =Status.Approved,
                UpdatedDate = DateTime.UtcNow
            };


            await _vehicleBookingRepository.UpdateAsync(booking, cancellationToken);
            await _bookingStatusHistory.AddAsync(statusHistory, cancellationToken);

            return new ApproveBookingCommandResponse
            {
                Message = "Booking Approved Successfully"
            };

        }

        public async Task<RejectBookingCommandResponse> RejectBookingAsync(RejectBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _vehicleBookingRepository.GetBookingByBookingIdAsync(request.BookingId, cancellationToken) ?? throw new NotFoundException("booking is not available");

            if (booking.BookingStatus != BookingStatus.Pending)
            {
                throw new BadRequestException("Only Pending Booking can be Rejected");
            }

            booking.BookingStatus = BookingStatus.Rejected;

            var statusHistory = new BookingStatusHistory
            {
                Id = Guid.NewGuid(), 
                VehicleBookingId=booking.Id,
                Status=Status.Rejected,
                UpdatedDate=DateTime.UtcNow

            };

            await _vehicleBookingRepository.UpdateAsync(booking, cancellationToken);
            await _bookingStatusHistory.AddAsync(statusHistory, cancellationToken);

            return new RejectBookingCommandResponse
            {
                Message = "Booking Rejected Successfully"
            };
        }

        public async Task<CompleteTripCommandResponse> CompleteTripAsync(CompleteTripCommand request, CancellationToken cancellationToken)
        {
            var booking = await _vehicleBookingRepository.GetBookingByBookingIdAsync(request.BookingId, cancellationToken) ?? throw new NotFoundException("booking is not available");

            if (booking.BookingStatus != BookingStatus.Allocated)
            {
                throw new BadRequestException("Trips can only be completed after the vehicle has been allocated.");
            }

            booking.BookingStatus = BookingStatus.Completed;
            var statusHistory = new BookingStatusHistory
            {
                Id = Guid.NewGuid(),
                VehicleBookingId = booking.Id,
                Status = Status.Completed,
                UpdatedDate = DateTime.UtcNow
            };

            await _vehicleBookingRepository.UpdateAsync(booking, cancellationToken);
            await _bookingStatusHistory.AddAsync(statusHistory, cancellationToken);

            return new CompleteTripCommandResponse
            {
                Message = "Trip Completed Successfully"
            };

           
        }

        public async Task<CreateBookingCommandResponse> CreateBookingAsync(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            if(!Guid.TryParse(_currentUser.Id, out var employeeId))
            {
                throw new UnauthorizedException("You are not authorized please login again with valid credentials");
            }

            if (string.IsNullOrWhiteSpace(request.PurposeOfTravel))
            {
                throw new BadRequestException("Purpose of the travel should not be null");
            }

            if (request.TravelDate < DateTime.UtcNow)
            {
                throw new BadRequestException("Travel Date cannot be in the past.");
            }

            if(request.ReturnDate < request.TravelDate)
            {
                throw new BadRequestException(" Return Date must be greater than or equal to Travel Date");
            }

            var vehicleHasAlreadyApprovedBooking = await _vehicleBookingRepository.VehicleHasAlreadyOverlapBookingAsync(request.VehicleId, request.ReturnDate, request.TravelDate, cancellationToken);

            if (vehicleHasAlreadyApprovedBooking)
            {
                throw new BadRequestException("Vehicle has already approved booking");
            }

            var employeeHasAlreadyApprovedBooking = await _vehicleBookingRepository.EmployeeHasAlreadyOverlapBookingAsync(employeeId, request.ReturnDate, request.TravelDate,cancellationToken);
            if (employeeHasAlreadyApprovedBooking)
            {
                throw new BadRequestException("Employee has already approved booking that overlaps on the same date");
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken) ?? throw new NotFoundException("Vehicle is not present");
            if (!vehicle.IsActive)
            {
                throw new BadRequestException("you can create booking only for active vehicle");
            }

            var booking = new VehicleBooking
            {
                Id=Guid.NewGuid(),
                VehicleId = request.VehicleId,
                EmployeeId = employeeId,
                TravelDate = request.TravelDate,
                ReturnDate=request.ReturnDate,
                Destination = request.Destination,
                Purpose = request.PurposeOfTravel,
                BookingStatus = BookingStatus.Pending,
                CreatedDate = DateTime.UtcNow
            };

            var statusHistory = new BookingStatusHistory
            {
                Id = Guid.NewGuid(),
                VehicleBookingId = booking.Id,
                Status = Status.Pending,
                UpdatedDate = DateTime.UtcNow,
            };


            booking.BookingStatusHistories.Add(statusHistory);
            await _vehicleBookingRepository.AddAsync(booking, cancellationToken);

            return new CreateBookingCommandResponse
            {
                Message = "Booking Created Successfully"
            };


        }

        public async Task<List<GetAllBookingQueryResponse>> GetAllBookingAsync(GetAllBookingQuery request, CancellationToken cancellationToken)
        {

            if(!Guid.TryParse(_currentUser.Id,out var employeeId))
            {
                throw new UnauthorizedException("You are not authorized please login again");
            }
            var bookings = await _vehicleBookingRepository.GetAllBookingAsync(request.PageNumber,request.PageSize,request.SortOrder,request.SortBy,request.Search,cancellationToken);

            if (bookings == null || !bookings.Any())
            {
                throw new NotFoundException("bookings are not available");
            }

            var bookingResponse = new List<GetAllBookingQueryResponse>();
            foreach (var booking in bookings)
            {
                bookingResponse.Add(new GetAllBookingQueryResponse
                {
                    VehicleId = booking.VehicleId,
                    EmployeeId = booking.EmployeeId,
                    TravelDate = booking.TravelDate,
                    ReturnDate = booking.ReturnDate,
                    Destination = booking.Destination,
                    Purpose = booking.Purpose
                });
            }

            return bookingResponse;
        }

        public async Task<GetBookingByIdQueryResponse> GetBookingByIdAsync(
    GetBookingByIdQuery request,
    CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUser.Id, out var employeeId))
            {
                throw new UnauthorizedException("You are not authorized. Please login again.");
            }

            var booking = await _vehicleBookingRepository
                .GetBookingByIdAsync(request.BookingId, cancellationToken);

            if (booking == null)
            {
                throw new NotFoundException("Booking not found.");
            }

            if (!_currentUser.IsInRole("Admin") && booking.EmployeeId != employeeId)
            {
                throw new BadRequestException(
                    "You can fetch only your own booking, not others."
                );
            }

            var bookingResponse = new GetBookingByIdQueryResponse
            {
                VehicleId = booking.VehicleId,
                EmployeeId = booking.EmployeeId,
                TravelDate = booking.TravelDate,
                ReturnDate = booking.ReturnDate,
                Destination = booking.Destination,
                Purpose = booking.Purpose
            };

            return bookingResponse;
        }

        public async Task<List<ViewMyBookingsQueryResponse>> ViewMyBookingsAsync(ViewMyBookingsQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUser.Id, out var employeeId))
            {
                throw new UnauthorizedException("You are not authorized please login again");
            }

            var bookings = await _vehicleBookingRepository.GetBookingByEmployeeIdAsync(employeeId, request.Search, request.SortOrder, request.SortBy, request.PageSize, request.PageNumber, cancellationToken);

            var myBookingResponse = new List<ViewMyBookingsQueryResponse>();
            foreach(var booking in bookings)
            {
                myBookingResponse.Add(new ViewMyBookingsQueryResponse
                {
                    VehicleId = booking.VehicleId,
                    EmployeeId = employeeId,
                    TravelDate = booking.TravelDate,
                    ReturnDate = booking.ReturnDate,
                    Destination = booking.Destination,
                    Purpose = booking.Purpose
                });
            }

            return myBookingResponse;
        }
    }
}
