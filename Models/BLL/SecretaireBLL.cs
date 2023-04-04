using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data.SqlClient;

namespace radio1.Models.BLL
{
	public class SecretaireBLL
	{
		public static Message AddSecretaire(Secretaire Secretaire, int? User_id)
		{
			return SecretaireDAL.AddSecretaire(Secretaire, User_id);
		}
		public static Message UpdateSecretaire(Secretaire Secretaire)
		{
			return SecretaireDAL.UpdateSecretaire(Secretaire);
		}
		public static Message DeleteSecretaire(int Id)
		{
			return SecretaireDAL.DeleteSecretaire(Id);
		}
		public static Secretaire GetById(int id)
		{
			return SecretaireDAL.GetById(id);
		}
		public static List<Secretaire> GetAll()
		{
			return SecretaireDAL.GetAll();
		}
		public static Secretaire GetByUserId(int User_Id)
		{
			return SecretaireDAL.GetByUserId(User_Id);
		}
	}
}
