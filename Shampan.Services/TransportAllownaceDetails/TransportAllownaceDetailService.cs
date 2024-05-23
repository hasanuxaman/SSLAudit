using Shampan.Core.Interfaces.Services.CISReport;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.TransportAllownaceDetails;
using Shampan.Core.Interfaces.Services.User;
using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.TransportAllownaceDetails
{
    public class TransportAllownaceDetailService : ITransportAllownaceDetailService
    {
		private IUnitOfWork _unitOfWork;
		public TransportAllownaceDetailService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<TransportAllownaceDetail> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<TransportAllownaceDetail>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.TransportAllownaceDetailRepository.GetAll(conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<TransportAllownaceDetail>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<TransportAllownaceDetail>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}
		}

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			using (var context = _unitOfWork.Create())
			{

				try
				{
					int count =
						context.Repositories.CISReportRepository.GetCount(tableName,
							"Id", null, null);
					context.SaveChanges();


					return count;

				}
				catch (Exception e)
				{
					context.RollBack();

					return 0;
				}

			}
		}

        public ResultModel<List<TransportAllownaceDetail>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.TransportAllownaceDetailRepository.GetIndexData(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<TransportAllownaceDetail>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<TransportAllownaceDetail>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}
		}

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.TransportAllownaceDetailRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<int>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<int>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}
			}
		}

        public ResultModel<TransportAllownaceDetail> Insert(TransportAllownaceDetail model)
        {
			using (var context = _unitOfWork.Create())
			{
				try
				{

                    TransportAllownaceDetail master = context.Repositories.TransportAllownaceDetailRepository.Insert(model);
					if (master.Id <= 0)
					{
						return new ResultModel<TransportAllownaceDetail>()
							{
								Status = Status.Fail,
								Message = MessageModel.MasterInsertFailed,
								Data = master
							};
						}

						context.SaveChanges();


						return new ResultModel<TransportAllownaceDetail>()
						{
							Status = Status.Success,
							Message = MessageModel.InsertSuccess,
							Data = master

						};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<TransportAllownaceDetail>()
					{
						Status = Status.Fail,
						Message = MessageModel.InsertFail,
						Exception = e
					};
				}
			}
		}

        public ResultModel<TransportAllownaceDetail> Update(TransportAllownaceDetail model)
        {
			using (var context = _unitOfWork.Create())
			{
				try
				{

                    TransportAllownaceDetail master = context.Repositories.TransportAllownaceDetailRepository.Update(model);
					context.SaveChanges();

					return new ResultModel<TransportAllownaceDetail>()
					{
						Status = Status.Success,
						Message = MessageModel.UpdateSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<TransportAllownaceDetail>()
					{
						Status = Status.Fail,
						Message = MessageModel.UpdateFail,
						Exception = e
					};
				}
			}
		}
    }
}
