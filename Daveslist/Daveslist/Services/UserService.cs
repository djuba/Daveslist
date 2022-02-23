using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Daveslist.Entities;
using Daveslist.Helpers;
using Daveslist.Models;

namespace Daveslist.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        RegisterResponse Register(RegisterRequest model, Roles role = Roles.RegisteredUser);
        User AddUser(RegisterRequest model);
        void AddRole(User user, Roles role);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = User._users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public RegisterResponse Register(RegisterRequest model, Roles role = Roles.RegisteredUser)
        {
            var userExists = User._users.Any(x => x.Username == model.Username);
            // return null if user is found
            if (userExists) return null;

            var user = AddUser(model);

            AddRole(user, role);

            return new RegisterResponse(user);
        }

        public User AddUser(RegisterRequest model)
        {
            var user = new User(model);
            User._users.Add(user);

            return user;
        }

        public void AddRole(User user, Roles role)
        {
            user.Roles.Add(role);
        }

        public IEnumerable<User> GetAll()
        {
            return User._users;
        }

        public User GetById(int id)
        {
            return User._users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}