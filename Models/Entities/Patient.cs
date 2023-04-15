namespace radio1.Models.Entities
{
	public class Patient
	{
		public int? Id { get; set; }
		public string? Prenom { get; set; }
		public string? Nom { get; set; }
		public string? Telephone { get; set; }
		public string? DateN { get; set; }
		public string? LieuN { get; set; }
		public string? SituationC { get; set; }
		public string? Sexe { get; set; }
		public string? Adresse { get; set; }
		public string? Ville { get; set; }
		public DateTime? DateCreation { get; set; }

		public Patient() { }

	}
}
