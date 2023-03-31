﻿using radio1.Controllers;
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
		public ActionResult State()
		{
			var Count_Tech = TechnicienBLL.GetAll().Count;
			var Count_Doctor = DoctorBLL.GetAll().Count;
			var Count_Salle = SalleBLL.GetAll().Count;
			return Json(new { Tech_Count = Count_Tech, Doctor_Count = Count_Doctor , Salle_Count = Count_Salle });
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