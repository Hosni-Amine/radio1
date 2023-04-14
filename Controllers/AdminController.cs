using radio1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using radio1.Models.BLL;
using radio1.Models.Entities;

namespace radio1.Controllers 
{ 

	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{


		/// <summary>
		/// A changer !!!!!
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> State()
		{
			var status = UsersBLL.State();
			return Json(status);
		}


		/// <summary>
		/// 3 methodes d'inscription d'utilisateurs avec ces roles
		/// </summary>
		/// <param name="objet"></param>
		/// <param name="user"></param>
		/// <returns>Message personalisée des resultats </returns>
		public IActionResult AddTech_User(Technicien technicien, Users user)
		{
			var msg = UsersBLL.AddTech_User(technicien, user);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		public IActionResult AddDoctor_User(Doctor doctor, Users user)
		{
			var msg = UsersBLL.AddDoctor_User(doctor, user);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
		public IActionResult AddAdmin_User(Users user)
	{
		var msg = UsersBLL.AddAdmin_User(user);
		return Json(new { Success = msg.Verification, Message = msg.Msg });
	}
	}
}
