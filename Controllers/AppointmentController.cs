using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data;

namespace radio1.Controllers
{
	[Authorize]
	public class AppointmentController : Controller
	{
		public IActionResult AppointmentList()
		{
            return View();
		}

		[HttpGet]
		public IActionResult EventsList()
		{
            var rendezvous = RendezVousBLL.GetAll();
			return Json(rendezvous);
		}

		[HttpGet]
		public IActionResult GetDisponibility(string operation) 
		{
			var dispos = RendezVousBLL.GetDisponibilite(operation);
			return Json(dispos);
		}


		[Authorize(Roles = "Admin , Secretaire")]
		public IActionResult AddAppointment()
		{
			var operations = TypeOperationBLL.GetAll(null, null);
			return View(operations);
		}

		[HttpPost]
		public IActionResult SubmitAddAppointment(RendezVous rendezvous ,string selectedAppareil)
		{
			var app = AppareilRadioBLL.GetByName(selectedAppareil);
			foreach(var op in app.Operations )
			{
				if(op.Nom == rendezvous.TypeOperation.Nom)
				{
					rendezvous.TypeOperation = op;
				}
			}
			var salle = SalleBLL.GetById(rendezvous.TypeOperation.SalleId);
			rendezvous.doctor = new Doctor();
			rendezvous.doctor = salle.Responsable;
			var msg = RendezVousBLL.AddRendezVous(rendezvous);
			return Json(msg);
		}
	}

}
