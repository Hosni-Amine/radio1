using FellowOakDicom;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.Entities;

public class AzurePacsController : Controller
{

	[HttpPost]
	public async Task<IActionResult> AzureUpLoadStudy(IFormFile dicom , int RendezVous_Id)
	{
		AzurePacsBLL azurePacsBLL = new AzurePacsBLL();
		var msg = await azurePacsBLL.UploadStudy(dicom,RendezVous_Id);
		return Ok(msg);
	}

	public async Task<IActionResult> GetStudy()
	{
		DicomReff dicomreff = new DicomReff();
		dicomreff.Image_Name = "mohamed";
		dicomreff.PatientId = 1;
		AzurePacsBLL azurePacsBLL = new AzurePacsBLL();
		var msg = await azurePacsBLL.GetStudy(dicomreff);
		return Ok(msg);
	}
}

