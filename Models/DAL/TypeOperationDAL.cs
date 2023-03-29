using radio1.Models.DAL.Connection;
using radio1.Models.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace radio1.Models.DAL
{
    public class TypeOperationDAL
	{
		public static Message AddTypeOperation(string Nom)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
                    Migration.CreateTypeOperationTableIfNotExists();
                    string sqlstr = "INSERT INTO dbo.TypeOperation (Nom) VALUES (@Nom)";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Nom", Nom);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Type d'operation ajouter avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "l'ajout");
			}
		}
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
		public static List<TypeOperation> GetAll()
		{
            Migration.CreateTypeOperationTableIfNotExists();
            SqlConnection connection = Connection.DbConnection.GetConnection();
			string sqlstr = "SELECT * FROM dbo.TypeOperation";
			connection.Open();
			SqlCommand command = new SqlCommand(sqlstr, connection);
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
				
				return app;
			}
			catch
			{
				return null;
			}
		}
	}
}
