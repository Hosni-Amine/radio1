using radio1.Models.DAL;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
	public class AppareilRadioBLL
	{
		public static Message DeleteSalleAppareilRadios(int Salle_Id)
		{
			return AppareilRadioDAL.DeleteSalleAppareilRadios(Salle_Id);
		}
		public static Message AddAppareilRadio(AppareilRadio operation)
		{
			return AppareilRadioDAL.AddAppareilRadio(operation);
		}
		public static Message EditAppareilRadio(AppareilRadio operation)
		{
			return AppareilRadioDAL.EditAppareilRadio(operation);
		}
		public static Message DeleteAppareilRadio(int id)
		{
			return AppareilRadioDAL.DeleteAppareilRadio(id);
		}
		public static List<AppareilRadio> GetAll(int? SalleId)
		{
			return AppareilRadioDAL.GetAll(SalleId);
		}
		public static AppareilRadio GetById(int Id)
		{
			return AppareilRadioDAL.GetById(Id);
		}
	}
}
