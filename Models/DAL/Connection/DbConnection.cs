using radio1.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace radio1.Models.DAL.Connection
{
    public class DbConnection
    {
        static string CS = "workstation id=radiologie.mssql.somee.com;packet size=4096;user id=hosniamine_SQLLogin_1;pwd=kkke54dsdo;data source=radiologie.mssql.somee.com;persist security info=False;initial catalog=radiologie";
        //static string CS = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=radio;Integrated Security=True";
        public static SqlConnection GetConnection()
        {
            SqlConnection connect = null;
            try
            {
                connect = new SqlConnection(CS);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return connect;
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
        /// Methode pour créer une command apartir d'une connection et la requete sql et affecter les parametre de command 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sqlstr"></param>
        /// <param name="technicien"></param>
        /// <returns>command a executer pour un technicien</returns>
        public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Technicien technicien)
        {
            SqlCommand command = new SqlCommand(sqlstr, connection);
            command.Parameters.AddWithValue("@Prenom", technicien.Prenom);
            command.Parameters.AddWithValue("@Nom", technicien.Nom);
            command.Parameters.AddWithValue("@Sexe", technicien.Sexe);
            return command;
        }




		public static SqlCommand CommandCreate(SqlConnection connection, string sqlstr, Users user)
		{
			SqlCommand command = new SqlCommand(sqlstr, connection);
            command.Parameters.AddWithValue("@UserName", user.UserName);
			command.Parameters.AddWithValue("@Password", user.Password);
			command.Parameters.AddWithValue("@Role", user.Role);
			return command;
		}





		/// <summary>
		/// Methode pour créer une command apartir d'une connection et la requete sql et affecter les parametre de command 
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
    }
}


