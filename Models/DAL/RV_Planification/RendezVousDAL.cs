using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using System.Data;
using System.Data.SqlClient;


namespace radio1.Models.DAL.RV_Planification
{
	public class RendezVousDAL
	{
		/// <summary>
		/// Methode permet de ajouter un un rendez-vous a la base de donnees 
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message EditRendezVous(RendezVous rendezVous)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sql = "UPDATE [dbo].[RendezVous] SET Date = @Date WHERE Id = @id ";
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue("@Id", rendezVous.Id);
					command.Parameters.AddWithValue("@Date", rendezVous.Date);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Rendez-Vous replanifier avec succes !");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la modification !");
			}
		}

		/// <summary>
		/// Fonctions qui peut retirer un Rendez-Vous avec le Id
		/// </summary>
		/// <returns>liste des medecins</returns>
		public static RendezVous GetById(int Id)
		{
			SqlConnection connection = DbConnection.GetConnection();
			connection.Open();
			string sqlstr = "SELECT rv.* , type_op.Nom AS TypeOperationName ,type_op.Id AS TypeOperation_Id, p.Nom AS PatientName ,p.Prenom AS PatientName2 , p.Telephone AS PatientPhone ,  d.Nom AS DoctorName ,d.Prenom AS DoctorName2 , d.Email AS DoctorEmail , t.Nom AS TechnicienName , t.Prenom AS TechnicienName2 , t.Email AS TechnicienEmail , s.Nom AS SecretaireName , s.Prenom AS SecretaireName2  , s.Email AS SecretaireEmail, a.numserie AS AppareilRadioNumSerie FROM RendezVous rv LEFT JOIN Doctor d ON rv.DoctorId = d.Id LEFT JOIN Technicien t ON rv.TechnicienId = t.Id LEFT JOIN Patient p ON rv.PatientId = p.Id LEFT JOIN TypeOperation type_op ON rv.TypeOperationId = type_op.Id LEFT JOIN Secretaire s ON rv.SecretaireId = s.Id LEFT JOIN AppareilRadio a ON type_op.AppareilRadioId = a.Id WHERE rv.Id = @Id";
			SqlCommand command = new SqlCommand(sqlstr, connection);
			command.Parameters.AddWithValue("@Id", Id);
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			if (table != null && table.Rows.Count != 0)
				return Get(table.Rows[0]);
			else
				return null;
		}

		/// <summary>
		/// Fonctions qui peut retirer la listes des Rendez-Vous avec les données nécessaire
		/// </summary>
		/// <returns>liste des medecins</returns>
		public static List<RendezVous> GetAll(Users user)
		{
			Migration.CreateRendezVousTableIfNotExists();
			SqlConnection connection = DbConnection.GetConnection();
			connection.Open();
			SqlCommand command = new SqlCommand();
			if (user.Role == "Doctor")
			{
				string sqlstr = "SELECT rv.* , type_op.Nom AS TypeOperationName ,type_op.Id AS TypeOperation_Id, p.Nom AS PatientName ,p.Prenom AS PatientName2 , p.Telephone AS PatientPhone ,  d.Nom AS DoctorName ,d.Prenom AS DoctorName2 , d.Email AS DoctorEmail , t.Nom AS TechnicienName , t.Prenom AS TechnicienName2 , t.Email AS TechnicienEmail , s.Nom AS SecretaireName , s.Prenom AS SecretaireName2  , s.Email AS SecretaireEmail, a.numserie AS AppareilRadioNumSerie FROM RendezVous rv LEFT JOIN Doctor d ON rv.DoctorId = d.Id LEFT JOIN Technicien t ON rv.TechnicienId = t.Id LEFT JOIN Patient p ON rv.PatientId = p.Id LEFT JOIN TypeOperation type_op ON rv.TypeOperationId = type_op.Id LEFT JOIN Secretaire s ON rv.SecretaireId = s.Id LEFT JOIN AppareilRadio a ON type_op.AppareilRadioId = a.Id WHERE rv.DoctorId IN (SELECT Id FROM Doctor WHERE User_Id = @UserId)";
				command = new SqlCommand(sqlstr, connection);
				command.Parameters.AddWithValue("@UserId", user.Id);
			}
			else if (user.Role == "Technicien")
			{
				string sqlstr = "SELECT rv.* , type_op.Nom AS TypeOperationName ,type_op.Id AS TypeOperation_Id, p.Nom AS PatientName ,p.Prenom AS PatientName2 , p.Telephone AS PatientPhone ,  d.Nom AS DoctorName ,d.Prenom AS DoctorName2 , d.Email AS DoctorEmail , t.Nom AS TechnicienName , t.Prenom AS TechnicienName2 , t.Email AS TechnicienEmail , s.Nom AS SecretaireName , s.Prenom AS SecretaireName2  , s.Email AS SecretaireEmail, a.numserie AS AppareilRadioNumSerie FROM RendezVous rv LEFT JOIN Doctor d ON rv.DoctorId = d.Id LEFT JOIN Technicien t ON rv.TechnicienId = t.Id LEFT JOIN Patient p ON rv.PatientId = p.Id LEFT JOIN TypeOperation type_op ON rv.TypeOperationId = type_op.Id LEFT JOIN Secretaire s ON rv.SecretaireId = s.Id LEFT JOIN AppareilRadio a ON type_op.AppareilRadioId = a.Id WHERE rv.TechnicienId IN (SELECT Id FROM Technicien WHERE User_Id = @UserId) ";
				command = new SqlCommand(sqlstr, connection);
				command.Parameters.AddWithValue("@UserId", user.Id);
			}
			else if (user.Role == "Patient")
			{
				string sqlstr = "SELECT rv.* , type_op.Nom AS TypeOperationName ,type_op.Id AS TypeOperation_Id, p.Nom AS PatientName ,p.Prenom AS PatientName2 , p.Telephone AS PatientPhone ,  d.Nom AS DoctorName ,d.Prenom AS DoctorName2 , d.Email AS DoctorEmail , t.Nom AS TechnicienName , t.Prenom AS TechnicienName2 , t.Email AS TechnicienEmail , s.Nom AS SecretaireName , s.Prenom AS SecretaireName2  , s.Email AS SecretaireEmail, a.numserie AS AppareilRadioNumSerie FROM RendezVous rv LEFT JOIN Doctor d ON rv.DoctorId = d.Id LEFT JOIN Technicien t ON rv.TechnicienId = t.Id LEFT JOIN Patient p ON rv.PatientId = p.Id LEFT JOIN TypeOperation type_op ON rv.TypeOperationId = type_op.Id LEFT JOIN Secretaire s ON rv.SecretaireId = s.Id LEFT JOIN AppareilRadio a ON type_op.AppareilRadioId = a.Id WHERE rv.PatientId IN (SELECT Id FROM Patient WHERE Id = @UserId)";
				command = new SqlCommand(sqlstr, connection);
				command.Parameters.AddWithValue("@UserId", user.Id);
			}
			else
			{
				string sqlstr = "SELECT rv.* , type_op.Nom AS TypeOperationName ,type_op.Id AS TypeOperation_Id, p.Nom AS PatientName ,p.Prenom AS PatientName2 , p.Telephone AS PatientPhone ,  d.Nom AS DoctorName ,d.Prenom AS DoctorName2 , d.Email AS DoctorEmail , t.Nom AS TechnicienName , t.Prenom AS TechnicienName2 , t.Email AS TechnicienEmail , s.Nom AS SecretaireName , s.Prenom AS SecretaireName2  , s.Email AS SecretaireEmail, a.numserie AS AppareilRadioNumSerie FROM RendezVous rv LEFT JOIN Doctor d ON rv.DoctorId = d.Id LEFT JOIN Technicien t ON rv.TechnicienId = t.Id LEFT JOIN Patient p ON rv.PatientId = p.Id LEFT JOIN TypeOperation type_op ON rv.TypeOperationId = type_op.Id LEFT JOIN Secretaire s ON rv.SecretaireId = s.Id LEFT JOIN AppareilRadio a ON type_op.AppareilRadioId = a.Id ";
				command = new SqlCommand(sqlstr, connection);
			}
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			return GetAll(table);
		}
		public static List<RendezVous> GetAll(DataTable table)
		{
			try
			{
				List<RendezVous> RendezVouss = new List<RendezVous>();
				foreach (DataRow row in table.Rows)
				{
					RendezVouss.Add(Get(row));
				}
				return RendezVouss;
			}
			catch
			{
				return null;
			}
		}
		public static RendezVous Get(DataRow raw)
		{
			try
			{
				RendezVous rendezvous = new RendezVous();
				rendezvous.doctor = new Doctor();
				rendezvous.technicien = new Technicien();
				rendezvous.secretaire = new Secretaire();
				rendezvous.patient = new Patient();
				rendezvous.Id = Convert.ToInt32(raw["Id"]);
				rendezvous.Date = Convert.ToDateTime(raw["Date"]);
				DateTime utcTime = DateTime.UtcNow;
				TimeZoneInfo cetTimeZone = TimeZoneInfo.Local;
				DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
				rendezvous.Status = ChangeStatus(rendezvous.Date, cetTime);
				rendezvous.Appareil_NumSerie = Convert.ToString(raw["AppareilRadioNumSerie"]);
				rendezvous.Examen = Convert.ToString(raw["Examen"]);
				// Données relative au Patient
				rendezvous.patient.Id = Convert.ToInt32(raw["PatientId"]);
				rendezvous.patient.Nom = raw["PatientName"].ToString();
				rendezvous.patient.Prenom = raw["PatientName2"].ToString();
				rendezvous.patient.Telephone = raw["PatientPhone"].ToString();
				// Données relative au Medecin
				rendezvous.doctor.Id = Convert.ToInt32(raw["DoctorId"]);
				rendezvous.doctor.Nom = Convert.ToString(raw["DoctorName"]);
				rendezvous.doctor.Prenom = Convert.ToString(raw["DoctorName2"]);
				rendezvous.doctor.Email = Convert.ToString(raw["DoctorEmail"]);
				// Données relative au Technicien 
				rendezvous.TypeOperation = new TypeOperation();
				rendezvous.TypeOperation.Id = Convert.ToInt32(raw["TypeOperation_Id"]);
				rendezvous.TypeOperation.Nom = Convert.ToString(raw["TypeOperationName"]);
				// Données relative au Technicien 
				rendezvous.technicien.Id = Convert.ToInt32(raw["TechnicienId"]);
				rendezvous.technicien.Nom = Convert.ToString(raw["TechnicienName"]);
				rendezvous.technicien.Prenom = Convert.ToString(raw["TechnicienName2"]);
				rendezvous.technicien.Email = Convert.ToString(raw["TechnicienEmail"]);
				// Données relative au Secretaire
				if (!string.IsNullOrEmpty(raw["SecretaireId"].ToString()))
				{
					rendezvous.secretaire.Id = Convert.ToInt32(raw["SecretaireId"]);
					rendezvous.secretaire.Nom = Convert.ToString(raw["SecretaireName"]);
					rendezvous.secretaire.Prenom = Convert.ToString(raw["SecretaireName2"]);
					rendezvous.secretaire.Email = Convert.ToString(raw["SecretaireEmail"]);
				}
				return rendezvous;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Methode permet de supprimer un medecin de la base de donnees
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message DeleteRendezVous(int id)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM RendezVous WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Rendez-Vous supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}

		/// <summary>
		/// Methode permet de ajouter un un rendez-vous a la base de donnees 
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Message personaliser de resultat</returns>
		public static Message AddRendezVous(RendezVous RendezVous)
		{
			try
			{
				Migration.CreateRendezVousTableIfNotExists();
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					DateTime utcTime = DateTime.UtcNow;
					TimeZoneInfo cetTimeZone = TimeZoneInfo.Local;
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					string sqlstr = "INSERT INTO [dbo].[RendezVous] ([Date], [Status], [Examen], [PatientId], [TypeOperationId], [DoctorId], [TechnicienId], [SecretaireId]) VALUES (@Date, @Status, @Examen, @PatientId, @TypeOperationId, @DoctorId, @TechnicienId, (SELECT [Id] FROM [dbo].[Secretaire] WHERE [User_Id] = @SecretaireId))"; 
					SqlCommand command = DbConnection.CommandCreate(connection, sqlstr, RendezVous);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Rondez-Vous Bien Planifié");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}

		/// <summary>
		/// Methode Permet de retourner la disponibilité de chaque couple salle medecin appareil avec les rendez-vous associeé
		/// </summary>
		/// <param name="typeoperation"></param>
		/// <returns></returns>
		public static List<Disponibilite> GetDisponibilite(string typeoperation)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					connection.Open();
					string sqlstr = "SELECT Salle.nom AS nom_salle,TypeOperation.Id AS TypeOperation_Id , AppareilRadio.NumSerie AS AppareilRadio_Nom , Doctor.Nom AS nom_Doctor , Doctor.Prenom AS prenom_Doctor FROM TypeOperation JOIN AppareilRadio ON TypeOperation.AppareilRadioId = AppareilRadio.id JOIN Salle ON TypeOperation.salleId = Salle.id JOIN Doctor ON Salle.Responsable = Doctor.id WHERE TypeOperation.nom = @TypeOperation AND TypeOperation.AppareilRadioId IS NOT NULL ORDER BY AppareilRadio_Nom ASC ";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@TypeOperation", typeoperation);
					DataTable tablereferences = new DataTable();
					SqlDataReader reader = command.ExecuteReader();
					tablereferences.Load(reader);
					Migration.CreateRendezVousTableIfNotExists();
					sqlstr = "SELECT RendezVous.Date AS Date ,AppareilRadio.NumSerie AS AppareilRadio_Nom FROM TypeOperation JOIN AppareilRadio ON TypeOperation.AppareilRadioId = AppareilRadio.id JOIN Rendezvous ON Rendezvous.TypeOperationId = TypeOperation.id WHERE TypeOperation.AppareilRadioId IN ( SELECT TypeOperation.AppareilRadioId FROM TypeOperation WHERE TypeOperation.nom = @TypeOperation ) ORDER BY AppareilRadio_Nom ASC";
					//sqlstr = "SELECT RendezVous.Date AS Date ,AppareilRadio.NumSerie AS AppareilRadio_Nom FROM TypeOperation JOIN AppareilRadio ON TypeOperation.AppareilRadioId = AppareilRadio.id JOIN Rendezvous ON Rendezvous.TypeOperationId = TypeOperation.id WHERE TypeOperation.nom = @TypeOperation AND TypeOperation.AppareilRadioId IS NOT NULL ORDER BY AppareilRadio_Nom ASC";
					command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@TypeOperation", typeoperation);
					DataTable tablerendervous = new DataTable();
					reader = command.ExecuteReader();
					tablerendervous.Load(reader);
					connection.Close();
					return GetAllDisponibilite(tablereferences, tablerendervous);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
		public static List<Disponibilite> GetAllDisponibilite(DataTable tablereferences, DataTable tablerendervous)
		{
			try
			{
				int i = 0;
				List<Disponibilite> disponibilites = new List<Disponibilite>();
				foreach (DataRow rawref in tablereferences.Rows)
				{
					Disponibilite disponibilite = new Disponibilite();
					disponibilite.Dates = new List<DateTime>();
					disponibilite.TypeOperation_Id = Convert.ToInt32(rawref["TypeOperation_Id"]);
					disponibilite.Nom_Doctor = Convert.ToString(rawref["nom_Doctor"]) + ' ' + Convert.ToString(rawref["prenom_Doctor"]);
					disponibilite.Nom_Appareil = Convert.ToString(rawref["AppareilRadio_Nom"]);
					disponibilite.Nom_Salle = Convert.ToString(rawref["nom_salle"]);
					disponibilites.Add(disponibilite);
				}
				foreach (DataRow rawrv in tablerendervous.Rows)
				{
					if (disponibilites[i].Nom_Appareil == Convert.ToString(rawrv["AppareilRadio_Nom"]))
					{
						disponibilites[i].Dates.Add(Convert.ToDateTime(rawrv["Date"]));
					}
					else
					{
						i++;
						disponibilites[i].Dates.Add(Convert.ToDateTime(rawrv["Date"]));
					}
				}
				return disponibilites;
			}
			catch
			{
				return null;
			}
		}

		public static string ChangeStatus(DateTime RV_date, DateTime CT)
		{
			if (RV_date.AddHours(1) >= CT && RV_date <= CT)
			{
				return "En cours";
			}

			else if (RV_date <= CT)
			{
				return "Terminé";
				//using (SqlConnection connection = DbConnection.GetConnection())
				//{
				//	string sqlstr = "UPDATE [dbo].[RendezVous] SET  [Status] = @status WHERE [Id] = @Id";
				//	SqlCommand command = new SqlCommand(sqlstr, connection);
				//	command.Parameters.AddWithValue("@id", RV.Id);
				//	command.Parameters.AddWithValue("@Status", "Terminé");
				//	DbConnection.NonQueryRequest(command);
				//}
			}
			else
			{
				return "Planifié";
			}
		}
	}
}