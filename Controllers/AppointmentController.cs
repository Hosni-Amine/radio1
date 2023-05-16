using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL.RV_Planification;
using radio1.Models.Entities;
using System.Security.Claims;

namespace radio1.Controllers
{
	[Authorize]
	public class AppointmentController : Controller
	{
		private readonly IWebHostEnvironment _env;
		public AppointmentController(IWebHostEnvironment env)
		{
			_env = env;
		}

		/// <summary>
		/// Fonctions les Interprétations textuelle
		/// </summary>
		/// <param></param>
		/// <returns></returns>
		[Authorize(Roles = "Doctor,Technicien")]
		[HttpPost]
		public IActionResult Edit_Inter_Text(RendezVous RV)
		{
			var msg = RendezVousBLL.EditRendezVous(RV);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		/// <summary>
		/// Fonction pour les interpretation PDF
		/// </summary>
		/// <param name="pdf"></param>
		/// <returns></returns>
		[Authorize(Roles = "Doctor,Technicien")]
		[HttpPost]
		public async Task<IActionResult> AddPDF(IFormFile pdf)
		{
			if (pdf == null || pdf.Length == 0)
			{
				return Ok("ERROR");
			}
			var fileName = pdf.FileName;
			string pdfPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "Interpretation_PDF", fileName);
			if (System.IO.File.Exists(pdfPath))
			{
				System.IO.File.Delete(pdfPath);
			}
			using (var stream = new FileStream(pdfPath, FileMode.Create))
			{
				await pdf.CopyToAsync(stream);	
			}
			string[] fileNameParts = fileName.Split('_');
			RendezVous RV = new RendezVous();
			RV.Id = Int32.Parse(fileNameParts[0]);
			RV.Inter_PDF = fileName;
			var msg = InterpretationDAL.Add_Inter_PDF(RV);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		public IActionResult DelPDF(string PdfName)
		{
			if (PdfName != null)
			{
				string pdfPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "Interpretation_PDF", PdfName);
				if (System.IO.File.Exists(pdfPath))
				{
					System.IO.File.Delete(pdfPath);
					return Json(new { Success = true, Message = "PDF supprimer avec success" });
				}
				else
				{
					return Json(new { Success = true, Message = "erreur de supprission !" });
				}
			}
			else
			{
				return Json(new { Success = true, Message = "PDF n'existe pas !" });
			}
		}
		public ActionResult DownloadPDF(string fileName)
		{
			string pdfPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "Interpretation_PDF", fileName);
			byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
			return File(pdfBytes, "application/pdf", fileName);
		}


		/// <summary>
		/// Fonctions de gestion des Rendez-Vous
		/// </summary>
		/// <param></param>
		/// <returns></returns> 
		[HttpGet]
		public IActionResult GetById(int Id)
		{
			var rendezvous = RendezVousBLL.GetById(Id);
			return Json(rendezvous);
		}
		[HttpHead]
		[HttpGet]
		public IActionResult AppointmentList()
		{
			return View();
		}
		[Authorize(Roles="Admin,Secretaire")]
		[HttpPost]
		public IActionResult EditAppointment(RendezVous RV)
		{
			var msg = RendezVousBLL.EditRendezVous(RV); 
			return Json(new { Success = msg.Verification, Message = msg.Msg });
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
		public IActionResult SubmitAddAppointment(RendezVous rendezvous, string selectedAppareil)
		{
			var app = AppareilRadioBLL.GetByName(selectedAppareil);
			foreach (var op in app.Operations)
			{
				if (op.Nom == rendezvous.TypeOperation.Nom)
				{
					rendezvous.TypeOperation = op;
				}
			}
			var salle = SalleBLL.GetById(rendezvous.TypeOperation.SalleId);
			rendezvous.doctor = new Doctor();
			rendezvous.doctor = salle.Responsable;
			rendezvous.technicien = new Technicien();
			rendezvous.technicien = salle.technicien;
			var msg = RendezVousBLL.AddRendezVous(rendezvous);
			if (msg.Verification)
			{ 
				//var msgemail = SendEmail(salle.Id);
				//if(msgemail.Verification)
				//{
				//	return Json(new { Success = msg.Verification, Message = msg.Msg });
				//}
				//else
				//{
				//	return Json(new { Success = msgemail.Verification, Message = msg.Msg });
				//}
				return Json(new { Success = msg.Verification, Message = msg.Msg });

			}
			else
			{
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
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
