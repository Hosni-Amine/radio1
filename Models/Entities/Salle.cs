namespace radio1.Models.Entities
{
	public class Salle
	{
		public int Id { get; set; }
		public string Nom { get; set; }
		public string Emplacement { get; set; }
		public int Responsable { get; set; }
		public int Operation { get; set; }
		
		public Salle() { }
	}
}
