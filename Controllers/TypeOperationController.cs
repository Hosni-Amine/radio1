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
		[HttpGet]
		public IActionResult TypeOperationList(int? SalleId)
		{
			var operations=TypeOperationBLL.GetAll(SalleId);
			return Json(new { operations });
		}
		[HttpPost]
		public IActionResult AddTypeOperation(TypeOperation operation)
		{
			Message msg = TypeOperationBLL.AddTypeOperation(operation);
			Console.WriteLine(msg.Msg);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		[HttpPost]
		public IActionResult EditTypeOperation(TypeOperation operation)
		{
			Message msg = TypeOperationBLL.EditTypeOperation(operation);
			Console.WriteLine(msg.Msg);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		[HttpDelete]
        public IActionResult DeleteTypeOperation(int Id)
        {
            Message msg = TypeOperationBLL.DeleteTypeOperation(Id);
            Console.WriteLine(msg.Msg);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }
    }
}
