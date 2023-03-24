namespace radio1.Models.DAL
{
	public class Message
	{
		public bool Verification { get; set; }
		public string Msg { get; set; }
		
		public Message() { }
		public Message(bool Verification,string msg) 
		{
			this.Verification = Verification;
			this.Msg = msg;
		}
        public static Message HandleException(Exception ex,string str)
        {
            if (ex.Message.Contains("unq_email") || ex.Message.Contains("unq_email1"))
            {
                return new Message(false, "cette adresse e-mail est déjà utilisée.");
            }
			else if (ex.Message.Contains("unq_UserName"))
			{
				return new Message(false, "Le nom d'utilisateur déjà utilisée.");
			}
			else if (ex.Message.Contains("unq_matricule"))
            {
                return new Message(false, "cette matricule est déjà utilisée.");

            }
            else if (ex.Message.Contains("unq_telephone"))
            {
                return new Message(false, "ce numero de telephone est déjà utilisée.");
            }
			else if (ex.Message.Contains("unq_Nom"))
			{
				return new Message(false, "ce Nom est déjà utilisée.");
			}
			else if (ex.Message.Contains("unq_Prenom"))
			{
				return new Message(false, "ce Prenom est déjà utilisée.");
			}
			else if (ex.Message.Contains("unq_TypeR"))
			{
				return new Message(false, "Ce type de Scanner est déjà exist.");
			}
			else if (ex.Message.Contains("unq_Name"))
			{
				return new Message(false, "Cette Salle est deja existe .");
			}
			else 
            {
                return new Message(false, "Une erreur est survenue lors de "+str+" : " + ex.Message);
            }
        }
    }
}
