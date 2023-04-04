using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data;
using System.Numerics;

namespace radio1.Controllers
{
	[Authorize(Roles = "Admin")]
	public class DoctorController : Controller
    {
		/// <summary>
		/// methode DoctorList qui donne une list des medecins sil n y a pas des parametres de recherche
		/// </summary>
		/// <param name="search"></param>
		/// <param name="searchon"></param>
		/// <returns>retourne une list de medecin </returns>
		public IActionResult DoctorList()
        {
		    var doctors = DoctorBLL.GetAll();
			return View(doctors);
		}

		public IActionResult DoctorListJson()
		{
			var doctors = DoctorBLL.GetAll();
			return Json(doctors);
		}



		/// <summary>
		/// La methode DeleteDoctor pour supprimer un medecin de database
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>retourne Json file decrit le resultat</returns>
		public IActionResult DeleteDoctor(int Id)
		{
			Message msg = DoctorBLL.DeleteDoctor(Id); 
            Console.WriteLine(msg.Msg);
	        return Json(new { Success = msg.Verification, Message = msg.Msg });       
        }

		/// <summary>
		/// Methode de recherche d'un medecin apartir de son Id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>retourner un medecin</returns>
		public IActionResult GetDoctorById(int Id)
		{
			var doctor = DoctorBLL.GetById(Id);
			return Json(doctor);
		}

		/// <summary>
		/// La methide Submit qui prendre en parametre les donneés de docteur et retourne un Json file qui decrire le resultat
		/// </summary>
		/// <param name="doctor"></param>
		/// <returns>retourne Json file</returns>
		public IActionResult SubmitEditDoctor(Doctor doctor)
        {
            Message msg = DoctorBLL.UpdateDoctor(doctor);
			Console.WriteLine(msg.Msg);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }

		/// <summary>
		/// methode de controlleur pour l'ajout de medecin 
		/// </summary>
		/// <param name="doctor"></param>
		/// <returns>Message decivant le resultat</returns>
		public IActionResult AddDoctor(Doctor doctor, int? User_Id)
		{
			Message msg = DoctorBLL.AddDoctor(doctor, User_Id);
			return Json(new { Success = msg.Verification, Message = msg.Msg });
		}
	}
}
