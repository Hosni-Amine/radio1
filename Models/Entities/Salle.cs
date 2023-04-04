namespace radio1.Models.Entities
{
	public class Salle
	{
		public int Id { get; set; }
		public string? Nom { get; set; }
		public string? Emplacement { get; set; }
		public Doctor? Responsable { get; set; }
		public List<TypeOperation>? Operations { get; set; }
		public DateTime? DateCreation { get; set; }


		public Salle() { }
	}
}
