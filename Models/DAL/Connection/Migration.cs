using radio1.Models.Entities;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;

namespace radio1.Models.DAL.Connection
{
    public class Migration
    {
        
        public static void CreateRendezVousTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
			string sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Doctor') BEGIN CREATE TABLE dbo.Doctor ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL, [Matricule] NVARCHAR(50) NOT NULL, [Telephone] NVARCHAR(50) NOT NULL, [Email] NVARCHAR(50) NOT NULL, [DateN] NVARCHAR(50) NOT NULL, [LieuN] NVARCHAR(50) NOT NULL, [SituationC] NVARCHAR(50) NOT NULL, [Sexe] NVARCHAR(50) NOT NULL, [Adresse] NVARCHAR(50) NOT NULL, [Ville] NVARCHAR(50) NOT NULL, [CodePostal] INT NOT NULL , [DateCreation] NVARCHAR(50) NOT NULL, [User_Id] INT , FOREIGN KEY (User_Id) REFERENCES Users(Id) , CONSTRAINT unq_email UNIQUE (Email), CONSTRAINT unq_matricule UNIQUE (Matricule), CONSTRAINT unq_telephone UNIQUE (Telephone) ) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Technicien') BEGIN CREATE TABLE dbo.Technicien ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL ,[Email] NVARCHAR(50) NOT NULL , [Sexe] NVARCHAR(50) NOT NULL ,[DateCreation] datetime NOT NULL , [User_Id] INT ,CONSTRAINT unq_email2 UNIQUE (Email), FOREIGN KEY (User_Id) REFERENCES Users(Id) ) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Secretaire') BEGIN CREATE TABLE dbo.Secretaire ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL , [Email] NVARCHAR(50) NOT NULL , [Sexe] NVARCHAR(50) NOT NULL ,[DateCreation] datetime NOT NULL , [User_Id] INT ,CONSTRAINT unq_email1 UNIQUE (Email), FOREIGN KEY (User_Id) REFERENCES Users(Id) ) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Salle') BEGIN CREATE TABLE dbo.Salle ( Id INT IDENTITY(1,1) PRIMARY KEY, [Nom] NVARCHAR(50) NOT NULL , [Responsable] INT  NULL [technicien_id] INT NULL,[Emplacement] NVARCHAR(100) NOT NULL,[DateCreation] datetime NOT NULL ,CONSTRAINT unq_Salle UNIQUE(Nom),CONSTRAINT fk_Responsable FOREIGN KEY (Responsable) REFERENCES dbo.Doctor(Id) ON DELETE SET NULL,CONSTRAINT fk_technicien_id FOREIGN KEY (technicien_id) REFERENCES dbo.Technicien(Id) ON DELETE SET NULL,CONSTRAINT unq_Emplacement UNIQUE(Emplacement)) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AppareilRadio') BEGIN CREATE TABLE dbo.AppareilRadio ( Id INT IDENTITY(1,1) PRIMARY KEY, [NumSerie] NVARCHAR(50) NOT NULL,[Maintenance] INT NOT NULL,[DateCreation] datetime NOT NULL , [SalleId] INT NOT NULL ,CONSTRAINT unq_NumSerie UNIQUE(NumSerie), CONSTRAINT FK_Salle FOREIGN KEY (SalleId) REFERENCES Salle (Id) ) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Patient') BEGIN CREATE TABLE dbo.Patient ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL , [Telephone] NVARCHAR(50) NOT NULL , [DateN] NVARCHAR(50) NOT NULL, [LieuN] NVARCHAR(50) NOT NULL, [SituationC] NVARCHAR(50) NOT NULL, [Sexe] NVARCHAR(50) NOT NULL, [Adresse] NVARCHAR(50) NOT NULL, [Ville] NVARCHAR(50) NOT NULL , [DateCreation] NVARCHAR(50) NOT NULL, [User_Id] INT , FOREIGN KEY (User_Id) REFERENCES Users(Id) , CONSTRAINT unq_telephone1 UNIQUE (Telephone) ) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TypeOperation') BEGIN CREATE TABLE dbo.TypeOperation ( Id INT IDENTITY(1,1) PRIMARY KEY, [Nom] NVARCHAR(50) NOT NULL,[SalleId] INT NOT NULL ,[AppareilRadioId] INT ,CONSTRAINT FK_Salle_two FOREIGN KEY (SalleId) REFERENCES Salle (Id),CONSTRAINT FK_AppareilRadio FOREIGN KEY (AppareilRadioId) REFERENCES AppareilRadio (Id)) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RendezVous') BEGIN CREATE TABLE RendezVous (Id INT IDENTITY(1,1) PRIMARY KEY, Date DATETIME, Status NVARCHAR(10) , Examen NVARCHAR(50), PatientId INT NULL , TypeOperationId INT NULL, DoctorId INT NULL, TechnicienId INT NULL, SecretaireId INT NULL, FOREIGN KEY(TypeOperationId) REFERENCES TypeOperation(Id) ON DELETE SET NULL, FOREIGN KEY(DoctorId) REFERENCES Doctor(Id) ON DELETE SET NULL, FOREIGN KEY(PatientId) REFERENCES Patient(Id) ON DELETE SET NULL , FOREIGN KEY(TechnicienId) REFERENCES Technicien(Id) ON DELETE SET NULL, FOREIGN KEY(SecretaireId) REFERENCES Secretaire(Id) ON DELETE SET NULL,CONSTRAINT uc_RendezVous UNIQUE(Date, PatientId )) END";
            DbConnection.NonQueryRequest(sqlstrTypeR, connection);
        }

        public static void CreateDoctorTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
            string sqlstr = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Doctor') BEGIN CREATE TABLE dbo.Doctor ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL, [Matricule] NVARCHAR(50) NOT NULL, [Telephone] NVARCHAR(50) NOT NULL, [Email] NVARCHAR(50) NOT NULL, [DateN] NVARCHAR(50) NOT NULL, [LieuN] NVARCHAR(50) NOT NULL, [SituationC] NVARCHAR(50) NOT NULL, [Sexe] NVARCHAR(50) NOT NULL, [Adresse] NVARCHAR(50) NOT NULL, [Ville] NVARCHAR(50) NOT NULL, [CodePostal] INT NOT NULL , [DateCreation] NVARCHAR(50) NOT NULL, [User_Id] INT , FOREIGN KEY (User_Id) REFERENCES Users(Id) , CONSTRAINT unq_email UNIQUE (Email), CONSTRAINT unq_matricule UNIQUE (Matricule), CONSTRAINT unq_telephone UNIQUE (Telephone) ) END";
            DbConnection.NonQueryRequest(sqlstr, connection);
        }
		public static void CreatePatientTableIfNotExists()
		{
			SqlConnection connection = DbConnection.GetConnection();
			string sqlstr = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Patient') BEGIN CREATE TABLE dbo.Patient ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL , [Telephone] NVARCHAR(50) NOT NULL , [DateN] NVARCHAR(50) NOT NULL, [LieuN] NVARCHAR(50) NOT NULL, [SituationC] NVARCHAR(50) NOT NULL, [Sexe] NVARCHAR(50) NOT NULL, [Adresse] NVARCHAR(50) NOT NULL, [Ville] NVARCHAR(50) NOT NULL , [DateCreation] NVARCHAR(50) NOT NULL, [User_Id] INT , FOREIGN KEY (User_Id) REFERENCES Users(Id) , CONSTRAINT unq_telephone1 UNIQUE (Telephone) ) END";
			DbConnection.NonQueryRequest(sqlstr, connection);
		}
		public static void CreateTechnicienTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
            string sqlstr = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Technicien') BEGIN CREATE TABLE dbo.Technicien ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL ,[Email] NVARCHAR(50) NOT NULL , [Sexe] NVARCHAR(50) NOT NULL ,[DateCreation] datetime NOT NULL , [User_Id] INT ,CONSTRAINT unq_email2 UNIQUE (Email), FOREIGN KEY (User_Id) REFERENCES Users(Id) ) END";
            DbConnection.NonQueryRequest(sqlstr, connection);
        }
        public static void CreateSecretaireTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
            string sqlstr = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Secretaire') BEGIN CREATE TABLE dbo.Secretaire ( Id INT IDENTITY(1,1) PRIMARY KEY, [Prenom] NVARCHAR(50) NOT NULL, [Nom] NVARCHAR(50) NOT NULL , [Email] NVARCHAR(50) NOT NULL , [Sexe] NVARCHAR(50) NOT NULL ,[DateCreation] datetime NOT NULL , [User_Id] INT ,CONSTRAINT unq_email1 UNIQUE (Email), FOREIGN KEY (User_Id) REFERENCES Users(Id) ) END";
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
            string sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Salle') BEGIN CREATE TABLE dbo.Salle ( Id INT IDENTITY(1,1) PRIMARY KEY, [Nom] NVARCHAR(50) NOT NULL , [Responsable] INT  NULL [technicien_id] INT NULL,[Emplacement] NVARCHAR(100) NOT NULL,[DateCreation] datetime NOT NULL ,CONSTRAINT unq_Salle UNIQUE(Nom),CONSTRAINT fk_Responsable FOREIGN KEY (Responsable) REFERENCES dbo.Doctor(Id) ON DELETE SET NULL,CONSTRAINT fk_technicien_id FOREIGN KEY (technicien_id) REFERENCES dbo.Technicien(Id) ON DELETE SET NULL,CONSTRAINT unq_Emplacement UNIQUE(Emplacement)) END";
            DbConnection.NonQueryRequest(sqlstrTypeR, connection);
        }
        public static void CreateTypeOperationTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
			string sqlstrTypeR = "IIF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TypeOperation') BEGIN CREATE TABLE dbo.TypeOperation ( Id INT IDENTITY(1,1) PRIMARY KEY, [Nom] NVARCHAR(50) NOT NULL,[SalleId] INT NOT NULL ,[AppareilRadioId] INT ,CONSTRAINT FK_Salle_two FOREIGN KEY (SalleId) REFERENCES Salle (Id),CONSTRAINT FK_AppareilRadio FOREIGN KEY (AppareilRadioId) REFERENCES AppareilRadio (Id)) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
        }
        public static void CreateAppareilRadioTableIfNotExists()
        {
            SqlConnection connection = DbConnection.GetConnection();
			string sqlstrTypeR = "IIF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TypeOperation') BEGIN CREATE TABLE dbo.TypeOperation ( Id INT IDENTITY(1,1) PRIMARY KEY, [Nom] NVARCHAR(50) NOT NULL,[SalleId] INT NOT NULL ,[AppareilRadioId] INT ,CONSTRAINT FK_Salle_two FOREIGN KEY (SalleId) REFERENCES Salle (Id),CONSTRAINT FK_AppareilRadio FOREIGN KEY (AppareilRadioId) REFERENCES AppareilRadio (Id)) END";
			DbConnection.NonQueryRequest(sqlstrTypeR, connection);
			sqlstrTypeR = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AppareilRadio') BEGIN CREATE TABLE dbo.AppareilRadio ( Id INT IDENTITY(1,1) PRIMARY KEY, [NumSerie] NVARCHAR(50) NOT NULL,[Maintenance] INT NOT NULL,[DateCreation] datetime NOT NULL , [SalleId] INT NOT NULL ,CONSTRAINT unq_NumSerie UNIQUE(NumSerie), CONSTRAINT FK_Salle FOREIGN KEY (SalleId) REFERENCES Salle (Id) ) END";
            DbConnection.NonQueryRequest(sqlstrTypeR, connection);
        }

	}
}
