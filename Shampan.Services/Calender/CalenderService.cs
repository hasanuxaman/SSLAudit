using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Calender;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Calender
{
	public class CalenderService : ICalendersService
	{
		private IUnitOfWork _unitOfWork;

		public CalenderService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<Calenders> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<Calenders>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.CalendersRepository.GetAll(conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<Calenders>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<Calenders>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}


		}

		public ResultModel<List<Calenders>> GetCalenderData()
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.CalendersRepository.GetCalenderData();
					context.SaveChanges();

					return new ResultModel<List<Calenders>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<Calenders>>()
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
						context.Repositories.CalendersRepository.GetCount(tableName,
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

		public ResultModel<List<Calenders>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.CalendersRepository.GetIndexData(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<Calenders>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<Calenders>>()
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
					var records = context.Repositories.CalendersRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<Calenders> Insert(Calenders model)
		{

			string CodeGroup = "CD";
			string CodeName = "Calenders";

			using (var context = _unitOfWork.Create())
			{
				try
				{
					if (model == null)
					{
						return new ResultModel<Calenders>()
						{
							Status = Status.Warning,
							Message = MessageModel.NotFoundForSave,
						};
					}


					string Code = context.Repositories.CalendersRepository.CodeGeneration(CodeGroup, CodeName);


					if (Code != "" || Code != null)
					{
						model.Code = Code;

						Calenders master = context.Repositories.CalendersRepository.Insert(model);

						if (master.Id <= 0)
						{
							return new ResultModel<Calenders>()
							{
								Status = Status.Fail,
								Message = MessageModel.MasterInsertFailed,
								Data = master
							};
						}


						context.SaveChanges();


						return new ResultModel<Calenders>()
						{
							Status = Status.Success,
							Message = MessageModel.InsertSuccess,
							Data = master
						};

					}
					else
					{
						return new ResultModel<Calenders>()
						{
							Status = Status.Fail,
							Message = MessageModel.DataLoadedFailed,

						};
					}



				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<Calenders>()
					{
						Status = Status.Fail,
						Message = MessageModel.InsertFail,
						Exception = e
					};
				}
			}

		}

		

		public ResultModel<Calenders> Update(Calenders model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {


                    Calenders master = context.Repositories.CalendersRepository.Update(model);

                    context.SaveChanges();


                    return new ResultModel<Calenders>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Calenders>()
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
