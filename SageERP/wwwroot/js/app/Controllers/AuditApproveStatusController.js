var AuditApproveStatusController = function (CommonService, AuditApproveStatusService) {



    var init = function () {


        //if ($("#TeamId").length) {
        //    LoadCombo("TeamId", '/Common/TeamName');
        //}
        //if ($("#AuditId").length) {
        //    LoadCombo("AuditId", '/Common/AuditName');
        //}

        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });


        var indexTable = AuditTable();
        var SelfAuditindexTable = SelfAuditTable();

        var auditStatusTable = AuditStatusTable();





    }

    /*init end*/




    //Multiplr Audit Approval

    $('#MultipleAA').on('click', function () {

        Confirmation("Are you sure? Do You Want to Approve Multiple Audit?", function (result) {
            console.log(result);
            if (result) {

                SelectData(true);
            }
        });

    });





    function SelectData(IsApprove) {

        var IDs = [];
        var $Items = $(".dSelected:input:checkbox:checked");

        if ($Items == null || $Items.length == 0) {
            ShowNotification(3, "You are requested to Select checkbox!");
            return;
        }

        $Items.each(function () {
            var ID = $(this).attr("data-Id");
            IDs.push(ID);
        });

        var model = {
            IDs: IDs,

        }

        //var dataTable = $('#ApproveStatusList').DataTable();
        //var rowData = dataTable.rows().data().toArray();
        //var filteredData = [];
        //var filteredData1 = [];
        //if (IsPost) {
        //    filteredData = rowData.filter(x => x.isPost === "Y" && IDs.includes(x.id.toString()));
        //}
        //else {
        //    filteredData = rowData.filter(x => x.isPush === "Y" && IDs.includes(x.id.toString()));
        //    filteredData1 = rowData.filter(x => x.isPost === "N" && IDs.includes(x.id.toString()));
        //}
        //if (IsPost) {
        //    if (filteredData.length > 0) {
        //        ShowNotification(3, "Data has already been Posted.");
        //        return;
        //    }
        //}

        if (IsApprove) {

            AuditApproveStatusService.MultipleAuditApproval(model, MultipleAuditApprovalDone, MultipleAuditApprovalFail);

        }



    }

    function MultipleAuditApprovalDone(result) {
        console.log(result.message);

        if (result.status == "200") {


            ShowNotification(1, result.message);
            var dataTable = $('#AuditStatusList').DataTable();
            dataTable.draw();




        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function MultipleAuditApprovalFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditStatusList').DataTable();
        dataTable.draw();

    }



    //End Multiple Audit Approval


    var AuditStatusTable = function () {

        $('#AuditStatusList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditStatusList thead');


        var dataTable = $("#AuditStatusList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            //dom: 'lBfrtip',
            dom: '<"pagination-left"lBfrtip>',
            bRetrieve: true,
            searching: false,


            buttons: [
                {
                    extend: 'pdfHtml5',
                    customize: function (doc) {
                        doc.content.splice(0, 0, {
                            text: ""
                        });
                    },
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: '/Audit/_auditStatusIndex',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "branchName": $("#md-BranchName").val(),
                            "approvalStatus": $("#md-ApprovalStatus").val()

                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href='/Audit/Edit/" + data + "?edit=preview' class='edit btn btn-primary btn-sm' title='Preview'><i class='fas fa-chart-bar' data-toggle='tooltip' title='Preview'></i></a>"
                            ;


                    },
                    "width": "10%",
                    "orderable": false
                },
                {
                    data: "code",
                    name: "Code",
                    "width": "8%",

                }
                ,
                {
                    data: "name",
                    name: "Name",
                    "width": "8%",

                }
                ,
                {
                    data: "approvalStatus",
                    name: "ApprovalStatus",
                    "width": "8%",

                }

                ,
                {
                    data: "branchName",
                    name: "BranchName",
                    "width": "8%"

                }
                ,
                {
                    data: "auditStatus",
                    name: "AuditStatus",
                    "width": "8%"

                },
                {
                    data: "startDate",
                    name: "StartDate",
                    "width": "8%"

                }
                ,
                {
                    data: "endDate",
                    name: "EndDate",
                    "width": "10%"

                },
                {

                    data: "isPlaned",
                    name: "IsPlaned",
                    "width": "8%",

                    render: function (data) {
                        if (data) {
                            return '✔';
                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                    //render: function (data) {
                    //    if (data) {
                    //        return '<i class="fas fa-check blue-icon"></i>';
                    //    } else {
                    //        return '<i class="fas fa-times blue-icon"></i>';
                    //    }
                    //}

                },
                {
                    data: "isApprovedL4",
                    name: "IsApprovedL4",
                    "width": "8%",

                    render: function (data) {
                        if (data) {
                            return '✔';
                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                },
                //{
                //    data: "isCompleteIssue",
                //    name: "IsCompleteIssue",
                //    "width": "8%",
                //    render: function (data) {
                //        if (data) {
                //            return '<i class="fas fa-check blue-icon"></i>';
                //        } else {
                //            return '<i class="fas fa-times blue-icon"></i>';
                //        }
                //    }
                //}
                //,
                {
                    data: "isCompleteIssueTeamFeedback",
                    name: "IsCompleteIssueTeamFeedback",
                    "width": "8%",


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
                    data: "isCompleteIssueBranchFeedback",
                    name: "IsCompleteIssueBranchFeedback",
                    "width": "8%",

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
                    data: "isPost",
                    name: "IsPost",
                    "width": "10%"

                }



            ]

        });


        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

                }

                else if ($(cell).hasClass('status')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Approved</option><option>Reject</option></select>');

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




        $("#AuditStatusList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#AuditStatusList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }



    $('.RejectSubmit').click('click', function () {


        RejectedComments = $("#RejectedComments").val();

        var auditreject = serializeInputs("frm_Audit");

        auditreject["RejectedComments"] = RejectedComments;

        Confirmation("Are you sure? Do You Want to Reject Data?", function (result) {
            if (RejectedComments === "" || RejectedComments === null) {
                ShowNotification(3, "Please Write down Reason Of Reject");
                $("#RejectedComments").focus();
                return;
            }

            if (result) {



                //For Issue IDs

                var edit = $("#Edit").val();
                if (edit == "issueApprove") {
                    var IssueIDs = [];
                    var $Items = $(".dSelected:input:checkbox:checked");
                    if ($Items == null || $Items.length == 0) {
                        ShowNotification(3, "You are requested to Select checkbox!");
                        return;
                    }
                    $Items.each(function () {
                        var ID = $(this).attr("data-Id");
                        IssueIDs.push(ID);
                    });
                    var model = {
                        IssueIDs: IssueIDs,
                    }
                    auditreject.IssueIDs = model.IssueIDs;

                }


                //End of Issue IDs



                var id = $("#Id").val();
                auditreject.Id = id;

                auditreject.IDs = auditreject.Id;
                AuditApproveStatusService.AuditMultipleRejectData(auditreject, AuditMultipleReject, AuditMultipleUnRejectFail);


            }
        });
    });

    function AuditMultipleReject(result) {
        console.log(result.message);

        if (result.status == "200") {
            //ShowNotification(1, result.message);
            ShowNotification(1, "Data Reject Successfully");

            $("#IsPost").val('N');
            //Visibility(false);
            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();

            //$(".btnReject").hide();
            //$(".btnApproved").hide();

            //change of button
            $(".btnReject").show();
            $(".btnApproved").show();
            //end

            var dataTable = $('#AuditList').DataTable();

            dataTable.draw();

            $('#modal-defaultReject').modal('hide');


            //ForHidingRejectAndApproved
            $(".btnReject, .ApprovedSubmit").hide();
            var container = $(".replaceData");
            container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
    }

    function AuditMultipleUnRejectFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditList').DataTable();

        dataTable.draw();
    }

    //ForDisableRejectAndApproveButtonAfterReject

    var isReject = $("#IsRejected").val();
    var issueIsRejected = $("#IssueIsRejected").val();
    var bFIsRejected = $("#BFIsRejected").val();
    var edit = $("#Edit").val();

    if (bFIsRejected == "True" && edit == "branchFeedbackApprove") {

        $(".btnReject, .ApprovedSubmit").hide();
        var container = $(".replaceData");
        container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');


    }
    if (issueIsRejected == "True" && edit == "issueApprove") {

        $(".btnReject, .ApprovedSubmit").hide();
        var container = $(".replaceData");
        container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');


    }
    if (isReject == "True" && edit == "auditStatus") {

        $(".btnReject, .ApprovedSubmit").hide();
        var container = $(".replaceData");
        container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');


    }

    //EndForDisableRejectAndApproveButtonAfterReject



    $('.ApprovedSubmit').click('click', function () {



        CommentsL1 = $("#CommentsL1").val();

        var auditapprove = serializeInputs("frm_Audit");

        auditapprove["CommentsL1"] = CommentsL1;

        Confirmation("Are you sure? Do You Want to Approve Data?", function (result) {
            if (CommentsL1 === "" || CommentsL1 === null) {
                ShowNotification(3, "Please Write down Reason Of Approved");
                $("#CommentsL1").focus();
                return;
            }

            if (result) {



                //For Issue IDs


                var edit = $("#Edit").val();
                if (edit == "issueApprove") {


                    var IssueIDs = [];
                    var $Items = $(".dSelected:input:checkbox:checked");

                    if ($Items == null || $Items.length == 0) {
                        ShowNotification(3, "You are requested to Select checkbox!");
                        return;
                    }

                    $Items.each(function () {
                        var ID = $(this).attr("data-Id");
                        IssueIDs.push(ID);
                    });

                    var model = {
                        IssueIDs: IssueIDs,

                    }

                    auditapprove.IssueIDs = model.IssueIDs;

                }


                //End of Issue IDs




                auditapprove.IDs = auditapprove.Id;
                AuditApproveStatusService.AuditMultipleApprovedData(auditapprove, AuditMultipleApproved, AuditMultipleApprovedFail);


            }
        });
    });


    function AuditMultipleApproved(result) {
        console.log(result.message);

        if (result.status == "200") {
            //ShowNotification(1, result.message);
            ShowNotification(1, "Audit Has Been Approved Successfully");
            $("#IsPost").val('N');
            //Visibility(false);
            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();


            //$(".btnReject").hide();
            //$(".btnApproved").hide();

            //change of button
            $(".btnReject").show();
            $(".btnApproved").show();
            //end


            var dataTable = $('#AuditList').DataTable();

            dataTable.draw();

            $('#modal-defaultApproved').modal('hide');

            //ForHidingRejectAndApproveButton
            //$(".btnReject, .ApprovedSubmit").prop("disabled", true);
            $(".btnReject, .ApprovedSubmit").hide();

            var container = $(".replaceData");

            // Replace the content with your desired text or HTML
            //container.html("<h1>Audit Is Approved</h1>");
            container.html('<button type="button" class="button sslPush btnPushed">Already Approved</button>');




        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
            // <-- display the error message here
            //ShowNotification(3, "You Don't have Premission for Audit Approved'");
        }
    }

    function AuditMultipleApprovedFail(result) {
        var post = $("#IsPost").val();
        var emil = result.SingleValue;
        //if (post === 'N') {
        //    ShowNotification(3, "Data is not post yet");

        //}
        //else {
        //ShowNotification(3, "Data is already Approve");
        //ShowNotification(3, "You Don't have Premission for Audit Approved'");
        ShowNotification(3, "You Have Already Approved It.Your Approved Premission Is Over");

        //}

        var dataTable = $('#AuditList').DataTable();

        dataTable.draw();
    }





    $('#modelClose').click('click', function () {

        $("#UnPostReason").val("");
        $('#modal-default').modal('hide');


    });


    var AuditTable = function () {

        $('#ApproveStatusList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#ApproveStatusList thead');


        var dataTable = $("#ApproveStatusList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: false,


            buttons: [
                {
                    extend: 'pdfHtml5',
                    customize: function (doc) {
                        doc.content.splice(0, 0, {
                            text: ""
                        });
                    },
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: '/Audit/_approveStatusIndex',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "description": $("#md-Description").val(),
                            "approveStatus": $("#md-ApproveStatus").val(),
                            "ispost": $("#md-Post").val(),


                            "ponumber": $("#md-PONumber").val(),
                            "ispost": $("#md-Post").val(),
                            "ispush": $("#md-Push").val(),
                            "fromDate": $("#FromDate").val(),
                            "toDate": $("#ToDate").val()
                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {


                        /*return "<a href=/Audit/Edit/" + data + "?edit=audit class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt  ' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  "*/

                        return "<a href=/Audit/Edit/" + data + "?edit=auditStatus class='edit btn btn-primary btn-sm' ><i class='fas fa-check tick-icon' data-toggle='tooltip' title='' data-original-title='Audit'></i></a>  "


                            + "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"

                            //return "<a href=/Audit/Edit/" + data + " class='edit' ><i class='editIcon' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>" 
                            //"<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"
                            //"<a href='/TeamMembers/Index/" + data + "' class='edit' title='Member'><i class='fas fa-building''></i></a>"

                            ;


                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "code",
                    name: "Code"

                }
                ,
                {
                    data: "name",
                    name: "Name"

                }


                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

                }



            ]

        });


        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

                }

                else if ($(cell).hasClass('status')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>0</option><option>1</option><option>2</option><option>3</option><option>4</option><option>R</option></select>');

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




        $("#ApproveStatusList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#ApproveStatusList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    var SelfAuditTable = function () {

        $('#SelfApproveStatusList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#SelfApproveStatusList thead');

        var Status = 'self';

        var dataTable = $("#SelfApproveStatusList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: false,


            buttons: [
                {
                    extend: 'pdfHtml5',
                    customize: function (doc) {
                        doc.content.splice(0, 0, {
                            text: ""
                        });
                    },
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: '/Audit/_selfapproveStatusIndex',
                //url: '/Audit/_approveStatusIndex',
                //url: '/Audit/_approveStatusIndex?status=' + Status,
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "description": $("#md-Description").val(),
                            "approveStatus": $("#md-ApproveStatus").val(),
                            "ispost": $("#md-Post").val(),


                            "ponumber": $("#md-PONumber").val(),
                            "ispost": $("#md-Post").val(),
                            "ispush": $("#md-Push").val(),
                            "fromDate": $("#FromDate").val(),
                            "toDate": $("#ToDate").val()
                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {


                        /*return "<a href=/Audit/Edit/" + data + "?edit=audit class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt  ' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  "*/

                        return "<a href=/Audit/Edit/" + data + "?edit=auditSelfApprove class='edit btn btn-primary btn-sm' ><i class='fas fa-file-invoice' data-toggle='tooltip' title='' data-original-title='Audit'></i></a>  "


                            //return "<a href=/Audit/Edit/" + data + " class='edit' ><i class='editIcon' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>" 
                            //"<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"
                            //"<a href='/TeamMembers/Index/" + data + "' class='edit' title='Member'><i class='fas fa-building''></i></a>"

                            ;


                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "code",
                    name: "Code"

                }
                ,
                {
                    data: "name",
                    name: "Name"

                }

                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

                }
                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

                }
                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

                }




            ]

        });


        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

                }

                else if ($(cell).hasClass('status')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>0</option><option>1</option><option>2</option><option>3</option><option>4</option><option>R</option></select>');

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




        $("#SelfApproveStatusList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#SelfApproveStatusList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }




    return {
        init: init

    }


}(CommonService, AuditApproveStatusService);