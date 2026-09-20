using SmartHealthCare.Model;
namespace SmartHealthCare.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);



    }
}
