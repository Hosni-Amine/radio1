using radio1.Models.DAL;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
	public class SalleBLL
	{
        public static Message AddSalle(Salle salle)
        {
            return SalleDAL.AddSalle(salle);
        }
        public static Message DeleteSalle(int id)
        {
            return SalleDAL.DeleteSalle(id);
        }
        public static List<Salle> GetAll()
        {
            return SalleDAL.GetAll();
        }
        public static Message appendTypes(int? app_id , int? salle_id , List<TypeOperation> operation)
        {
            try
            {
                foreach (var op in operation)
                {
                    op.SalleId = salle_id;
                    TypeOperationBLL.AddTypeOperation(op);
                }
                return new Message (true , "tous les type ajouter avec success !");
            }
            catch(Exception ex) 
            {
				return new Message(false, ex.Message);
			}
		}
        
        public static Salle GetById(int? Id)
        {
            return SalleDAL.GetById(Id);
        }
		public static Message SalleAffectation(int salle_Id, int Id)
        {
            return SalleDAL.SalleAffectation(salle_Id,Id);
		}
		public static Message EditSalle(Salle salle)
        {
            return SalleDAL.EditSalle(salle);
		}

	}
}
