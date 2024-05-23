using Shampan.Core.Interfaces.Services.Company;
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Company
{
    public class CompanyInfosService : ICompanyInfosService
    {
        private IUnitOfWork _unitOfWork;

        public CompanyInfosService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<Companyinfos> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<Companyinfos>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {

            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.CompanyInfosRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Companyinfos>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Companyinfos>>()
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
                        context.Repositories.CompanyInfosRepository.GetCount(tableName,
                            "CompanyID", null, null);
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

        public ResultModel<List<Companyinfos>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.CompanyInfosRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Companyinfos>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Companyinfos>>()
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
                    var records = context.Repositories.CompanyInfosRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<Companyinfos> Insert(Companyinfos model)
        {


            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<Companyinfos>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    Companyinfos master = context.Repositories.CompanyInfosRepository.Insert(model);

                    if (master.CompanyID <= 0)
                    {
                        return new ResultModel<Companyinfos>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }


                    context.SaveChanges();



                    return new ResultModel<Companyinfos>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };





                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Companyinfos>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }


        public ResultModel<Companyinfos> Update(Companyinfos model)
        {


            using (var context = _unitOfWork.Create())
            {

                try
                {
                    Companyinfos master = context.Repositories.CompanyInfosRepository.Update(model);

                    context.SaveChanges();


                    return new ResultModel<Companyinfos>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Companyinfos>()
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
    
