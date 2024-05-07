using AutoMapper;
using Azure.Core;
using Ipz.Services;
using Ipz_server.Models;
using Ipz_server.Models.Database;
using Ipz_server.Models.Dto.Locations;
using Ipz_server.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Services
{
    public class LocationService : ILocationService
    {
        private readonly FoodDeliveryContext _context;
        private readonly IMapper _mapper;

        public LocationService(FoodDeliveryContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateLocation(LocationCreateRequestDto request)
        {
            var apiResponse = new ApiResponse();
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Invalid model state");

                return apiResponse;
            }

            try
            {
                var location = _mapper.Map<Location>(request);
                location.LocationId = Guid.NewGuid();

                await _context.Locations.AddAsync(location);
                await _context.SaveChangesAsync();

                apiResponse.Success = true;
                apiResponse.Data = location;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating location");
                throw;
            }
           
        }

        public async Task<ApiResponse> UpdateLocation(LocationUpdateRequestDto request)
        {
            var apiResponse = new ApiResponse();
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Invalid model state");

                return apiResponse;
            }

            try
            {
                var location = await _context.Locations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.LocationId == request.LocationId);

                if (location == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("Location not found");

                    return apiResponse;
                }

                location = _mapper.Map<Location>(request);

                _context.Locations.Update(location);
                await _context.SaveChangesAsync();

                apiResponse.Success = true;
                apiResponse.Data = location;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while updating location");
                throw;
            }

        }
    }
}
