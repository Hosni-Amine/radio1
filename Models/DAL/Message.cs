namespace radio1.Models.DAL
{
	public class Message
	{
		public bool Verification { get; set; }
		public string Msg { get; set; }
		public int? MsgId { get; set; }

		public Message() { }
		public Message(bool Verification,string msg) 
		{
			this.Verification = Verification;
			this.Msg = msg;
		}
		public Message(bool Verification, string msg,int MsgId)
		{
			this.Verification = Verification;
			this.Msg = msg;
			this.MsgId = MsgId;
		}
		public static Message HandleException(Exception ex,string str)
        {
            if (ex.Message.Contains("unq_email") || ex.Message.Contains("unq_email1") || ex.Message.Contains("unq_email2"))
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
			else if (ex.Message.Contains("unq_Emplacement"))
			{
				return new Message(false, "Ce Nom de fichier est deja utilisé merci de le renomer! ");
			}
            else if (ex.Message.Contains("unq_Responsable"))
            {
                return new Message(false, "Ce responsable est déja affecter a une autre salle ! ");
            }
            else if (ex.Message.Contains("unq_TypeOperation"))
            {
                return new Message(false, "Cette operation est déja existe ! ");
            }
            else if (ex.Message.Contains("unq_Salle"))
            {
                return new Message(false, "Cette Salle est déja existe ! ");
            }
            else if (ex.Message.Contains("fk_Responsable"))
            {
                return new Message(false, "Ce responsable n'existe pas ! ");
            }
            else if (ex.Message.Contains("fk_Operation"))
            {
                return new Message(false, "Cette operation n'existe pas ! ");
            }
            else if (ex.Message.Contains("unq_NumSerie"))
			{
				return new Message(false, "Ce Numero de Serie deja exist ! ");
			}
			else 
            { 
                return new Message(false, "Une erreur est survenue lors de "+str+" : " + ex.Message);
            }
        }
    }
}
