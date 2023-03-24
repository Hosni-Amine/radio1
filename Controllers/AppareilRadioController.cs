using Microsoft.AspNetCore.Mvc;
using radio1.Models.BLL;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Numerics;

namespace radio1.Controllers
{
	public class AppareilRadioController : Controller
	{
		public IActionResult AppareilRadioList()
		{
			List<AppareilRadio> list = new List<AppareilRadio>();
			list=(AppareilRadioBLL.GetAll());
			return Json(list);
		}
		public IActionResult AddAppareilRadio(int typeId)
		{
			Message msg = AppareilRadioBLL.AddAppareilRadio(typeId);
			if (msg.Verification)
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
			else
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
		}






		public IActionResult TypeRadioList()
		{
			List<AppareilRadio> list = new List<AppareilRadio>();
			list = (AppareilRadioBLL.GetAll());
			return Json(list);
		}
		public IActionResult AddTypeRadio(string typeR)
		{
			Message msg = AppareilRadioBLL.AddTypeRadio(typeR);
			if (msg.Verification)
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
			else
			{
				Console.WriteLine(msg.Msg);
				return Json(new { Success = msg.Verification, Message = msg.Msg });
			}
		}
	}
}
