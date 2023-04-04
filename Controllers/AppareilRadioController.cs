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
		/// Fonction pour retourner tous les Distincts appareils , si on a un Salle_Id en parametre le retour est les appareils associée a cette salle 
		/// </summary>
		/// <param name="SalleId"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult AppareilRadioList(int? SalleId)
		{
			var appareilradios = AppareilRadioBLL.GetAll(SalleId);
			return View(appareilradios);
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
			Console.WriteLine(msg.Msg);
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
	}

}
