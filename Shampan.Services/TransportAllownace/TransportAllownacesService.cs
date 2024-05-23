using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.TransportAllownace;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.TransportAllownace
{
	public class TransportAllownacesService : ITransportAllownacesService
	{
		private IUnitOfWork _unitOfWork;

		public TransportAllownacesService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<TransportAllownaces> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<TransportAllownaces>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TransportAllownacesRepository.GetAll(conditionalFields, conditionalValue);

                    //ForOthers
                    conditionalFields[0] = "TransportAllowanceId";
                    List<TransportFoodAndOthers> FoodAndOthers = context.Repositories.TransportAllownaceDetailRepository.GetAllFoodAndOthers(conditionalFields, conditionalValue);
                    //var food = FoodAndOthers.FirstOrDefault();
                    foreach(var item in FoodAndOthers)
                    {
                        records.FirstOrDefault().TransportFoodAndOthersDetails.Add(item);

                    }


                    //ForLessAdvance

                    conditionalFields[0] = "TransportAllowanceId";
                    List<LessAdvance> lessAdvances = context.Repositories.TransportAllownaceDetailRepository.GetAllLessAdvance(conditionalFields, conditionalValue);                
                    foreach (var item in lessAdvances)
                    {
                        records.FirstOrDefault().LessAdvanceDetails.Add(item);

                    }

                    //EndLessAdvance



                    context.SaveChanges();

                    
                    
                    return new ResultModel<List<TransportAllownaces>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<TransportAllownaces>>()
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
                        context.Repositories.TransportAllownacesRepository.GetCount(tableName,
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

		public ResultModel<List<TransportAllownaces>> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{
					var records = context.Repositories.TransportAllownacesRepository.GetIndexDataStatus(index, conditionalFields, conditionalValue);
					context.SaveChanges();

					return new ResultModel<List<TransportAllownaces>>()
					{
						Status = Status.Success,
						Message = MessageModel.DataLoaded,
						Data = records
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<List<TransportAllownaces>>()
					{
						Status = Status.Fail,
						Message = MessageModel.DataLoadedFailed,
						Exception = e
					};
				}

			}


		}


        public ResultModel<List<TransportAllownaces>> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TransportAllownacesRepository.GetIndexDataStatus(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<TransportAllownaces>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<TransportAllownaces>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }


        }


        public ResultModel<List<TransportAllownaces>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TransportAllownacesRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<TransportAllownaces>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<TransportAllownaces>>()
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
                    var records = context.Repositories.TransportAllownacesRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<TransportAllownaces> Insert(TransportAllownaces model)
		{
            string CodeGroup = "TA";
            string CodeName = "TransportAllownaces";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<TransportAllownaces>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.TransportAllownacesRepository.CodeGeneration(CodeGroup, CodeName);


                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

                        TransportAllownaces master = context.Repositories.TransportAllownacesRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<TransportAllownaces>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }




                        //TransportAllowanceDetails
                        foreach (var detail in model.TransportAllownaceDetails)
                        {
                            detail.TransportAllowanceId = master.Id;
                            //detail.BranchId = 1;
                            //detail.CompanyId = 1;

                            TransportAllownaceDetail returnDetail =context.Repositories.TransportAllownaceDetailRepository.Insert(detail);

                            if (returnDetail.Id == 0)
                            {
                                return new ResultModel<TransportAllownaces>()
                                {
                                    Status = Status.Fail,
                                    Message = MessageModel.DetailInsertFailed,
                                    Data = master
                                };
                            }
                        }

                        //FoodAndOthers
                        foreach (var detail in model.TransportFoodAndOthersDetails)
                        {
                            detail.TransportAllowanceId = master.Id;

                            TransportFoodAndOthers returnDetail = context.Repositories.TransportAllownaceDetailRepository.InsertFoodAndOthers(detail);

                            if (returnDetail.Id == 0)
                            {
                                return new ResultModel<TransportAllownaces>()
                                {
                                    Status = Status.Fail,
                                    Message = MessageModel.DetailInsertFailed,
                                    Data = master
                                };
                            }
                        }

                        //LessAdvance
                        foreach (var detail in model.LessAdvanceDetails)
                        {
                            detail.TransportAllowanceId = master.Id;

                            LessAdvance returnDetail = context.Repositories.TransportAllownaceDetailRepository.InsertLessAdvance(detail);

                            if (returnDetail.Id == 0)
                            {
                                return new ResultModel<TransportAllownaces>()
                                {
                                    Status = Status.Fail,
                                    Message = MessageModel.DetailInsertFailed,
                                    Data = master
                                };
                            }
                        }



                        context.SaveChanges();
                        master.TransportAllownaceDetails = model.TransportAllownaceDetails;
              



                        return new ResultModel<TransportAllownaces>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };


                        //context.SaveChanges();
                        //return new ResultModel<TransportAllownaces>()
                        //{
                        //    Status = Status.Success,
                        //    Message = MessageModel.InsertSuccess,
                        //    Data = master
                        //};

                    }
                    else
                    {
                        return new ResultModel<TransportAllownaces>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<TransportAllownaces>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          

        }

		public ResultModel<TransportAllownaces> MultiplePost(TransportAllownaces model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					bool CheckPostStatus = false;
					CheckPostStatus = context.Repositories.ToursRepository.CheckPostStatus("TransportAllownaces", new[] { "Id" }, new[] { model.Id.ToString() });
					if (CheckPostStatus)
					{
						return new ResultModel<TransportAllownaces>()
						{
							Status = Status.Fail,
							Message = MessageModel.PostAlready,

						};
					}

					TransportAllownaces master = context.Repositories.TransportAllownacesRepository.MultiplePost(model);



					context.SaveChanges();


					return new ResultModel<TransportAllownaces>()
					{
						Status = Status.Success,
						Message = MessageModel.PostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<TransportAllownaces>()
					{
						Status = Status.Fail,
						Message = MessageModel.UpdateFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<TransportAllownaces> MultipleUnPost(TransportAllownaces model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{


					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("TransportAllownaces", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<TransportAllownaces>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}



					TransportAllownaces master = context.Repositories.TransportAllownacesRepository.MultipleUnPost(model);



					context.SaveChanges();


					return new ResultModel<TransportAllownaces>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<TransportAllownaces>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<TransportAllownaces> Update(TransportAllownaces model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					bool CheckPostStatus = false;
					CheckPostStatus = context.Repositories.TransportAllownacesRepository.CheckPostStatus("TransportAllownaces", new[] { "Id" }, new[] { model.Id.ToString() });
					if (CheckPostStatus)
					{
						return new ResultModel<TransportAllownaces>()
						{
							Status = Status.Fail,
							Message = MessageModel.PostAlready,

						};
					}


					TransportAllownaces master = context.Repositories.TransportAllownacesRepository.Update(model);

                    //Details
                    if (master.Id > 0)
                    {


                        int recordCount = context.Repositories.TransportAllownaceDetailRepository.DetailsDelete(
                            TableName.TransportAllownaceDetails, new[] { "TransportAllowanceId" },new[] { master.Id.ToString() });


                        if (recordCount < 0)
                        {
                            return new ResultModel<TransportAllownaces>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.DeleteFail,
                            };
                        }

                        foreach (TransportAllownaceDetail detail in master.TransportAllownaceDetails)
                        {
                            detail.TransportAllowanceId = master.Id;
                            //detail.BranchId = 1;
                            //detail.CompanyId = 1;
                            //detail.Audit.LastUpdateBy = master.Audit.LastUpdateBy;
                            //detail.Audit.LastUpdateOn = master.Audit.LastUpdateOn;

                            var returnDetail = context.Repositories.TransportAllownaceDetailRepository.Insert(detail);

                            if (returnDetail.Id == 0)
                            {
                                return new ResultModel<TransportAllownaces>()
                                {
                                    Status = Status.Fail,
                                    Message = MessageModel.DetailInsertFailed,
                                    Data = master
                                };
                            }
                        }

                    }

                    //FoodAndOthers
                    if (master.Id > 0)
                    {


                        int recordCount = context.Repositories.TransportAllownaceDetailRepository.DetailsDelete(
                            TableName.TransportAllownaceOthers, new[] { "TransportAllowanceId" }, new[] { master.Id.ToString() });


                        if (recordCount < 0)
                        {
                            return new ResultModel<TransportAllownaces>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.DeleteFail,
                            };
                        }

                        foreach (TransportFoodAndOthers detailOthers in master.TransportFoodAndOthersDetails)
                        {
                            detailOthers.TransportAllowanceId = master.Id;
                            

                            var returnDetail = context.Repositories.TransportAllownaceDetailRepository.InsertFoodAndOthers(detailOthers);

                            if (returnDetail.Id == 0)
                            {
                                return new ResultModel<TransportAllownaces>()
                                {
                                    Status = Status.Fail,
                                    Message = MessageModel.DetailInsertFailed,
                                    Data = master
                                };
                            }
                        }

                    }

                    //LessAdvance

                    if (master.Id > 0)
                    {


                        int recordCount = context.Repositories.TransportAllownaceDetailRepository.DetailsDelete(
                            TableName.TransportAllownaceLessAdvance, new[] { "TransportAllowanceId" }, new[] { master.Id.ToString() });


                        if (recordCount < 0)
                        {
                            return new ResultModel<TransportAllownaces>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.DeleteFail,
                            };
                        }

                        foreach (LessAdvance detailOthers in master.LessAdvanceDetails)
                        {
                            detailOthers.TransportAllowanceId = master.Id;


                            var returnDetail = context.Repositories.TransportAllownaceDetailRepository.InsertLessAdvance(detailOthers);

                            if (returnDetail.Id == 0)
                            {
                                return new ResultModel<TransportAllownaces>()
                                {
                                    Status = Status.Fail,
                                    Message = MessageModel.DetailInsertFailed,
                                    Data = master
                                };
                            }
                        }

                    }





                    context.SaveChanges();
                    return new ResultModel<TransportAllownaces>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<TransportAllownaces>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
           // throw new NotImplementedException();

        }

        public ResultModel<List<TransportAllownaces>> GetTADAForReports(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.TransportAllownacesRepository.GetTADAForReports(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<TransportAllownaces>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<TransportAllownaces>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }
    }
}
