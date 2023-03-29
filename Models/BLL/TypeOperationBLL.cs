using radio1.Models.DAL;
using radio1.Models.Entities;
using Message = radio1.Models.DAL.Message;


namespace radio1.Models.BLL
{
	public class TypeOperationBLL
	{



		public static Message AddTypeOperation(string nom)
		{
			return TypeOperationDAL.AddTypeOperation(nom);
		}
		public static Message DeleteTypeOperation(int id)
		{
			return TypeOperationDAL.DeleteTypeOperation(id);
		}
		public static List<TypeOperation> GetAll()
		{
			return TypeOperationDAL.GetAll();
		}
		public static TypeOperation GetById(int Id)
		{
			return TypeOperationDAL.GetById(Id);
		}
	}
}
