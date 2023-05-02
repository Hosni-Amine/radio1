using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data;

namespace radio1.Controllers
{
	[Authorize(Roles = "Admin")]
	public class TechnicienController : Controller
	{

		/// <summary>
		/// methode TechnicienList qui donne une list des Techniciens sil n y a pas des parametres de recherche
		/// </summary>
		/// <param name="search"></param>
		/// <param name="searchon"></param>
		/// <returns>retourne une list de Technicien </returns>
		[HttpGet]
		[HttpHead]
		public IActionResult TechnicienList()
		{
			var techniciens = TechnicienBLL.GetAll();
			return View(techniciens);
		}
		public IActionResult TechnicienListJson()
		{
			var techniciens = TechnicienBLL.GetAll();
			return Json(techniciens);
		}

		/// <summary>
		/// La methode DeleteTechnicien pour supprimer un Technicien de database
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>retourne Json file decrit le resultat</returns>
		[HttpDelete]
		public IActionResult DeleteTechnicien(int Id)
		{
			Message msg = TechnicienBLL.DeleteTechnicien(Id);
			if (msg.Verification)
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
			else
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
		}

		/// <summary>
		/// Methode de recherche d'un Technicien apartir de son Id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>retourner un Technicien</returns>
		[HttpGet]
		public IActionResult GetTechnicienById(int Id)
		{
			var Technicien = TechnicienBLL.GetById(Id);
			return Json(Technicien);
		}


		/// <summary>
		/// La methide Submit qui prendre en parametre les donneés de docteur et retourne un Json file qui decrire le resultat
		/// </summary>
		/// <param name="Technicien"></param>
		/// <returns>retourne Json file</returns>
		public IActionResult SubmitEditTechnicien(Technicien Technicien)
		{
			Message msg = TechnicienBLL.UpdateTechnicien(Technicien);
			if (msg.Verification)
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
			else
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
		}


		/// <summary>
		/// methode de controlleur pour l'ajout de Technicien 
		/// </summary>
		/// <param name="Technicien"></param>
		/// <returns>Message decivant le resultat</returns>
		[HttpPost]
		public IActionResult AddTechnicien(Technicien Technicien,int? User_Id)
		{
			Message msg = TechnicienBLL.AddTechnicien(Technicien,User_Id);
			if (msg.Verification)
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
			else
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
		}

	}
}
