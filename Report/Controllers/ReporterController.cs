using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Report.Controllers
{
    public class ReporterController : Controller
    {
        [HttpGet]
        [Route("GetOfferLetterReport")]
        public IActionResult GetOfferLetterReport()
        {
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/testpdf.pdf");
            byte[] bytes = System.IO.File.ReadAllBytes(pdfPath);
            return File(bytes, "application/pdf");
        }
    }
}
