using Shampan.Core.Interfaces.Services.User;
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.User
{
    public class UserBranchService : IUserBranchService
    {

        private IUnitOfWork _unitOfWork;
        public UserBranchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<UserBranch> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<UserBranch>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.UserBranchRepository.GetAll(conditionalFields, conditionalValue);
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
    

        public ResultModel<List<UserBranch>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.UserBranchRepository.GetIndexData(index, conditionalFields, conditionalValue);
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

        public ResultModel<UserBranch> Insert(UserBranch model)
        {


            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<UserBranch>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    UserBranch master = context.Repositories.UserBranchRepository.Insert(model);

                    if (master.Id <= 0)
                    {
                        return new ResultModel<UserBranch>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }


                    context.SaveChanges();



                    return new ResultModel<UserBranch>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };





                }
                catch (Exception ex)
                {
                    context.RollBack();

                    return new ResultModel<UserBranch>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = ex
                    };
                }
            }
        }
    

        public ResultModel<UserBranch> Update(UserBranch model)
        {

            using (var context = _unitOfWork.Create())
            {

                try
                {
                    UserBranch master = context.Repositories.UserBranchRepository.Update(model);

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
        }

    }
}
