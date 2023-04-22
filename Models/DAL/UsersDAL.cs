using radio1.Models.Entities;
using System.Data;
using System.Data.SqlClient;

namespace radio1.Models.DAL
{
	public class UsersDAL
	{
		/// <summary>
		/// Fonction permet d'ajouter un utilisateur 
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static Message AddUser(Users user)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					Connection.Migration.CreateUsersTableIfNotExists();
					DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.Local;
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					Console.Write(cetTime);
					string sqlstr = "INSERT INTO dbo.Users (UserName, Password, Role, DateCreation) VALUES (@UserName, @Password ,@Role,@DateCreation)";
					SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, user);	
					command.Parameters.AddWithValue("@DateCreation", cetTime);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Utilisateur ajouter avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}

		/// <summary>
		/// Fonction permet de retourner un objet utilisateur d'apres son non s'il existe
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Users? GetByUserName(string str)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				Connection.Migration.CreateUsersTableIfNotExists();
				string sqlstr = "SELECT * FROM Users WHERE UserName = @UserName ";
				SqlCommand command = new SqlCommand(sqlstr, connection);
				command.Parameters.AddWithValue("@UserName", str);
				DataTable table = new DataTable();
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				table.Load(reader);
				connection.Close();
				if (table != null && table.Rows.Count != 0)
					return Get(table.Rows[0]);
				else
					return null;
			}
		}

		/// <summary>
		/// Fonction qui retourn lobjet user d'apres son id de la base de données
		/// </summary>
		/// <param name="User_Id"></param>
		/// <returns></returns>
		public static Users GetById(int User_Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				Connection.Migration.CreateUsersTableIfNotExists();
				string sqlstr = "SELECT * FROM Users WHERE Id = @User_Id";
				SqlCommand command = new SqlCommand(sqlstr, connection);
				command.Parameters.AddWithValue("@User_Id", User_Id);
				DataTable table = new DataTable();
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				table.Load(reader);
				connection.Close();
				if (table != null && table.Rows.Count != 0)
					return Get(table.Rows[0]);
				else
					return null;
			}
		}


		/// <summary>
		/// Fonction permet de supprimer un utilisateur d'apres son UserName
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Message DeleteUser(string str)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM Users WHERE UserName = @UserName";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@UserName", str);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Element supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}


		/// <summary>
		/// Fonction permet de retourner un objet utilisateur d'apres un datarow 
		/// </summary>
		/// <param name="raw"></param>
		/// <returns></returns>
		public static Users Get(DataRow raw)
		{
			try
			{
				Users user = new Users();
				user.Id = Int32.Parse(raw["Id"].ToString());
				user.UserName = raw["UserName"].ToString();
				user.Password = raw["Password"].ToString();
				user.Role = raw["Role"].ToString();
				user.DateCreation = DateTime.Parse(raw["DateCreation"].ToString());
				return user;
			}
			catch
			{
				return null;
			}
		}

		  
	}
}
