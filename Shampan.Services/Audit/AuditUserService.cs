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
    public class AuditUserService : IAuditUserService
    {

        private IUnitOfWork _unitOfWork;

        public AuditUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ResultModel<List<AuditUser>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var records = context.Repositories.AuditUserRepository.GetAll(conditionalFields, conditionalValue);
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

        public ResultModel<List<AuditUser>> GetIndexData(IndexModel index,
            string[] conditionalFields,
            string[] conditionalValue,
            PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditUser> records = context.Repositories.AuditUserRepository.GetIndexData(index, conditionalFields, conditionalValue);
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

        public ResultModel<AuditUser> Insert(AuditUser model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {


				






				if (model is null)
                {
                    return new ResultModel<AuditUser>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }

                AuditUser master = context.Repositories.AuditUserRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditUser>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                context.SaveChanges();


                return new ResultModel<AuditUser>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditUser>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditUser> Update(AuditUser model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                AuditUser master = context.Repositories.AuditUserRepository.Update(model);

                context.SaveChanges();

                return new ResultModel<AuditUser>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditUser>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditUser> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }
    }
}
