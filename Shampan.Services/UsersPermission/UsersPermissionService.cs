using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.UsersPermission;
using Shampan.Models;
using Shampan.Services.AuditIssues;
using UnitOfWork.Interfaces;

namespace Shampan.Services.UsersPermission
{
	public class UsersPermissionService : IUsersPermissionService
	{
		private IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        long maxSizeInBytes = 2 * 1024 * 1024;  
        string saveDirectory = "wwwroot\\files";
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx", ".docx" };

        public UsersPermissionService(IUnitOfWork unitOfWork, IFileService fileService)
        {
			_unitOfWork = unitOfWork;
            _fileService = fileService;

        }

		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<Users> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<Users>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.UsersPermissionRepository.GetAll(conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<Users>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<Users>>()
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
						context.Repositories.UsersPermissionRepository.GetCount(tableName,
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

		public ResultModel<List<Users>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.UsersPermissionRepository.GetIndexData(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<Users>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<Users>>()
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
					var records = context.Repositories.UsersPermissionRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<SubmanuList>> GetNodesIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.UsersPermissionRepository.GetNodesIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<SubmanuList>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<SubmanuList>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<int> GetNodesIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.UsersPermissionRepository.GetNodesIndexDataCount(index, conditionalFields, conditionalValue);
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

        public void ImageInsert(UserProfile model)
        {

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    List<string> savedPaths = new List<string>();
                    UserProfile master = new UserProfile();

                    savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;

                    foreach (string savedPath in savedPaths)
                    {
                        UserProfileAttachments UserProfileAttachments = new UserProfileAttachments
                        {
                            //AuditId = master.AuditId,
                            FileName = Path.GetFileName(savedPath),
                            UserId = model.UserId,
                            UserName = model.UserName
                            
                        };

                        UserProfileAttachments = context.Repositories.UsersPermissionRepository.ImageInsert(UserProfileAttachments);
                        master.AttachmentsList.Add(UserProfileAttachments);
                    }

                    context.SaveChanges();


                }
                catch (Exception e)
                {
                    context.RollBack();
                    //return new UserProfile();
                }

            }
        }

        public ResultModel<Users> Insert(Users model)
		{
            string CodeGroup = "UP";
            string CodeName = "UsersPermission";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    


                    bool CheckUserStatus = false;
                    CheckUserStatus = context.Repositories.UsersPermissionRepository.CheckUserStatus(model);
                    if (CheckUserStatus)
                    {
                        return new ResultModel<Users>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.UserSavedAlready,

                        };
                    }
                    // bool CheckPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue);






                    if (model == null)
                    {
                        return new ResultModel<Users>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.UsersPermissionRepository.CodeGeneration(CodeGroup, CodeName);


                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

                        Users master = context.Repositories.UsersPermissionRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<Users>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }


                        context.SaveChanges();


                        return new ResultModel<Users>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<Users>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Users>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }

        public ResultModel<Users> Update(Users model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {


                    Users master = context.Repositories.UsersPermissionRepository.Update(model);

                    context.SaveChanges();


                    return new ResultModel<Users>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Users>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<SubmanuList> UpdateNodes(SubmanuList model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {


                    SubmanuList master = context.Repositories.UsersPermissionRepository.UpdateNoes(model);

                    context.SaveChanges();


                    return new ResultModel<SubmanuList>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<SubmanuList>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<List<UserProfileAttachments>> GetAllImage(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<UserProfileAttachments> records = context.Repositories.UsersPermissionRepository.GetAllImage(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<UserProfileAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<UserProfileAttachments>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<UserProfileAttachments>> GetImageByUserName(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<UserProfileAttachments> records = context.Repositories.UsersPermissionRepository.GetImageByUserName(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<UserProfileAttachments>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<UserProfileAttachments>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public void ImageUpdate(UserProfile model)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    List<string> savedPaths = new List<string>();
                    UserProfile master = new UserProfile();


                    int id = context.Repositories.UsersPermissionRepository.Delete("AspNetUsersAttachments", new[] { "UserName" }, new[] { model.UserName });

                    //if(id != 0)
                    //{

                        savedPaths = _fileService.UploadFiles(model.Attachments, saveDirectory, allowedExtensions, maxSizeInBytes).Result;
                        foreach (string savedPath in savedPaths)
                        {
                            UserProfileAttachments UserProfileAttachments = new UserProfileAttachments
                            {
                                //AuditId = master.AuditId,
                                FileName = Path.GetFileName(savedPath),
                                UserId = model.UserId,
                                UserName = model.UserName

                            };

                            UserProfileAttachments = context.Repositories.UsersPermissionRepository.ImageInsert(UserProfileAttachments);
                            master.AttachmentsList.Add(UserProfileAttachments);
                        }
                        context.SaveChanges();

                    //}


                }
                catch (Exception e)
                {
                    context.RollBack();
                    //return new UserProfile();
                }

            }
        }

        public UserProfileAttachments GetUserIdByName(string Name)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<UserProfileAttachments> records = context.Repositories.UsersPermissionRepository.GetUserIdByName(Name);
                context.SaveChanges();

                //return new ResultModel<List<UserProfile>>()
                //{
                //    Status = Status.Success,
                //    Message = MessageModel.DataLoaded,
                //    Data = records
                //};

                return records[0];

            }
            catch (Exception e)
            {
                context.RollBack();

                //return new ResultModel<List<UserProfile>>()
                //{
                //    Status = Status.Fail,
                //    Message = MessageModel.DataLoadedFailed,
                //    Exception = e
                //};

                return new UserProfileAttachments();
            }
        }

        public ResultModel<UserProfile> UserProfileDeActivate(UserProfile model)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    UserProfile master = context.Repositories.UsersPermissionRepository.UserProfileDeActivate(model);
                    context.SaveChanges();

                    return new ResultModel<UserProfile>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DeActivateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();
                    return new ResultModel<UserProfile>()
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
