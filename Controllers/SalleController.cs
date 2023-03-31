using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data;

namespace radio1.Controllers
{

	[Authorize(Roles = "Admin")]
    public class SalleController : Controller
    {
		private readonly IWebHostEnvironment _env;
		
        public SalleController(IWebHostEnvironment env)
		{
			_env = env;
		}

		/// <summary>
		/// Methode retourne une liste des salle
		/// </summary>
		/// <returns></returns> 
		public IActionResult SalleList()
        {
            var salles = SalleBLL.GetAll();
            return View(salles);
        }

		/// <summary>
		/// Ajouter une salle a la base de données 
		/// </summary>
		/// <param name="salle"></param>
		/// <returns></returns>
		public IActionResult AddSalle(Salle salle,string str ,TypeOperation operation)
        {
            var doc = DoctorBLL.GetByStr(str);
            if (doc != null)
            {
				List<TypeOperation> types = new List<TypeOperation>();
				types.Add(operation);
				salle.Responsable = doc;
				Message msg = SalleBLL.AddSalle(salle);
				if (msg.Verification)
				{
					var msg2 = SalleBLL.appendType(salle, types);
				}
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
			else
			{
				return Json(new { Success = false , Message = "Responsable pas identifiée!"});
			}
		}


        /// <summary>
        /// Supprimer une salle de la base de données 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult DeleteSalle(int Id)
        {
            Message msg = SalleBLL.DeleteSalle(Id);
            Console.WriteLine(msg.Msg);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }

		/// <summary>
		/// Fonction qui peut ajouter un pdf coté serveur
		/// </summary>
		/// <param name="pdf"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
        public async Task<IActionResult> AddPDF(IFormFile pdf)
        {
			if (pdf == null || pdf.Length == 0)
			{
				return Ok("ERROR");
			}
			var fileName = pdf.FileName;
			string pdfPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "Emplacement", fileName);
			using (var stream = new FileStream(pdfPath, FileMode.Create))
			{
				await pdf.CopyToAsync(stream);
			}
			return Ok(fileName);
		}

	}
}
