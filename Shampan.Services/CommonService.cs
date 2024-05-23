using Newtonsoft.Json;
using Shampan.Core.Interfaces.Services;
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services
{
	public class CommonService : ICommonService
	{

		private IUnitOfWork _unitOfWork;

		public CommonService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		//public IList<UserBranch> GetBranch()
		//{
		//    List<UserBranch> records = new List<UserBranch>();
		//    using (var context = _unitOfWork.Create())
		//    {

		//        try
		//        {
		//            records = context.Repositories.CommonRepository.GetBranch().Select(c => new UserBranch
		//            {
		//                UserName = c.Name,
		//                BranchName = c.Value
		//            }).ToList();
		//            context.SaveChanges();

		//            return records;
		//        }
		//        catch (Exception e)
		//        {
		//            context.RollBack();


		//        }
		//    }
		//    return records;

		//}

		public List<CommonDropDown> GetAllProductType()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{

				try
				{
					records = context.Repositories.CommonRepository.GetAllProductType().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();


				}
			}
			return records;


		}


		public List<CommonDropDown>? GetIssues(string auditId)
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetIssues(auditId).Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();


			}

			return records;

		}


		public List<CommonDropDown>? GetBranchFeedbackIssues(string auditId, string userName,bool team)
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetBranchFeedbackIssues(auditId, userName,team).Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();


			}

			return records;

		}


		public List<CommonDropDown> GetAllStore()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{

				try
				{
					records = context.Repositories.CommonRepository.GetAllStore().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();


				}
			}
			return records;


		}
		public List<CommonDropDown> GetAllUom()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{

				try
				{
					records = context.Repositories.CommonRepository.GetAllUom().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();


				}
			}
			return records;


		}
		public List<CommonDropDown> GetAllColor()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{

				try
				{
					records = context.Repositories.CommonRepository.GetAllColor().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();


				}
			}
			return records;


		}
		public List<CommonDropDown> GetAllCurrencys()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllCurrencys().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllDepartment()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllDepartment().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> GetAllUserName()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllUserName().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllSize()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllSize().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllVendor()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllVendor().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> GetAllCustomers()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllCustomers().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}


		public List<CommonDropDown>? GetAuditType(bool isPlanned = true)
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetAuditType(isPlanned).Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;
		}

		public List<CommonDropDown>? GetTeams()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetTeams().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;
		}

		public List<CommonDropDown>? GetReportStatus()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetReportStatus().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;
		}

		public List<CommonDropDown>? GetCircularType()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetCircularType().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;
		}
		public List<CommonDropDown>? GetAuditStatus()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetAuditStatus().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;
		}



		public List<CommonDropDown> GetAllProduct()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllProduct().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> GetAllOrderCategories()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllOrderCategories().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}


		public List<CommonDropDown> GetAllInsuranceCompanies()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllInsuranceCompanies().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> GetAllBanks()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllBanks().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> GetAllPaymentTerms()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllPaymentTerms().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> GetAllPorts()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllPorts().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> GetAllDeliveryTerms()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllDeliveryTerms().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllShipmentModes()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllShipmentModes().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllLCCategories()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllLCCategories().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllMasterLC()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllMasterLC().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllPI()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllPI().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllExpOrder()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllExpOrder().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllBtbLC()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllBtbLC().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllExportPIContracts()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllExportPIContracts().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllPackingMode()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllPackingMode().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllExpInvoice()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllExpInvoice().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllCountry()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllCountry().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllCnFAgents()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllCnFAgents().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllCouriers()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllCouriers().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllImportTypes()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllImportTypes().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllEmployees()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllEmployees().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> GetAllProductCategories()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllProductCategories().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public List<CommonDropDown> AutocompleteProduct(string Prefix)
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{

				try
				{
					records = context.Repositories.CommonRepository.AutocompleteProduct(Prefix).Select(c => new CommonDropDown
					{
						Name = c.Name,
						Id = c.Id
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();


				}
			}
			return records;


		}
		public List<CommonDropDown> ProductIdByName(string Name)
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{

				try
				{
					records = context.Repositories.CommonRepository.ProductIdByName(Name).Select(c => new CommonDropDown
					{
						Id = c.Id
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();


				}
			}
			return records;


		}
		public List<CommonDropDown> AutocompleteRequisitionNo(string Prefix)
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.AutocompleteRequisitionNo(Prefix).Select(c => new CommonDropDown
					{
						Name = c.Name,
						Id = c.Id
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}






		//mychange
		public IList<CommonDropDown> GetAllHeaders()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetAllHeaders().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public IList<CommonDropDown> EntryTypes()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.EntryTypes().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		//BANK
		public IList<CommonDropDown> DepositType()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.DepositType().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public IList<CommonDropDown> BankEntryType()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.BankEntryType().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}


		public IList<CommonDropDown> DocumentType()
		{

			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.DocumentType().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;


			///List<CommonDropDown> records = new List<CommonDropDown>();
			//using var context = _unitOfWork.Create();
			//try
			//{
			///    records = context.Repositories.CommonRepository.DocumentType().Select(c => new CommonDropDown
			//    {
			//        Name = c.Value,
			//        Value = c.Name
			//    }).ToList();
			//    context.SaveChanges();

			//    return records;
			//}
			//catch (Exception e)
			//{
			//    context.RollBack();
			//}

			//return records;
		}

		public IList<CommonDropDown> APDocumentType()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.APDocumentType().Select(c => new CommonDropDown
				{
					Name = c.Value,
					Value = c.Name
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;
		}

		public IList<CommonDropDown> ProrationMethod()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.ProrationMethod().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
		public IList<CommonDropDown> POType()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.POType().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}


		public List<CommonDropDown> ApplyMethod()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.ApplyMethod().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> TransactionType()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.TransactionType().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public List<CommonDropDown> OrderBy()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.OrderBy().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public IList<CommonDropDown> UserBranch()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.UserBranch().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}



		public IList<CommonDropDown> UserId()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.UserId().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}
        public IList<CommonDropDown> ModulId()
        {
            List<CommonDropDown> records = new List<CommonDropDown>();
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    records = context.Repositories.CommonRepository.ModulId().Select(c => new CommonDropDown
                    {
                        Name = c.Value,
                        Value = c.Name
                    }).ToList();
                    context.SaveChanges();

                    return records;
                }
                catch (Exception e)
                {
                    context.RollBack();
                }
            }
            return records;
        }
        public IList<CommonDropDown> NodeName()
        {
            List<CommonDropDown> records = new List<CommonDropDown>();
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    records = context.Repositories.CommonRepository.NodeName().Select(c => new CommonDropDown
                    {
                        Name = c.Value,
                        Value = c.Name
                    }).ToList();
                    context.SaveChanges();

                    return records;
                }
                catch (Exception e)
                {
                    context.RollBack();
                }
            }
            return records;
        }
        public IList<CommonDropDown> GetModuleName()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.GetModuleName().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		List<CommonDropDown> ICommonService.Branchs(string UserId)
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.Branchs(UserId).Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;

		}


		//SSLAudit
		public List<CommonDropDown>? GetAuditName()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetAuditName().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;

		}

		public List<CommonDropDown>? GetIssuePriority()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetIssuePriority().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;

		}

		public List<CommonDropDown>? GetIssueStatus()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetIssueStatus().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;

		}


        //Status
        public List<CommonDropDown>? GetStatus()
        {
            List<CommonDropDown> records = new List<CommonDropDown>();
            using var context = _unitOfWork.Create();
            try
            {
                records = context.Repositories.CommonRepository.GetStatus().Select(c => new CommonDropDown
                {
                    Name = c.Name,
                    Value = c.Value
                }).ToList();
                context.SaveChanges();

                return records;
            }
            catch (Exception e)
            {
                context.RollBack();
            }

            return records;

        }


        public List<CommonDropDown>? GetAuditTypes()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using var context = _unitOfWork.Create();
			try
			{
				records = context.Repositories.CommonRepository.GetAuditTypes().Select(c => new CommonDropDown
				{
					Name = c.Name,
					Value = c.Value
				}).ToList();
				context.SaveChanges();

				return records;
			}
			catch (Exception e)
			{
				context.RollBack();
			}

			return records;

		}


		public IList<CommonDropDown> TeamName()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.TeamName().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public IList<CommonDropDown> AuditName()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.AuditName().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public IList<CommonDropDown> ColorName()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.ColorName().Select(c => new CommonDropDown
					{
						Name = c.Value,
						Value = c.Name
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

		public IList<CommonDropDown> ReportingManagers()
		{
			List<CommonDropDown> records = new List<CommonDropDown>();
			using (var context = _unitOfWork.Create())
			{
				try
				{
					records = context.Repositories.CommonRepository.ReportingManagers().Select(c => new CommonDropDown
					{
						Name = c.Name,
						Value = c.Value
					}).ToList();
					context.SaveChanges();

					return records;
				}
				catch (Exception e)
				{
					context.RollBack();
				}
			}
			return records;
		}

        public List<CommonDropDown> BranchName(string UserId)
        {
            List<CommonDropDown> records = new List<CommonDropDown>();
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    records = context.Repositories.CommonRepository.BranchName(UserId).Select(c => new CommonDropDown
                    {
                        Name = c.Name,
                        Value = c.Value
                    }).ToList();
                    context.SaveChanges();

                    return records;
                }
                catch (Exception e)
                {
                    context.RollBack();
                }
            }
            return records;
        }
    }
}
