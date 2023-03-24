using NuGet.Protocol.Plugins;
using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data.SqlClient;
using System.Data;
using Message = radio1.Models.DAL.Message;
using radio1.Models.DAL.Connection;

namespace radio1.Models.BLL
{
	public class AppareilRadioBLL
	{



		public static Message AddAppareilRadio(int typeR)
		{
			return AppareilRadioDAL.AddAppareilRadio(typeR);
		}
		public static Message DeleteAppareilRadio(int id)
		{
			return AppareilRadioDAL.DeleteAppareilRadio(id);
		}
		public static List<AppareilRadio> GetAll()
		{
			return AppareilRadioDAL.GetAll();

		}
		public static AppareilRadio GetById(int Id)
		{
			return AppareilRadioDAL.GetById(Id);
		}


	




		public static Message AddTypeRadio(string typeR)
		{
			return AppareilRadioDAL.AddTypeRadio(typeR);
		}
		public static Message DeleteTypeRadio(int id)
		{
			return AppareilRadioDAL.DeleteTypeRadio(id);
		}
		public static List<TypeRadio> GetAllType()
		{
			return AppareilRadioDAL.GetAllType();
		}
	}
}
