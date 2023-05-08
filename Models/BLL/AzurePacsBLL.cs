using radio1.Models.DAL;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
	public class AzurePacsBLL
	{
		public async Task<Message> UploadStudy(IFormFile dicom, int RendezVous_Id)
		{
			AzurePacsDAL azurePacsDAL = new AzurePacsDAL();
			return await azurePacsDAL.UploadStudy( dicom, RendezVous_Id);
		}
		public async Task<Message> GetStudy(DicomReff dicomReff)
		{
			AzurePacsDAL azurePacsDAL = new AzurePacsDAL();
			return await azurePacsDAL.GetStudy(dicomReff);
		}

	}
}
