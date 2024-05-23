using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Team
{
	public class TeamsService : ITeamsService
	{
		private IUnitOfWork _unitOfWork;

		public TeamsService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<Teams> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<Teams>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TeamsRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Teams>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Teams>>()
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
                        context.Repositories.TeamsRepository.GetCount(tableName,
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

		public ResultModel<List<Teams>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TeamsRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Teams>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Teams>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

		//change
		public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.TeamsRepository.GetSingleValeByID(null,ReturnFields,null,null);
					context.SaveChanges();
					return records;
				

				}
				catch (Exception e)
				{
					context.RollBack();

					return "";
				}

			}
		}


		public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TeamsRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<Teams> Insert(Teams model)
		{
			string CodeGroup = "TM";
			string CodeName = "Teams";

			using (var context = _unitOfWork.Create())
			{
				try
				{
					if (model == null)
					{
						return new ResultModel<Teams>()
						{
							Status = Status.Warning,
							Message = MessageModel.NotFoundForSave,
						};
					}
					

				    string Code = context.Repositories.TeamsRepository.CodeGeneration(CodeGroup, CodeName);
					//string Code = "TM";

					if (Code != "" || Code != null)
					{
						model.Code = Code;

						Teams master = context.Repositories.TeamsRepository.Insert(model);

						if (master.Id <= 0)
						{
							return new ResultModel<Teams>()
							{
								Status = Status.Fail,
								Message = MessageModel.MasterInsertFailed,
								Data = master
							};
						}
			

						context.SaveChanges();


						return new ResultModel<Teams>()
						{
							Status = Status.Success,
							Message = MessageModel.InsertSuccess,
							Data = master
						};

					}
					else
					{
						return new ResultModel<Teams>()
						{
							Status = Status.Fail,
							Message = MessageModel.DataLoadedFailed,

						};
					}



				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<Teams>()
					{
						Status = Status.Fail,
						Message = MessageModel.InsertFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<Teams> Update(Teams model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					Teams master = context.Repositories.TeamsRepository.Update(model);

					context.SaveChanges();


					return new ResultModel<Teams>()
					{
						Status = Status.Success,
						Message = MessageModel.UpdateSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<Teams>()
					{
						Status = Status.Fail,
						Message = MessageModel.UpdateFail,
						Exception = e
					};
				}
			}
		}

        public string GetAuditTypeeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TeamsRepository.GetAuditTypeeByID(null, ReturnFields, null, null);
                    context.SaveChanges();
                    return records;


                }
                catch (Exception e)
                {
                    context.RollBack();

                    return "";
                }

            }
        }

        public string GetBranchByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TeamsRepository.GetBranchByID(null, ReturnFields, null, null);
                    context.SaveChanges();
                    return records;


                }
                catch (Exception e)
                {
                    context.RollBack();

                    return "";
                }

            }
        }
    }
}
