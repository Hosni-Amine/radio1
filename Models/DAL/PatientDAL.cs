using radio1.Models.DAL.Connection;
using radio1.Models.Entities; 
using System.Data;
using System.Data.SqlClient;


namespace radio1.Models.DAL
{
	public class PatientDAL
	{
		/// <summary>
		/// GetAll Fonction retourner une liste de docteurs apartir de base de données
		/// </summary>
		/// <returns>liste des patients</returns>
		public static List<Patient> GetAll()
		{
			Migration.CreatePatientTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.Patient";
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
		/// <returns>liste des patients</returns>
		public static List<Patient> GetAll(DataTable table)
		{
			try
			{
				List<Patient> Patients = new List<Patient>();
				foreach (DataRow row in table.Rows)
				{
					Patients.Add(Get(row));
				}
				return Patients;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Methode permet de ajouter un patient a la base de donnees et affecter lattribut User_id s'il est associer avec un compte utilisateur
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message AddPatient(Patient patient, int? User_Id)
		{
			try
			{
				Migration.CreatePatientTableIfNotExists();
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.Local;
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					string sqlstr = "INSERT INTO Patient (Prenom, Nom , Telephone , DateN, LieuN, SituationC, Sexe, Adresse, Ville , DateCreation ,User_Id) VALUES ( @Prenom, @Nom , @Telephone, @DateN, @LieuN, @SituationC, @Sexe, @Adresse, @Ville , @DateCreation , @User_Id)";
					SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, patient);
					command.Parameters.AddWithValue("@DateCreation", cetTime);
					if (User_Id != null) { command.Parameters.AddWithValue("@User_Id", User_Id); }
					else { command.Parameters.AddWithValue("@User_Id", DBNull.Value); }
					Connection.DbConnection.NonQueryRequest(command);
				}
				if (User_Id != null)
				{
					return new Message(true, "Utilisateur ajouté avec Role Patient, Merci de s'identifier .");
				}
				else
				{
					return new Message(true, "Patient ajouté avec succès");
				}
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}

		/// <summary>
		/// Methode permet de modifier un patient de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message UpdatePatient(Patient Patient)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "UPDATE Patient SET Prenom = @Prenom, Nom = @Nom , Telephone = @Telephone , DateN = @DateN, LieuN = @LieuN, SituationC = @SituationC, Sexe = @Sexe, Adresse = @Adresse, Ville = @Ville WHERE id = @id";
					SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, Patient);
					command.Parameters.AddWithValue("@Id", Patient.Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Patient modifier avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "le modification");
			}
		}

		/// <summary>
		/// Methode permet de supprimer un patient de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message DeletePatient(int id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM Patient WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Patient supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}

		/// <summary>
		/// Cette methode permet de retirer un patient de database 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns>un object patient</returns>
		public static Patient GetById(int Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Patient WHERE Id = @Id";
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
		/// <returns> objet patient</returns>
		public static Patient Get(DataRow raw)
		{
			try
			{
				Patient Patient = new Patient();
				Patient.Id = Int32.Parse(raw["Id"].ToString());
				Patient.Prenom = raw["Prenom"].ToString();
				Patient.Nom = raw["Nom"].ToString();
				Patient.Telephone = raw["Telephone"].ToString();
				Patient.Sexe = raw["Sexe"].ToString();
				Patient.Adresse = raw["Adresse"].ToString();
				Patient.DateCreation = DateTime.Parse(raw["DateCreation"].ToString());
				Patient.DateN = raw["DateN"].ToString();
				Patient.LieuN = raw["LieuN"].ToString();
				Patient.SituationC = raw["SituationC"].ToString();
				Patient.Ville = raw["Ville"].ToString();

				return Patient;
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
		/// <returns>objet patient</returns>
		public static Patient GetByUserId(int User_Id)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Patient WHERE User_Id= @User_Id";
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
		/// La fonction GetByStr permet de retourner s'il y a un patient avec le matricule ou email egale a str 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Patient GetByStr(string str)
		{
			using (SqlConnection connection = Connection.DbConnection.GetConnection())
			{
				string sqlstr = "SELECT * FROM Patient WHERE Matricule = @str OR Email = @str";
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

	}
}
