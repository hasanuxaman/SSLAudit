using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.AuditIssues
{
    public class AuditIssueAttachmentsService : IAuditIssueAttachmentsService
    {
        private IUnitOfWork _unitOfWork;
        public AuditIssueAttachmentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ResultModel<List<AuditIssueAttachments>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssueAttachments> records = context.Repositories.AuditIssueAttachmentsRepository.GetAll(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditIssueAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditIssueAttachments>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditIssueAttachments>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssueAttachments> records = context.Repositories.AuditIssueAttachmentsRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditIssueAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditIssueAttachments>>()
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
                int records = context.Repositories.AuditIssueAttachmentsRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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
                    context.Repositories.AuditIssueAttachmentsRepository.GetCount(tableName,
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

        public ResultModel<AuditIssueAttachments> Insert(AuditIssueAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<AuditIssueAttachments>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditIssueAttachments master = context.Repositories.AuditIssueAttachmentsRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditIssueAttachments>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }

                context.SaveChanges();

                return new ResultModel<AuditIssueAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditIssueAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditIssueAttachments> Update(AuditIssueAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                AuditIssueAttachments master = context.Repositories.AuditIssueAttachmentsRepository.Update(model);

                context.SaveChanges();

                return new ResultModel<AuditIssueAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditIssueAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditIssueAttachments> Delete(int id)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var count = context.Repositories.AuditIssueAttachmentsRepository.Delete(
                    TableName.A_AuditIssueAttachments,
                    new[] { "Id" }, new[] { id.ToString() }
                );
                context.SaveChanges();
                return new ResultModel<AuditIssueAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DeleteSuccess,
                    EffectedRows = count
                };
            }
            catch (Exception e)
            {

                context.RollBack();

                return new ResultModel<AuditIssueAttachments>()
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
