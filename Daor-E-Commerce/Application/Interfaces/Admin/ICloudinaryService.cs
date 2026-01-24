using Daor_E_Commerce.Application.DTOs.Admin.Product;
using Microsoft.AspNetCore.Http;



namespace Daor_E_Commerce.Application.Interfaces.Admin
{
    
    public interface ICloudinaryService
    {
        Task<UploadResultDto> UploadImageAsync(IFormFile file);
        Task DeleteImageAsync(string publicId);
    }
}
