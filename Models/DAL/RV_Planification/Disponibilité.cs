namespace radio1.Models.DAL.RV_Planification
{
	public class Disponibilite
	{
        public  List<DateTime>? Dates { get; set; }
		public string? Nom_Appareil { get; set; }
		public string? Nom_Salle { get; set; }
		public string? Nom_Doctor { get; set; }

		public Disponibilite() { }
	}



}
