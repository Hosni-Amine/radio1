using radio1.Models.DAL;
using radio1.Models.Entities;
using System.Data.SqlClient;
using radio1.Models.DAL.Connection;
using System.Data;

namespace radio1.Models.BLL
{
	public class UsersBLL
	{
		public static Status State()
		{
            using (SqlConnection connection = DbConnection.GetConnection())
            {
                string sqlstr = "SELECT (SELECT COUNT(*) FROM Technicien) AS TechnicienCount, (SELECT COUNT(*) FROM Doctor) AS DoctorCount, (SELECT COUNT(*) FROM Salle) AS SalleCount, (SELECT COUNT(*) FROM AppareilRadio) AS AppareilRadioCount;";
                DataTable table = new DataTable();
                connection.Open();
				SqlCommand command = new SqlCommand(sqlstr, connection);
                SqlDataReader reader = command.ExecuteReader();
                table.Load(reader);
                connection.Close();
				if (table != null && table.Rows.Count != 0)
				{
					Status status = new Status();
					status.WorkersCount = Int32.Parse(table.Rows[0]["TechnicienCount"].ToString());
					status.DoctorsCount = Int32.Parse(table.Rows[0]["DoctorCount"].ToString());
					status.SallesCount = Int32.Parse(table.Rows[0]["SalleCount"].ToString());
					status.AppareilsCount = Int32.Parse(table.Rows[0]["AppareilRadioCount"].ToString());
					return status;
				}
				else
                    return null;
            }
		}

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
