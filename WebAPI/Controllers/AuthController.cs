#region --Using--
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using WebAPI.DTO;
#endregion

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        private readonly IConfiguration _config; 

        public AuthController(IAuthService service, IConfiguration config)
        {
            _config = config;
            _service = service;
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDTO userForRegisterDTO)
        {
            userForRegisterDTO.Name = userForRegisterDTO.Name.ToLower();

            if(_service.UserExists(userForRegisterDTO.Name))
                return BadRequest("User already exists");
            
            var user = new User 
            {
                Name = userForRegisterDTO.Name,
            };

            user = _service.Register(user, userForRegisterDTO.Password);

            return StatusCode(201, user);
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDTO userForLoginDTO)
        { 
            var userFromRepo = _service.Login(userForLoginDTO.Username, userForLoginDTO.Password);

            if(userFromRepo is null)
                return Unauthorized();

            /*
                Estrutura de um JWT:

                1: Header = Algoritmo usado no token e tipo do JWT.
                2: Payload = Informações do usuário.
                3: Credentials = Credencias para dizer se este token é válido ou não.
            */ 


            // Cria as Claims: Informações básicas do usuário. Neste caso Id e nome. Essa informações podem ser vistas por qualquer um que quebre o JWT.

            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Name)
            };

            // A chave de criptografia que diz que esse JWT é válido para as API's.

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value));


            // Cria as credênciais.

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Cria o "Corpo" do token. Subject = Informações do usuário. Expires = Tempo máximo válido do token. SingingCredentials = as credências para validar o JSON.

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            // Cria o token.

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            // Retorna ele.

            return Ok(new 
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}


