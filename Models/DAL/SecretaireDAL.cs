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
	public class SecretaireDAL
	{
		/// <summary>
		/// / La fonction Get pour transformer une column en un object
		/// </summary>
		/// <param name="raw"></param>
		/// <returns> objet Secretaire</returns>
		public static Secretaire Get(DataRow raw)
		{
			try
			{
				Secretaire Secretaire = new Secretaire();
				Secretaire.Id = Int32.Parse(raw["Id"].ToString());
				Secretaire.Prenom = raw["Prenom"].ToString();
				Secretaire.Nom = raw["Nom"].ToString();
				Secretaire.Email = raw["Email"].ToString();
				Secretaire.Sexe = raw["Sexe"].ToString();
				Secretaire.DateCreation = DateTime.Parse(raw["DateCreation"].ToString());

				return Secretaire;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// GetAll Fonction retourner une liste de Secretaire apartir de base de données
		/// </summary>
		/// <returns>liste des Secretaires</returns>
		public static List<Secretaire> GetAll()
		{
			Migration.CreateSecretaireTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.Secretaire";
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
		/// <returns>liste des Secretaire</returns>
		public static List<Secretaire> GetAll(DataTable table)
		{
			try
			{
				List<Secretaire> Secretaires = new List<Secretaire>();
				foreach (DataRow row in table.Rows)
				{
					Secretaires.Add(Get(row));
				}
				return Secretaires;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Methode permet d'ajouter un Secretaire de database et affecter lattribut User_id s'il est associer avec un compte utilisateur
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message AddSecretaire(Secretaire Secretaire , int? User_Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					Connection.Migration.CreateSecretaireTableIfNotExists();
					DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					string sqlstr = "INSERT INTO Secretaire (Prenom, Nom,Email, Sexe,DateCreation , User_Id ) VALUES ( @Prenom , @Nom ,@Email, @Sexe ,@DateCreation,@User_Id)";
					SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, Secretaire);
					command.Parameters.AddWithValue("@DateCreation", cetTime);
					if(User_Id != null) { command.Parameters.AddWithValue("@User_Id", User_Id); }
					else { command.Parameters.AddWithValue("@User_Id", DBNull.Value); }
					Connection.DbConnection.NonQueryRequest(command);
				}
				if (User_Id != null)
				{
					return new Message(true, "Utilisateur ajouté avec Role Secretaire , Merci de s'identifier .");
				}
				else
				{
					return new Message(true, "Secretaire ajouté avec succès");
				}
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex , "l'ajout");
			}
		}

		/// <summary>
		/// Methode permet de modifier un Secretaire de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message UpdateSecretaire(Secretaire Secretaire)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "UPDATE Secretaire SET Prenom = @Prenom, Nom = @Nom ,Email = @Email, Sexe = @Sexe WHERE id = @id";
					SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, Secretaire);
					command.Parameters.AddWithValue("@Id", Secretaire.Id);
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
		/// Methode permet de supprimer un Secretaire de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message DeleteSecretaire(int id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM Secretaire WHERE id = @id";
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
		/// Cette methode permet de retirer un Secretaire de la base de donnees 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>un object Secretaire</returns>
		public static Secretaire GetById(int Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Secretaire WHERE Id = @Id";
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
		/// <returns>objet Secretaire</returns>
		public static Secretaire GetByUserId(int User_Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Secretaire WHERE User_Id= @User_Id";
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
