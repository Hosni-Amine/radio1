using radio1.Models.DAL;
using radio1.Models.Entities;
using Message = radio1.Models.DAL.Message;


namespace radio1.Models.BLL
{
	public class TypeOperationBLL
	{



		public static Message AddTypeOperation(TypeOperation operation)
		{
			return TypeOperationDAL.AddTypeOperation(operation);
		}
		public static Message EditTypeOperation(TypeOperation operation)
		{
			return TypeOperationDAL.EditTypeOperation(operation);
		}
		public static Message DeleteTypeOperation(int id)
		{
			return TypeOperationDAL.DeleteTypeOperation(id);
		}
		public static List<TypeOperation> GetAll(int? SalleId)
		{
			return TypeOperationDAL.GetAll(SalleId);
		}
		public static TypeOperation GetById(int Id)
		{
			return TypeOperationDAL.GetById(Id);
		}
	}
}
