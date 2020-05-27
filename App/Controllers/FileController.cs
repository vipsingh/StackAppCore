using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using StackErp.Model;
using StackErp.UI.View.PageAction;
using StackErp.UI.View.PageBuilder;
using StackErp.ViewModel.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.App.Controllers
{
    public class FileController : StackErp.UI.Controllers.BaseController
    {
        public FileController(ILogger<EntityController> logger): base(logger)
        {

        }

        public async Task<IActionResult> UploadImage(IFormFile file)
        { 
            if (file == null || file.Length == 0)  
                return Json(new { Status = "error", Message = "Error in uploading" }); 
            
            string randomFileName = Path.GetRandomFileName();
            var path = Path.Combine(this.StackAppContext.ImageStorePath, "temp", randomFileName);
  
            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            } 
  
            return Json(new { Status = "success", FileName = randomFileName });   
        }

        public IActionResult ObjectImage()
        {
            var fileName = RequestQuery.Name;
            var path = Path.Combine(this.StackAppContext.ImageStorePath, "store_" + StackAppContext.MasterId, fileName);

            var imageFileStream = System.IO.File.OpenRead(path);
            return File(imageFileStream, "image/png");
        }        
        
    }
}