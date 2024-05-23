using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.CompanyInfo
{
    public class CompanyInfoService: ICompanyInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultModel<List<Models.CompanyInfo>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.CreateAuth())
            {

                try
                {
                    List<Models.CompanyInfo> companyInfos =
                        context.Repositories.CompanyInfoRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Models.CompanyInfo>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = companyInfos
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Models.CompanyInfo>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }


            }
        }

        public ResultModel<List<Models.CompanyInfo>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<Models.CompanyInfo> Insert(Models.CompanyInfo model)
        {
            throw new NotImplementedException();
        }

        public ResultModel<Models.CompanyInfo> Update(Models.CompanyInfo model)
        {
            throw new NotImplementedException();
        }

        public ResultModel<Models.CompanyInfo> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<BranchProfile>> GetBranches(string[] conditionalFields, string[] conditionalValue)
        {
            using var context = _unitOfWork.Create();
            try
            {
                List<BranchProfile> branchProfiles =
                    context.Repositories.CompanyInfoRepository.GetBranches(conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<BranchProfile>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = branchProfiles
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
}
