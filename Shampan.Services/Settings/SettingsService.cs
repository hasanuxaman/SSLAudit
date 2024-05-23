using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private IUnitOfWork _unitOfWork;

        public SettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }
        public ResultModel<DbUpdateModel> DbUpdate(DbUpdateModel model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    #region Table Add

                    #region TestHeader
                    String sqlText = " ";
                    sqlText = @"
    CREATE TABLE [dbo].[TestHeader](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](50) NULL,
	[GLAccount] [nvarchar](50) NULL,
	[TransDate] [date] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[PostedBy] [nvarchar](50) NULL,
	[PostedOn] [datetime] NULL,
	[PostedFrom] [nvarchar](50) NULL,
	[PushedBy] [nvarchar](50) NULL,
	[PushedOn] [datetime] NULL,
	[PushedFrom] [nvarchar](50) NULL,
	[BranchId] [int] NULL,
	[CompanyId] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_TestHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";
                    context.Repositories.SettingsRepository.NewTableAdd("TestHeader", sqlText);

                    #endregion

                    #region TestDetails
                    sqlText = " ";

                    sqlText = @"
CREATE TABLE [dbo].[TestDetails](
	[Id] [int] NOT NULL,
	[TestHeaderId] [int] NULL,
	[BankCode] [nvarchar](50) NULL,
	[Amount] [decimal](18, 4) NULL,
	[Quantity] [decimal](18, 4) NULL,
 CONSTRAINT [PK_TestDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


"
                        ;


                    context.Repositories.SettingsRepository.NewTableAdd("TestDetails", sqlText);



                    #endregion



                    #region TransportAllownaceDetails

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[TransportAllownaceDetails](
	[Id] [int] NOT NULL,
	[TransportAllowanceId] [int] NULL,
	[Date] [datetime] NULL,
	[Particulars] [nvarchar](MAX) NULL,
	[Amount] [decimal](18, 0) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_TransportAllownaceDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("TransportAllownaceDetails", sqlText);

                    #endregion TransportAllownaceDetails


                    #region TransportAllownaceOthers

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[TransportAllownaceOthers](
	[Id] [int] NOT NULL,
	[TransportAllowanceId] [int] NULL,
	[Date] [datetime] NULL,
	[Details] [nvarchar](MAX) NULL,
	[Amount] [decimal](18, 0) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_TransportAllownaceOthers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("TransportAllownaceOthers", sqlText);

                    #endregion TransportAllownaceOthers




                    #region TransportAllownaceLessAdvance

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[TransportAllownaceLessAdvance](
	[Id] [int] NOT NULL,
	[TransportAllowanceId] [int] NULL,
	[Date] [datetime] NULL,
	[Details] [nvarchar](MAX) NULL,
	[Amount] [decimal](18, 0) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_TransportAllownaceLessAdvance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("TransportAllownaceLessAdvance", sqlText);

                    #endregion TransportAllownaceOthers



                    #endregion Table




                    #region  AddField

                    context.Repositories.SettingsRepository.DBTableFieldAdd("TestHeader", "Amount", "decimal(18, 2)", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("TestDetails", "TestAmount", "int", true);

                    #endregion





                    #region FieldAlter/Update

                    context.Repositories.SettingsRepository.DBTableFieldAlter("TestDetails", "BankCode", "nvarchar(400)");
                    context.Repositories.SettingsRepository.DBTableFieldAlter("TestDetails", "BankCode", "nvarchar(500)");

                    #endregion


                    context.SaveChanges();

                    return new ResultModel<DbUpdateModel>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DbUpdateSuccess,
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<DbUpdateModel>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DbUpdateFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<SettingsModel> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<SettingsModel>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.SettingsRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<SettingsModel>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<SettingsModel>>()
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
            throw new NotImplementedException();
        }

        public ResultModel<List<SettingsModel>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<SettingsModel> Insert(SettingsModel model)
        {

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    string[] conditionField = { "SettingGroup", "SettingName" };
                    string[] conditionValue = { model.SettingGroup.Trim(), model.SettingName.Trim() };

                    bool exist = true;// context.Repositories.IPOReceiptsMasterRepository.CheckExists("Settings", conditionField, conditionValue);


                    if (!exist)
                    {

                        SettingsModel master = context.Repositories.SettingsRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<SettingsModel>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }


                        context.SaveChanges();

                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };


                    }
                    else
                    {
                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<SettingsModel>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }

        public ResultModel<SettingsModel> Update(SettingsModel model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {



                    SettingsModel master = context.Repositories.SettingsRepository.Update(model);

                    if (master.Id == 0)
                    {
                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DetailInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();


                    return new ResultModel<SettingsModel>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<SettingsModel>()
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


