using System.Threading.Tasks;

namespace ProjectHub.Application.InterFaces
{
    public interface IUserService
    {    // Registracija na nov korisnik
        Task<bool> RegisterAsync(string userName, string email, string password);
        //Logiranje ke vraka JWT token ili null ako ne uspee
        Task<string> LoginAsync(string userName, string password);
    }
}
