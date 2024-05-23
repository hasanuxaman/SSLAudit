using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.AuditFeedbackService
{
    public class AuditFeedbackAttachmentsService : IAuditFeedbackAttachmentsService
    {

        private IUnitOfWork _unitOfWork;
        public AuditFeedbackAttachmentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ResultModel<List<AuditFeedbackAttachments>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditFeedbackAttachments> records = context.Repositories.AuditFeedbackAttachmentsRepository.GetAll(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditFeedbackAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditFeedbackAttachments>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditFeedbackAttachments>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditFeedbackAttachments> records = context.Repositories.AuditFeedbackAttachmentsRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditFeedbackAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditFeedbackAttachments>>()
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
                int records = context.Repositories.AuditFeedbackAttachmentsRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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
                    context.Repositories.AuditFeedbackAttachmentsRepository.GetCount(tableName,
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

        public ResultModel<AuditFeedbackAttachments> Insert(AuditFeedbackAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<AuditFeedbackAttachments>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditFeedbackAttachments master = context.Repositories.AuditFeedbackAttachmentsRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditFeedbackAttachments>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }

                context.SaveChanges();

                return new ResultModel<AuditFeedbackAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditFeedbackAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditFeedbackAttachments> Update(AuditFeedbackAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                AuditFeedbackAttachments master = context.Repositories.AuditFeedbackAttachmentsRepository.Update(model);

                context.SaveChanges();

                return new ResultModel<AuditFeedbackAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditFeedbackAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditFeedbackAttachments> Delete(int id)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var count = context.Repositories.AuditIssueAttachmentsRepository.Delete(
                    TableName.A_AuditFeedbackAttachments,
                    new[] { "Id" }, new[] { id.ToString() }
                );
                context.SaveChanges();
                return new ResultModel<AuditFeedbackAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DeleteSuccess,
                    EffectedRows = count
                };
            }
            catch (Exception e)
            {

                context.RollBack();

                return new ResultModel<AuditFeedbackAttachments>()
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
