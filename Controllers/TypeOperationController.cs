using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;

namespace radio1.Controllers
{
	[Authorize (Roles ="Admin")]
	public class TypeOperationController : Controller
	{
		/// <summary>
		/// Fonction pour retourner tous les Distincts types d'operation relative a un parametre
		/// </summary>
		/// <param name="SalleId"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TypeOperationList(bool? ForApp,bool? ForSalle,int? SalleId)
		{
			var operations=TypeOperationBLL.GetAll(ForApp,ForSalle,SalleId);
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
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}

		/// <summary>
		/// Fonction permet de supprimer une type d'operation
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpDelete]
        public IActionResult DeleteTypeOperation(int Id)
        {
            Message msg = TypeOperationBLL.DeleteTypeOperation(Id);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }
    }
}
