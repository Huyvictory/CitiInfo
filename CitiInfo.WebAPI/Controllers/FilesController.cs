using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CitiInfo.WebAPI.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtenstionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtenstionContentTypeProvider)
        {
            _fileExtenstionContentTypeProvider = fileExtenstionContentTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtenstionContentTypeProvider));
        }

        [HttpGet]
        public ActionResult GetFile()
        {
            var pathToFile = "Program.cs";

            // Check if the file exists
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtenstionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
