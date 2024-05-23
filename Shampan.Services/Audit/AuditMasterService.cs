using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Models;
using Shampan.Models.AuditModule;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Audit
{
    public class AuditMasterService : IAuditMasterService
    {

        private IUnitOfWork _unitOfWork;

        public AuditMasterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ResultModel<List<AuditMaster>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditMaster> records = context.Repositories.AuditMasterRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }



        public ResultModel<List<AuditMaster>> FeedBackGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {
                try
                {
                    List<AuditMaster> records = context.Repositories.AuditMasterRepository.FeedBackGetIndexDataStatus(index, conditionalFields, conditionalValue,vm);
                    context.SaveChanges();
                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };
                }
                catch (Exception e)
                {
                    context.RollBack();
                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }
            }
        }
        public ResultModel<List<AuditMaster>> IssueGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditMaster> records = context.Repositories.AuditMasterRepository.IssueGetIndexDataStatus(index, conditionalFields, conditionalValue,vm);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<AuditMaster>> GetAuditApproveIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditMaster> records = context.Repositories.AuditMasterRepository.GetAuditApproveIndexDataStatus(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<AuditMaster>> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditMaster> records = context.Repositories.AuditMasterRepository.GetIndexDataSelfStatus(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }




        public ResultModel<List<AuditMaster>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditMaster> records = context.Repositories.AuditMasterRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue,
            PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.AuditMasterRepository.GetIndexDataCount(index, conditionalFields, conditionalValue,vm);
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

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int count =
                        context.Repositories.AuditMasterRepository.GetCount(tableName,
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

        public ResultModel<AuditMaster> Insert(AuditMaster model)
        {
            string CodeGroup = "Audit";
            string CodeName = "AuditMaster";

            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }


                string code = context.Repositories.AuditMasterRepository.GenerateCode(CodeGroup, CodeName, Convert.ToInt32(model.BranchID));

                if (code == "" && code == null)
                {
                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        //Exception = e
                    };
                }

                model.Code = code;

                AuditMaster master = context.Repositories.AuditMasterRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                context.SaveChanges();


                return new ResultModel<AuditMaster>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditMaster>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditMaster> Update(AuditMaster model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {


                bool CheckPostStatus = false;
                CheckPostStatus = context.Repositories.AuditMasterRepository.CheckPostStatus("A_Audits", new[] { "Id" }, new[] { model.Id.ToString() });
                if (CheckPostStatus)
                {
                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.PostAlready,

                    };
                }



                AuditMaster master = context.Repositories.AuditMasterRepository.Update(model);

                context.SaveChanges();


                return new ResultModel<AuditMaster>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditMaster>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditMaster> Delete(int id)
        {
			using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {


				string stringValue = id.ToString();

				int master = context.Repositories.AuditMasterRepository.Delete("AuditUsers", new[] { "Id" }, new[] { stringValue });

				context.SaveChanges();


				return new ResultModel<AuditMaster>()
				{
					Status = Status.Success,
					Message = MessageModel.DeleteSuccess
					//Data = model
				};

			}
			catch (Exception e)
			{
				context.RollBack();

				return new ResultModel<AuditMaster>()
				{
					Status = Status.Fail,
					Message = MessageModel.UpdateFail,
					Exception = e
				};
			}
		}

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<AuditMaster> MultiplePost(AuditMaster model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.AuditMasterRepository.CheckPostStatus("A_Audits", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<AuditMaster>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    AuditMaster master = context.Repositories.AuditMasterRepository.MultiplePost(model);



                    context.SaveChanges();


                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                    
                }
            }
        }

        public ResultModel<AuditMaster> MultipleUnPost(AuditMaster model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    //if (model.Operation == "unpost")
                    //{
					//	bool CheckUpPostStatus = false;
					//	CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("A_Audits", new[] { "Id" }, new[] { model.Id.ToString() });
					//	if (CheckUpPostStatus)
					//	{
					//		return new ResultModel<AuditMaster>()
					//		{
					//			Status = Status.Fail,
					//			Message = MessageModel.UpPostAlready,

					//		};
					//	}
					//}

                    AuditMaster master = context.Repositories.AuditMasterRepository.MultipleUnPost(model);
                    context.SaveChanges();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UnPostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UnPostFail,
						Data = model,
						Exception = e
                    };
                }
            }
        }

        public ResultModel<List<AuditUser>> GetAuditUserTeamId(string TeamId,PeramModel pm)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditUser> records = context.Repositories.AuditMasterRepository.GetAuditUserByTeamId(TeamId,pm);
                    context.SaveChanges();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };


                    
                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

		public ResultModel<int> GetAuditApprovDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.AuditMasterRepository.GetAuditApprovedDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<int> GetAuditIssueDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.AuditMasterRepository.GetAuditIssueDataCount(index, conditionalFields, conditionalValue);
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
		public ResultModel<int> GetAuditFeedBackDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.AuditMasterRepository.GetAuditFeedBackDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<AuditMaster> AuditStatusUpdate(AuditMaster model)
		{
			using IUnitOfWorkAdapter context = _unitOfWork.Create();
			try
			{



				AuditMaster master = context.Repositories.AuditMasterRepository.AuditStatusUpdate(model);

				context.SaveChanges();


				return new ResultModel<AuditMaster>()
				{
					Status = Status.Success,
					Message = MessageModel.UpdateSuccess,
					Data = model
				};

			}
			catch (Exception e)
			{
				context.RollBack();

				return new ResultModel<AuditMaster>()
				{
					Status = Status.Fail,
					Message = MessageModel.UpdateFail,
					Exception = e
				};
			}
		}

		public ResultModel<int> GetAuditStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.AuditMasterRepository.GetAuditStatusDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<bool> CheckUnPostStatus(AuditMaster model)
		{

			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					bool CheckUpPostStatus = false;
					CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("A_Audits", new[] { "Id" }, new[] { model.Id.ToString() });
					if (CheckUpPostStatus)
					{
						return new ResultModel<bool>()
						{
							Status = Status.Fail,
							Message = MessageModel.PostAlready,

						};
					}

					return new ResultModel<bool>()
					{
						Status = Status.Fail,
						Message = MessageModel.PostSuccess,

					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<bool>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}

			
		}

		public ResultModel<List<AuditResponse>> AuditResponseGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					List<AuditResponse> records = context.Repositories.AuditMasterRepository.AuditResponseGetIndexData(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<AuditResponse>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<AuditResponse>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}
		}

		public ResultModel<MailSetting> SaveUrl(MailSetting model)
		{
	

			using IUnitOfWorkAdapter context = _unitOfWork.Create();
			try
			{
				if (model is null)
				{
					return new ResultModel<MailSetting>()
					{
						Status = Status.Warning,
						Message = MessageModel.NotFoundForSave,
					};
				}

				MailSetting master = context.Repositories.AuditMasterRepository.SaveUrl(model);

				if (master.Id <= 0)
				{
					return new ResultModel<MailSetting>()
					{
						Status = Status.Fail,
						Message = MessageModel.MasterInsertFailed,
						Data = master
					};
				}


				context.SaveChanges();


				return new ResultModel<MailSetting>()
				{
					Status = Status.Success,
					Message = MessageModel.InsertSuccess,
					Data = master
				};
			}
			catch (Exception e)
			{
				context.RollBack();

				return new ResultModel<MailSetting>()
				{
					Status = Status.Fail,
					Message = MessageModel.InsertFail,
					Exception = e
				};
			}
		}

		public ResultModel<List<UserProfile>> GetEamil(UserProfile Email)
		{
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<UserProfile> records = context.Repositories.AuditMasterRepository.GetEamil(Email);
                    context.SaveChanges();

                    return new ResultModel<List<UserProfile>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<UserProfile>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }

		}

		public ResultModel<List<MailSetting>> GetUrl(MailSetting Url)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					List<MailSetting> records = context.Repositories.AuditMasterRepository.GetUrl(Url);
					context.SaveChanges();

					return new ResultModel<List<MailSetting>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<MailSetting>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}
		}

        public ResultModel<List<AuditUser>> GetAuditUserAuditId(string AuditId)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditUser> records = context.Repositories.AuditMasterRepository.GetAuditUserByAuditId(AuditId);
                    context.SaveChanges();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<MailSetting>> GetUrlForIssue(MailSetting Url)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<MailSetting> records = context.Repositories.AuditMasterRepository.GetUrlForIssue(Url);
                    context.SaveChanges();

                    return new ResultModel<List<MailSetting>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<MailSetting>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<UserProfile>> GetEamilForIssue(UserProfile Url)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<UserProfile> records = context.Repositories.AuditMasterRepository.GetEamilForIssue(Url);
                    context.SaveChanges();

                    return new ResultModel<List<UserProfile>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<UserProfile>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<UserProfile>> GetEamilForBranchFeedback(UserProfile Url)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<UserProfile> records = context.Repositories.AuditMasterRepository.GetEamilForBranchFeedback(Url);
                    context.SaveChanges();

                    return new ResultModel<List<UserProfile>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<UserProfile>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<AuditMaster> IssueCompleted(AuditMaster model)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    AuditMaster records = context.Repositories.AuditMasterRepository.IssueCompleted(model);

                    context.SaveChanges();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Data = model
                    };
                }

            }
        }

        public ResultModel<AuditMaster> BranchFeedbackCompleteCompleted(AuditMaster model)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    AuditMaster records = context.Repositories.AuditMasterRepository.BranchFeedbackCompleteCompleted(model);

                    context.SaveChanges();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Data = model
                    };
                }

            }
        }

        public ResultModel<AuditMaster> FeedbackComplete(AuditMaster model)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    AuditMaster records = context.Repositories.AuditMasterRepository.FeedbackComplete(model);

                    context.SaveChanges();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Data = model
                    };
                }

            }
        }

        public ResultModel<List<AuditUser>> GetAuditUsers(int Id)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditUser> records = context.Repositories.AuditMasterRepository.GetAuditUsers(Id);
                    context.SaveChanges();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<AuditUser>> GetAuditIssueUsers(int Id)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditUser> records = context.Repositories.AuditMasterRepository.GetAuditIssueUsers(Id);
                    context.SaveChanges();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditUser>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<AuditMaster> UpdateHOD(AuditMaster model)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    AuditMaster records = context.Repositories.AuditMasterRepository.UpdateHOD(model);
                    context.SaveChanges();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<int> AuditResponseGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.AuditMasterRepository.AuditResponseGetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<AuditMaster>> GetAuditStatusData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditMaster> records = context.Repositories.AuditMasterRepository.GetAuditStatusData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<int> GetStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.AuditMasterRepository.GetStatusDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<AuditIssue>> GetReportData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditIssue> records = context.Repositories.AuditMasterRepository.GetReportData(conditionalFields, conditionalValue);
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

        public ResultModel<List<AuditBranchFeedback>> GetReportBranchFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditBranchFeedback> records = context.Repositories.AuditMasterRepository.GetReportBranchFeedbackData(conditionalFields, conditionalValue,vm);
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
        }

        public ResultModel<List<AuditFeedback>> GetReportAuditFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditFeedback> records = context.Repositories.AuditMasterRepository.GetReportAuditFeedbackData(conditionalFields, conditionalValue,vm);
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

		public ResultModel<AuditReportHeading> AuditReportHeadingInsert(AuditReportHeading model)
		{


			using IUnitOfWorkAdapter context = _unitOfWork.Create();
			try
			{



				AuditReportHeading master = context.Repositories.AuditMasterRepository.AuditReportHeadingInsert(model);

				if (master.Id <= 0)
				{
					return new ResultModel<AuditReportHeading>()
					{
						Status = Status.Fail,
						Message = MessageModel.MasterInsertFailed,
						Data = master
					};
				}


				context.SaveChanges();


				return new ResultModel<AuditReportHeading>()
				{
					Status = Status.Success,
					Message = MessageModel.InsertSuccess,
					Data = master
				};
			}
			catch (Exception e)
			{
				context.RollBack();

				return new ResultModel<AuditReportHeading>()
				{
					Status = Status.Fail,
					Message = MessageModel.InsertFail,
					Exception = e
				};
			}
		}

		public ResultModel<AuditReportHeading> AuditReportHeadingUpdate(AuditReportHeading model)
		{
			using IUnitOfWorkAdapter context = _unitOfWork.Create();
			try
			{

				AuditReportHeading master = context.Repositories.AuditMasterRepository.AuditReportHeadingUpdate(model);

				context.SaveChanges();


				return new ResultModel<AuditReportHeading>()
				{
					Status = Status.Success,
					Message = MessageModel.UpdateSuccess,
					Data = model
				};

			}
			catch (Exception e)
			{
				context.RollBack();

				return new ResultModel<AuditReportHeading>()
				{
					Status = Status.Fail,
					Message = MessageModel.UpdateFail,
					Exception = e
				};
			}
		}

		public ResultModel<List<AuditReportHeading>> GetReportHeadingData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					List<AuditReportHeading> records = context.Repositories.AuditMasterRepository.GetReportHeadingData(conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<AuditReportHeading>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<AuditReportHeading>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}
		}

        public ResultModel<List<AuditReportHeading>> GetReportHeadingById(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditReportHeading> records = context.Repositories.AuditMasterRepository.GetReportHeadingById(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditReportHeading>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditReportHeading>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<AuditBranchFeedback>> GetBranchFeedbackDeprtemnetFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditBranchFeedback> records = context.Repositories.AuditMasterRepository.GetBranchFeedbackDeprtemnetFollowUpData(conditionalFields, conditionalValue,vm);
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
        }

        public ResultModel<List<AuditBranchFeedback>> GetBranchFeedbackAuditResponseFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditBranchFeedback> records = context.Repositories.AuditMasterRepository.GetBranchFeedbackAuditResponseFollowUpData(conditionalFields, conditionalValue,vm);
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
        }

        public ResultModel<List<AuditApprove>> GetAuditById(int id, string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditApprove> records = context.Repositories.AuditMasterRepository.GetAuditById(id,UserName);
                    context.SaveChanges();

                    return new ResultModel<List<AuditApprove>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditApprove>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<UserRolls>> GetUserRoles(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<UserRolls> records = context.Repositories.AuditMasterRepository.GetUserRoles(UserName);
                    context.SaveChanges();

                    return new ResultModel<List<UserRolls>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<UserRolls>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<AuditIssueUser>> GetAuditIssueUserById(int AuditId)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditIssueUser> records = context.Repositories.AuditMasterRepository.GetAuditIssueUserById(AuditId);
                    context.SaveChanges();

                    return new ResultModel<List<AuditIssueUser>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditIssueUser>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public int GetTotoalIssuesById(int id, string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {
                try
                {
                    int issues = context.Repositories.AuditMasterRepository.GetTotoalIssuesById(id, UserName);
                    return issues;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

        }

        public ResultModel<AuditMaster> MultipleAuditApproval(AuditMaster model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    AuditMaster master = context.Repositories.AuditMasterRepository.MultipleAuditApproval(model);

                    context.SaveChanges();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UnPostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UnPostFail,
                        Data = model,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<AuditMaster> ReportDataUpdate(AuditMaster model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {

                AuditMaster master = context.Repositories.AuditMasterRepository.ReportDataUpdate(model);

                context.SaveChanges();


                return new ResultModel<AuditMaster>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditMaster>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<List<UserBranch>> GetUserIdbyUserName(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<UserBranch> records = context.Repositories.AuditMasterRepository.GetUserIdbyUserName(UserName);
                    context.SaveChanges();

                    return new ResultModel<List<UserBranch>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<UserBranch>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<UserBranch> UpdateBranchName(UserBranch user)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {

                UserBranch model = context.Repositories.AuditMasterRepository.UpdateBranchName(user);

                context.SaveChanges();

                return new ResultModel<UserBranch>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<UserBranch>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<HODdetails> GetHODdetails()
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {

                HODdetails model = context.Repositories.AuditMasterRepository.GetHODdetails();

                context.SaveChanges();

                return new ResultModel<HODdetails>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<HODdetails>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<DataTable> DetailsInformation(ReportModel model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    DataTable records = context.Repositories.AuditMasterRepository.DetailsInformation(model);

                    context.SaveChanges();

                    return new ResultModel<DataTable>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<DataTable>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }
    }
}
