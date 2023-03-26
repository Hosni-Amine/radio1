using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using radio1.Models.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using radio1.Models.DAL;
using radio1.Models.BLL;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using System.Numerics;

namespace radio1.Controllers
{

	public class AccountController : Controller
	{
		private readonly IConfiguration _config; 
		public AccountController(IConfiguration configuration)
		{
			_config = configuration;
		}



		/// <summary>
		/// authentification et verification des données de l'utilisateur
		/// </summary>
		/// <param name="usertest"></param>
		/// <returns></returns>
		private Users? AuthenticateUser(Users usertest)
		{
			Users user = UsersDAL.GetByUserName(usertest.UserName);
			if (user != null)
			{
				if (user.Password == usertest.Password)
				{
					return user;
				}
				return null;
			}
			else
			{
				return null;
			}
		}




		/// <summary>
		/// Methode qui genére un jwt token avec un payload qui contient les données de l'utilisateur
		/// </summary>
		/// <param name="user"></param>
		/// <returns>String Token </returns>
		private string GenerateToken(Users user)
		{
			var jwtTokenHundler = new JwtSecurityTokenHandler();
			var key=Encoding.UTF8.GetBytes(_config.GetSection(key: "Jwt:Key").Value);
			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(type: "Id", value: user.Id.ToString()),
					new Claim(type: "Role", value: user.Role)
				}),
				Expires = DateTime.Now.AddMinutes(60),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
			};
			var token = jwtTokenHundler.CreateToken(tokenDescriptor);
			var jwtToken = jwtTokenHundler.WriteToken(token);
			return jwtToken;
		}




		/// <summary>
		/// Methode qui va ajouter le token genérer au cookies s'il y a une authentification
		/// </summary>
		/// <param name="user"></param>
		/// <returns>le Id de l'utilisateur</returns>
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login (Users user)
		{
			Users _user = AuthenticateUser(user);
			if(_user != null)
			{
				var token = GenerateToken(_user);
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Secure = false, 
				};
				Response.Cookies.Append("poupa_donuts", token, cookieOptions);
				return Ok ( new{ user = _user, token = token });
			}
			return BadRequest();
		}




		/// <summary>
		/// 3 methodes d'inscription d'utilisateurs avec ces roles
		/// </summary>
		/// <param name="objet"></param>
		/// <param name="user"></param>
		/// <returns>Message personalisée des resultats </returns>
		public IActionResult AddTech_User(Technicien technicien , Users user)
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




		/// <summary>
		/// Methode qui initialise l'objet de retour associer au lutilisateur
		/// </summary>
		/// <param name="User_Id"></param>
		/// <returns>Object reference</returns>
		public IActionResult HomePage(int User_Id)
		{
			var user = UsersBLL.GetById(User_Id);
			Doctor doctor = null;
			Technicien technicien = null;
			if (user.Role == "Doctor")
			{
				doctor= DoctorBLL.GetByUserId(user.Id);
			}
			else if (user.Role == "Technicien")
			{
				technicien= TechnicienBLL.GetByUserId(user.Id);
			}
			
			var viewModel = new { User = user, Doctor = doctor, Technicien = technicien };
			return View(viewModel);
		}
	}
}
