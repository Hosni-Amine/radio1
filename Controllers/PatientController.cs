using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.Entities;
using System.Numerics;


namespace radio1.Controllers
{

	public class PatientController : Controller
	{
		[Authorize(Roles = "Admin , Secretaire")]
		[HttpPost]
		public IActionResult AddPatient(Patient patient)
		{
			var msg = PatientBLL.AddPatient(patient,null);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		[Authorize]
		[HttpGet]
		public IActionResult PatientListJson()
		{
			List<Patient> patients = PatientBLL.GetAll();
			return Json(patients);
		}

		[Authorize(Roles = "Admin , Secretaire")]
		[HttpGet]
		public IActionResult PatientList()
		{
			var patients = PatientBLL.GetAll();
			return View(patients);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public IActionResult DeletePatient(int id) 
		{
			var msg = PatientBLL.DeletePatient(id);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult GetPatientById(int id)
		{
			var patient = PatientBLL.GetPatientById(id);
			return Json(patient);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult SubmitEditPatient(Patient patient)
		{
			var msg = PatientBLL.UpdatePatient(patient);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
	}
}
