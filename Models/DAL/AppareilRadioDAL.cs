using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace radio1.Models.DAL
{
    public class AppareilRadioDAL
	{
		/// <summary>
		/// API pour les appareil te radioligies et les type disponible dans le systeme
		/// </summary>
		/// <param name="typeR"></param>
		/// <returns></returns>
		public static Message AddAppareilRadio(int TypeId)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sqlstr = "INSERT INTO dbo.AppareilRadio (TypeId) VALUES (@TypeId)";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@TypeId", TypeId);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Appareil ajouter avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}
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
				return new Message(true, "Element supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}
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
		public static List<AppareilRadio> GetAll()
		{
			//Migration.CreateAppareilRadiologieTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.AppareilRadio";
			connection.Open();
			SqlCommand command = new SqlCommand(sqlstr, connection);
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			return GetAll(table);
		}
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
		public static AppareilRadio Get(DataRow raw)
		{
			try
			{
				AppareilRadio app = new AppareilRadio();
				app.Id = Int32.Parse(raw["Id"].ToString());
				app.TypeId = Int32.Parse(raw["TypeId"].ToString());
				app.SalleId = Int32.Parse(raw["SalleId"].ToString());
				return app;
			}
			catch
			{
				return null;
			}
		}




		/// <summary>
		/// API pour les type d'appareil de radiologie disponible dans le systeme
		/// </summary>
		/// <param name="typeR"></param>
		/// <returns></returns>
		public static Message AddTypeRadio (string typeRadio)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sqlstr = "INSERT INTO dbo.TypeRadio (TypeR) VALUES (@typeR)";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@TypeR", typeRadio);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Type de scanner ajouter avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}
		public static List<TypeRadio> GetAllType(DataTable table)
		{
			try
			{
				List<TypeRadio> apps = new List<TypeRadio>();
				foreach (DataRow row in table.Rows)
				{
					apps.Add(GetT(row));
				}
				return apps;
			}
			catch
			{
				return null;
			}
		}
		public static List<TypeRadio> GetAllType()
		{
			Migration.CreateTechnicienTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.TypeRadio";
			connection.Open();
			SqlCommand command = new SqlCommand(sqlstr, connection);
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			return GetAllType(table);
		}
		public static Message DeleteTypeRadio(int id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM TypeRadio WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Element supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}
		public static TypeRadio GetT(DataRow raw)
		{
			try
			{
				TypeRadio app = new TypeRadio();
				app.Id = Int32.Parse(raw["Id"].ToString());
				app.TypeR = raw["TypeR"].ToString();
				return app;
			}
			catch
			{
				return null;
			}
		}
		





	}
}
