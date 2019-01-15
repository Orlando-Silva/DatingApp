#region --Using--
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Collections.Generic;
using WebAPI.DTO;
#endregion

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetUsers()
        {

            var users = _userService.GetUsers();

            var usersForList = _mapper.Map<List<UserForListDTO>>(users);

            return Ok(usersForList);
        }

        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            var user = _userService.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailsDTO>(user);

            return Ok(userToReturn);
        }

    }
}