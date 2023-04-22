using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data;
using System.Security.Claims;

namespace radio1.Controllers
{
	[Authorize]
	public class AppointmentController : Controller
	{
		[HttpHead]
		[HttpGet]
		public IActionResult AppointmentList()
		{
            return View();
		}

		[HttpGet]
		public IActionResult EventsList()
		{
			Users user = new Users();
			user.Id= Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			user.Role = User.FindFirstValue(ClaimTypes.Role);
			var rendezvous = RendezVousBLL.GetAll(user);
			return Json(rendezvous);
		}

		[HttpGet]
		public IActionResult GetDisponibility(string? operation ) 
		{
			var dispos = RendezVousBLL.GetDisponibilite(operation);
			return Json(dispos);
		}


		[Authorize(Roles = "Admin , Secretaire")]
		public IActionResult AddAppointment()
		{
			var operations = TypeOperationBLL.GetAll(false,false, null);
			return View(operations);
		}

		[Authorize(Roles = "Admin , Secretaire")]
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
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		[Authorize(Roles = "Admin , Secretaire")]
		[HttpDelete]
		public IActionResult DeleteAppointment(int id)
		{
			var msg = RendezVousBLL.DeleteRendezVous(id);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
	}

}
