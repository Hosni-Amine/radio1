using NuGet.Protocol.Plugins;
using radio1.Models.DAL;
using radio1.Models.DAL.RV_Planification;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
    public class RendezVousBLL
    {
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
	}
}
