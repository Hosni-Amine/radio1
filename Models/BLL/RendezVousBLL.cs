using radio1.Models.DAL.RV_Planification;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
    public class RendezVousBLL
    {
        public static RendezVous GetById(int Id)
        {
            return RendezVousDAL.GetById(Id);
        }

		public static List<RendezVous> GetAll(Users user)
        {
            return RendezVousDAL.GetAll(user);
        }
		public static DAL.Message AddRendezVous(RendezVous rendezvous)
		{
			return RendezVousDAL.AddRendezVous(rendezvous);
		}

        public static List<Disponibilite> GetDisponibilite(string typeoperation)
        {
            return RendezVousDAL.GetDisponibilite(typeoperation);
        }
        public static DAL.Message DeleteRendezVous(int id)
        {
            return RendezVousDAL.DeleteRendezVous(id);
        }
		public static DAL.Message EditRendezVous(RendezVous rendezVous)
		{
			return RendezVousDAL.EditRendezVous(rendezVous);
		}
	}
}
