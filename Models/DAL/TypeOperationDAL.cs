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
					string sqlstr = "INSERT INTO dbo.TypeOperation (Nom , SalleId , AppareilRadioId) VALUES (@Nom ,@SalleId,@AppareilRadioId)";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					if (operation.AppareilRadioId == null)
					{
						sqlstr = "INSERT INTO dbo.TypeOperation (Nom , SalleId , AppareilRadioId) VALUES (@Nom ,@SalleId,@AppareilRadioId)";
						command = new SqlCommand(sqlstr, connection);
						command.Parameters.AddWithValue("@AppareilRadioId", DBNull.Value);
					}
					else 
					{
						command.Parameters.AddWithValue("@AppareilRadioId", operation.AppareilRadioId);
					}
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
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM TypeOperation WHERE id = @id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@id", id);
					DbConnection.NonQueryRequest(command);
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
		public static Message EditTypeOperations(List<TypeOperation> operations)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					connection.Open();
					string sqlstr = "DELETE FROM dbo.TypeOperation WHERE AppareilRadioId = @AppareilRadioId;";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@AppareilRadioId", operations[0].AppareilRadioId);
					command.ExecuteNonQuery();
					connection.Close();
					foreach (var operation in operations) 
					{
						AddTypeOperation(operation);
					}
				}
				return new Message(true, "Types d'operation modifier avec succés");
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
		/// Permet de supprimer les types d'operation associée a une salle
		/// </summary>
		/// <param name="Salle_Id"></param>
		/// <returns></returns>
		public static Message DeleteSalleTypeOperations(int Salle_Id)
		{
			try
			{
				using (SqlConnection connection = Connection.DbConnection.GetConnection())
				{
					string sqlstr = "DELETE FROM TypeOperation WHERE Salleid = @Salle_Id";
					SqlCommand command = new SqlCommand(sqlstr, connection);
					command.Parameters.AddWithValue("@Salle_Id", Salle_Id);
					Connection.DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Groupe de type supprimer avec succés");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la suppression");
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
		public static List<TypeOperation> GetAll(bool? ForApp,bool? ForSalle ,int? SalleId)
		{
            Migration.CreateTypeOperationTableIfNotExists();
            SqlConnection connection = DbConnection.GetConnection();
			string sqlstr = "SELECT DISTINCT Nom FROM dbo.TypeOperation";
			SqlCommand command = new SqlCommand();
			if (ForSalle == false && ForApp == true && SalleId != null)
			{
				sqlstr = "SELECT * FROM dbo.TypeOperation WHERE AppareilRadioId = @SalleId AND SalleId IS NOT NULL";
				command.Parameters.AddWithValue("@SalleId", SalleId);
			}
			else if (ForSalle == true && ForApp == false && SalleId != null)
			{
				sqlstr = "SELECT * FROM dbo.TypeOperation WHERE AppareilRadioId IS NULL AND SalleId = @SalleId";
				command.Parameters.AddWithValue("@SalleId", SalleId);
			}
			else if (ForApp == true) 
			{
				sqlstr = "SELECT * FROM dbo.TypeOperation WHERE AppareilRadioId IS NOT NULL";
			}
			else if (ForSalle == true)
			{
				sqlstr = "SELECT * FROM dbo.TypeOperation WHERE AppareilRadioId IS NULL";
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
				if (raw.Table.Columns.Contains("Id") && raw.Table.Columns.Contains("SalleId"))
				{
					app.Id = Int32.Parse(raw["Id"].ToString());
					app.SalleId = Int32.Parse(raw["SalleId"].ToString());
				}
				if (raw.Table.Columns.Contains("AppareilRadioId") && (raw["AppareilRadioId"] != DBNull.Value))
				{
					app.AppareilRadioId = Int32.Parse(raw["AppareilRadioId"].ToString());
				}
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
