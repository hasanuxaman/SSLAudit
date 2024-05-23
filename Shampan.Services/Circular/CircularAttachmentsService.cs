using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Circular;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Circular
{
    public class CircularAttachmentsService : ICircularAttachmentsService
	{
        private IUnitOfWork _unitOfWork;
        public CircularAttachmentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ResultModel<List<CircularAttachments>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<CircularAttachments> records = context.Repositories.CircularAttachmentsRepository.GetAll(conditionalFields, conditionalValue);

                context.SaveChanges();

                return new ResultModel<List<CircularAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<CircularAttachments>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<CircularAttachments>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<CircularAttachments> records = context.Repositories.CircularAttachmentsRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<CircularAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<CircularAttachments>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue,
            PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                int records = context.Repositories.CircularAttachmentsRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue,
            PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                int count =
                    context.Repositories.CircularAttachmentsRepository.GetCount(tableName,
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

        public ResultModel<CircularAttachments> Insert(CircularAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<CircularAttachments>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

				CircularAttachments master = context.Repositories.CircularAttachmentsRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<CircularAttachments>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }

                context.SaveChanges();

                return new ResultModel<CircularAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<CircularAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<CircularAttachments> Update(CircularAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
				CircularAttachments master = context.Repositories.CircularAttachmentsRepository.Update(model);

                context.SaveChanges();

                return new ResultModel<CircularAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<CircularAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<CircularAttachments> Delete(int id)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var count = context.Repositories.CircularAttachmentsRepository.Delete(
                    TableName.CircularAttachments,
                    new[] { "Id" }, new[] { id.ToString() }
                );
                context.SaveChanges();
                return new ResultModel<CircularAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DeleteSuccess,
                    EffectedRows = count
                };
            }
            catch (Exception e)
            {

                context.RollBack();

                return new ResultModel<CircularAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DeleteFail,
                    EffectedRows = 0
                };
            }
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }
    }
}
