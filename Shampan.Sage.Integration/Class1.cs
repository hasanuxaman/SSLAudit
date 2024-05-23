
using ACCPAC.Advantage;

namespace Shampan.Sage.Integration
{
    public class Class1
    {
        private Session session;

        private void GLEntryExistingBatch()
        {
            try
            {
                Session();
                DBLink mDBLinkCmpRW;
                mDBLinkCmpRW = session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadWrite);

                DBLink mDBLinkSysRW;
                mDBLinkSysRW = session.OpenDBLink(DBLinkType.System, DBLinkFlags.ReadWrite);

                bool temp;

                ACCPAC.Advantage.View GLBATCH1batch = mDBLinkCmpRW.OpenView("GL0008");
                ACCPAC.Advantage.View GLBATCH1header = mDBLinkCmpRW.OpenView("GL0006");
                ACCPAC.Advantage.View GLBATCH1detail1 = mDBLinkCmpRW.OpenView("GL0010");
                ACCPAC.Advantage.View GLBATCH1detail2 = mDBLinkCmpRW.OpenView("GL0402");

                ACCPAC.Advantage.View GLPOST2 = mDBLinkCmpRW.OpenView("GL0030");
                ACCPAC.Advantage.View GLBATCH3batch = mDBLinkCmpRW.OpenView("GL0008");
                ACCPAC.Advantage.View GLBATCH3header = mDBLinkCmpRW.OpenView("GL0006");
                ACCPAC.Advantage.View GLBATCH3detail1 = mDBLinkCmpRW.OpenView("GL0010");
                ACCPAC.Advantage.View GLBATCH3detail2 = mDBLinkCmpRW.OpenView("GL0402");
                ACCPAC.Advantage.View GLPOST4 = mDBLinkCmpRW.OpenView("GL0030");



                GLBATCH1batch.Compose(new ACCPAC.Advantage.View[] { GLBATCH1header });
                GLBATCH1header.Compose(new ACCPAC.Advantage.View[] { GLBATCH1batch, GLBATCH1detail1 });
                GLBATCH1detail1.Compose(new ACCPAC.Advantage.View[] { GLBATCH1header, GLBATCH1detail2 });
                GLBATCH1detail2.Compose(new ACCPAC.Advantage.View[] { GLBATCH1detail1 });

                GLBATCH3batch.Compose(new ACCPAC.Advantage.View[] { GLBATCH3header });
                GLBATCH3header.Compose(new ACCPAC.Advantage.View[] { GLBATCH3batch, GLBATCH3detail1 });
                GLBATCH3detail1.Compose(new ACCPAC.Advantage.View[] { GLBATCH3header, GLBATCH3detail2 });
                GLBATCH3detail2.Compose(new ACCPAC.Advantage.View[] { GLBATCH3detail1 });


                GLBATCH3batch.Fields.FieldByName("BATCHID").SetValue("000248", false);
                GLBATCH3batch.Read(false);
                GLBATCH3batch.Fields.FieldByName("PROCESSCMD").SetValue("1", false);

                GLBATCH3batch.Process();

                GLBATCH3header.Fields.FieldByName("BTCHENTRY").SetValue("", false);
                GLBATCH3header.Browse("", true);
                GLBATCH3header.Fetch(false);

                GLBATCH3header.RecordCreate(0);
                temp = GLBATCH3header.Exists;

                GLBATCH3header.Fields.FieldByName("DOCDATE").SetValue("2022, 5, 30", false);
                GLBATCH3header.Fields.FieldByName("DATEENTRY").SetValue("2022, 5, 30",false);
                GLBATCH3header.Fields.FieldByName("JRNLDESC").SetValue("", false);
                GLBATCH3header.Fields.FieldByName("SRCETYPE").SetValue("RT", false);

                GLBATCH3detail1.RecordClear();

                GLBATCH3detail1.RecordCreate(0);

                GLBATCH3detail1.Fields.FieldByName("ACCTID").SetValue("1000", false);
                GLBATCH3detail1.Fields.FieldByName("SCURNAMT").SetValue("1201", false);
                GLBATCH3detail1.Fields.FieldByName("TRANSNBR").SetValue("-000000001", false);
                GLBATCH3detail1.Insert();

                GLBATCH3detail1.RecordCreate(0);
                GLBATCH3detail1.Fields.FieldByName("ACCTID").SetValue("1021", false);
                GLBATCH3detail1.Fields.FieldByName("SCURNAMT").SetValue("-1201", false);
                GLBATCH3detail1.Fields.FieldByName("TRANSNBR").SetValue("-000000002", false);
                GLBATCH3detail1.Insert();

 

                GLBATCH3header.Insert();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Session()
        {
            try
            {
                try
                {
                    session = new Session();
                    session.Init("", "XY", "XY1000", "64A");
                    session.Open("ADMIN", "Admin@2023_", "SAMLTD", DateTime.Now, 0);
                }
                catch (Exception ex)
                {
                    //throw new ArgumentNullException("Can't open sage api", "");
                }
            }
            catch (Exception)
            {

                //Error2 = "Can't open sage api";
                throw new ArgumentNullException("Can't open sage api", "");
            }
        }

    }
}