using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.User;
using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Deshboard
{
    public class DeshboardService : IDeshboardService
    {

        private IUnitOfWork _unitOfWork;
        public DeshboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public bool AuditBranchUserGetAll(string UserName)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    var records = context.Repositories.DeshboardRepository.AuditBranchUserGetAll();

                    foreach(var item in records)
                    {
                        if(UserName == item.UserName)
                        {
                            context.SaveChanges();
                            return true;                        
                        }
                    }

                }
                catch (Exception e)
                {
                    context.RollBack();
                    
                }

                return false;
            }
        }

        public int BeforeDeadLineIssue(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.BeforeDeadLineIssue(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

        public int DeadLineForResponse(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.DeadLineForResponse(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

        public ResultModel<AuditMaster> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int FinalAuidtApproved(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.FinalAuidtApproved(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

		public int FollowUpAuditIssues(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.DeshboardRepository.FollowUpAuditIssues(UserName);
					context.SaveChanges();

					return records;



				}
				catch (Exception e)
				{
					context.RollBack();

					throw e;

				}

			}
		}

		public ResultModel<List<AuditMaster>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            //using (var context = _unitOfWork.Create())
            //{

            //    try
            //    {
            //        var records = context.Repositories.UserBranchRepository.GetAll(conditionalFields, conditionalValue);
            //        context.SaveChanges();

            //        return new ResultModel<List<AuditMaster>>()
            //        {
            //            Status = Status.Success,
            //            Message = MessageModel.DataLoaded,
            //            Data = records
            //        };

            //    }
            //    catch (Exception e)
            //    {
            //        context.RollBack();

            //        return new ResultModel<List<AuditMaster>>()
            //        {
            //            Status = Status.Fail,
            //            Message = MessageModel.DataLoadedFailed,
            //            Exception = e
            //        };
            //    }

            //}
            throw new NotImplementedException();
        }

        public UserBranch GetBranchName(string UserId)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {
                try
                {
                    var records = context.Repositories.DeshboardRepository.GetBranchName(UserId);
                    context.SaveChanges();
                    return records;

                }
                catch (Exception e)
                {
                    context.RollBack();
                    throw e;

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
                        context.Repositories.UserBranchRepository.GetCount(tableName,
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
    

        public ResultModel<List<AuditMaster>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            //using (var context = _unitOfWork.Create())
            //{

            //    try
            //    {
            //        var records = context.Repositories.UserBranchRepository.GetIndexData(index, conditionalFields, conditionalValue);
            //        context.SaveChanges();

            //        return new ResultModel<List<AuditMaster>>()
            //        {
            //            Status = Status.Success,
            //            Message = MessageModel.DataLoaded,
            //            Data = records
            //        };

            //    }
            //    catch (Exception e)
            //    {
            //        context.RollBack();

            //        return new ResultModel<List<AuditMaster>>()
            //        {
            //            Status = Status.Fail,
            //            Message = MessageModel.DataLoadedFailed,
            //            Exception = e
            //        };
            //    }

            //}
            throw new NotImplementedException();


        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {

            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.UserBranchRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public PrePaymentMaster GetPrePayment()
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {
                try
                {
                    var records = context.Repositories.DeshboardRepository.GetPrePayment();
                    context.SaveChanges();
                    return records;

                }
                catch (Exception e)
                {
                    context.RollBack();
                    throw e;

                }
            }
        }

        public List<AuditComponent> GetUnPlanAuditComponents()
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditComponent> records = context.Repositories.DeshboardRepository.GetUnPlanAuditComponents();
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

        public ResultModel<AuditMaster> Insert(AuditMaster model)
        {


            //using (var context = _unitOfWork.Create())
            //{
            //    try
            //    {
            //        if (model == null)
            //        {
            //            return new ResultModel<AuditMaster>()
            //            {
            //                Status = Status.Warning,
            //                Message = MessageModel.NotFoundForSave,
            //            };
            //        }

            //        AuditMaster master = context.Repositories.UserBranchRepository.Insert(model);

            //        if (master.Id <= 0)
            //        {
            //            return new ResultModel<AuditMaster>()
            //            {
            //                Status = Status.Fail,
            //                Message = MessageModel.MasterInsertFailed,
            //                Data = master
            //            };
            //        }


            //        context.SaveChanges();



            //        return new ResultModel<AuditMaster>()
            //        {
            //            Status = Status.Success,
            //            Message = MessageModel.InsertSuccess,
            //            Data = master
            //        };





            //    }
            //    catch (Exception ex)
            //    {
            //        context.RollBack();

            //        return new ResultModel<AuditMaster>()
            //        {
            //            Status = Status.Fail,
            //            Message = MessageModel.InsertFail,
            //            Exception = ex
            //        };
            //    }
            //}

            throw new NotImplementedException();
        }

		public int MissDeadLineIssues(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.DeshboardRepository.MissDeadLineIssues(UserName);
					context.SaveChanges();

					return records;



				}
				catch (Exception e)
				{
					context.RollBack();

					throw e;

				}

			}
		}

		public ResultModel<List<PrePaymentMaster>> NonFinancialGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.DeshboardRepository.NonFinancialGetAll(conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<PrePaymentMaster>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<PrePaymentMaster>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}
		}

		public int NonFinancialGetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					int count =
						context.Repositories.DeshboardRepository.GetCount(tableName,
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

		public ResultModel<List<PrePaymentMaster>> NonFinancialGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.DeshboardRepository.NonFinancialGetIndexData(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<PrePaymentMaster>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<PrePaymentMaster>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}
		}

		public ResultModel<int> NonFinancialGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.DeshboardRepository.NonFinancialGetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<PrePaymentMaster> NonFinancialInsert(PrePaymentMaster model)
		{
			string CodeGroup = "NF";
			string CodeName = "NonFinancial";

			using (var context = _unitOfWork.Create())
			{
				try
				{


					string Code = context.Repositories.DeshboardRepository.CodeGeneration(CodeGroup, CodeName);

					if (Code != "" || Code != null)
					{
						model.Code = Code;

						PrePaymentMaster master = context.Repositories.DeshboardRepository.NonFinancialInsert(model);

						if (master.Id <= 0)
						{
							return new ResultModel<PrePaymentMaster>()
							{
								Status = Status.Fail,
								Message = MessageModel.MasterInsertFailed,
								Data = master
							};
						}

						context.SaveChanges();



						return new ResultModel<PrePaymentMaster>()
						{
							Status = Status.Success,
							Message = MessageModel.InsertSuccess,
							Data = master

						};


					}


					return new ResultModel<PrePaymentMaster>()
					{
						Status = Status.Fail,
						Message = MessageModel.MasterInsertFailed,
						//Data = master

					};


				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<PrePaymentMaster>()
					{
						Status = Status.Fail,
						Message = MessageModel.InsertFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<PrePaymentMaster> NonFinancialUpdate(PrePaymentMaster model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					PrePaymentMaster master = context.Repositories.DeshboardRepository.NonFinancialUpdate(model);

					context.SaveChanges();


					return new ResultModel<PrePaymentMaster>()
					{
						Status = Status.Success,
						Message = MessageModel.UpdateSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<PrePaymentMaster>()
					{
						Status = Status.Fail,
						Message = MessageModel.UpdateFail,
						Exception = e
					};
				}
			}
		}

		public int PendingAuditApproval(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.PendingAuditApproval(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

		public int PendingAuditResponse(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.DeshboardRepository.PendingAuditResponse(UserName);
					context.SaveChanges();

					return records;



				}
				catch (Exception e)
				{
					context.RollBack();

					throw e;

				}

			}
		}

		public int PendingForApproval(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.DeshboardRepository.PendingForApproval(UserName);
					context.SaveChanges();

					return records;



				}
				catch (Exception e)
				{
					context.RollBack();

					throw e;

				}

			}
		}

		public int PendingForAuditFeedback(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.DeshboardRepository.PendingForAuditFeedback(UserName);
					context.SaveChanges();
					return records;

				}
				catch (Exception e)
				{
					context.RollBack();
					throw e;

				}

			}
		}

        public int PendingForIssueApproval(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {
                try
                {
                    int records = context.Repositories.DeshboardRepository.PendingForIssueApproval(UserName);
                    context.SaveChanges();
                    return records;
                }
                catch (Exception e)
                {
                    context.RollBack();
                    throw e;
                }
            }
        }


        public int PendingForReviewerFeedback(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {
                try
                {
                    int records = context.Repositories.DeshboardRepository.PendingForReviewerFeedback(UserName);
                    context.SaveChanges();
                    return records;
                }
                catch (Exception e)
                {
                    context.RollBack();
                    throw e;
                }
            }
        }

        public ResultModel<List<PrePaymentMaster>> PrePaymentGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
        
                    var records = context.Repositories.DeshboardRepository.PrePaymentGetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<PrePaymentMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<PrePaymentMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public int PrePaymentGetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    int count =
                        context.Repositories.DeshboardRepository.GetCount(tableName,
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

        public ResultModel<List<PrePaymentMaster>> PrePaymentGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.DeshboardRepository.PrePaymentGetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<PrePaymentMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<PrePaymentMaster>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<int> PrePaymentGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.DeshboardRepository.PrePaymentGetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<PrePaymentMaster> PrePaymentInsert(PrePaymentMaster model)
		{
			string CodeGroup = "PP";
			string CodeName = "PrePayment";

			using (var context = _unitOfWork.Create())
			{
				try
				{
					string Code = context.Repositories.DeshboardRepository.CodeGeneration(CodeGroup, CodeName);
                    PrePaymentMaster master = null;

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

                        master = context.Repositories.DeshboardRepository.PrePaymentInsert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<PrePaymentMaster>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();

                        return new ResultModel<PrePaymentMaster>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master

                        };


                    }

                    return new ResultModel<PrePaymentMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };


                }
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<PrePaymentMaster>()
					{
						Status = Status.Fail,
						Message = MessageModel.InsertFail,
						Exception = e
					};
				}
			}

		}

        public PrepaymentReview PrepaymentReview()
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {
                try
                {
                    var records = context.Repositories.DeshboardRepository.PrepaymentReview();
                    context.SaveChanges();
                    return records;

                }
                catch (Exception e)
                {
                    context.RollBack();
                    throw e;

                }
            }
        }

        public ResultModel<PrepaymentReview> PrepaymentReviewInsert(PrepaymentReview model)
        {

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<PrepaymentReview>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    PrepaymentReview master = context.Repositories.DeshboardRepository.PrepaymentReviewInsert(model);

                    if (master.Id <= 0)
                    {
                        return new ResultModel<PrepaymentReview>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();

                    return new ResultModel<PrepaymentReview>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };

                }
                catch (Exception ex)
                {
                    context.RollBack();

                    return new ResultModel<PrepaymentReview>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = ex
                    };
                }
            }
        }

        public ResultModel<PrePaymentMaster> PrePaymentUpdate(PrePaymentMaster model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					PrePaymentMaster master = context.Repositories.DeshboardRepository.PrePaymentUpdate(model);
					context.SaveChanges();
					return new ResultModel<PrePaymentMaster>()
					{
						Status = Status.Success,
						Message = MessageModel.UpdateSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();
					return new ResultModel<PrePaymentMaster>()
					{
						Status = Status.Fail,
						Message = MessageModel.UpdateFail,
						Exception = e
					};
				}
			}
		}

        public ResultModel<int> TotalAdditionalPaymentCount()
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.DeshboardRepository.TotalAdditionalPaymentCount();
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

        public int TotalAuditApproved(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.TotalAuditApproved(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

        public int TotalAuditRejected(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.TotalAuditRejected(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

        public List<AuditReports> TotalCompletedOngoingRemaing(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    List<AuditReports> records = context.Repositories.DeshboardRepository.TotalCompletedOngoingRemaing(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

		public int TotalFollowUpAudit(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.DeshboardRepository.TotalFollowUpAudit(UserName);
					context.SaveChanges();

					return records;



				}
				catch (Exception e)
				{
					context.RollBack();

					throw e;

				}

			}
		}

		public int TotalIssueRejected(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.TotalIssueRejected(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

		public int TotalPendingIssueReview(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					int records = context.Repositories.DeshboardRepository.TotalPendingIssueReview(UserName);
					context.SaveChanges();

					return records;



				}
				catch (Exception e)
				{
					context.RollBack();

					throw e;

				}

			}
		}

		public int TotalRisk(string UserName)
        {
            using (IUnitOfWorkAdapter context = _unitOfWork.Create())
            {

                try
                {
                    int records = context.Repositories.DeshboardRepository.TotalRisk(UserName);
                    context.SaveChanges();

                    return records;



                }
                catch (Exception e)
                {
                    context.RollBack();

                    throw e;

                }

            }
        }

		public List<AuditReports> UnPlan(string UserName)
		{
			using (IUnitOfWorkAdapter context = _unitOfWork.Create())
			{

				try
				{
					List<AuditReports> records = context.Repositories.DeshboardRepository.UnPlan(UserName);
					context.SaveChanges();

					return records;



				}
				catch (Exception e)
				{
					context.RollBack();

					throw e;

				}

			}
		}

		public ResultModel<AuditMaster> Update(AuditMaster model)
        {

            //using (var context = _unitOfWork.Create())
            //{

            //    try
            //    {
            //        AuditMaster master = context.Repositories.UserBranchRepository.Update(model);

            //        context.SaveChanges();


            //        return new ResultModel<AuditMaster>()
            //        {
            //            Status = Status.Success,
            //            Message = MessageModel.UpdateSuccess,
            //            Data = model
            //        };

            //    }
            //    catch (Exception e)
            //    {
            //        context.RollBack();

            //        return new ResultModel<AuditMaster>()
            //        {
            //            Status = Status.Fail,
            //            Message = MessageModel.UpdateFail,
            //            Exception = e
            //        };
            //    }
            //}
            throw new NotImplementedException();
        }

    }
}
