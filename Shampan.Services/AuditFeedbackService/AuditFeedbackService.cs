using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shampan.Core.Interfaces.Repository;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

using System.IO.MemoryMappedFiles;
using System.Reflection;
//using Microsoft.AspNetCore.Http.Internal;

namespace Shampan.Services.AuditFeedbackService
{
    public class AuditFeedbackService : IAuditFeedbackService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        long maxSizeInBytes = 2 * 1024 * 1024;  // 2MB

        string saveDirectory = "wwwroot\\files";
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx", ".docx" };

        public AuditFeedbackService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public ResultModel<List<AuditFeedback>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditFeedback> records = context.Repositories.AuditFeedbackRepository.GetAll(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditFeedback>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditFeedback>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditFeedback>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditFeedback> records = context.Repositories.AuditFeedbackRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditFeedback>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditFeedback>>()
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
                int records = context.Repositories.AuditFeedbackRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<AuditFeedback> Insert(AuditFeedback model)
        {
            List<string> savedPaths = new List<string>();


            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {

				//user check


				bool CheckUserStatus = true;
				List<AuditFeedback> users = context.Repositories.AuditFeedbackRepository.GetAuditUsers("AuditUsers", new[] { "AuditId" }, new[] { model.AuditId.ToString() });
				
                foreach(var user in users)
                {
                    if(user.UserName == model.Audit.CreatedBy)
                    {
                        CheckUserStatus = false;

					}
                }
                if (CheckUserStatus)
				{
					return new ResultModel<AuditFeedback>()
					{
						Status = Status.Fail,
						Message = MessageModel.Permission,

					};
				}


				//end of check




				//change

				//if (model.Id != null && model.Id != 0 && model.Operation == "add")
				//{
				//    List<AuditFeedbackAttachments> auditFeedback = new List<AuditFeedbackAttachments>();

				//    auditFeedback = context.Repositories.AuditFeedbackAttachmentsRepository.GetAll(new[] { "AuditFeedbackId" }, new[] { model.Id.ToString() });


				//    List<IFormFile> formFiles = new List<IFormFile>();

				//    foreach (var attachment in auditFeedback)
				//    {


				//        byte[] fileBytes = File.ReadAllBytes(attachment.FileName);
				//        using (var stream = new MemoryStream(fileBytes))
				//        {

				//        }


				//    }


				//}

				//end


				savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;

                if (model is null)
                {
                    return new ResultModel<AuditFeedback>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditFeedback master = context.Repositories.AuditFeedbackRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                



                foreach (string savedPath in savedPaths)
                {
                    AuditFeedbackAttachments auditIssueAttachment = new AuditFeedbackAttachments
                    {
                        AuditId = master.AuditId,
                        FileName = Path.GetFileName(savedPath),
                        AuditIssueId = master.AuditIssueId,
                        AuditFeedbackId = master.Id
                    };

                    auditIssueAttachment = context.Repositories.AuditFeedbackAttachmentsRepository.Insert(auditIssueAttachment);
                    master.AttachmentsList.Add(auditIssueAttachment);
                }


                context.SaveChanges();

                return new ResultModel<AuditFeedback>()
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

                return new ResultModel<AuditFeedback>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditFeedback> Update(AuditFeedback model)
        {
            List<string> savedPaths = new List<string>();

            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                //Post UnPost Check

                bool CheckPostStatus = false;
                CheckPostStatus = context.Repositories.AuditFeedbackRepository.CheckPostStatus("A_AuditFeedbacks", new[] { "Id" }, new[] { model.Id.ToString() });
                if (CheckPostStatus)
                {
                    return new ResultModel<AuditFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.PostAlready,

                    };
                }


                //End Check Unpost Check


                //user check

                bool CheckUserStatus = true;
				List<AuditFeedback> users = context.Repositories.AuditFeedbackRepository.GetAuditUsers("AuditUsers", new[] { "AuditId" }, new[] { model.AuditId.ToString() });

				foreach (var user in users)
				{
					if (user.UserName == model.Audit.LastUpdateBy)
					{
						CheckUserStatus = false;

					}
				}
				if (CheckUserStatus)
				{
					return new ResultModel<AuditFeedback>()
					{
						Status = Status.Fail,
						Message = MessageModel.Permission,

					};
				}

				//end of check









				savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;
                AuditFeedback master = context.Repositories.AuditFeedbackRepository.Update(model);

                foreach (string savedPath in savedPaths)
                {
                    AuditFeedbackAttachments auditIssueAttachment = new AuditFeedbackAttachments
                    {
                        AuditId = master.AuditId,
                        FileName = Path.GetFileName(savedPath),
                        AuditIssueId = master.AuditIssueId,
                        AuditFeedbackId = master.Id
                    };

                    auditIssueAttachment = context.Repositories.AuditFeedbackAttachmentsRepository.Insert(auditIssueAttachment);
                    master.AttachmentsList.Add(auditIssueAttachment);
                }

                context.SaveChanges();

                return new ResultModel<AuditFeedback>()
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

                return new ResultModel<AuditFeedback>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditFeedback> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<AuditIssue> GetAuditLastFeedback(AuditIssue model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssue> records = context.Repositories.AuditFeedbackRepository.GetAuditLastFeedback(model);

                AuditIssue recorddata = records.FirstOrDefault();

                context.SaveChanges();

                return new ResultModel<AuditIssue>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = recorddata
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditIssue>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditFeedback> MultiplePost(AuditFeedback model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.AuditFeedbackRepository.CheckPostStatus("A_AuditFeedbacks", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<AuditFeedback>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    AuditFeedback master = context.Repositories.AuditFeedbackRepository.MultiplePost(model);



                    context.SaveChanges();


                    return new ResultModel<AuditFeedback>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<AuditFeedback> MultipleUnPost(AuditFeedback model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    AuditFeedback master = context.Repositories.AuditFeedbackRepository.MultipleUnPost(model);



                    context.SaveChanges();


                    return new ResultModel<AuditFeedback>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UnPostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditFeedback>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UnPostFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<List<AuditFeedback>> GetAuditFeedbackByAuditId(AuditMaster model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditFeedback> records = context.Repositories.AuditFeedbackRepository.GetAuditFeedbackByAuditId(model);
                context.SaveChanges();

                return new ResultModel<List<AuditFeedback>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditFeedback>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }
    }
}
