using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Audit
{
    public class AuditIssueUserService : IAuditIssueUserService
    {

        private IUnitOfWork _unitOfWork;

        public AuditIssueUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ResultModel<List<AuditIssueUser>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var records = context.Repositories.AuditIssueUserRepository.GetAll(conditionalFields, conditionalValue);
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

        public ResultModel<List<AuditIssueUser>> GetIndexData(IndexModel index,
            string[] conditionalFields,
            string[] conditionalValue,
            PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditIssueUser> records = context.Repositories.AuditIssueUserRepository.GetIndexData(index, conditionalFields, conditionalValue);
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

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue,
            PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                int records = context.Repositories.AuditUserRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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
                    context.Repositories.AuditMasterRepository.GetCount(TableName.AuditUsers,
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

        public ResultModel<AuditIssueUser> Insert(AuditIssueUser model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<AuditIssueUser>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditIssueUser master = context.Repositories.AuditIssueUserRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditIssueUser>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                context.SaveChanges();


                return new ResultModel<AuditIssueUser>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditIssueUser>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditIssueUser> Update(AuditIssueUser model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                AuditIssueUser master = context.Repositories.AuditIssueUserRepository.Update(model);

                context.SaveChanges();

                return new ResultModel<AuditIssueUser>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditIssueUser>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditIssueUser> Delete(int id)
        {
			using IUnitOfWorkAdapter context = _unitOfWork.Create();
			try
			{


				string stringValue = id.ToString();

				int master = context.Repositories.AuditIssueUserRepository.Delete("AuditIssueUsers", new[] { "Id" }, new[] { stringValue });

				context.SaveChanges();


				return new ResultModel<AuditIssueUser>()
				{
					Status = Status.Success,
					Message = MessageModel.DeleteSuccess
					//Data = model
				};

			}
			catch (Exception e)
			{
				context.RollBack();

				return new ResultModel<AuditIssueUser>()
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

        public List<AuditIssueUser> GetAuditIssueUsersById(int id)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                string stringValue = id.ToString();
                List<AuditIssueUser> master = context.Repositories.AuditIssueUserRepository.GetAuditIssueUsersById(id);
                context.SaveChanges();
                return master;
               

            }
            catch (Exception e)
            {
                context.RollBack();
                return new List<AuditIssueUser>();
                
            }
        }

        public ResultModel<AuditReportUsers> AuditReportUsersInsert(AuditReportUsers model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<AuditReportUsers>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditReportUsers master = context.Repositories.AuditIssueUserRepository.AuditReportUsersInsert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditReportUsers>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                context.SaveChanges();


                return new ResultModel<AuditReportUsers>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditReportUsers>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditReportUsers> AuditReportUsersUpdate(AuditReportUsers model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                AuditReportUsers master = context.Repositories.AuditIssueUserRepository.AuditReportUsersUpdate(model);

                context.SaveChanges();

                return new ResultModel<AuditReportUsers>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditReportUsers>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditReportUsers>> AuditReportUsersGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditReportUsers> records = context.Repositories.AuditIssueUserRepository.AuditReportUsersGetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditReportUsers>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditReportUsers>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditReportUsers>> AuditReportUsersGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var records = context.Repositories.AuditIssueUserRepository.AuditReportUsersGetAll(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditReportUsers>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditReportUsers>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditReportUsers> ReportDelete(int id)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {


                string stringValue = id.ToString();

                int master = context.Repositories.AuditIssueUserRepository.Delete("AuditReportUsers", new[] { "Id" }, new[] { stringValue });

                context.SaveChanges();


                return new ResultModel<AuditReportUsers>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DeleteSuccess
                    //Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditReportUsers>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }
    }
}
