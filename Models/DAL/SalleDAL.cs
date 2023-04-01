using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using radio1.Models.DAL;
using System.Data;

using System.Data.SqlClient;


namespace radio1.Models.DAL
{
	public class SalleDAL
	{
		/// <summary>
		/// Fonction permet dajouter une salle
		/// </summary>
		/// <param name="salle"></param>
		/// <returns></returns>
		public static Message AddSalle(Salle salle)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
                    Migration.CreateSalleIfNotExists();
                    DateTime utcTime = DateTime.UtcNow;
                    TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
                    string sql = "INSERT INTO dbo.Salle (Nom, Responsable, Emplacement, DateCreation) VALUES (@Nom, @Responsable, @Emplacement, @DateCreation)";
                    SqlCommand command = DbConnection.CommandCreate(connection, sql, salle);
                    command.Parameters.AddWithValue("@DateCreation", cetTime);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Salle ajouter avec succés !");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}

		/// <summary>
		/// Fonction permet de supprimer une salle avec ses type d'operation 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public static Message DeleteSalle(int Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM TypeOperation WHERE SalleId = @Id ; DELETE FROM Salle WHERE Id = @Id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Id", Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Salle et ces types d'operation supprimer avec succés ");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}

		/// <summary>
		/// Fonction qui retourn un objet salle a partir de son id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public static Salle GetById(int Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM dbo.Salle WHERE Id = @Id";
				SqlCommand command = new SqlCommand(sqlstr, connection);
				command.Parameters.AddWithValue("@Id", Id);
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
		/// Fonction qui retourn un objet salle a partir de son Nom
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Salle GetByName(string str)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM dbo.Salle WHERE Nom = @str";
				SqlCommand command = new SqlCommand(sqlstr, connection);
				command.Parameters.AddWithValue("@str", str);
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
		/// 3 Fonction permet de retourner la liste des salle de la base de données
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public static List<Salle> GetAll(DataTable table)
		{
			try
			{
				List<Salle> apps = new List<Salle>();
				foreach (DataRow row in table.Rows)
				{
					var salle = Get(row);
					List<TypeOperation> operations = TypeOperationDAL.GetAll(salle.Id);
					if(salle.Responsable != null)
					{
						salle.Responsable = DoctorDAL.GetById(salle.Responsable.Id);
					}
					salle.Operations= operations;
					apps.Add(salle);
				}
				return apps;
			}
			catch
			{
				return null;
			}
		}
		public static List<Salle> GetAll()
		{
			Migration.CreateSalleIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.Salle";
			connection.Open();
			SqlCommand command = new SqlCommand(sqlstr, connection);
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			return GetAll(table);
		}
		public static Salle Get(DataRow raw)
		{
			try
			{
				Salle salle = new Salle();
				if (!string.IsNullOrEmpty(raw["Responsable"].ToString()))
				{
					Doctor doc = new Doctor();
					salle.Responsable = doc;
					salle.Responsable.Id = Int32.Parse(raw["Responsable"].ToString());
				}
				salle.Id = Int32.Parse(raw["Id"].ToString());
				salle.Nom = raw["Nom"].ToString();
                salle.Emplacement = raw["Emplacement"].ToString();
				salle.DateCreation = DateTime.Parse(raw["DateCreation"].ToString());
				return salle;
			}
			catch
			{
				return null;
			}
		}
	}
}
