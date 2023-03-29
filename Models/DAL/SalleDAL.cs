using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Plugins;
using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;

namespace radio1.Models.DAL
{
	public class SalleDAL
	{
		public static Message AddSalle(Salle salle)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
                    Migration.CreateSalleIfNotExists();
                    DateTime utcTime = DateTime.UtcNow;
                    TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
                    string sql = "INSERT INTO dbo.Salle (Nom, Responsable, Emplacement, Operation, DateCreation) VALUES (@Nom, @Responsable, @Emplacement, @Operation, @DateCreation)";
                    SqlCommand command = Connection.DbConnection.CommandCreate(connection, sql, salle);
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
		public static Message DeleteSalle(int Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM Salle WHERE Id = @Id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Id", Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Salle supprimer avec succés ");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}
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
		public static List<Salle> GetAll(DataTable table)
		{
			try
			{
				List<Salle> apps = new List<Salle>();
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
				salle.Id = Int32.Parse(raw["Id"].ToString());
				salle.Responsable = Int32.Parse(raw["Responsable"].ToString());
                salle.Emplacement = raw["Emplacement"].ToString();
                salle.Operation = Int32.Parse(raw["Operation"].ToString());
                salle.Nom = raw["Nom"].ToString();
                return salle;
			}
			catch
			{
				return null;
			}
		}
	}
}
