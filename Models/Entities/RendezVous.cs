namespace radio1.Models.Entities
{
    public enum RendezVousStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }
    public class RendezVous
	{
		public int? Id { get; set; }
		public DateTime Date { get; set; }
		public string? Status { get; set; }
		public TypeOperation? TypeOperation { get; set; }
		public string? Examen { get; set; }
		public Patient? patient { get; set; }
		public Doctor? doctor { get; set; }
		public Technicien? technicien { get; set; }
		public Secretaire? secretaire { get; set; }
		public string? Appareil_NumSerie { get; set; }
		public string? Image_Name { get; set; }
        public string? Interpretation { get; set; }
		public string? Inter_PDF { get; set; }
		public string? Inter_Vocal { get; set; }



	}
}
