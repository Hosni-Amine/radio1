using NuGet.Versioning;
using radio1.Models.DAL;
using radio1.Models.Entities;

namespace radio1.Models.BLL
{
	public class AppareilRadioBLL
	{
		public static Message DeleteSalleAppareilRadios(int Salle_Id)
		{
			return AppareilRadioDAL.DeleteSalleAppareilRadios(Salle_Id);
		}
		public static Message AddAppareilRadio(AppareilRadio appareil)
		{
			return AppareilRadioDAL.AddAppareilRadio(appareil);
		}
		public static Message EditAppareilRadio(AppareilRadio appareil)
		{
			var ops = appareil.Operations;
			for ( var i=0;i<ops.Count;i++)
			{
				ops[i].SalleId = appareil.SalleId;
			}
			var msg = TypeOperationDAL.EditTypeOperations(ops);
			return AppareilRadioDAL.EditAppareilRadio(appareil);
		}
		public static List<Salle> GetAllwithappareils() 
		{
			return SalleDAL.GetAllwithappareils();
		}
		public static Message DeleteAppareilRadio(int id)
		{
			return AppareilRadioDAL.DeleteAppareilRadio(id);
		}
		public static List<AppareilRadio> GetAll(int? SalleId)
		{
			return AppareilRadioDAL.GetAll(SalleId);
		}
		public static AppareilRadio GetById(int Id)
		{
			return AppareilRadioDAL.GetById(Id);
		}
		public static AppareilRadio GetByName(string str)
		{
			return AppareilRadioDAL.GetByName(str);
		}

		public static Message AppendTypes(int? app_id, int? salle_id, List<TypeOperation> operation)
		{
			try
			{
				foreach (var op in operation)
				{
					op.SalleId = salle_id;
					if(app_id != null)
					{
						op.AppareilRadioId = app_id;
					}
					TypeOperationBLL.AddTypeOperation(op);
				}
				return new Message(true, "tous les type ajouter avec success !");
			}
			catch (Exception ex)
			{
				return new Message(false, ex.Message);
			}
		}
	}
}
