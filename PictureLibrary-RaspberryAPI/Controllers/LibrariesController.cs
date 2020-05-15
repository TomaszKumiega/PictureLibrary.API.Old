using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PictureLibraryModel.Services;

namespace PictureLibrary_RaspberryAPI.Controllers
{
    public class LibrariesController
    {
        private readonly ILogger<FileSystemService> _logger;

        public LibrariesController()
        {
            _logger = new Logger<FileSystemService>(new LoggerFactory());
        }
    }
}
