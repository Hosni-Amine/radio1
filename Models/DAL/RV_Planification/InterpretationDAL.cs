using radio1.Models.Entities;
using System.Data.SqlClient;
using radio1.Models.DAL.Connection;

namespace radio1.Models.DAL.RV_Planification
{
    public class InterpretationDAL
    {

        public static Message Add_Inter_PDF(RendezVous RV)
        {
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sql = "UPDATE [dbo].[RendezVous] SET Inter_PDF=@Inter_PDF WHERE (RendezVous.Id = @id) ";
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue("@id", RV.Id);
					command.Parameters.AddWithValue("@Inter_PDF", RV.Inter_PDF);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Interpretation ajoutée avec succes !");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la modification !");
			}
		}
		public static Message Add_Inter_Vocal(RendezVous RV)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sql = "UPDATE [dbo].[RendezVous] SET Inter_Vocal=@Inter_Vocal WHERE (RendezVous.Id = @id) ";
					SqlCommand command = new SqlCommand(sql, connection);
					command.Parameters.AddWithValue("@id", RV.Id);
					command.Parameters.AddWithValue("@Inter_Vocal", RV.Inter_Vocal);
					DbConnection.NonQueryRequest(command);
				}
				return new Message(true, "Interpretation Vocale ajoutée avec succes !");
			}
			catch (Exception ex)
			{
				return Message.HandleException(ex, "la modification !");
			}
		}
	}
}
