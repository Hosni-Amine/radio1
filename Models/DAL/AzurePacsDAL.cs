using FellowOakDicom;
using Microsoft.Health.Dicom.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Azure.Storage.Blobs;
using radio1.Models.DAL.Connection;
using System.Data.SqlClient;
	
namespace radio1.Models.DAL
{
	
	public class AzurePacsDAL
	{
		/// <summary>
		/// Fonction pour etablir la connection avec Microsoft Azure
		/// </summary>
		/// <param name="TokenValue"></param>
		/// <returns></returns>
		public static Message TestConnection(string TokenValue)
		{
			try
			{
				string webServerUrl = "https://pfe-preclinic.dicom.azurehealthcareapis.com";
				var httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(webServerUrl);
				IDicomWebClient client = new DicomWebClient(httpClient);
				client.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenValue);
			}
			catch (Exception ex)
			{
				return new Message(false, "Error connection to Azur : " + ex.Message);
			}
				return new Message(true, "Connection successfully established !");
		}
		public async Task<Message> GetAzurToken()
		{
			try
			{
				string clientId = "e4312849-54b8-4995-9992-f4d53c6d8039";
				string clientSecret = "dmn8Q~jH.oWAtszifmUot6ayjSDCxl_vZ203jaNI";
				string resource = "https://login.microsoftonline.com/";
				var clientCredential = new ClientCredential(clientId, clientSecret);
				var authority = "https://login.microsoftonline.com/5121a861-51c1-4d61-a4ca-ed11c8097a22/";
				var authContext = new AuthenticationContext(authority);
				var result = await authContext.AcquireTokenAsync(resource, clientCredential);
				string accessToken = result.AccessToken;
				if (result == null)
				{
					throw new InvalidOperationException("Failed to get the access token !");
				}
				return new Message(true,accessToken);
			}
			catch ( Exception ex)
			{
				Console.WriteLine(ex.Message);
				return new Message(false, "Erreur de connection au Microsoft Azure");
			}
		}
		public async Task<IDicomWebClient> CreateDICOMWebClient()
		{
			try
			{
				var msg = await GetAzurToken();
				string webServerUrl = "https://pfe-pre-clinqiue.dicom.azurehealthcareapis.com";
				var httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(webServerUrl);
				IDicomWebClient client = new DicomWebClient(httpClient);
				client.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", msg.Msg);

				return client;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
		
		/// <summary>
		/// Fonction pour mettre une image médicale d'un patient
		/// </summary>
		/// <param name="path"></param>
		/// <param name="Image_Name"></param>
		/// <returns></returns>
		public async Task<Message> UploadStudy(IFormFile dicom, int RendezVous_Id)
		{
			try
			{
				var Msg = CheckImage(RendezVous_Id);
				if(Msg.Verification == true && Msg.Msg==null)
				{
					// Create a BlobServiceClient object using the connection string
					BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=dicoms;AccountKey=PVh3WY2Dybc4TZAy+qMenUl/vS1JVa8hbdUYrljka8UcLm6J9/SbPTyVnv05hvi/TqF1iNK7CRUn+AStLL0E2w==;EndpointSuffix=core.windows.net");
					// Get a reference to the container
					BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("dicomcontainer");
					// Get a reference to the blob
					string Reference = dicom.FileName.Substring(0, dicom.FileName.Length - 3)+RendezVous_Id;
					BlobClient blobClient = containerClient.GetBlobClient(Reference);
					// Upload the file to the blob
					using Stream stream = dicom.OpenReadStream();
					await blobClient.UploadAsync(stream, true);
					// Ajout de la reffrence a la base de données
					var Msg2 = AddImageReff(Reference, RendezVous_Id);
					if(Msg2.Verification)
					{
						return new Message(true, "Image bien sauvgarder !");
					}
					else
					{
						return new Message(true, "Image sauvgarder dans le serveur mais sans reference ! ");
					}
				}
				else
				{
					return new Message(false, "Le nom de l'image deja exist pour ce patient !");
				}
			}
			catch (Exception ex)	
			{
				return new Message(false,"Erreur de connection au PACS !");
			}
		}
		public static Message CheckImage(int RendezVous_Id)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					connection.Open();
					string sqlQuery = "SELECT RendezVous.Image_Name FROM RendezVous WHERE Id = @Id";	
					SqlCommand command = new SqlCommand(sqlQuery, connection);
					command.Parameters.AddWithValue("@Id", RendezVous_Id);
					string imageName = null;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						reader.Read();
						if (!reader.IsDBNull(0))
						{
							imageName = reader.GetString(0);
						}
					}
					connection.Close();
					return new Message(true, imageName);
				}
			}
			catch (Exception ex)
			{
				return new Message (false,ex.Message+"erreur de check !");
			}
		}
		public static Message AddImageReff(string Image_Name , int RendezVous_Id)
		{
			try
			{
				using (SqlConnection connection = DbConnection.GetConnection())
				{
					string sqlQuery = "UPDATE RendezVous SET Image_Name = @Image_Name WHERE Id = @Id ";
					SqlCommand command = new SqlCommand(sqlQuery, connection);
					command.Parameters.AddWithValue("@Id", RendezVous_Id);
					command.Parameters.AddWithValue("@Image_Name", Image_Name);
					DbConnection.NonQueryRequest(command);
					return new Message(true,"Refference ajouter avec succes ");
				}
			}
			catch (Exception ex)
			{
				return new Message(false, ex.Message + "L'ajout");
			}
		}

		/// <summary>
		/// Fonction pour télécharger une image médicale d'un patient
		/// </summary>
		/// <param name="Image_Name"></param>
		/// <returns></returns>
		public async Task<Message> GetStudy(int RendezVous_Id , string Path)
		{
			var Msg = CheckImage(RendezVous_Id);
            var directory = new DirectoryInfo(Path);
            var file = directory.GetFiles();
            var filename = file[0].Name;
            if ((Msg.Verification) && !(filename.Contains(Msg.Msg)))
			{
				file[0].Delete();
                try
				{
					// Create a BlobServiceClient object using the connection string
					BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=dicoms;AccountKey=PVh3WY2Dybc4TZAy+qMenUl/vS1JVa8hbdUYrljka8UcLm6J9/SbPTyVnv05hvi/TqF1iNK7CRUn+AStLL0E2w==;EndpointSuffix=core.windows.net");
					// Get a reference to the container
					BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("dicomcontainer");
					// Get a reference to the blob
					BlobClient blobClient = containerClient.GetBlobClient(Msg.Msg);
					Path += "\\" + Msg.Msg + ".dcm";
					using FileStream downloadFileStream = File.OpenWrite(Path);
					await blobClient.DownloadToAsync(downloadFileStream);
					downloadFileStream.Close();
					return new Message(true , Path );
				} 
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return new Message(false, "Erreur de telechargement !");

				}
			}
			else if (filename.Contains(Msg.Msg))
			{
                return new Message(true, Path);
            }
			else
			{
				return null;
			}

		}
	}
}
