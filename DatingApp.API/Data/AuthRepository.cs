using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            user = CreatePasswordHash(user, password);    
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(_ => _.Name == username);

            if(user is null)
                return null;

            if(!VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash))
            {
                return null;
            }    
            else 
            {
                return user;
            }
        }

        public async Task<bool> UserExistsAsync(string username) => await _context.Users.AnyAsync(_ => _.Name == username);

        private User CreatePasswordHash(User user, string password)
        {
            using(var hmac = new HMACSHA512()) 
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));   
            }
            return user;
        }

        
        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        { 
            using(var hmac = new HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for(var i = 0; i < computedHash.Length; i ++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}