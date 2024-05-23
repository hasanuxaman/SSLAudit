using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Audit
{
    public class AuditAreasService : IAuditAreasService
    {
        private IUnitOfWork _unitOfWork;

        public AuditAreasService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ResultModel<List<AuditAreas>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using var context = _unitOfWork.Create();
            try
            {
                var records = context.Repositories.AuditAreasRepository.GetAll(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditAreas>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditAreas>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<List<AuditAreas>> GetIndexData(IndexModel index,
            string[] conditionalFields,
            string[] conditionalValue,
            PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List<AuditAreas> records = context.Repositories.AuditAreasRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<AuditAreas>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<AuditAreas>>()
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
                int records = context.Repositories.AuditAreasRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<AuditAreas> Insert(AuditAreas model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                if (model is null)
                {
                    return new ResultModel<AuditAreas>()
                    {
                        Status = Status.Warning,
                        Message = MessageModel.NotFoundForSave,
                    };
                }
         



                AuditAreas master = context.Repositories.AuditAreasRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<AuditAreas>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                context.SaveChanges();


                return new ResultModel<AuditAreas>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditAreas>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditAreas> Update(AuditAreas model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                AuditAreas master = context.Repositories.AuditAreasRepository.Update(model);

                context.SaveChanges();

                return new ResultModel<AuditAreas>()
                {
                    Status = Status.Success,
                    Message = MessageModel.UpdateSuccess,
                    Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<AuditAreas>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<AuditAreas> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }
    }
}
