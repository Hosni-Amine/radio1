using radio1.Models.DAL;
using radio1.Models.Entities;
using Message = radio1.Models.DAL.Message;


namespace radio1.Models.BLL
{
	public class TypeOperationBLL
	{

		public static Message DeleteSalleTypeOperations(int Salle_Id)
		{
			return TypeOperationDAL.DeleteSalleTypeOperations(Salle_Id);
		}
		public static Message AddTypeOperation(TypeOperation operation)
		{
			return TypeOperationDAL.AddTypeOperation(operation);
		}
		public static Message DeleteTypeOperation(int id)
		{
			return TypeOperationDAL.DeleteTypeOperation(id);
		}
		public static List<TypeOperation> GetAll(bool? ForApp, bool? ForSalle, int? SalleId)
		{
			return TypeOperationDAL.GetAll(ForApp, ForSalle, SalleId);
		}
		public static TypeOperation GetById(int Id)
		{
			return TypeOperationDAL.GetById(Id);
		}
	}
}
