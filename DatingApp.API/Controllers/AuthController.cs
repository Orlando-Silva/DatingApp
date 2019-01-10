using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config; //Para conseguir acessar as informações no JSON appSettings.
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            userForRegisterDTO.Name = userForRegisterDTO.Name.ToLower();

            if(await _repo.UserExistsAsync(userForRegisterDTO.Name))
                return BadRequest("User already exists");
            
            var user = new User 
            {
                Name = userForRegisterDTO.Name,
            };

            user = await _repo.RegisterAsync(user, userForRegisterDTO.Password);

            return StatusCode(201, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserForLoginDTO userForLoginDTO)
        { 
            var userFromRepo = await _repo.LoginAsync(userForLoginDTO.Username, userForLoginDTO.Password);

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));


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


