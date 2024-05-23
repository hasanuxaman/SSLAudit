using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Circular;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Circular
{
	public class CircularsService : ICircularsService
    {
		private IUnitOfWork _unitOfWork;
		private readonly IFileService _fileService;
		long maxSizeInBytes = 2 * 1024 * 1024;  // 2MB

		string saveDirectory = "wwwroot\\files";
		string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx", ".docx" };

		public CircularsService(IUnitOfWork unitOfWork, IFileService fileService)
		{
			_unitOfWork = unitOfWork;
			_fileService = fileService;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<Circulars> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<Circulars>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.CircularsRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Circulars>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Circulars>>()
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
                        context.Repositories.CircularsRepository.GetCount(tableName,
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

		public ResultModel<List<Circulars>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.CircularsRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Circulars>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Circulars>>()
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
                    var records = context.Repositories.CircularsRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<Circulars> Insert(Circulars model)
		{
            string CodeGroup = "CR";
            string CodeName = "Circulars";

			List<string> savedPaths = new List<string>();


			using (var context = _unitOfWork.Create())
            {
                try
                {

					savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;



					if (model == null)
                    {
                        return new ResultModel<Circulars>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.ToursRepository.CodeGeneration(CodeGroup, CodeName);


                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

                        Circulars master = context.Repositories.CircularsRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<Circulars>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }


						foreach (string savedPath in savedPaths)
						{
							CircularAttachments circularAttachments = new CircularAttachments
							{
								//AuditId = master.AuditId,
								FileName = Path.GetFileName(savedPath),
								CircularId = master.Id
							};

							circularAttachments = context.Repositories.CircularAttachmentsRepository.Insert(circularAttachments);
							master.AttachmentsList.Add(circularAttachments);
						}



						context.SaveChanges();


                        return new ResultModel<Circulars>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<Circulars>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Circulars>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

           

        }

		

		public ResultModel<Circulars> Update(Circulars model)
		{

			List<string> savedPaths = new List<string>();


			using (var context = _unitOfWork.Create())
            {

                try
                {

					savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;



					Circulars master = context.Repositories.CircularsRepository.Update(model);

					foreach (string savedPath in savedPaths)
					{
						CircularAttachments circularAttachments = new CircularAttachments
						{
							//AuditId = master.AuditId,
							FileName = Path.GetFileName(savedPath),
							CircularId = master.Id
						};

						circularAttachments = context.Repositories.CircularAttachmentsRepository.Insert(circularAttachments);
						master.AttachmentsList.Add(circularAttachments);
					}




					context.SaveChanges();


                    return new ResultModel<Circulars>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Circulars>()
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
