using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Repository;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.AuditFeedbackService
{
    public class AuditBranchFeedbackService : IAuditBranchFeedbackService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        long maxSizeInBytes = 2 * 1024 * 1024;  // 2MB

        string saveDirectory = "wwwroot\\files";
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx", ".docx" };

        public AuditBranchFeedbackService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public ResultModel<List<AuditBranchFeedback>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditBranchFeedback> records = context.Repositories.AuditBranchFeedbackRepository.GetAll(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditBranchFeedback>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditBranchFeedback>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditBranchFeedback>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditBranchFeedback> records = context.Repositories.AuditBranchFeedbackRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditBranchFeedback>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditBranchFeedback>>()
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
                int records = context.Repositories.AuditBranchFeedbackRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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
                    context.Repositories.AuditIssueRepository.GetCount(tableName,
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

        public ResultModel<AuditBranchFeedback> Insert(AuditBranchFeedback model)
        {
            List<string> savedPaths = new List<string>();


            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
				//Issue User Check

				//bool CheckIssueUserStatus = true;
				//List<AuditBranchFeedback> users = context.Repositories.AuditBranchFeedbackRepository.GetAuditIssueUsers("AuditIssueUsers", new[] { "AuditIssueId" }, new[] { model.AuditIssueId.ToString() });

				//foreach (var user in users)
				//{
				//	if (user.UserName == model.Audit.CreatedBy)
				//	{
				//		CheckIssueUserStatus = false;

				//	}
				//}
				//if (CheckIssueUserStatus)
				//{
				//	return new ResultModel<AuditBranchFeedback>()
				//	{
				//		Status = Status.Fail,
				//		Message = MessageModel.Permission,

				//	};
				//}

				//end of Issu User Check





				savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;

                if (model is null)
                {
                    return new ResultModel<AuditBranchFeedback>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditBranchFeedback master = context.Repositories.AuditBranchFeedbackRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditBranchFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                foreach (string savedPath in savedPaths)
                {
                    AuditBranchFeedbackAttachments auditIssueAttachment = new AuditBranchFeedbackAttachments
                    {
                        AuditId = master.AuditId,
                        FileName = Path.GetFileName(savedPath),
                        AuditIssueId = master.AuditIssueId,
                        AuditBranchFeedbackId = master.Id
                    };

                    auditIssueAttachment = context.Repositories.AuditBranchFeedbackAttachmentsRepository.Insert(auditIssueAttachment);
                    master.AttachmentsList.Add(auditIssueAttachment);
                }


                context.SaveChanges();

                return new ResultModel<AuditBranchFeedback>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                foreach (var path in savedPaths)
                {
                    File.Delete(path);
                }

                return new ResultModel<AuditBranchFeedback>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditBranchFeedback> Update(AuditBranchFeedback model)
        {
            List<string> savedPaths = new List<string>();

            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {


				//Issue User Check
				//bool CheckIssueUserStatus = true;
				//List<AuditBranchFeedback> users = context.Repositories.AuditBranchFeedbackRepository.GetAuditIssueUsers("AuditIssueUsers", new[] { "AuditIssueId" }, new[] { model.AuditIssueId.ToString() });

				//foreach (var user in users)
				//{
				//	if (user.UserName == model.Audit.LastUpdateBy)
				//	{
				//		CheckIssueUserStatus = false;

				//	}
				//}
				//if (CheckIssueUserStatus)
				//{
				//	return new ResultModel<AuditBranchFeedback>()
				//	{
				//		Status = Status.Fail,
				//		Message = MessageModel.Permission,

				//	};
				//}
				//end of Issu User Check






				bool CheckPostStatus = false;
                CheckPostStatus = context.Repositories.AuditBranchFeedbackRepository.CheckPostStatus("A_AuditBranchFeedbacks", new[] { "Id" }, new[] { model.Id.ToString() });
                if (CheckPostStatus)
                {
                    return new ResultModel<AuditBranchFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.PostAlready,

                    };
                }



                savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;
                AuditBranchFeedback master = context.Repositories.AuditBranchFeedbackRepository.Update(model);

                foreach (string savedPath in savedPaths)
                {
                    AuditBranchFeedbackAttachments auditIssueAttachment = new AuditBranchFeedbackAttachments
                    {
                        AuditId = master.AuditId,
                        FileName = Path.GetFileName(savedPath),
                        AuditIssueId = master.AuditIssueId,
                        AuditBranchFeedbackId = master.Id
                    };

                    auditIssueAttachment = context.Repositories.AuditBranchFeedbackAttachmentsRepository.Insert(auditIssueAttachment);
                    master.AttachmentsList.Add(auditIssueAttachment);
                }

                context.SaveChanges();

                return new ResultModel<AuditBranchFeedback>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                foreach (var path in savedPaths)
                {
                    File.Delete(path);
                }

                return new ResultModel<AuditBranchFeedback>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditBranchFeedback> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<AuditBranchFeedback> MultiplePost(AuditBranchFeedback model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.AuditBranchFeedbackRepository.CheckPostStatus("A_AuditBranchFeedbacks", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<AuditBranchFeedback>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    AuditBranchFeedback master = context.Repositories.AuditBranchFeedbackRepository.MultiplePost(model);



                    context.SaveChanges();


                    return new ResultModel<AuditBranchFeedback>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditBranchFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<AuditBranchFeedback> MultipleUnPost(AuditBranchFeedback model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    AuditBranchFeedback master = context.Repositories.AuditBranchFeedbackRepository.MultipleUnPost(model);



                    context.SaveChanges();


                    return new ResultModel<AuditBranchFeedback>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UnPostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditBranchFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UnPostFail,
                        Exception = e
                    };
                }
            }
        }

        public List<AuditBranchFeedback> GetAuditIssuesFromBranch(AuditBranchFeedback model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    List<AuditBranchFeedback> master = context.Repositories.AuditBranchFeedbackRepository.GetAuditIssuesFromBranch(model);
                    context.SaveChanges();

                    return master;
                    

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new List<AuditBranchFeedback>();
                }
            }
        }

        public AuditBranchFeedback GetTopOneBranchId(int id)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    AuditBranchFeedback master = context.Repositories.AuditBranchFeedbackRepository.GetTopOneBranchId(id);
                    context.SaveChanges();

                    return master;


                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new AuditBranchFeedback();
                }
            }
        }

        public List<IssueRejectComments> GetIssueRejectComments(int model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    List<IssueRejectComments> master = context.Repositories.AuditBranchFeedbackRepository.GetIssueRejectComments(model);
                    context.SaveChanges();

                    return master;


                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new List<IssueRejectComments>();
                }
            }
        }

        public List<IssueRejectComments> GetIssueApproveComments(int model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    List<IssueRejectComments> master = context.Repositories.AuditBranchFeedbackRepository.GetIssueApproveComments(model);
                    context.SaveChanges();

                    return master;


                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new List<IssueRejectComments>();
                }
            }
        }
    }
}
