using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data;

namespace radio1.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AppareilRadioController : Controller
	{

		/// <summary>
		/// Fonction pour retourner tous les salles avec les appareils associés
		/// </summary>
		/// <param name="SalleId"></param>
		/// <returns></returns>
		[HttpGet]
		[HttpHead]
		public IActionResult AppareilRadioList()
		{
			var salles = AppareilRadioBLL.GetAllwithappareils();
			if(salles.Count == 0 )
			{
				return RedirectToAction("AddSalle", "Salle");
			}
			return View(salles);
		}

		/// <summary>
		/// Fonction permet d'affecter un appareil a une salle 
		/// </summary>
		/// <param name="appareilradio"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult AddAppareilRadio(AppareilRadio appareilradio)
		{
			Message msg = AppareilRadioBLL.AddAppareilRadio(appareilradio);
			if(msg.Verification)
			{
				var msg2 = AppareilRadioBLL.AppendTypes(msg.MsgId,appareilradio.SalleId,appareilradio.Operations);
			}
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		/// <summary>
		/// Fonction permet de changer le nom d'appareilradio 
		/// </summary>
		/// <param name="appareilradio"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult EditAppareilRadio(AppareilRadio appareilradio)
		{
			Message msg = AppareilRadioBLL.EditAppareilRadio(appareilradio);
			Console.WriteLine(msg.Msg);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		/// <summary>
		/// Fonction permet de supprimer une appareilradio
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpDelete]
		public IActionResult DeleteAppareilRadio(int Id)
		{
			Message msg = AppareilRadioBLL.DeleteAppareilRadio(Id);
			Console.WriteLine(msg.Msg);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		/// <summary>
		/// Fonction qui retourne un objet d'apres son id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult GetById(int Id)
		{
			var appareilradio = AppareilRadioBLL.GetById(Id);
			return Json (appareilradio);
		}
	}

}
