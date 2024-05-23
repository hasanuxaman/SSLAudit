using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.AuditIssues
{
    public class AuditIssuesService : IAuditIssueService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        long maxSizeInBytes = 2 * 1024 * 1024;  // 2MB

        string saveDirectory = "wwwroot\\files";
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx", ".docx" };
        public AuditIssuesService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }


        public ResultModel<List<AuditIssue>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssue> records = context.Repositories.AuditIssueRepository.GetAll(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditIssue>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssue> records = context.Repositories.AuditIssueRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditIssue>>()
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
                int records = context.Repositories.AuditIssueRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<AuditIssue> Insert(AuditIssue model)
        {

            List<string> savedPaths = new List<string>(); 
            

            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;

                if (model is null)
                {
                    return new ResultModel<AuditIssue>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditIssue master = context.Repositories.AuditIssueRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditIssue>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                foreach (string savedPath in savedPaths)
                {
                    AuditIssueAttachments auditIssueAttachment = new AuditIssueAttachments
                    {
                        AuditId = master.AuditId,
                        FileName = Path.GetFileName(savedPath),
                        AuditIssueId = master.Id
                    };

                    auditIssueAttachment = context.Repositories.AuditIssueAttachmentsRepository.Insert(auditIssueAttachment);
                    master.AttachmentsList.Add(auditIssueAttachment);
                }


                context.SaveChanges();

                return new ResultModel<AuditIssue>()
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

                return new ResultModel<AuditIssue>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditIssue> Update(AuditIssue model)
        {
            List<string> savedPaths = new List<string>();

            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                bool CheckPostStatus = false;
                CheckPostStatus = context.Repositories.AuditIssueRepository.CheckPostStatus("A_AuditIssues", new[] { "Id" }, new[] { model.Id.ToString() });
                if (CheckPostStatus)
                {
                    return new ResultModel<AuditIssue>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.PostAlready,

                    };
                }





                savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;
                AuditIssue master = context.Repositories.AuditIssueRepository.Update(model);

                foreach (string savedPath in savedPaths)
                {
                    AuditIssueAttachments auditIssueAttachment = new AuditIssueAttachments
                    {
                        AuditId = master.AuditId,
                        FileName = Path.GetFileName(savedPath),
                        AuditIssueId = master.Id
                    };

                    auditIssueAttachment = context.Repositories.AuditIssueAttachmentsRepository.Insert(auditIssueAttachment);
                    master.AttachmentsList.Add(auditIssueAttachment);
                }

                context.SaveChanges();

                return new ResultModel<AuditIssue>()
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

                return new ResultModel<AuditIssue>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditIssue> MultiplePost(AuditIssue model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.AuditIssueRepository.CheckPostStatus("A_AuditIssues", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<AuditIssue>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    AuditIssue master = context.Repositories.AuditIssueRepository.MultiplePost(model);



                    context.SaveChanges();


                    return new ResultModel<AuditIssue>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditIssue>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }


		public ResultModel<AuditIssue> MultipleUnPost(AuditIssue model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					AuditIssue master = context.Repositories.AuditIssueRepository.MultipleUnPost(model);



					context.SaveChanges();


					return new ResultModel<AuditIssue>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<AuditIssue>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}



		public ResultModel<AuditIssue> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

		public ResultModel<AuditIssue> ReportStatusUpdate(AuditIssue model)
		{
	

			using IUnitOfWorkAdapter context = _unitOfWork.Create();
			try
			{

				AuditIssue master = context.Repositories.AuditIssueRepository.ReportStatusUpdate(model);



				context.SaveChanges();

				return new ResultModel<AuditIssue>()
				{
					Status = Status.Success,
					Message = MessageModel.UpdateSuccess,
					Data = model
				};

			}
			catch (Exception e)
			{
				context.RollBack();


				return new ResultModel<AuditIssue>()
				{
					Status = Status.Fail,
					Message = MessageModel.UpdateFail,
					Exception = e
				};
			}
		}

        public ResultModel<List<AuditIssue>> GetAuditIssueIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssue> records = context.Repositories.AuditIssueRepository.GetAuditIssueIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<int> GetAuditIssueIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                int records = context.Repositories.AuditIssueRepository.GetAuditIssueIndexCount(index, conditionalFields, conditionalValue);
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
        public ResultModel<IssueRejectComments> IssueApprove(IssueRejectComments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {

                IssueRejectComments master = context.Repositories.AuditIssueRepository.IssueApprove(model);



                context.SaveChanges();

                return new ResultModel<IssueRejectComments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = master
                };

            }
            catch (Exception e)
            {
                context.RollBack();


                return new ResultModel<IssueRejectComments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }
        public ResultModel<AuditIssue> IssueApproveStatusUpdate(AuditIssue model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {

                AuditIssue master = context.Repositories.AuditIssueRepository.IssueApproveStatusUpdate(model);



                context.SaveChanges();

                return new ResultModel<AuditIssue>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = master
                };

            }
            catch (Exception e)
            {
                context.RollBack();


                return new ResultModel<AuditIssue>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<IssueRejectComments> IssueRejectStatusUpdate(IssueRejectComments model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {

                IssueRejectComments master = context.Repositories.AuditIssueRepository.IssueRejectStatusUpdate(model);



                context.SaveChanges();

                return new ResultModel<IssueRejectComments>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = master
                };

            }
            catch (Exception e)
            {
                context.RollBack();


                return new ResultModel<IssueRejectComments>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditIssue>> GetExcelIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssue> records = context.Repositories.AuditIssueRepository.GetExcelIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<int> GetExcelIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                int records = context.Repositories.AuditIssueRepository.GetExcelIndexCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<AuditIssue>> GetIssuesByAuditId(AuditMaster model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssue> records = context.Repositories.AuditIssueRepository.GetIssuesByAuditId(model);
                context.SaveChanges();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditIssue>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }
    }
}
