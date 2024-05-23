var CISReportController = function (CommonService, CISReportService) {


    var init = function () {


        //MRWiseChangeLog

        var mrWiseChangeLogTable = MRWiseChangeLogTable();
        $(".btnSaveMRWiseChangeLog").click(function () {
            save($table);
        });
        var $table = $('#MRWiseChangeLogDetails');
        $('#MRWiseChangeLogAddRow').on('click', function () {
            addRow($table);
        });
        var table = initEditTable($table, { searchHandleAfterEdit: false });

        //End MRWiseChangeLog




        //var $tableNonFinacila = $('#NonFinancialListDetails');     
        //var tableNonFinacila = initEditTable($tableNonFinacila, { searchHandleAfterEdit: false });    
        //$('#preAddRowNonFinacial').on('click', function () {
        //    addRow($tableNonFinacila);
        //});  
        //var NonFinacilaindexTable = NonFinacilaPrepamentTable();
        //$(".btnSaveNonFinancial").click(function () {
        //    btnSaveNonFinancial($tableNonFinacila);
        //});

    }

    //End Of Init



    //MRWiseChangeLog

    function save($table) {

        var operation = $("#Operation").val();
        var id = $("#Id").val();

        if (hasInputFieldInTableCells($table)) {
            ShowNotification(3, "Complete Details Entry");
            return;

        };
        if (!hasLine($table)) {
            ShowNotification(3, "Complete Details Entry");
            return;

        };

        var MRWiseChangeLogMaster = {};
        var MRWiseChangeLogDetails = serializeTable($table);

        MRWiseChangeLogMaster.MRWiseChangeLogDetails = MRWiseChangeLogDetails;
        MRWiseChangeLogMaster.Operation = operation;
        MRWiseChangeLogMaster.Id = id;

        var size = MRWiseChangeLogDetails.length;

        for (var i = 0; i < size; i++) {

            if (MRWiseChangeLogDetails[i].MRNo == " " || MRWiseChangeLogDetails[i].MRNo == "") {
                ShowNotification(3, "Please Enter MR No");
                return;
            }
            if (MRWiseChangeLogDetails[i].PCNo == " " || MRWiseChangeLogDetails[i].PCNo == "") {
                ShowNotification(3, "Please Enter PC No");
                return;
            }
            if (MRWiseChangeLogDetails[i].UserId == " " || MRWiseChangeLogDetails[i].UserId == "") {
                ShowNotification(3, "Please Enter User Id");
                return; 
            }
            if (MRWiseChangeLogDetails[i].Status == " " || MRWiseChangeLogDetails[i].Status == "") {
                ShowNotification(3, "Please Enter Status");
                return; 
            }
            if (MRWiseChangeLogDetails[i].EditDate == " " || MRWiseChangeLogDetails[i].EditDate == "  " || MRWiseChangeLogDetails[i].EditDate == "") {
                ShowNotification(3, "Please Enter Edit Date");
                return;
            }

        }
 

        CISReportService.save(MRWiseChangeLogMaster, MRWiseChangeLogSaveDone, MRWiseChangeLogSaveFail);


    }

    function MRWiseChangeLogSaveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);          
                result.data.operation = "update";
                $(".btnSaveMRWiseChangeLog").html('Update');
                $("#Id").val(result.data.id);
                $("#Operation").val(result.data.operation);

               
            }
            else {
                ShowNotification(1, result.message);
                
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error); 
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message || result.error);
        }
    }


    function MRWiseChangeLogSaveFail(result) {
        console.log(result);
        //ShowNotification(3, "Something gone wrong");
        ShowNotification(3, "Please Fill All Field First");
    }

    var MRWiseChangeLogTable = function () {

        $('#MRWiseChangeLogList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#MRWiseChangeLogList thead');


        var dataTable = $("#MRWiseChangeLogList").DataTable({
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
                url: '/CISReport/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "mrNo": $("#md-MRNo").val(),
                            "pcNo": $("#md-PCNo").val(),
                            "userId": $("#md-UserId").val(),
                            "editDate": $("#md-EditDate").val(),
                            "status": $("#md-Status").val(),
                            "mrNet": $("#md-MRNet").val(),
                            "mrVat": $("#md-MRVat").val(),
                            "mrStamp": $("#md-MRStamp").val(),
                            "mrCoinsPayable": $("#md-MRCoinsPayable").val(),
                            "mrDateTime": $("#md-MRDateTime").val()
                            


                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/CISReport/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> ";


                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "mrNo",
                    name: "MRNo"

                }
                ,
                {
                    data: "pcNo",
                    name: "PCNo"

                }
                ,
                {
                    data: "userId",
                    name: "UserId"

                }
                ,
                {
                    data: "editDate",
                    name: "EditDate"

                }

                ,
                {
                    data: "status",
                    name: "Status"

                }
                ,
                {
                    data: "mrNet",
                    name: "MRNet"

                }
                ,
                {
                    data: "mrVat",
                    name: "MRVat"

                }
                ,
                {
                    data: "mrStamp",
                    name: "MRStamp"

                }
                ,
                {
                    data: "mrCoinsPayable",
                    name: "MRCoinsPayable"

                }
                ,
                {
                    data: "mrDateTime",
                    name: "MRDateTime"

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

                } else {
                    $(cell).html('<input type="text" class="acc-filters filter-input"  placeholder="' +
                        title +
                        '"  id="md-' +
                        title.replace(/ /g, "") +
                        '"/>');
                }
            });
        }




        $("#MRWiseChangeLogList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#MRWiseChangeLogList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    //End MRWiseChangeLog




    //NonFinancial

    //function btnSaveNonFinancial($table) {

    //    var operation = $("#Operation").val();
    //    var id = $("#Id").val();

    //    if (hasInputFieldInTableCells($table)) {
    //        ShowNotification(3, "Complete Details Entry");
    //        return;

    //    };
    //    if (!hasLine($table)) {
    //        ShowNotification(3, "Complete Details Entry");
    //        return;

    //    };

    //    var NonFinancialMaster = {};
    //    var NonFinancialDetails = serializeTable($table);

    //    NonFinancialMaster.NonFinancialDetails = NonFinancialDetails;
    //    NonFinancialMaster.Operation = operation;
    //    NonFinancialMaster.Id = id;

    //    var size = NonFinancialDetails.length;

    //    for (var i = 0; i < size; i++) {

    //        if (NonFinancialDetails[i].Auditor == " " || NonFinancialDetails[i].Auditor == "") {
    //            ShowNotification(3, "Please Enter Auditor First");
    //            return;
    //        }
    //        if (NonFinancialDetails[i].EmployeeName == " " || NonFinancialDetails[i].EmployeeName == "") {
    //            ShowNotification(3, "Please Enter EmployeeName First");
    //            return;
    //        }
    //        if (NonFinancialDetails[i].FinalCorrectionDate == " " || NonFinancialDetails[i].FinalCorrectionDate == "  " || NonFinancialDetails[i].FinalCorrectionDate == "") {
    //            ShowNotification(3, "Please Enter Final Correction Date First");
    //            return;
    //        }

    //    }


    //    DeshboardEntryService.saveNonFinancial(NonFinancialMaster, NonFinancialSaveDone, NonFinancialSaveFail);



    //}

    //function NonFinancialSaveDone(result) {
    //    debugger
    //    if (result.status == "200") {
    //        if (result.data.operation == "add") {


    //            ShowNotification(1, result.message);
    //            result.data.operation = "update";


    //        }
    //        else {
    //            ShowNotification(1, result.message);

    //        }
    //    }
    //    else if (result.status == "400") {
    //        ShowNotification(3, result.message || result.error);
    //    }
    //    else if (result.status == "199") {
    //        ShowNotification(3, result.message || result.error);
    //    }
    //}


    //function NonFinancialSaveFail(result) {
    //    console.log(result);
        
    //    ShowNotification(3, "Please Fill All Field First");
    //}


    //var NonFinacilaPrepamentTable = function () {

    //    $('#NonFinancialList thead tr')
    //        .clone(true)
    //        .addClass('filters')
    //        .appendTo('#NonFinancialList thead');


    //    var dataTable = $("#NonFinancialList").DataTable({
    //        orderCellsTop: true,
    //        fixedHeader: true,
    //        serverSide: true,
    //        "processing": true,
    //        "bProcessing": true,
    //        dom: 'lBfrtip',
    //        bRetrieve: true,
    //        searching: false,


    //        buttons: [
    //            {
    //                extend: 'pdfHtml5',
    //                customize: function (doc) {
    //                    doc.content.splice(0, 0, {
    //                        text: ""
    //                    });
    //                },
    //                exportOptions: {
    //                    columns: [1, 2, 3, 4]
    //                }
    //            },
    //            {
    //                extend: 'copyHtml5',
    //                exportOptions: {
    //                    columns: [1, 2, 3, 4]
    //                }
    //            },
    //            {
    //                extend: 'excelHtml5',
    //                exportOptions: {
    //                    columns: [1, 2, 3, 4]
    //                }
    //            },
    //            'csvHtml5'
    //        ],


    //        ajax: {
    //            url: '/Deshboard/_indexNonFinacila',
    //            type: 'POST',
    //            data: function (payLoad) {
    //                return $.extend({},
    //                    payLoad,
    //                    {



    //                        "code": $("#md-Code").val()


    //                    });
    //            }
    //        },
    //        columns: [

    //            {
    //                data: "id",
    //                render: function (data) {

    //                    return "<a href=/Deshboard/NonFinacilaEdit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> "
    //                        //+

    //                        //"<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"

    //                        //"<a href='/TeamMembers/Index/" + data + "' class='edit' title='Member'><i class='fas fa-building''></i></a>"

    //                        ;


    //                },
    //                "width": "9%",
    //                "orderable": false
    //            },
    //            {
    //                data: "code",
    //                name: "Code"

    //            }
    //            ,
    //            {
    //                data: "auditor",
    //                name: "Auditor"

    //            }
    //            ,
    //            {
    //                data: "employeeName",
    //                name: "EmployeeName"

    //            }
    //            ,
    //            {
    //                data: "details",
    //                name: "Details"

    //            }

    //            ,
    //            {
    //                data: "finalCorrectionDate",
    //                name: "FinalCorrectionDate"

    //            }
    //            ,
    //            {
    //                data: "previousAmount",
    //                name: "PreviousAmount"

    //            }
    //            ,
    //            {
    //                data: "correctedAmount",
    //                name: "CorrectedAmount"

    //            }
    //            ,
    //            {
    //                data: "additionalPayment",
    //                name: "AdditionalPayment"

    //            }



    //        ]

    //    });


    //    if (dataTable.columns().eq(0)) {
    //        dataTable.columns().eq(0).each(function (colIdx) {

    //            var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

    //            var title = $(cell).text();


    //            if ($(cell).hasClass('action')) {
    //                $(cell).html('');

    //            } else if ($(cell).hasClass('bool')) {

    //                $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

    //            } else {
    //                $(cell).html('<input type="text" class="acc-filters filter-input"  placeholder="' +
    //                    title +
    //                    '"  id="md-' +
    //                    title.replace(/ /g, "") +
    //                    '"/>');
    //            }
    //        });
    //    }




    //    $("#NonFinancialList").on("change",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });
    //    $("#NonFinancialList").on("keyup",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });

    //    return dataTable;

    //}


    return {
        init: init
    }


}(CommonService, CISReportService);