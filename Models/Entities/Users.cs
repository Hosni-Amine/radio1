namespace radio1.Models.Entities
{
	public class Users
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
		public DateTime DateCreation { get; set; }
		public Users() { }
	}
}
