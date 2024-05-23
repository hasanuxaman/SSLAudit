using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Circular;
using Shampan.Core.Interfaces.Services.Oragnogram;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Oragnogram
{
    public class EmployeesHierarchyAttachmentsService : IEmployeesHiAttachmentsService
	{
        private IUnitOfWork _unitOfWork;
        public EmployeesHierarchyAttachmentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ResultModel<List<EmployeesHierarchyAttachments>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<EmployeesHierarchyAttachments> records = context.Repositories.EmployeesHiAttachmentsRepository.GetAll(conditionalFields, conditionalValue);

                context.SaveChanges();

                return new ResultModel<List<EmployeesHierarchyAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<EmployeesHierarchyAttachments>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<EmployeesHierarchyAttachments>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<EmployeesHierarchyAttachments> records = context.Repositories.EmployeesHiAttachmentsRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<EmployeesHierarchyAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<EmployeesHierarchyAttachments>>()
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

        public ResultModel<EmployeesHierarchyAttachments> Insert(EmployeesHierarchyAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<EmployeesHierarchyAttachments>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

				EmployeesHierarchyAttachments master = context.Repositories.EmployeesHiAttachmentsRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<EmployeesHierarchyAttachments>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }

                context.SaveChanges();

                return new ResultModel<EmployeesHierarchyAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<EmployeesHierarchyAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<EmployeesHierarchyAttachments> Update(EmployeesHierarchyAttachments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
				EmployeesHierarchyAttachments master = context.Repositories.EmployeesHiAttachmentsRepository.Update(model);

                context.SaveChanges();

                return new ResultModel<EmployeesHierarchyAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<EmployeesHierarchyAttachments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<EmployeesHierarchyAttachments> Delete(int id)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var count = context.Repositories.EmployeesHiAttachmentsRepository.Delete(
                    TableName.EmployeesHierarchyAttachments,
                    new[] { "Id" }, new[] { id.ToString() }
                );
                context.SaveChanges();
                return new ResultModel<EmployeesHierarchyAttachments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DeleteSuccess,
                    EffectedRows = count
                };
            }
            catch (Exception e)
            {

                context.RollBack();

                return new ResultModel<EmployeesHierarchyAttachments>()
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
