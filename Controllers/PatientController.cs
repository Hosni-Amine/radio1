using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;


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

        [Authorize(Roles = "Admin , Secretaire, Doctor , Technicien")]
        [HttpGet]
		public IActionResult PatientListJson()
		{
			List<Patient> patients = PatientBLL.GetAll();
			return Json(patients);
		}

        [Authorize(Roles = "Admin , Secretaire, Doctor , Technicien")]
        [HttpGet]
		public IActionResult EventsListPatientjson(int patient_Id)
		{
			Message msg = new Message();
			Users user = new Users();
			user.Id = patient_Id;
			user.Role = "Patient";
			var rendezvous = RendezVousBLL.GetAll(user);
			if(rendezvous.Count() == 0)
			{
				msg.Verification = false;
				return Json(msg);
			}
			else
			{
				msg.Verification = true;
				return Json(msg);
			}
		}

		[Authorize(Roles = "Admin , Secretaire, Doctor , Technicien")]
		[HttpGet]
		public IActionResult EventsListPatient(int patient_Id)
		{
			Users user = new Users();
			user.Id = patient_Id;
			user.Role = "Patient";
			var rendezvous = RendezVousBLL.GetAll(user);	
			return View(rendezvous);
		}

		[Authorize(Roles = "Admin , Secretaire, Doctor , Technicien")]
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

		[Authorize]
		[HttpGet]
		public IActionResult GetPatientById(int id)
		{
			var patient = PatientBLL.GetPatientById(id);
			return Json(patient);
		}

		[Authorize(Roles = "Admin , Secretaire")]
		[HttpPost]
		public IActionResult SubmitEditPatient(Patient patient)
		{
			var msg = PatientBLL.UpdatePatient(patient);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
	}
}
