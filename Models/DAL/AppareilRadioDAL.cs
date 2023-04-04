using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using System.Data.SqlClient;
using System.Data;

namespace radio1.Models.DAL
{
	public class AppareilRadioDAL
	{
		/// <summary>
		/// Permet d'ajouter un type d'appareilradio 
		/// </summary>
		/// <param name="appareilradio"></param>
		/// <returns></returns>
		public static Message AddAppareilRadio(AppareilRadio appareilradio)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					Migration.CreateAppareilRadioTableIfNotExists();
					DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					string sqlstr = "INSERT INTO dbo.AppareilRadio (NumSerie ,Maintenance,DateCreation, SalleId) VALUES (@NumSerie ,@Maintenance,@DateCreation,@SalleId)";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@NumSerie", appareilradio.NumSerie);
					command.Parameters.AddWithValue("@SalleId", appareilradio.SalleId);
					command.Parameters.AddWithValue("@DateCreation", appareilradio.DateCreation);
					command.Parameters.AddWithValue("@Maintenance", appareilradio.Maintenance);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Type d'appareilradio ajouter avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}

		/// <summary>
		/// Permet de supprimer un type d'appareilradio 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Message DeleteAppareilRadio(int id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM AppareilRadio WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Appareil supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}
		
		/// <summary>
		/// Permet de modifier un type d'appareilradio 
		/// </summary>
		/// <param name="appareilradio"></param>
		/// <returns></returns>
		public static Message EditAppareilRadio(AppareilRadio appareilradio)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					Migration.CreateAppareilRadioTableIfNotExists();
					string sqlstr = "UPDATE AppareilRadio SET NumSerie = @NumSerie ,Maintenance = @Maintenance  WHERE Id = @Id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Maintenance", appareilradio.Maintenance);
					command.Parameters.AddWithValue("@NumSerie", appareilradio.NumSerie);
					command.Parameters.AddWithValue("@Id", appareilradio.Id);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Appareil modifier avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la modification");
			}
		}
		
		/// <summary>
		/// Permet de retirer un type d'appareilradio de la base de données
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public static AppareilRadio GetById(int Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM dbo.AppareilRadio WHERE Id = @Id";
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
		/// Permet de supprimer les types d'appareilradio associée a une salle
		/// </summary>
		/// <param name="Salle_Id"></param>
		/// <returns></returns>
		public static Message DeleteSalleAppareilRadios(int Salle_Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM AppareilRadio WHERE Salleid = @Salle_Id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Salle_Id", Salle_Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Groupe de type supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}

		/// <summary>
		/// 3 methode permet de retirer tous les elements de la base de données specifiquement pour GetAll qui peut retourner les types associé a une salle
		/// </summary>
		/// <param name="SalleId"></param>
		/// <returns></returns>
		public static List<AppareilRadio> GetAll(DataTable table)
		{
			try
			{
				List<AppareilRadio> apps = new List<AppareilRadio>();
				foreach (DataRow row in table.Rows)
				{
					apps.Add(Get(row));
				}
				return apps;
			}
			catch
			{
				return null;
			}
		}
		public static List<AppareilRadio> GetAll(int? SalleId)
		{
			Migration.CreateAppareilRadioTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.AppareilRadio";
			SqlCommand command = new SqlCommand();
			if (SalleId != null)
			{
				sqlstr = "SELECT * FROM dbo.AppareilRadio WHERE SalleId = @SalleId";
				command.Parameters.AddWithValue("@SalleId", SalleId);
			}
			command.CommandText = sqlstr;
			command.Connection = connection;
			connection.Open();
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			return GetAll(table);
		}
		public static AppareilRadio Get(DataRow raw)
		{
			try
			{
				AppareilRadio app = new AppareilRadio();
				app.Id = Int32.Parse(raw["Id"].ToString());
				app.NumSerie = raw["NumSerie"].ToString();
				app.Maintenance= Int32.Parse(raw["Maintenance"].ToString());
				app.SalleId = Int32.Parse(raw["SalleId"].ToString());
				return app;
			}
			catch
			{
				return null;
			}
		}
	}
}
