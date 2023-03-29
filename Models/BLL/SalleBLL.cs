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
        public static Salle GetById(int Id)
        {
            return SalleDAL.GetById(Id);
        }
    }
}
