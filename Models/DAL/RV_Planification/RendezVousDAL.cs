using Microsoft.CodeAnalysis.CSharp.Syntax;
using Org.BouncyCastle.Asn1.X509;
using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using System.Data;
using System.Data.SqlClient;


namespace radio1.Models.DAL.RV_Planification
{
	public class RendezVousDAL
	{
		/// <summary>
		/// Fonctions qui peut retirer la listes des Rendez-Vous avec les données nécessaire
		/// </summary>
		/// <returns>liste des medecins</returns>
		public static List<RendezVous> GetAll()
		{
			Migration.CreateRendezVousTableIfNotExists();
			SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT rv.* , type_op.Nom AS TypeOperationName , d.Prenom AS DoctorName2 , d.Nom AS DoctorName,d.Prenom AS DoctorName2 , d.Email AS DoctorEmail , t.Nom AS TechnicienName , t.Prenom AS TechnicienName2 , t.Email AS TechnicienEmail , s.Nom AS SecretaireName , s.Prenom AS SecretaireName2  , s.Email AS SecretaireEmail FROM RendezVous rv LEFT JOIN Doctor d ON rv.DoctorId = d.Id LEFT JOIN Technicien t ON rv.TechnicienId = t.Id LEFT JOIN TypeOperation type_op ON rv.TypeOperationId = type_op.Id LEFT JOIN Secretaire s ON rv.SecretaireId = s.Id";
            connection.Open();
			SqlCommand command = new SqlCommand(sqlstr, connection);
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
				rendezvous.Id = Convert.ToInt32(raw["Id"]);
				rendezvous.Date = Convert.ToDateTime(raw["Date"]);
				rendezvous.Status = Convert.ToString(raw["Status"]);
				rendezvous.Examen = Convert.ToString(raw["Examen"]);
				rendezvous.Nom_Patient = Convert.ToString(raw["Nom_Patient"]);
				// Données relative au Medecin
				rendezvous.doctor.Id = Convert.ToInt32(raw["DoctorId"]);
				rendezvous.doctor.Nom = Convert.ToString(raw["DoctorName"]);
				rendezvous.doctor.Prenom = Convert.ToString(raw["DoctorName2"]);
				rendezvous.doctor.Email = Convert.ToString(raw["DoctorEmail"]);
				// Données relative au Technicien 
				rendezvous.TypeOperation = new TypeOperation();
				rendezvous.TypeOperation.Nom = Convert.ToString(raw["TypeOperationName"]);
				// Données relative au Technicien 
				rendezvous.technicien.Id = Convert.ToInt32(raw["TechnicienId"]);
				rendezvous.technicien.Nom = Convert.ToString(raw["TechnicienName"]);
				rendezvous.technicien.Prenom = Convert.ToString(raw["TechnicienName2"]);
				rendezvous.technicien.Email = Convert.ToString(raw["TechnicienEmail"]);
				// Données relative au Secretaire
				rendezvous.secretaire.Id = Convert.ToInt32(raw["SecretaireId"]);
				rendezvous.secretaire.Nom = Convert.ToString(raw["SecretaireName"]);
				rendezvous.secretaire.Prenom = Convert.ToString(raw["SecretaireName2"]);
				rendezvous.secretaire.Email = Convert.ToString(raw["SecretaireEmail"]);

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
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM RendezVous WHERE id = @id";
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
					TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
					DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cetTimeZone);
					string sqlstr = "INSERT INTO [dbo].[RendezVous] ([Date], [Status], [Examen], [Nom_Patient], [TypeOperationId], [DoctorId], [TechnicienId], [SecretaireId]) VALUES (@Date, @Status, @Examen, @Nom_Patient, @TypeOperationId, @DoctorId, @TechnicienId, @SecretaireId)";
					SqlCommand command = DbConnection.CommandCreate(connection, sqlstr, RendezVous);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Rondez-Vous Bien Planifié");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}

		public static List<Disponibilite> GetDisponibilite(string typeoperation)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					connection.Open();
					string sqlstr = "SELECT Salle.nom AS nom_salle,  AppareilRadio.NumSerie AS AppareilRadio_Nom , Doctor.Nom AS nom_Doctor , Doctor.Prenom AS prenom_Doctor FROM TypeOperation JOIN AppareilRadio ON TypeOperation.AppareilRadioId = AppareilRadio.id JOIN Salle ON TypeOperation.salleId = Salle.id JOIN Doctor ON Salle.Responsable = Doctor.id WHERE TypeOperation.nom = @TypeOperation AND TypeOperation.AppareilRadioId IS NOT NULL ORDER BY AppareilRadio_Nom ASC";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@TypeOperation", typeoperation);
					DataTable tablereferences = new DataTable();
					SqlDataReader reader = command.ExecuteReader();
					tablereferences.Load(reader);
					sqlstr = "SELECT RendezVous.Date AS Date ,AppareilRadio.NumSerie AS AppareilRadio_Nom FROM TypeOperation JOIN AppareilRadio ON TypeOperation.AppareilRadioId = AppareilRadio.id JOIN Rendezvous ON Rendezvous.TypeOperationId = TypeOperation.id WHERE TypeOperation.nom = @TypeOperation AND TypeOperation.AppareilRadioId IS NOT NULL ORDER BY AppareilRadio_Nom ASC";
					command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@TypeOperation", typeoperation);
					DataTable tablerendervous = new DataTable();
					reader = command.ExecuteReader();
					tablerendervous.Load(reader);
					connection.Close();
					return GetAllDisponibilite(tablereferences,tablerendervous);
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
	}
}






/// <summary>
/// Methode permet de modifier un medecin de la base de donnees
/// </summary>
/// <param name="id"></param>
/// <returns>Message personaliser de resultat</returns>
//public static Message UpdateRendezVous(RendezVous RendezVous)
//{
//	try
//	{
//		using (SqlConnection connection = Connection.DbConnection.GetConnection())
//		{
//			string sqlstr = "UPDATE RendezVous SET Prenom = @Prenom, Nom = @Nom, Matricule = @Matricule, Telephone = @Telephone, Email = @Email, DateN = @DateN, LieuN = @LieuN, SituationC = @SituationC, Sexe = @Sexe, Adresse = @Adresse, Ville = @Ville, CodePostal = @CodePostal WHERE id = @id";
//			SqlCommand command = Connection.DbConnection.CommandCreate(connection, sqlstr, RendezVous);
//			command.Parameters.AddWithValue("@Id", RendezVous.Id);
//			Connection.DbConnection.NonQueryRequest(command);
//		}
//		return new Message(true, "Element modifier avec succés");
//	}
//	catch (Exception ex)
//	{
//		return Message.HandleException(ex, "le modification");
//	}
//}

