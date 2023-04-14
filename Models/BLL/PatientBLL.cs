using radio1.Models.DAL;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
	public class PatientBLL
	{
		public static Message AddPatient(Patient Patient, int? User_Id)
		{
			return PatientDAL.AddPatient(Patient, User_Id);
		}
		public static Message UpdatePatient(Patient Patient)
		{
			return PatientDAL.UpdatePatient(Patient);
		}
		public static Message DeletePatient(int Id)
		{
			return PatientDAL.DeletePatient(Id);
		}
		public static Patient GetById(int id)
		{
			return PatientDAL.GetById(id);
		}
		public static List<Patient> GetAll()
		{
			return PatientDAL.GetAll();
		}
		public static Patient GetByUserId(int User_Id)
		{
			return PatientDAL.GetByUserId(User_Id);
		}
		public static Patient GetByStr(string Str)
		{
			return PatientDAL.GetByStr(Str);

		}
	}
}
