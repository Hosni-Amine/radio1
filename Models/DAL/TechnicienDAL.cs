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
    public class TechnicienDAL
	{
		/// <summary>
		/// / La fonction Get pour transformer une column en un object
		/// </summary>
		/// <param name="raw"></param>
		/// <returns> objet technicien</returns>
		public static Technicien Get(DataRow raw)
		{
			try
			{
				Technicien Technicien = new Technicien();
				Technicien.Id = Int32.Parse(raw["Id"].ToString());
				Technicien.Prenom = raw["Prenom"].ToString();
				Technicien.Nom = raw["Nom"].ToString();
				Technicien.Sexe = raw["Sexe"].ToString();
				Technicien.DateCreation = DateTime.Parse(raw["DateCreation"].ToString());

				return Technicien;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// GetAll Fonction retourner une liste de Technicien apartir de base de données
		/// </summary>
		/// <returns>liste des techniciens</returns>
		public static List<Technicien> GetAll()
		{
			Migration.CreateTechnicienTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.Technicien";
			connection.Open();
			SqlCommand command = new SqlCommand(sqlstr, connection);
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			return GetAll(table);
		}

		/// <summary>
		/// L'autre fonction GetAll permet de transformer un datatable en une liste
		/// </summary>
		/// <param name="table"></param>
		/// <returns>liste des Technicien</returns>
		public static List<Technicien> GetAll(DataTable table)
		{
			try
			{
				List<Technicien> Techniciens = new List<Technicien>();
				foreach (DataRow row in table.Rows)
				{
					Techniciens.Add(Get(row));
				}
				return Techniciens;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Methode permet d'ajouter un technicien de database et affecter lattribut User_id s'il est associer avec un compte utilisateur
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message AddTechnicien(Technicien Technicien , int? User_Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					Connection.Migration.CreateTechnicienTableIfNotExists();
					DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					string sqlstr = "INSERT INTO Technicien (Prenom, Nom, Sexe,DateCreation , User_Id ) VALUES ( @Prenom , @Nom , @Sexe ,@DateCreation,@User_Id)";
					SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, Technicien);
					command.Parameters.AddWithValue("@DateCreation", cetTime);
					if(User_Id != null) { command.Parameters.AddWithValue("@User_Id", User_Id); }
					else { command.Parameters.AddWithValue("@User_Id", DBNull.Value); }
					Connection.DbConnection.NonQueryRequest(command);
				}
				if (User_Id != null)
				{
					return new Message(true, "Utilisateur ajouté avec Role technicien , Merci de s'identifier .");
				}
				else
				{
					return new Message(true, "Technicien ajouté avec succès");
				}
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex , "l'ajout");
			}
		}

		/// <summary>
		/// Methode permet de modifier un technicien de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message UpdateTechnicien(Technicien Technicien)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "UPDATE Technicien SET Prenom = @Prenom, Nom = @Nom , Sexe = @Sexe WHERE id = @id";
					SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, Technicien);
					command.Parameters.AddWithValue("@Id", Technicien.Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Element modifier avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "le modification");
			}
		}

		/// <summary>
		/// Methode permet de supprimer un technicien de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message DeleteTechnicien(int id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM Technicien WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Element suppression avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}

		/// <summary>
		/// Cette methode permet de retirer un technicien de la base de donnees 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>un object technicien</returns>
		public static Technicien GetById(int Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Technicien WHERE Id = @Id";
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
		/// La fonction getbyuserid va retourner un objet apartir de cle etranger user_id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>objet Technicien</returns>
		public static Technicien GetByUserId(int User_Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Technicien WHERE User_Id= @User_Id";
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
	}
}
