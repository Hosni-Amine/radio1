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
    public class DoctorDAL
    {
		/// <summary>
		/// GetAll Fonction retourner une liste de docteurs apartir de base de données
		/// </summary>
		/// <returns>liste des medecins</returns>
		public static List<Doctor> GetAll()
		{
			Migration.CreateDoctorTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.Doctor";
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
		/// <returns>liste des medecins</returns>
		public static List<Doctor> GetAll(DataTable table)
		{
			try
			{
				List<Doctor> doctors = new List<Doctor>();
				foreach (DataRow row in table.Rows)
				{
					doctors.Add(Get(row));
				}
				return doctors;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Methode permet de ajouter un medecin a la base de donnees et affecter lattribut User_id s'il est associer avec un compte utilisateur
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message AddDoctor(Doctor doctor, int? User_Id)
        {
            try
            {
				Migration.CreateDoctorTableIfNotExists();
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
                {
					DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					string sqlstr = "INSERT INTO Doctor (Prenom, Nom, Matricule, Telephone, Email, DateN, LieuN, SituationC, Sexe, Adresse, Ville, CodePostal,DateCreation ,User_Id) VALUES ( @Prenom, @Nom, @Matricule, @Telephone, @Email, @DateN, @LieuN, @SituationC, @Sexe, @Adresse, @Ville, @CodePostal, @DateCreation , @User_Id)";
                    SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, doctor);
					command.Parameters.AddWithValue("@DateCreation", cetTime);
					if (User_Id != null) { command.Parameters.AddWithValue("@User_Id", User_Id); }
					else { command.Parameters.AddWithValue("@User_Id", DBNull.Value); }
					Connection.DbConnection.NonQueryRequest(command);
                }
				if (User_Id != null)
				{
					return new Message(true, "Utilisateur ajouté avec Role Medecin, Merci de s'identifier .");
				}
				else
				{
					return new Message(true, "Medecin ajouté avec succès");
				}
            }
            catch (Exception ex)
            {
                return Message.HandleException(ex,"l'ajout");
            }
		}

		/// <summary>
		/// Methode permet de modifier un medecin de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message UpdateDoctor(Doctor doctor)
        {
            try
            {
                using (SqlConnection connection = Connection.DbConnection.GetConnection())
                {
                    string sqlstr = "UPDATE Doctor SET Prenom = @Prenom, Nom = @Nom, Matricule = @Matricule, Telephone = @Telephone, Email = @Email, DateN = @DateN, LieuN = @LieuN, SituationC = @SituationC, Sexe = @Sexe, Adresse = @Adresse, Ville = @Ville, CodePostal = @CodePostal WHERE id = @id";
                    SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, doctor);
                    command.Parameters.AddWithValue("@Id", doctor.Id);
                    Connection.DbConnection.NonQueryRequest(command);
                }
				return new Message(true, "Element modifier avec succés");
			}
            catch (Exception ex)
            {
                return Message.HandleException(ex,"le modification");
            }
        }

		/// <summary>
		/// Methode permet de supprimer un medecin de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message DeleteDoctor(int id)
        {
            try
            {
                using (SqlConnection connection = Connection.DbConnection.GetConnection())
                {
                    string sqlstr = "DELETE FROM Doctor WHERE id = @id";
                    SqlCommand command = new SqlCommand(sqlstr, connection);
                    command.Parameters.AddWithValue("@id", id);
                    Connection.DbConnection.NonQueryRequest(command);
                }
				return new Message(true, "Element supprimer avec succés");
			}
			catch (Exception ex)
			{
                return Message.HandleException(ex,"la suppression");
            }
        }

		/// <summary>
		/// Cette methode permet de retirer un medecin de database 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>un object Medecin</returns>
		public static Doctor GetById(int Id)
        {
            using (SqlConnection connection = Connection.DbConnection.GetConnection())
            {
                string sqlstr = "SELECT * FROM Doctor WHERE Id = @Id";
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
        /// / La fonction Get pour transformer une column en un object
        /// </summary>
        /// <param name="raw"></param>
        /// <returns> objet Medecin</returns>
        public static Doctor Get(DataRow raw)
        {
            try
            {
                Doctor doctor = new Doctor();
                doctor.Id = Int32.Parse(raw["Id"].ToString());
                doctor.Prenom = raw["Prenom"].ToString();
                doctor.Nom = raw["Nom"].ToString();
                doctor.Matricule = raw["Matricule"].ToString();
                doctor.Telephone = raw["Telephone"].ToString();
                doctor.Email = raw["Email"].ToString();
                doctor.DateN = raw["DateN"].ToString();
                doctor.LieuN = raw["LieuN"].ToString();
                doctor.SituationC = raw["SituationC"].ToString();
                doctor.Sexe = raw["Sexe"].ToString();
                doctor.Adresse = raw["Adresse"].ToString();
                doctor.Ville = raw["Ville"].ToString();
                doctor.CodePostal = Int32.Parse(raw["CodePostal"].ToString());
				doctor.DateCreation = DateTime.Parse(raw["DateCreation"].ToString());

				return doctor;
            }
            catch
            {
                return null;
            }
        }

		/// <summary>
		/// La fonction get by user id va retourner un objet apartir de cle etranger user_id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>objet medecin</returns>
		public static Doctor GetByUserId(int User_Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Doctor WHERE User_Id= @User_Id";
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
