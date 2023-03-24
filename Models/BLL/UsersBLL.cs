using radio1.Models.DAL;
using radio1.Models.Entities;
namespace radio1.Models.BLL
{
	public class UsersBLL
	{
		public static Message AddDoctor_User(Doctor doctor, Users user)
		{
			var msguser = UsersDAL.AddUser(user);
			if (msguser.Verification)
			{
				var _user = UsersDAL.GetByUserName(user.UserName);
				var msgdoc = DoctorBLL.AddDoctor(doctor, _user.Id);
				if (msgdoc.Verification)
				{
					return msguser;
				}
				else
				{
					UsersDAL.DeleteUser(_user.UserName);
					return msgdoc;
				}
			}
			else
			{
				return msguser;
			}
		}

		public static Message AddTech_User(Technicien technicien, Users user)
		{
			var msguser = UsersDAL.AddUser(user);
			if (msguser.Verification)
			{
				var _user = UsersDAL.GetByUserName(user.UserName);
				var msgtech = TechnicienBLL.AddTechnicien(technicien, _user.Id);
				if (msgtech.Verification)
				{
					return msguser;
				}
				else
				{
					UsersDAL.DeleteUser(_user.UserName);
					return msgtech;
				}
			}
			else
			{
				return msguser;
			}
		}

		public static Message AddAdmin_User(Users user)
		{
			return UsersDAL.AddUser(user);
		}

		public static Users GetByUserName(string str)
			{
			return UsersDAL.GetByUserName(str);
			}

		public static Users GetById(int User_Id)
		{
			return UsersDAL.GetById(User_Id);
		}
	}
}
