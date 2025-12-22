using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProjectHub.Domin.Entites;
using ProjectHub.Infrastructure.Data;
using ProjectHub.Infrastructure.Settings;
using System.Security.Cryptography;
using ProjectHub.Application.InterFaces;
using ProjectHub.Infrastructure;



namespace ProjectHub.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly JWTSettings _jWTSettings;
        public UserService(AppDbContext context, IOptions<JWTSettings> jwtOptions)
        {
            _context = context;
            _jWTSettings = jwtOptions.Value;
        }
        public async Task<bool>RegisterAsync(string userName, string email, string password)
        { 
            //Proverka dali postoi koisnik so ist usernsme ili email
            var exists = await _context.Users
                 .AnyAsync(u => u.UserName == userName || u.Email == email);
            if (exists)
            {
                return false;
            }
            //Hasiraj lozinka (ednostavno za demo)
            var passwordHash = HashPassword(password);
            //Kreiraj nov user
            var user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash

            };
     
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<string?> LoginAsync(string userName, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return null;

            var hashed = HashPassword(password);
            if (user.PasswordHash != hashed)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jWTSettings.Secret);

            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        }),

                // ВАЖНО: Expires > NotBefore
                NotBefore = now,
                IssuedAt = now,
                Expires = now.AddHours(
                    _jWTSettings.ExireHours > 0 ? _jWTSettings.ExireHours : 2),

                Issuer = _jWTSettings.Issuer,
                Audience = _jWTSettings.Audience,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //Pomosna metoda za hashiranje
        private static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashBytes = sha.ComputeHash(bytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
