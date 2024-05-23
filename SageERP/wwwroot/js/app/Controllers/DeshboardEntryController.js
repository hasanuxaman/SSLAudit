var DeshboardEntryController = function (CommonService, DeshboardEntryService) {


    var init = function () {

        var $table = $('#PrepamentList');
        var $tableNonFinacila = $('#NonFinancialListDetails');

        var table = initEditTable($table, { searchHandleAfterEdit: false });
        var tableNonFinacila = initEditTable($tableNonFinacila, { searchHandleAfterEdit: false });

        $('#preAddRow').on('click', function () {
            addRow($table);
        });
        $('#preAddRowNonFinacial').on('click', function () {
            addRow($tableNonFinacila);
        });

        //$(".chkAll").click(function () {
        //    $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        //});

        var indexTable = PrepamentTable();
        var NonFinacilaindexTable = NonFinacilaPrepamentTable();

        $(".btnSave").click(function () {
            save($table);
        });

        $(".btnSaveNonFinancial").click(function () {
            btnSaveNonFinancial($tableNonFinacila);
        });

        //Calculation

        $('#PrepamentList').on('blur', ".td-PreviousAmount", function (event) {
            computeSubtotal($(this));
        });
        $('#PrepamentList').on('blur', ".td-CorrectedAmount", function (event) {
            computeSubtotal($(this));
        });

        function computeSubtotal(row) {
            try {
                debugger;

                var previous = parseFloat(row.closest("tr").find("td:eq(4)").text().replace(',', ''));
                var corrected = parseFloat(row.closest("tr").find("td:eq(5)").text().replace(',', ''));

                if (!isNaN(previous+corrected)) {

                    var val = Number(parseFloat(previous - corrected).toFixed(2)).toLocaleString('en', { minimumFractionDigits: 2 });
                    row.closest("tr").find("td:eq(6)").text(val);                
                }
            } catch (ex) {

            }
        }


    }

    /*init end*/


    $('.btnPrepaymentReviewed').on('click', function () {

        Confirmation("Are you sure? Do You Want to Save Data?", function (result) {
            console.log(result);
            if (result) {
                SaveData(true);
            }
        });

    });
    function SaveData(data) {
        debugger;
        var value = $("#PrepaymentReview").val();
        var PrepaymentReview = {};
        PrepaymentReview.Value = value;
        DeshboardEntryService.SavePrepaymentReviewed(PrepaymentReview, PrepaymentReviewedSave, PrepaymentReviewedFail);
    }

    function PrepaymentReviewedSave(result) {

        ShowNotification(1, result.message);

    }

    function PrepaymentReviewedFail(result) {
        ShowNotification(3, result.message);
   
    }





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
        debugger;
        var PrePaymentMaster = {};
        var prePaymentDetails = serializeTable($table);

        PrePaymentMaster.PrePaymentDetails = prePaymentDetails;
        PrePaymentMaster.Operation = operation;
        PrePaymentMaster.Id = id;

        var size = prePaymentDetails.length;

        for (var i = 0; i < size; i++) {

            if (prePaymentDetails[i].Auditor == " " || prePaymentDetails[i].Auditor == "") {
                ShowNotification(3, "Please Enter Auditor First");
                return;
            }
            if (prePaymentDetails[i].EmployeeName == " " || prePaymentDetails[i].EmployeeName == "") {
                ShowNotification(3, "Please Enter EmployeeName First");
                return;
            }
            if (prePaymentDetails[i].FinalCorrectionDate == " " || prePaymentDetails[i].FinalCorrectionDate == "  " || prePaymentDetails[i].FinalCorrectionDate == "") {
                ShowNotification(3, "Please Enter Final Correction Date First");
                return;
            }

        }

        
        


        DeshboardEntryService.save(PrePaymentMaster, PrePaymentSaveDone, PrePaymentSaveFail);



    }

    function PrePaymentSaveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);          
                result.data.operation = "update";

               
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


    function PrePaymentSaveFail(result) {
        console.log(result);
        //ShowNotification(3, "Something gone wrong");
        ShowNotification(3, "Please Fill All Field First");
    }

    var PrepamentTable = function () {

        $('#PrePaymentList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#PrePaymentList thead');


        var dataTable = $("#PrePaymentList").DataTable({
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
                url: '/Deshboard/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {



                            "code": $("#md-Code").val()


                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/Deshboard/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> "
                            //+

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
                    data: "auditor",
                    name: "Auditor"

                }
                ,
                {
                    data: "employeeName",
                    name: "EmployeeName"

                }
                ,
                {
                    data: "details",
                    name: "Details"

                }

                ,
                {
                    data: "finalCorrectionDate",
                    name: "FinalCorrectionDate"

                }
                ,
                {
                    data: "previousAmount",
                    name: "PreviousAmount"

                }
                ,
                {
                    data: "correctedAmount",
                    name: "CorrectedAmount"

                }
                ,
                {
                    data: "additionalPayment",
                    name: "AdditionalPayment"

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




        $("#PrePaymentList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#PrePaymentList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    //NonFinancial


    function btnSaveNonFinancial($table) {

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

        var NonFinancialMaster = {};
        var NonFinancialDetails = serializeTable($table);

        NonFinancialMaster.NonFinancialDetails = NonFinancialDetails;
        NonFinancialMaster.Operation = operation;
        NonFinancialMaster.Id = id;

        var size = NonFinancialDetails.length;

        for (var i = 0; i < size; i++) {

            if (NonFinancialDetails[i].Auditor == " " || NonFinancialDetails[i].Auditor == "") {
                ShowNotification(3, "Please Enter Auditor First");
                return;
            }
            if (NonFinancialDetails[i].EmployeeName == " " || NonFinancialDetails[i].EmployeeName == "") {
                ShowNotification(3, "Please Enter EmployeeName First");
                return;
            }
            if (NonFinancialDetails[i].FinalCorrectionDate == " " || NonFinancialDetails[i].FinalCorrectionDate == "  " || NonFinancialDetails[i].FinalCorrectionDate == "") {
                ShowNotification(3, "Please Enter Final Correction Date First");
                return;
            }

        }


        DeshboardEntryService.saveNonFinancial(NonFinancialMaster, NonFinancialSaveDone, NonFinancialSaveFail);



    }

    function NonFinancialSaveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);
                result.data.operation = "update";


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


    function NonFinancialSaveFail(result) {
        console.log(result);
        //ShowNotification(3, "Something gone wrong");
        ShowNotification(3, "Please Fill All Field First");
    }


    var NonFinacilaPrepamentTable = function () {

        $('#NonFinancialList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#NonFinancialList thead');


        var dataTable = $("#NonFinancialList").DataTable({
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
                url: '/Deshboard/_indexNonFinacila',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {



                            "code": $("#md-Code").val()


                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/Deshboard/NonFinacilaEdit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> "
                            //+

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
                    data: "auditor",
                    name: "Auditor"

                }
                ,
                {
                    data: "employeeName",
                    name: "EmployeeName"

                }
                ,
                {
                    data: "details",
                    name: "Details"

                }

                ,
                {
                    data: "finalCorrectionDate",
                    name: "FinalCorrectionDate"

                }
                ,
                {
                    data: "previousAmount",
                    name: "PreviousAmount"

                }
                ,
                {
                    data: "correctedAmount",
                    name: "CorrectedAmount"

                }
                ,
                {
                    data: "additionalPayment",
                    name: "AdditionalPayment"

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




        $("#NonFinancialList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#NonFinancialList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    return {
        init: init
    }


}(CommonService, DeshboardEntryService);