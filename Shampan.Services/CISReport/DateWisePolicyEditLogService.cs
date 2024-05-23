using Shampan.Core.Interfaces.Services.CISReport;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.User;
using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.CISReport
{
    public class DateWisePolicyEditLogService : IDateWisePolicyEditLogService
    {
		private IUnitOfWork _unitOfWork;
		public DateWisePolicyEditLogService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<DateWisePolicyEditLog> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<DateWisePolicyEditLog>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.DateWisePolicyEditLogRepository.GetAll(conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<DateWisePolicyEditLog>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<DateWisePolicyEditLog>>()
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
						context.Repositories.DateWisePolicyEditLogRepository.GetCount(tableName,
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

        public ResultModel<List<DateWisePolicyEditLog>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.DateWisePolicyEditLogRepository.GetIndexData(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<DateWisePolicyEditLog>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<DateWisePolicyEditLog>>()
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
					var records = context.Repositories.DateWisePolicyEditLogRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<DateWisePolicyEditLog> Insert(DateWisePolicyEditLog model)
        {


			using (var context = _unitOfWork.Create())
			{
				try
				{




					DateWisePolicyEditLog master = context.Repositories.DateWisePolicyEditLogRepository.Insert(model);

					if (master.Id <= 0)
					{
						return new ResultModel<DateWisePolicyEditLog>()
						{
							Status = Status.Fail,
							Message = MessageModel.MasterInsertFailed,
							Data = master
						};
					}

					context.SaveChanges();



					return new ResultModel<DateWisePolicyEditLog>()
					{
						Status = Status.Success,
						Message = MessageModel.InsertSuccess,
						Data = master

					};


					return new ResultModel<DateWisePolicyEditLog>()
					{
						Status = Status.Fail,
						Message = MessageModel.MasterInsertFailed,
						Data = master

					};


				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<DateWisePolicyEditLog>()
					{
						Status = Status.Fail,
						Message = MessageModel.InsertFail,
						Exception = e
					};
				}
			}
		}

        public ResultModel<DateWisePolicyEditLog> Update(DateWisePolicyEditLog model)
        {
			using (var context = _unitOfWork.Create())
			{

				try
				{

					DateWisePolicyEditLog master = context.Repositories.DateWisePolicyEditLogRepository.Update(model);

					context.SaveChanges();


					return new ResultModel<DateWisePolicyEditLog>()
					{
						Status = Status.Success,
						Message = MessageModel.UpdateSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<DateWisePolicyEditLog>()
					{
						Status = Status.Fail,
						Message = MessageModel.UpdateFail,
						Exception = e
					};
				}

				throw new NotImplementedException();
            }
		}
    }
}
