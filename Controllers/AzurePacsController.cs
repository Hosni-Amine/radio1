using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using EvilDICOM.Core;
using EvilDICOM.Core.IO.Reading;
using System.Diagnostics;


public class AzurePacsController : Controller
{
	private readonly IWebHostEnvironment _env;

	public AzurePacsController(IWebHostEnvironment env)
	{
		_env = env;
	}

	[HttpPost]
	public async Task<IActionResult> AzureUpLoadStudy(IFormFile dicom , int RendezVous_Id)
	{
		AzurePacsBLL azurePacsBLL = new AzurePacsBLL();
		var msg = await azurePacsBLL.UploadStudy(dicom,RendezVous_Id);
		return Ok(msg);
	}

	[HttpGet]
	public async Task<IActionResult> Study(int RendezVous_Id)
	{
		var rootpath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "dicom");
		AzurePacsBLL azurePacsBLL = new AzurePacsBLL();
		var msg = await azurePacsBLL.GetStudy(RendezVous_Id, rootpath);
		if (msg != null && msg.Verification == true)
		{
			return Ok(msg.Msg);
        }
        else
		{
			return BadRequest();
		}
	}

    [HttpGet]
    public IActionResult LaunchViewer()
	{
		try
		{
            string pathToExe = @"C:\Users\Amine\Desktop\dicom_viewer\bin\Debug\net6.0-windows\Dicom_Viewer.exe";
            Process.Start(pathToExe);
            return Ok();
        }
		catch(Exception ex) 
		{
			return BadRequest();
		}
	}
}


