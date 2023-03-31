using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using System.Data;
using System.Data.SqlClient;

namespace radio1.Models.DAL
{
    public class TypeOperationDAL
	{
		/// <summary>
		/// Permet d'ajouter un type d'operation 
		/// </summary>
		/// <param name="operation"></param>
		/// <returns></returns>
		public static Message AddTypeOperation(TypeOperation operation)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
                    Migration.CreateTypeOperationTableIfNotExists();
                    string sqlstr = "INSERT INTO dbo.TypeOperation (Nom , SalleId) VALUES (@Nom ,@SalleId)";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Nom", operation.Nom);
					command.Parameters.AddWithValue("@SalleId", operation.SalleId);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Type d'operation ajouter avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}
		/// <summary>
		/// Permet de supprimer un type d'operation 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Message DeleteTypeOperation(int id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM TypeOperation WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Type d'operation supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
			}
		}
		/// <summary>
		/// Permet de modifier un type d'operation 
		/// </summary>
		/// <param name="operation"></param>
		/// <returns></returns>
		public static Message EditTypeOperation(TypeOperation operation)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					Migration.CreateTypeOperationTableIfNotExists();
					string sqlstr = "UPDATE TypeOperation SET Nom = @Nom WHERE Id = @Id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Nom", operation.Nom);
					command.Parameters.AddWithValue("@Id", operation.Id);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Type d'operation modifier avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la modification");
			}
		}
		/// <summary>
		/// Permet de retirer un type d'operation de la base de données
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public static TypeOperation GetById(int Id)
        {
            using (SqlConnection connection = Connection.DbConnection.GetConnection())
            {
                string sqlstr = "SELECT * FROM dbo.TypeOperation WHERE Id = @Id";
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
		/// 3 methode permet de retirer tous les elements de la base de données specifiquement pour GetAll qui peut retourner les types associé a une salle
		/// </summary>
		/// <param name="SalleId"></param>
		/// <returns></returns>
		public static List<TypeOperation> GetAll(DataTable table)
		{
			try
			{
				List<TypeOperation> apps = new List<TypeOperation>();
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
		public static List<TypeOperation> GetAll(int? SalleId)
		{
            Migration.CreateTypeOperationTableIfNotExists();
            SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.TypeOperation";
			SqlCommand command = new SqlCommand();
			if (SalleId != null) 
			{
				sqlstr = "SELECT * FROM dbo.TypeOperation WHERE SalleId = @SalleId";
				command.Parameters.AddWithValue("@SalleId", SalleId);
			}
			command.CommandText = sqlstr;
			command.Connection = connection;
			connection.Open();
			DataTable table = new DataTable();
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			connection.Close();
			return GetAll(table);
		}
		public static TypeOperation Get(DataRow raw)
		{
			try
			{
                TypeOperation app = new TypeOperation();
				app.Id = Int32.Parse(raw["Id"].ToString());
				app.Nom = raw["Nom"].ToString();
				app.SalleId = Int32.Parse(raw["SalleId"].ToString());
				return app;
			}
			catch
			{
				return null;
			}
		}
	}
}
