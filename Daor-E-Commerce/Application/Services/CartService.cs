


using Daor_E_Commerce.Application.DTOs.Cart;
using Daor_E_Commerce.Application.Interfaces.IServices;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Daor_E_Commerce.Common;
using Microsoft.EntityFrameworkCore;


public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<string>> AddToCart(int userId, AddToCartDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null || !product.IsActive)
            return new ApiResponse<string>(404, "Product not found");
        if (product.Stock <= 0)
            return new ApiResponse<string>(400, "Insufficient stock");

        if (product.Stock < dto.Quantity)
            return new ApiResponse<string>(400, "Insufficient stock");
        if (dto.Quantity < 0)
            return new ApiResponse<string>(400, "please enter a valid quantity");

        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var item = cart.CartItems
            .FirstOrDefault(x => x.ProductId == dto.ProductId);

        if (item != null)
            item.Quantity += dto.Quantity;
        else
            cart.CartItems.Add(new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });

        await _context.SaveChangesAsync();
        return new ApiResponse<string>(200, "Item added to cart");
    }

    public async Task<ApiResponse<string>> UpdateCart(int userId, UpdateCartItemDto dto)
    {
        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
            return new ApiResponse<string>(404, "Cart not found");

        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == dto.ProductId);
        if (item == null)
            return new ApiResponse<string>(404, "Item not found in cart");

        if (item.Quantity <= 0)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return new ApiResponse<string>(200, "Item removed from cart");
        }
        item.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();

        return new ApiResponse<string>(200, "Cart updated");
    }

    public async Task<ApiResponse<string>> RemoveFromCart(int userId, int productId)
    {
        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
            return new ApiResponse<string>(404, "Cart not found");

        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
        if (item == null)
            return new ApiResponse<string>(404, "Item not found");

        cart.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return new ApiResponse<string>(200, "Item removed");
    }

    public async Task<ApiResponse<object>> GetCart(int userId)
    {
        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
            return new ApiResponse<object>(200, "Cart is empty", Array.Empty<object>());


        var response = cart.CartItems.Select(x => new
        {
            CartItemId = x.Id,
            x.ProductId,
            x.Product.Name,
            x.Quantity,
            x.Product.Price,
            Total = x.Quantity * x.Product.Price
        });

        return new ApiResponse<object>(200, "Cart retrieved", response);
    }
}
