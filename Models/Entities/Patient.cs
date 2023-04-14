namespace radio1.Models.Entities
{
	public class Patient
	{
		public int? Id { get; set; }
		public string? Prenom { get; set; }
		public string? Nom { get; set; }
		public int? Telephone { get; set; }
		public string? Sexe { get; set; }
		public string? Adresse { get; set; }
		public DateTime? DateCreation { get; set; }

		public Patient() { }

	}
}
