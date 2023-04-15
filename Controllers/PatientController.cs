using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.Entities;


namespace radio1.Controllers
{
	public class PatientController : Controller
	{


		[HttpPost]
		public IActionResult AddPatient(Patient patient)
		{
			var msg = PatientBLL.AddPatient(patient,null);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		[HttpGet]
		public IActionResult PatientListJson()
		{
			List<Patient> patients = PatientBLL.GetAll();
			return Json(patients);
		}
	}
}
