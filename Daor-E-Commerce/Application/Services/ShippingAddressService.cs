using Daor_E_Commerce.Application.DTOs.Shipping;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services
{
    public class ShippingAddressService : IShippingAddressService
    {
        private readonly AppDbContext _context;

        public ShippingAddressService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> Add(int userId, AddShippingAddressDto dto)
        {
            var address = new ShippingAddress
            {
                UserId = userId,
                FullName = dto.FullName,
                Phone = dto.Phone,
                AddressLine = dto.AddressLine,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country
            };

            _context.ShippingAddresses.Add(address);
            await _context.SaveChangesAsync();

            return new ApiResponse<object>(200, "Shipping address added");
        }

        public async Task<ApiResponse<object>> GetMyAddresses(int userId)
        {
            var addresses = await _context.ShippingAddresses
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    a.Id,
                    a.FullName,
                    a.Phone,
                    a.AddressLine,
                    a.City,
                    a.State,
                    a.PostalCode,
                    a.Country,
                    a.IsActive
                })
                .ToListAsync();

            return new ApiResponse<object>(200, "My shipping addresses", addresses);
        }

        public async Task<ApiResponse<object>> Update(int userId, int id, UpdateShippingAddressDto dto)
        {
            var address = await _context.ShippingAddresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return new ApiResponse<object>(404, "Address not found");

            address.FullName = dto.FullName;
            address.Phone = dto.Phone;
            address.AddressLine = dto.AddressLine;
            address.City = dto.City;
            address.State = dto.State;
            address.PostalCode = dto.PostalCode;
            address.Country = dto.Country;

            await _context.SaveChangesAsync();
            return new ApiResponse<object>(200, "Address updated");
        }

        public async Task<ApiResponse<object>> SetActive(int userId, int id)
        {
            var addresses = await _context.ShippingAddresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            foreach (var a in addresses)
                a.IsActive = false;

            var selected = addresses.FirstOrDefault(a => a.Id == id);
            if (selected == null)
                return new ApiResponse<object>(404, "Address not found");

            selected.IsActive = true;
            await _context.SaveChangesAsync();

            return new ApiResponse<object>(200, "Active address updated");
        }

        public async Task<ApiResponse<object>> Delete(int userId, int id)
        {
            var address = await _context.ShippingAddresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return new ApiResponse<object>(404, "Address not found");

            _context.ShippingAddresses.Remove(address);
            await _context.SaveChangesAsync();

            return new ApiResponse<object>(200, "Address deleted");
        }
    }
}
