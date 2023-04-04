namespace radio1.Models.Entities
{
	public class Secretaire
	{
		public int? Id { get; set; }
		public string? Prenom { get; set; }
		public string? Nom { get; set; }
		public string? Email { get; set; }
		public string? Sexe { get; set; }
		public DateTime? DateCreation { get; set; }

		public Secretaire() { }
	}
}
