namespace Daor_E_Commerce.Application.Interfaces.IServices
{
    public interface IEmailService 
    {
        Task SendAsync(string toEmail, string subject, string body);
    }
}
