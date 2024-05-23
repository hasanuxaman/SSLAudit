using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Calender;
using Shampan.Core.Interfaces.Services.Oragnogram;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Oragnogram
{
	public class OragnogramService : IOragnogramService
	{
		private IUnitOfWork _unitOfWork;
		private readonly IFileService _fileService;
		long maxSizeInBytes = 2 * 1024 * 1024;  // 2MB

		//string saveDirectory = "wwwroot\\files";
		string saveDirectory = "wwwroot\\Images";
		string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx", ".docx" };

		public OragnogramService(IUnitOfWork unitOfWork, IFileService fileService)
		{
			_unitOfWork = unitOfWork;
			_fileService = fileService;


		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<EmployeesHierarchy> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<EmployeesHierarchy>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.OragnogramRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<EmployeesHierarchy>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<EmployeesHierarchy>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }



        }

        public ResultModel<List<object>> GetEmployeesData()
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.OragnogramRepository.GetEmployeesData();
					context.SaveChanges();

					return new ResultModel<List<object>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<object>>()
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
                        context.Repositories.OragnogramRepository.GetCount(tableName,
							"EmployeeId", null, null);
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

		public ResultModel<List<EmployeesHierarchy>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.OragnogramRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<EmployeesHierarchy>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<EmployeesHierarchy>>()
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
                    var records = context.Repositories.OragnogramRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<EmployeesHierarchy> Insert(EmployeesHierarchy model)
		{

            string CodeGroup = "EH";
            string CodeName = "EmployeesHierarchy";

			List<string> savedPaths = new List<string>();


			using (var context = _unitOfWork.Create())
            {
                try
                {

					//for image


					//savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;



					//end image








					if (model == null)
                    {
                        return new ResultModel<EmployeesHierarchy>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }




					string Code = context.Repositories.OragnogramRepository.CodeGeneration(CodeGroup, CodeName);


                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

                        EmployeesHierarchy master = context.Repositories.OragnogramRepository.Insert(model);


                        //for images

						model.EmployeeId = master.EmployeeId;		
						savedPaths = _fileService.UploadFilesById(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes,model.EmployeeId.ToString()).Result;

						//end




						if (master.EmployeeId <= 0)
                        {
                            return new ResultModel<EmployeesHierarchy>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }







						//for image

						foreach (string savedPath in savedPaths)
						{
							EmployeesHierarchyAttachments employeesHierarchyAttachments = new EmployeesHierarchyAttachments
							{
								//AuditId = master.AuditId,
								FileName = Path.GetFileName(savedPath),
								EmployeeId = master.EmployeeId
							};

							employeesHierarchyAttachments = context.Repositories.EmployeesHiAttachmentsRepository.Insert(employeesHierarchyAttachments);
							master.AttachmentsList.Add(employeesHierarchyAttachments);
						}

						//end image







						context.SaveChanges();


                        return new ResultModel<EmployeesHierarchy>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<EmployeesHierarchy>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<EmployeesHierarchy>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }

		

		public ResultModel<EmployeesHierarchy> Update(EmployeesHierarchy model)
		{
            using (var context = _unitOfWork.Create())
            {
				List<string> savedPaths = new List<string>();

				try
				{

					//model.EmployeeId = master.EmployeeId;

					//for images

					//savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;

					//end



					EmployeesHierarchy master = context.Repositories.OragnogramRepository.Update(model);


					//for images
					model.EmployeeId = master.EmployeeId;
					savedPaths = _fileService.UploadFilesById(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes, model.EmployeeId.ToString()).Result;

					//end



					//for images

					foreach (string savedPath in savedPaths)
					{
						EmployeesHierarchyAttachments employeesHierarchyAttachments = new EmployeesHierarchyAttachments
						{
							//AuditId = master.AuditId,
							FileName = Path.GetFileName(savedPath),
							EmployeeId = master.EmployeeId
						};

						employeesHierarchyAttachments = context.Repositories.EmployeesHiAttachmentsRepository.Insert(employeesHierarchyAttachments);
						master.AttachmentsList.Add(employeesHierarchyAttachments);
					}

					//images

					context.SaveChanges();


                    return new ResultModel<EmployeesHierarchy>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<EmployeesHierarchy>()
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
