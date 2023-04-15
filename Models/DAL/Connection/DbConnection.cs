using radio1.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace radio1.Models.DAL.Connection
{
    public class DbConnection
    {
        static string CS = "workstation id=radiologie.mssql.somee.com;packet size=4096;user id=hosniamine_SQLLogin_1;pwd=kkke54dsdo;data source=radiologie.mssql.somee.com;persist security info=False;initial catalog=radiologie";
        //static string CS = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=radio;Integrated Security=True";

        public static SqlConnection? GetConnection()
        {
            SqlConnection connect;
            try
            {
                connect = new SqlConnection(CS);
				return connect;
			}
			catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Methode a pour connecter et executer la command sql en prendre consediration des erreur de connection et execution 
        /// </summary>
        /// <param name="StrRequest"></param>
        /// <param name="MyConnection"></param>
        /// <returns>Message personalisé qui decrit le resultat</returns>
        public static int NonQueryRequest(SqlCommand MyCommand)
        {
            try
            {
                try
                {
                    MyCommand.Connection.Open();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                return MyCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }

        /// <summary>
        /// Methode a pour connecter et executer la command sql en prendre consediration des erreur de connection et execution 
        /// </summary>
        /// <param name="StrRequest"></param>
        /// <param name="MyConnection"></param>
        /// <returns>Message personalisé qui decrit le resultat</returns>
        public static Message NonQueryRequest(string StrRequest, SqlConnection MyConnection)
        {
            try
            {
                try
                {
                    MyConnection.Open();
                }
                catch (SqlException e)
                {

                    return new Message(false, "DataBase Connection Error : " + e);
                }

                SqlCommand MyCommand = new SqlCommand(StrRequest, MyConnection);
                MyCommand.ExecuteNonQuery();
                return new Message(true, "Database and Query Execution successfuly");
            }
            catch (SqlException e)
            {
                return new Message(false, "Query Execution Error " + e);
            }
            finally
            {
                MyConnection.Close();
            }
        }
         
		/// <summary>
		/// Fonction pour créer une command pour l'objet Secretaire
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlstr"></param>
		/// <param name="Secretaire"></param>
		/// <returns>command a executer pour un Medecin</returns>
		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Technicien technicien)
        {
            SqlCommand command = new SqlCommand(sqlstr, connection);
            command.Parameters.AddWithValue("@Prenom", technicien.Prenom);
            command.Parameters.AddWithValue("@Nom", technicien.Nom);
            command.Parameters.AddWithValue("@Sexe", technicien.Sexe);
			command.Parameters.AddWithValue("@Email", technicien.Email);

			return command;
        }

		/// <summary>
		/// Fonction pour créer une command pour l'objet Patient
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlstr"></param>
		/// <param name="Secretaire"></param>
		/// <returns>command a executer pour un Medecin</returns>
		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Patient patient)
		{
			SqlCommand command = new SqlCommand(sqlstr, connection);
			command.Parameters.AddWithValue("@Prenom", patient.Prenom);
			command.Parameters.AddWithValue("@Nom", patient.Nom);
			command.Parameters.AddWithValue("@Telephone", patient.Telephone);
			command.Parameters.AddWithValue("@DateN", patient.DateN);
			command.Parameters.AddWithValue("@LieuN", patient.LieuN);
			command.Parameters.AddWithValue("@SituationC", patient.SituationC);
			command.Parameters.AddWithValue("@Sexe", patient.Sexe);
			command.Parameters.AddWithValue("@Adresse", patient.Adresse);
			command.Parameters.AddWithValue("@Ville", patient.Ville);
			return command;
		}

		/// <summary>
		/// Fonction pour créer une command pour l'objet Secretaire
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlstr"></param>
		/// <param name="Secretaire"></param>
		/// <returns>command a executer pour un Medecin</returns>
		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Secretaire secretaire)
		{
			SqlCommand command = new SqlCommand(sqlstr, connection);
			command.Parameters.AddWithValue("@Prenom", secretaire.Prenom);
			command.Parameters.AddWithValue("@Nom", secretaire.Nom);
			command.Parameters.AddWithValue("@Sexe", secretaire.Sexe);
			command.Parameters.AddWithValue("@Email", secretaire.Email);
			return command;
		}

		/// <summary>
		/// Fonction pour créer une command pour l'objet user
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlstr"></param>
		/// <param name="user"></param>
		/// <returns>command a executer pour un Medecin</returns>
		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Users user)
		{
			SqlCommand command = new SqlCommand(sqlstr, connection);
            command.Parameters.AddWithValue("@UserName", user.UserName);
			command.Parameters.AddWithValue("@Password", user.Password);
			command.Parameters.AddWithValue("@Role", user.Role);
			return command;
		}

		/// <summary>
		/// Fonction pour créer une command pour l'objet doctor
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlstr"></param>
		/// <param name="doctor"></param>
		/// <returns>command a executer pour un Medecin</returns>
		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Doctor doctor)
        {
            SqlCommand command = new SqlCommand(sqlstr, connection);
            command.Parameters.AddWithValue("@Prenom", doctor.Prenom);
            command.Parameters.AddWithValue("@Nom", doctor.Nom);
            command.Parameters.AddWithValue("@Matricule", doctor.Matricule);
            command.Parameters.AddWithValue("@Telephone", doctor.Telephone);
            command.Parameters.AddWithValue("@Email", doctor.Email);
            command.Parameters.AddWithValue("@DateN", doctor.DateN);
            command.Parameters.AddWithValue("@LieuN", doctor.LieuN);
            command.Parameters.AddWithValue("@SituationC", doctor.SituationC);
            command.Parameters.AddWithValue("@Sexe", doctor.Sexe);
            command.Parameters.AddWithValue("@Adresse", doctor.Adresse);
            command.Parameters.AddWithValue("@Ville", doctor.Ville);
            command.Parameters.AddWithValue("@CodePostal", doctor.CodePostal);
            return command;
        }

		/// <summary>
		/// Fonction pour créer une command pour l'objet salle
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlstr"></param>
		/// <param name="salle"></param>
		/// <returns>command a executer pour un Medecin</returns>
		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Salle salle)
        {
            SqlCommand command = new SqlCommand(sqlstr, connection);
            command.Parameters.AddWithValue("@Nom", salle.Nom);
            command.Parameters.AddWithValue("@Responsable", salle.Responsable.Id);
            command.Parameters.AddWithValue("@Emplacement", salle.Emplacement);
            return command;
        }

		/// <summary>
		/// Fonction pour créer une command pour l'objet rendez-vous
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlstr"></param>
		/// <param name="salle"></param>
		/// <returns>command a executer pour un Medecin</returns>
		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, RendezVous rendezvous)
		{
			SqlCommand command = new SqlCommand(sqlstr, connection);
			command.Parameters.AddWithValue("@Date", rendezvous.Date);
			command.Parameters.AddWithValue("@Status", rendezvous.Status);
			command.Parameters.AddWithValue("@Examen", rendezvous.Examen);
			command.Parameters.AddWithValue("@PatientId", rendezvous.patient.Id);
			command.Parameters.AddWithValue("@TypeOperationId", rendezvous.TypeOperation.Id);
			command.Parameters.AddWithValue("@DoctorId", rendezvous.doctor.Id);
			command.Parameters.AddWithValue("@TechnicienId", rendezvous.technicien.Id);
			command.Parameters.AddWithValue("@SecretaireId", rendezvous.secretaire.Id);
			return command;
		}

	}
}


