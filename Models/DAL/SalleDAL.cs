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
		public static Message AddSalle(string Name)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "INSERT INTO dbo.AppareilRadio (Name) VALUES (@Name)";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Name", Name);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Salle ajouter avec succés avec aucune appareil !");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}
		public static Message DeleteSalle(int id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM Salle WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
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
			//Migration.CreateSalleIfNotExists();
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
				salle.Name= raw["Name"].ToString();
				salle.NombreAppareil = Int32.Parse(raw["NombreAppareil"].ToString());
				return salle;
			}
			catch
			{
				return null;
			}
		}
	}
}
