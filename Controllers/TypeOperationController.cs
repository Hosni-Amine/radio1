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
		[HttpGet]
		public IActionResult TypeOperationList()
		{
			var operations=TypeOperationBLL.GetAll();
			return Json(new { operations });
		}
		public IActionResult AddTypeOperation(string nom )
		{
			Message msg = TypeOperationBLL.AddTypeOperation(nom);
			Console.WriteLine(msg.Msg);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
        public IActionResult DeleteTypeOperation(int Id)
        {
            Message msg = TypeOperationBLL.DeleteTypeOperation(Id);
            Console.WriteLine(msg.Msg);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }

    }
}
