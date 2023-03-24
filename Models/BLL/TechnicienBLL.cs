using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data.SqlClient;

namespace radio1.Models.BLL
{
	public class TechnicienBLL
	{
		public static Message AddTechnicien(Technicien Technicien , int? User_id)
		{
			return TechnicienDAL.AddTechnicien(Technicien,User_id);
		}
		public static Message UpdateTechnicien(Technicien Technicien)
		{
			return TechnicienDAL.UpdateTechnicien(Technicien);
		}
		public static Message DeleteTechnicien(int Id)
		{
			return TechnicienDAL.DeleteTechnicien(Id);
		}
		public static Technicien GetById(int id)
		{
			return TechnicienDAL.GetById(id);
		}
		public static List<Technicien> GetAll()
		{
			return TechnicienDAL.GetAll();
		}
		public static Technicien GetByUserId(int User_Id)
		{
			return TechnicienDAL.GetByUserId(User_Id);
		}
	}
}
