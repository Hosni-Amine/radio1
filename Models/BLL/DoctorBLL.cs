using radio1.Models.DAL;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
    public class DoctorBLL
    {
        public static Message AddDoctor(Doctor doctor , int? User_Id)
        {
            return DoctorDAL.AddDoctor(doctor, User_Id);
        }
        public static Message UpdateDoctor(Doctor doctor)
        {
            return DoctorDAL.UpdateDoctor(doctor);
        }
        public static Message DeleteDoctor(int Id)
        {
            return DoctorDAL.DeleteDoctor(Id);
        }
        public static Doctor GetById(int id)
        {
            return DoctorDAL.GetById(id);
        }
        public static List<Doctor> GetAll()
        {
            return DoctorDAL.GetAll();
        }
		public static Doctor GetByUserId(int User_Id)
        {
            return DoctorDAL.GetByUserId(User_Id);
		}
        public static Doctor GetByStr(string Str) 
        {
            return DoctorDAL.GetByStr(Str);

		}
	}
}
