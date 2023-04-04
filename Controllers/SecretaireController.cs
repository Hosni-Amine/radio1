using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data;

namespace radio1.Controllers
{
	[Authorize(Roles = "Admin,Doctor")]
	public class SecretaireController : Controller
	{

		/// <summary>
		/// methode SecretaireList qui donne une list des Secretaires sil n y a pas des parametres de recherche
		/// </summary>
		/// <param name="search"></param>
		/// <param name="searchon"></param>
		/// <returns>retourne une list de Secretaire </returns>
		[HttpGet]
		public IActionResult SecretaireList()
		{
			var Secretaires = SecretaireBLL.GetAll();
			return View(Secretaires);
		}




		/// <summary>
		/// La methode DeleteSecretaire pour supprimer un Secretaire de database
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>retourne Json file decrit le resultat</returns>
		[HttpDelete]
		public IActionResult DeleteSecretaire(int Id)
		{
			Message msg = SecretaireBLL.DeleteSecretaire(Id);
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
		/// Methode de recherche d'un Secretaire apartir de son Id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>retourner un Secretaire</returns>
		[HttpGet]
		public IActionResult GetSecretaireById(int Id)
		{
			var Secretaire = SecretaireBLL.GetById(Id);
			return Json(Secretaire);
		}




		/// <summary>
		/// La methide Submit qui prendre en parametre les donneés de docteur et retourne un Json file qui decrire le resultat
		/// </summary>
		/// <param name="Secretaire"></param>
		/// <returns>retourne Json file</returns>
		public IActionResult SubmitEditSecretaire(Secretaire Secretaire)
		{
			Message msg = SecretaireBLL.UpdateSecretaire(Secretaire);
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
		/// methode de controlleur pour l'ajout de Secretaire 
		/// </summary>
		/// <param name="Secretaire"></param>
		/// <returns>Message decivant le resultat</returns>
		[HttpPost]
		public IActionResult AddSecretaire(Secretaire Secretaire, int? User_Id)
		{
			Message msg = SecretaireBLL.AddSecretaire(Secretaire, User_Id);
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
