var AuditIssueController = function (AuditIssueService) {


    var indexTable;

    var init = function (param) {

        //if ($("#AuditId").length) {
        //    LoadCombo("AuditId", '/Common/GetAuditName');
        //}
        if ($("#IssuePriority").length) {
            LoadCombo("IssuePriority", '/Common/GetIssuePriority');
        }
        $(".btnSave").on("click", function () {
            Save();
        });
        if ($("#AuditIssueIndexList").length) {
            var Condigs = getIssuesIndexTableData()
            indexTable = $("#AuditIssueIndexList").DataTable();
            //dataTableissue = $("#AuditIssueIndexList").DataTable(tableConfigsissue);
        }
        if ($("#ExcelIndexList").length) {
            var Condigs = excelIndexListTableData()
            indexTable = $("#ExcelIndexList").DataTable();
            //dataTableissue = $("#AuditIssueIndexList").DataTable(tableConfigsissue);
        }     
        if ($("#AuditIssueList").length) {
            var tableConfigs = GetIndexTable(param)
            detailTable = $("#AuditIssueList").DataTable(tableConfigs);
        }
    }
    //End of Init

    //Total Pending Issues Review
    $('#AuditIssueIndexList').on('click', '.displayInfoTotalPendingIssue', function () {
        debugger;
        var rowData = indexTable.row($(this).closest('tr')).data();

        var auditIssueId = rowData.id;
        var auditId = rowData.auditId;
        var masterObj = {
            AuditIssueId: auditIssueId,
            AuditId: auditId
        };
        AuditIssueService.TotalPendingIssuesReview(masterObj, TotalPendingIssuesReviewDone, TotalPendingIssuesReviewFail);

    });
    function TotalPendingIssuesReviewDone(result) {

        if (result.status == "200") {

            ShowNotification(1, "Email Has Been Sent Successfully");
        }

        else {
            ShowNotification(3, "You Have To Add Email First");
        }

    }
    function TotalPendingIssuesReviewFail(result) {
        console.log(result);
        ShowNotification(3, "Item Is Not Found");
    }

    //For Sending Mail To Audit Issue
    $('#AuditIssueIndexList').on('click', '.displayInfoFollowUpIssues', function () {
        debugger;
        var rowData = indexTable.row($(this).closest('tr')).data();
        var auditIssueId = rowData.id;
        var auditId = rowData.auditId;

        var masterObj = {
            AuditIssueId: auditIssueId,
            AuditId: auditId
        };
        AuditIssueService.FollowUpAuditIssues(masterObj, FollowUpAuditIssuesDone, FollowUpAuditIssuesFail);

    });
    function FollowUpAuditIssuesDone(result) {

        if (result.status == "200") {

            ShowNotification(1, "Email Has Been Sent Successfully");

        }

        else {
            ShowNotification(3, "You Have To Add Email First");


        }

    }
    function FollowUpAuditIssuesFail(result) {
        console.log(result);
        ShowNotification(3, "Item Is Not Found");

    }

    
    //ForMissDeadLine
    $('#AuditIssueIndexList').on('click', '.displayInfoMissDeadLine', function () {
        debugger;
        var rowData = indexTable.row($(this).closest('tr')).data();

        var auditIssueId = rowData.id;
        var auditId = rowData.auditId;

        var masterObj = {
            AuditIssueId: auditIssueId,
            AuditId: auditId
        };

        AuditIssueService.IssuedeadLineLapsed(masterObj, IssuedeadLineLapsedDone, IssuedeadLineLapsedFail);

    });
    function IssuedeadLineLapsedDone(result) {

        if (result.status == "200") {
            ShowNotification(1, "Email Has Been Sent Successfully");
        }

        else {
            ShowNotification(3, "You Have To Add Email First");
        }

    }
    function IssuedeadLineLapsedFail(result) {
        console.log(result);
        ShowNotification(3, "Item Is Not Found");
    }

    //EndOfIssueDeadLine


    //AuditIssueIndex

    var getIssuesIndexTableData = function () {
        $('#AuditIssueIndexList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditIssueIndexList thead');


        var Status = $("#Edit").val();
        var databaseRowCount = 1000;


        var dataTable = $("#AuditIssueIndexList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            //dom: 'lBfrtip',
            dom: '<"pagination-left"lBfrtip>',
            bRetrieve: true,
            searching: false,
            //scrollX: true,



            lengthMenu: [10, 25, 50, 100, 125,150,175,200,300,400,500],
            buttons: [
                {
                    extend: 'pdfHtml5',
                    title: 'GDIC Audit Pdf File',
                    exportOptions: {                      
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]

                    }
                },
                {
                    extend: 'copyHtml5',
                    title: 'GDIC Audit',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: 'GDIC Audit Excel File',
                    exportOptions: {
                        columns: [1, 2, 3, 4,5,6,7,8,9,10,11,12,13,14,15,16]
                    }
                },                
                'csvHtml5'
            ],

            ajax: {     
                url: '/AuditIssue/_auditIssueIndex?edit=' + Status,
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "auditCode": $("#md-AuditCode").val(),
                            "auditName": $("#md-AuditName").val(),
                            "auditStatus": $("#md-AuditStatus").val(),

                            "issuename": $("#md-IssueName").val(),
                            "issuepriority": $("#md-IssuePriority").val(),
                            "dateofsubmission": $("#md-DateOfSubmission").val(),
                            "operational": $("#md-Operational").val(),
                            "financial": $("#md-Financial").val(),
                            "compliance": $("#md-Compliance").val(),

                            "enddate": $("#md-EndDate").val(),
                            "ispost": $("#md-Post").val()

                        });
                }
            },


            columns: [

                {
                    //PendingForReviewerFeedback
                    
                    data: "auditId",               
                    render: function (data, type, row) {

                        var edit = $('#Edit').val();  

                        var output = "<a href='/Audit/Edit/" + data + "?edit=issue'  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>"
                        //if (edit === "MissDeadLineIssues") {
                        if (edit === "Issuedeadlinelapsed") {
                            //output += "<a data-id='" + data + "' class='displayInfoMissDeadLine' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                        }
                        else if (edit === "FollowUpAuditIssues") {
                            //output += "<a data-id='" + data + "' class='displayInfoFollowUpIssues' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                        }
                        else if (edit === "TotalPendingIssueReview") {
                            output += "<a data-id='" + data + "' class='displayInfoTotalPendingIssue' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                        }
                        else if (edit === "PendingForReviewerFeedback") {
                            output = "<a href='/Audit/Edit/" + data + "?edit=feedback'  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>"
                        }
                        return output;
                    },

                   
                    "width": "5%",
                    "orderable": false
                },
                {
                    data: null,
                    name: "ID",
                    //"width": "4%",
                    render: function (data, type, row, meta) {
                        if (type === 'display') {
                            var displayCount = meta.row + meta.settings._iDisplayStart + 1;
                            if (displayCount <= databaseRowCount) {
                                return displayCount;
                            } else {
                                return '';
                            }
                        }
                        return data;
                    }              
                },
                {
                    data: "code",
                    name: "Code",
                    "width": "5%"
                },
                {
                    data: "name",
                    name: "Name",
                    "width": "5%"
                },
                {
                    data: "issueName",
                    name: "IssueName",
                    "width": "5%"
                },
                {
                    data: "issueDetails",
                    name: "IssueDetails",
                    width: "5%",
                    className: "scrollable-column",
                    render: function (data, type, row) {
                        return '<div class="scrollable-content">' + data + '</div>';
                    }
                },               
                {
                    data: "risk",
                    name: "Risk",
                    "width": "5%"

                },
                {
                    data: "feedbackDetails",
                    name: "FeedbackDetails",
                    "width": "5%"

                },               
                {
                    data: "auditStatus",
                    name: "AuditStatus",
                    "width": "5%"
                },
                {
                    data: "issuePriority",
                    name: "IssuePriority",
                    "width": "5%"

                }
                ,
                {
                    data: "issueStatus",
                    name: "IssueStatus",
                    "width": "5%"

                },
                {
                    
                    data: "investigationOrForensis",
                    name: "InvestigationOrForensis",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }
                }
                ,
                {
                   
                    data: "stratigicMeeting",
                    name: "StratigicMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {                   
                    data: "managementReviewMeeting",
                    name: "ManagementReviewMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
                    
                    data: "otherMeeting",
                    name: "OtherMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                , 
                {
                    
                    data: "training",
                    name: "Training",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,            
                {
                    
                    data: "operational",
                    name: "Operational",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
                    
                    data: "financial",
                    name: "Financial",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }              
                ,
                {
                   
                    data: "compliance",
                    name: "Compliance",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }  
                ,                            
                {
                    data: "dateOfSubmission",
                    name: "DateOfSubmission",
                    "width": "5%"
                }
                
            ],

            order: [1, "asc"],

        });

        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());
                var title = $(cell).text();
                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>True</option><option>False</option></select>');

                }
                else if ($(cell).hasClass('private')) {

                    $(cell).html('');

                }
                else {
                    $(cell).html('<input type="text" class="acc-filters filter-input"  placeholder="' +
                        title +
                        '"  id="md-' +
                        title.replace(/ /g, "") +
                        '"/>');
                }
            });
        }

        $("#AuditIssueIndexList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        $("#AuditIssueIndexList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        return dataTable;

    }


    //ExcelIndex

    var excelIndexListTableData = function () {

        $('#ExcelIndexList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#ExcelIndexList thead');

        var Status = $("#Edit").val();
        var databaseRowCount = 1000;


        var dataTable = $("#ExcelIndexList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            //dom: 'lBfrtip',
            dom: '<"pagination-left"lBfrtip>',

            bRetrieve: true,
            searching: false,
            lengthMenu: [10, 25, 50, 100, 125, 150, 175, 200, 300, 400, 500],

            buttons: [
                {
                    extend: 'pdfHtml5',
                    title: 'GDIC Audit Pdf File',
                    exportOptions: {                
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17,18]
                    }
                },
                {
                    extend: 'copyHtml5',
                    title: 'GDIC Audit',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: 'GDIC Audit Excel File',
                    exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17,18,19]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: '/AuditIssue/_excelIndex?edit=' + Status,
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "auditCode": $("#md-AuditCode").val(),
                            "auditName": $("#md-AuditName").val(),
                            "auditStatus": $("#md-AuditStatus").val(),

                            "issuename": $("#md-IssueName").val(),
                            "issueHeading": $("#md-IssueHeading").val(),
                            "issuepriority": $("#md-IssuePriority").val(),
                            "issueStatus": $("#md-IssueStatus").val(),
                            "dateofsubmission": $("#md-DateOfSubmission").val(),

                            "investigationOrforensis": $("#md-InvestigationOrForensis").val(),
                            "stratigicMeeting": $("#md-StratigicMeeting").val(),
                            "managementReviewMeeting": $("#md-ManagementReviewMeeting").val(),
                            "otherMeeting": $("#md-OtherMeeting").val(),
                            "training": $("#md-Training").val(),

                            "operational": $("#md-Operational").val(),
                            "financial": $("#md-Financial").val(),
                            "compliance": $("#md-Compliance").val(),

                            "enddate": $("#md-EndDate").val(),
                            "ispost": $("#md-Post").val()



                        });
                }
            },

            columns: [

                {
                    
                    data: "auditId",
                    render: function (data) {

                        return "<a href='/Audit/Edit/" + data + "?edit=issue'  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>  "

                    },
                    //"width": "4%",
                    "orderable": false
                },

                {

                    data: null,
                    name: "ID",
                    "width": "4%",
                    render: function (data, type, row, meta) {
                        if (type === 'display') {
                            var displayCount = meta.row + meta.settings._iDisplayStart + 1;
                            if (displayCount <= databaseRowCount) {
                                return displayCount;
                            } else {
                                return '';
                            }
                        }
                        return data;
                    }
                },

                {
                    data: "code",
                    name: "Code"
                }
                ,
                {
                    data: "name",
                    name: "Name"
                },
                {
                    data: "issueName",
                    name: "IssueName"
                },
                {
                    data: "issueDetails",
                    name: "IssueDetails",

                },
                {
                    data: "risk",
                    name: "Risk",

                },
                {
                    data: "feedbackDetails",
                    name: "FeedbackDetails",

                }
                ,
                {
                    data: "branchFeedBackDetails",
                    name: "BranchFeedBackDetails",

                },
                {
                    data: "auditStatus",
                    name: "AuditStatus"
                }    
                ,
                {
                    data: "issuePriority",
                    name: "IssuePriority",

                }
                ,
                {
                    data: "issueStatus",
                    name: "IssueStatus",
                },
                {
                    data: "dateOfSubmission",
                    name: "DateOfSubmission"
                },
                {
                   
                    data: "investigationOrForensis",
                    name: "InvestigationOrForensis",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
                    
                    data: "stratigicMeeting",
                    name: "StratigicMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
                   
                    data: "managementReviewMeeting",
                    name: "ManagementReviewMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
                   
                    data: "otherMeeting",
                    name: "OtherMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
                   
                    data: "training",
                    name: "Training",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
             
                    data: "operational",
                    name: "Operational",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }
                }
                ,
                {
                    
                    data: "financial",
                    name: "Financial",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {
                 
                    data: "compliance",
                    name: "Compliance",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                

            ],

            order: [1, "asc"],

        });


        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>True</option><option>False</option></select>');

                }
                else if ($(cell).hasClass('private')) {

                    $(cell).html('');

                }
                else {
                    $(cell).html('<input type="text" class="acc-filters filter-input"  placeholder="' +
                        title +
                        '"  id="md-' +
                        title.replace(/ /g, "") +
                        '"/>');
                }
            });
        }


        $("#ExcelIndexList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        $("#ExcelIndexList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        return dataTable;

    }



    function GetIndexTable(param) {
        return {

            "processing": true,
            serverSide: true,

            ajax: {
                url: '/AuditIssue/_index?id=' + param.auditId,
                type: 'POST',
                data: function (payLoad) {

                    return $.extend({},
                        payLoad,
                        {
                            //"search2": $("#name").val()
                        });
                }
            },

            columns: [
                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/AuditIssue/Edit/" + data + " class='edit' ><i class='editIcon' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  ";

                    },
                    "width": "7%",
                    "orderable": false
                },
                {
                    data: "issueName",
                    name: "IssueName"
                }
                ,
                {
                    data: "issuePriority",
                    name: "IssuePriority"
                }
                ,
                {
                    data: "dateOfSubmission",
                    name: "DateOfSubmission"
                }
            ],
            order: [1, "asc"],

        }
    }



    function Save() {

        var validator = $("#frm_Audit").validate();
        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var form = $("#frm_Audit")[0];
        var formData = new FormData(form);

        AuditIssueService.save(formData, saveDone, saveFail);

    }

    function addListItem(result) {
        var list = $(".fileGroup");

        result.data.attachmentsList.forEach(function (item) {

            var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                item.displayName +
                '</span><a target="_blank" href="/AuditIssue/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                '<button onclick="AuditIssueController.deleteFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                '</li>';

            list.append(item);
        });
    }


    function saveDone(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSave").html('Update');
                $("#Id").val(result.data.id);

                result.data.operation = "update";

                $("#Operation").val(result.data.operation);

                addListItem(result);


            } else {

                addListItem(result);
                ShowNotification(1, result.message);
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }

    function saveFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    var deleteFile = function deleteFile(fileId, filePath) {

        AuditIssueService.deleteFile({ id: fileId, filePath: filePath }, (result) => {


            if (result.status == "200") {
                $("#file-" + fileId).remove();

                ShowNotification(1, result.message);
            }
            else if (result.status == "400") {
                ShowNotification(3, result.message);
            }



        }, saveFailDelete);

    };



    function saveFailDelete(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    return {
        init: init,
        deleteFile
    }

}(AuditIssueService);