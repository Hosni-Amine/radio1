﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.IO;
using System.Web;


namespace radio1.Controllers
{

	[Authorize(Roles = "Admin")]
    public class SalleController : Controller
    {
		private readonly IWebHostEnvironment _env;
		
        public SalleController(IWebHostEnvironment env)
		{
			_env = env;
		}

		/// <summary>
		/// Methode retourne une liste des salle
		/// </summary>
		/// <returns></returns> 
		public IActionResult SalleList()
        {
            var salles = SalleBLL.GetAll();
            return View(salles);
        }

		[HttpPost]
		/// <summary>
		/// Fonction qui permet de changer le responsable de la salle
		/// </summary>
		/// <param name="salle_id"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult SalleAffectation(int salle_id, int id)
		{
			var msg = SalleBLL.SalleAffectation(salle_id,id);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		/// <summary>
		/// Fonction qui retourn le view pour ajouter une salle 
		/// </summary>
		/// <returns></returns>
		public IActionResult AddSalle ()
		{
			var operations = TypeOperationBLL.GetAll(null);
			var doctors = DoctorBLL.GetAll();
			var viewModel = new
			{
				Doctors = doctors,
				Operations = operations
			};
			return View(viewModel); 
		}

		/// <summary>
		/// Ajouter une salle a la base de données 
		/// </summary>
		/// <param name="salle"></param>
		/// <returns></returns>
		public IActionResult Submit_AddSalle(Salle salle , List<TypeOperation> operations)
        {
				Message msg = SalleBLL.AddSalle(salle);
				if (msg.Verification)
				{
					var msg2 = SalleBLL.appendType(salle,operations);
				}
				return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		
		/// <summary>
		/// Supprimer une salle de la base de données 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public IActionResult DeleteSalle(int Id)
        {
			var salle = SalleBLL.GetById(Id);
            Message msg = SalleBLL.DeleteSalle(Id);
			if(msg.Verification)
			{
				DelPDF(salle.Emplacement);
			}
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }

		/// <summary>
		/// Fonction qui peut ajouter un pdf coté serveur
		/// </summary>
		/// <param name="pdf"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
        public async Task<IActionResult> AddPDF(IFormFile pdf)
        {
			if (pdf == null || pdf.Length == 0)
			{
				return Ok("ERROR");
			}
			var fileName = pdf.FileName;
			string pdfPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "Emplacement", fileName);
			using (var stream = new FileStream(pdfPath, FileMode.Create))
			{
				await pdf.CopyToAsync(stream);
			}
			return Ok(fileName);
		}

		/// <summary>
		/// Fonction permet de supprimer un fichier pdf associée a une salle 
		/// </summary>
		/// <param name="PdfName"></param>
		/// <returns></returns>
		public IActionResult DelPDF (string PdfName)
		{
			if(PdfName != null)
			{
				string pdfPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "Emplacement", PdfName);
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

		/// <summary>
		/// Fonction permet de telecharger un fichier pdf associée a une salle 
		/// </summary>
		/// <param name="PdfName"></param>
		/// <returns></returns>
		public ActionResult DownloadPDF(string fileName)
		{
			string pdfPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "Emplacement", fileName);
			byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
			return File(pdfBytes, "application/pdf", fileName);
		}

		
	}
}
