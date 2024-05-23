using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Node;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Node
{
	public class NodeService : INodeService
	{
		private IUnitOfWork _unitOfWork;

		public NodeService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<SubmanuList> Delete(int id)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {


                string stringValue = id.ToString();

                int master = context.Repositories.NodeRepository.Delete("NodePermission", new[] { "Id" }, new[] { stringValue });

                context.SaveChanges();


                return new ResultModel<SubmanuList>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DeleteSuccess
                    //Data = model
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<SubmanuList>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.UpdateFail,
                    Exception = e
                };
            }
        }

        public ResultModel<List<SubmanuList>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.NodeRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<SubmanuList>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<SubmanuList>>()
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
                        context.Repositories.NodeRepository.GetCount(tableName,
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

        public ResultModel<List<SubmanuList>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.NodeRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<SubmanuList>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<SubmanuList>>()
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
                    var records = context.Repositories.NodeRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public SubmanuList GetNodeById(string id)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    SubmanuList item = context.Repositories.NodeRepository.GetNodeById(id);
                    //context.SaveChanges();

                    //return new ResultModel<List<SubmanuList>>()
                    //{
                    //    Status = Status.Success,
                    //    Message = MessageModel.DataLoaded,
                    //    Data = records
                    //};
                    return item;

                }
                catch (Exception e)
                {
                    context.RollBack();
                    return null;

                    //return new ResultModel<List<SubmanuList>>()
                    //{
                    //    Status = Status.Fail,
                    //    Message = MessageModel.DataLoadedFailed,
                    //    Exception = e
                    //};
                }

            }
        }

        public ResultModel<SubmanuList> Insert(SubmanuList model)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {


                SubmanuList master = context.Repositories.NodeRepository.Insert(model);

                if (master.Id <= 0)
                {
                    return new ResultModel<SubmanuList>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.MasterInsertFailed,
                        Data = master
                    };
                }


                context.SaveChanges();


                return new ResultModel<SubmanuList>()
                {
                    Status = Status.Success,
                    Message = MessageModel.InsertSuccess,
                    Data = master
                };
            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<SubmanuList>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.InsertFail,
                    Exception = e
                };
            }
        }

        public ResultModel<SubmanuList> Update(SubmanuList model)
        {
            using (var context = _unitOfWork.Create())
            {
             
                try
                {


                    SubmanuList master = context.Repositories.NodeRepository.Update(model);

                    context.SaveChanges();


                    return new ResultModel<SubmanuList>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<SubmanuList>()
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
