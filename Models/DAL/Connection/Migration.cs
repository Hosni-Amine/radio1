using System.Data.SqlClient;

namespace radio1.Models.DAL.Connection
{
    public class Migration
    {
        public static void CreateDoctorTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
            string sqlstr = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Doctor') BEGIN CREATE TABLE dbo.Doctor ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL, [Matricule] NVARCHAR(50) NOT NULL, [Telephone] NVARCHAR(50) NOT NULL, [Email] NVARCHAR(50) NOT NULL, [DateN] NVARCHAR(50) NOT NULL, [LieuN] NVARCHAR(50) NOT NULL, [SituationC] NVARCHAR(50) NOT NULL, [Sexe] NVARCHAR(50) NOT NULL, [Adresse] NVARCHAR(50) NOT NULL, [Ville] NVARCHAR(50) NOT NULL, [CodePostal] INT NOT NULL , [DateCreation] NVARCHAR(50) NOT NULL, [User_Id] INT , FOREIGN KEY (User_Id) REFERENCES Users(Id) , CONSTRAINT unq_email UNIQUE (Email), CONSTRAINT unq_matricule UNIQUE (Matricule), CONSTRAINT unq_telephone UNIQUE (Telephone) ) END";
            DbConnection.NonQueryRequest(sqlstr, connection);
        }

        public static void CreateTechnicienTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
            string sqlstr = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Technicien') BEGIN CREATE TABLE dbo.Technicien ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL , [Sexe] NVARCHAR(50) NOT NULL ,[DateCreation] datetime NOT NULL , [User_Id] INT , FOREIGN KEY (User_Id) REFERENCES Users(Id) ) END";
            DbConnection.NonQueryRequest(sqlstr, connection);
        }

		public static void CreateUsersTableIfNotExists()
		{
			SqlConnection connection = DbConnection.GetConnection();
            string sqlstr = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users') BEGIN CREATE TABLE dbo.Users ( Id INT IDENTITY(1,1) PRIMARY KEY , [UserName] NVARCHAR(50) NOT NULL,[Password] NVARCHAR(50) NOT NULL, [Role] NVARCHAR(15) NOT NULL, [DateCreation] datetime NOT NULL ,CONSTRAINT unq_UserName UNIQUE(UserName)) END";
          	DbConnection.NonQueryRequest(sqlstr, connection);
		}


        public static void CreateSalleIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
            string sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Salle') BEGIN CREATE TABLE dbo.Salle ( Id INT IDENTITY(1,1) PRIMARY KEY, [Nom] NVARCHAR(50) NOT NULL , [Responsable] INT  NULL ,[Emplacement] NVARCHAR(100) NOT NULL,[DateCreation] datetime NOT NULL ,CONSTRAINT unq_Salle UNIQUE(Nom),CONSTRAINT unq_Responsable UNIQUE(Responsable),CONSTRAINT fk_Responsable FOREIGN KEY (Responsable) REFERENCES dbo.Doctor(Id) ON DELETE SET NULL,CONSTRAINT unq_Emplacement UNIQUE(Emplacement)) END";
            DbConnection.NonQueryRequest(sqlstrTypeR, connection);
        }




        public static void CreateTypeOperationTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
            string sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TypeOperation') BEGIN CREATE TABLE dbo.TypeOperation ( Id INT IDENTITY(1,1) PRIMARY KEY, [Nom] NVARCHAR(50) NOT NULL,[SalleId] INT NOT NULL ,CONSTRAINT FK_Salle FOREIGN KEY (SalleId) REFERENCES Salle (Id)) END";
            DbConnection.NonQueryRequest(sqlstrTypeR, connection);
        }

    }
}
