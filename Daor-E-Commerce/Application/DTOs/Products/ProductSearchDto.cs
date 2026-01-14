namespace Daor_E_Commerce.Application.DTOs.Products
{
    public class ProductSearchDto
    {
        public string Search { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
