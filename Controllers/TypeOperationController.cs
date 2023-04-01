using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Linq;

namespace radio1.Controllers
{
	[Authorize (Roles ="Admin")]
	public class TypeOperationController : Controller
	{
		/// <summary>
		/// Fonction pour retourner tous les Distincts types d'operation , si on a un Salle_Id en parametre le retour est les types d'operation associée a cette salle 
		/// </summary>
		/// <param name="SalleId"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TypeOperationList(int? SalleId)
		{
			var operations=TypeOperationBLL.GetAll(SalleId);
			return Json(new { operations });
		}

		/// <summary>
		/// Fonction permet d'ajouter un type d'operation 
		/// </summary>
		/// <param name="operation"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult AddTypeOperation(TypeOperation operation)
		{
			Message msg = TypeOperationBLL.AddTypeOperation(operation);
			Console.WriteLine(msg.Msg);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		
		/// <summary>
		/// Fonction permet de changer le nom d'operation 
		/// </summary>
		/// <param name="operation"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult EditTypeOperation(TypeOperation operation)
		{
			Message msg = TypeOperationBLL.EditTypeOperation(operation);
			Console.WriteLine(msg.Msg);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		/// <summary>
		/// Fonction permet de supprimer une operation
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpDelete]
        public IActionResult DeleteTypeOperation(int Id)
        {
            Message msg = TypeOperationBLL.DeleteTypeOperation(Id);
            Console.WriteLine(msg.Msg);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }
    }
}
