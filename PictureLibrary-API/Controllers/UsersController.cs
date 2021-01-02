using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PictureLibrary_API.Helpers;
using PictureLibrary_API.Services;

namespace PictureLibrary_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IOptions<AppSettings> appSettings, IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userService = userService;
        }
    }
}