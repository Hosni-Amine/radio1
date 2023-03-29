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
        public IActionResult SalleList()
        {
            var salles = SalleBLL.GetAll();
            return Json(salles);
        }
        public IActionResult AddSalle(Salle salle)
        {
            Message msg = SalleBLL.AddSalle(salle);
            Console.WriteLine(msg.Msg);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }
        public IActionResult DeleteSalle(int Id)
        {
            Message msg = SalleBLL.DeleteSalle(Id);
            Console.WriteLine(msg.Msg);
            return Json(new { Success = msg.Verification, Message = msg.Msg });
        }
    }
}
