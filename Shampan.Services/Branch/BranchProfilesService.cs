using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Branch
{
    public class BranchProfilesService : IBranchProfileService
    {
        private IUnitOfWork _unitOfWork;

        public BranchProfilesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

		public ResultModel<BranchProfile> BranchProfileUpdate(BranchProfile model)
		{
			using (var context = _unitOfWork.Create())
			{
				try
				{
					BranchProfile master = context.Repositories.BranchProfileRepository.BranchProfileUpdate(model);
					context.SaveChanges();

					return new ResultModel<BranchProfile>()
					{
						Status = Status.Success,
						Message = MessageModel.DeleteSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();
					return new ResultModel<BranchProfile>()
					{
						Status = Status.Fail,
						Message = MessageModel.UpdateFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<BranchProfile> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<BranchProfile>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BranchProfileRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BranchProfile>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BranchProfile>>()
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
                        context.Repositories.BranchProfileRepository.GetCount(tableName,
                            "BranchID", null, null);
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

        public ResultModel<List<BranchProfile>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BranchProfileRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BranchProfile>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BranchProfile>>()
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
                    var records = context.Repositories.BranchProfileRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<BranchProfile> Insert(BranchProfile model)
        {

            
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BranchProfile>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                        BranchProfile master = context.Repositories.BranchProfileRepository.Insert(model);

                        if (master.BranchID <= 0)
                        {
                            return new ResultModel<BranchProfile>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }


                        context.SaveChanges();

                        

                        return new ResultModel<BranchProfile>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                   



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BranchProfile>()
                    {
                        Status = Status.Fail,
                        Message = e.Message,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<BranchProfile> Update(BranchProfile model)
        {

            using (var context = _unitOfWork.Create())
            {

                try
                {
                    BranchProfile master = context.Repositories.BranchProfileRepository.Update(model);

                    context.SaveChanges();


                    return new ResultModel<BranchProfile>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BranchProfile>()
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
