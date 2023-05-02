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
				int id = 0;
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					connection.Open();
					Migration.CreateSalleIfNotExists();
                    DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.Local;
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
                    string sql = "INSERT INTO dbo.Salle (Nom, Responsable, Emplacement, DateCreation) VALUES (@Nom, @Responsable, @Emplacement, @DateCreation)"+
					"SELECT SCOPE_IDENTITY() AS NouvelElementId;";
					SqlCommand command = DbConnection.CommandCreate(connection, sql, salle);
                    command.Parameters.AddWithValue("@DateCreation", cetTime);
					id = Convert.ToInt32(command.ExecuteScalar());
					connection.Close();
				}
				return new Message(true, "Salle ajouté avec succés !", id );
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}

		/// <summary>
		/// Fonction permet dajouter une salle
		/// </summary>
		/// <param name="salle"></param>
		/// <returns></returns>
		public static Message EditSalle(Salle salle)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sql = "UPDATE [dbo].[Salle] SET Nom=@Nom , Emplacement=@Emplacement WHERE (Salle.Id = @id) ";
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue("@Nom", salle.Nom);
					command.Parameters.AddWithValue("@id", salle.Id);
					command.Parameters.AddWithValue("@Emplacement", salle.Emplacement);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "salle modifié avec succes !");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la modification !");
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
					string sqlstr = " DELETE FROM AppareilRadio WHERE SalleId = @Id ; DELETE FROM TypeOperation WHERE SalleId = @Id ; DELETE FROM Salle WHERE Id = @Id ; ";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Id", Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Salle et ces types d'operation supprimé avec succés ");
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
		public static Salle GetById(int? Id)
		{
			using (SqlConnection connection = DbConnection.GetConnection())
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
		/// Fonction qui permet de changer le responsable d'une salle
		/// </summary>
		/// <param name="salle_Id"></param>
		/// <param name="Id"></param>
		/// <returns></returns>
		public static Message SalleAffectation(int salle_Id,int Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "UPDATE [dbo].[Salle] SET [Responsable]=@Id WHERE ([dbo].[Salle].[Id] = @salle_Id);";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Id", Id);
					command.Parameters.AddWithValue("@salle_Id", salle_Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Responsable affecté avec Succés ");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'affectation !");
			}
		}

		public static Message SalleAffectationtech(int salle_Id, int Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "UPDATE [dbo].[Salle] SET [technicien_id]=@Id WHERE ([dbo].[Salle].[Id] = @salle_Id);";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Id", Id);
					command.Parameters.AddWithValue("@salle_Id", salle_Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Technicien affecté avec Succés ");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'affectation !");
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
				List<Salle> salles = new List<Salle>();
				List<TypeOperation> operations = TypeOperationDAL.GetAll(false, true, null);
				foreach (DataRow row in table.Rows)
				{
					var salle = Get(row);
					var list_op = new List<TypeOperation>();
					salle.Operations = list_op;
					foreach (var op in operations)
					{
						if(op.SalleId ==  salle.Id)
						{
							salle.Operations.Add(op);
						}
					}
					salle.Responsable = DoctorDAL.GetById(salle.Responsable.Id);
					salle.technicien = TechnicienDAL.GetById(salle.technicien.Id);
					salles.Add(salle);
				}
				return salles;
			}
			catch
			{
				return null;
			}
		}
		public static List<Salle> GetAllwithappareils()
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
			return GetAllwithappareils(table);
		}
		public static List<Salle> GetAllwithappareils(DataTable table)
		{
			try
			{
				List<Salle> salles = new List<Salle>();
				List<AppareilRadio> apps = AppareilRadioDAL.GetAll();
				foreach (DataRow row in table.Rows)
				{
					var salle = Get(row);
					salle.AppareilRadios = new List<AppareilRadio>();
					foreach(var app in apps)
					{
						if (salle.Id == app.SalleId)
						{
							salle.AppareilRadios.Add(app);
						}
					}
					salles.Add(salle);
				}
				return salles;
			}
			catch
			{
				return null;
			}
		}
	
		public static List<Salle> GetAll()
		{
			Migration.CreateSalleIfNotExists();
			SqlConnection connection = DbConnection.GetConnection();
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
				if (!string.IsNullOrEmpty(raw["technicien_id"].ToString()))
				{
					Technicien tec = new Technicien();
					salle.technicien = tec;
					salle.technicien.Id = Int32.Parse(raw["technicien_id"].ToString());
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
