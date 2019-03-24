using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagement : Controller
    {
        private readonly IFileUploader _fileUploader;

        public AdvertManagement(IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
        }
        [HttpGet]
        public IActionResult Create(CreateAdvertViewModel model)
        {
            return View(model);
        }

        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var id = "1111";
                // Must make a call to advert api, create the advertisement in the database and return the Id

                var fileName = "";

                if (imageFile != null)
                {
                    fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using (var readStream = imageFile.OpenReadStream())
                        {
                            var result = await _fileUploader.UploadFileAsync(filePath, readStream);
                            if(!result)
                                throw new Exception("Could not upload the image to the file repository. Please see the logs for details.");

                            // Call advert api and confirm the advertisement.

                            return RedirectToAction("Index", "Home");
                        }
                    }
                    catch (Exception e)
                    {
                        // Call Advert Api and cancel the advertisement
                        Console.WriteLine(e);
                    }
                }
            }

            return View(model);
        }
    }
}