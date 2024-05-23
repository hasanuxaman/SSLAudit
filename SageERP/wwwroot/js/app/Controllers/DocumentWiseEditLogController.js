var DocumentWiseEditLogController = function (CommonService, DocumentWiseEditLogService) {


    var init = function () {


        var documentWiseEditLogTable = DocumentWiseEditLogTable();
        $(".btnSaveDocumentWiseEditLog").click(function () {             
            save($table);
        });
        var $table = $('#DocumentWiseEditLogDetails');
        $('#DocumentWiseEditLogAddRow').on('click', function () {
            addRow($table);
        });
        var table = initEditTable($table, { searchHandleAfterEdit: false });


    }

    //End Of Init

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

        var DocumentWiseEditLogMaster = {};
        var DocumentWiseEditLogDetails = serializeTable($table);

        DocumentWiseEditLogMaster.DocumentWiseEditLogDetails = DocumentWiseEditLogDetails;
        DocumentWiseEditLogMaster.Operation = operation;
        DocumentWiseEditLogMaster.Id = id;

        var size = DocumentWiseEditLogDetails.length;

        for (var i = 0; i < size; i++) {

            if (DocumentWiseEditLogDetails[i].DocNo == " " || DocumentWiseEditLogDetails[i].DocNo == "") {
                ShowNotification(3, "Please Enter Doc No");
                return;
            }
            if (DocumentWiseEditLogDetails[i].PCName == " " || DocumentWiseEditLogDetails[i].PCName == "") {
                ShowNotification(3, "Please Enter PC Name");
                return;
            }
            if (DocumentWiseEditLogDetails[i].UserId == " " || DocumentWiseEditLogDetails[i].UserId == "") {
                ShowNotification(3, "Please Enter User Id");
                return; 
            }
            if (DocumentWiseEditLogDetails[i].BranchCode == " " || DocumentWiseEditLogDetails[i].BranchCode == "") {
                ShowNotification(3, "Please Enter Branch Code"); 
                return; 
            }
            if (DocumentWiseEditLogDetails[i].EntryDate == " " || DocumentWiseEditLogDetails[i].EntryDate == "  " || DocumentWiseEditLogDetails[i].EntryDate == "") {
                ShowNotification(3, "Please Enter Entry Date");
                return;
            }

        }
 

        DocumentWiseEditLogService.save(DocumentWiseEditLogMaster, DocumentWiseEditLogSaveDone, DocumentWiseEditLogSaveFail);


    }

    function DocumentWiseEditLogSaveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);          
                result.data.operation = "update";
                $(".btnSaveDocumentWiseEditLog").html('Update');
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


    function DocumentWiseEditLogSaveFail(result) {
        console.log(result);
        //ShowNotification(3, "Something gone wrong");
        ShowNotification(3, "Please Fill All Field First");
    }

    var DocumentWiseEditLogTable = function () {

        $('#DocumentWiseEditLogList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#DocumentWiseEditLogList thead');


        var dataTable = $("#DocumentWiseEditLogList").DataTable({
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
                url: '/DocumentWiseEditLog/_index',
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

                        return "<a href=/DocumentWiseEditLog/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> ";


                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "docNo",
                    name: "DocNo"

                }
                ,
                {
                    data: "pcName",
                    name: "PCName"

                }
                ,
                {
                    data: "userId",
                    name: "UserId"

                }
                ,
                {
                    data: "entryDate",
                    name: "EntryDate"

                }
                ,
                {
                    data: "classCode",
                    name: "ClassCode"

                }
                ,
                {
                    data: "status",
                    name: "Status"

                }
                ,
                {
                    data: "docDate",
                    name: "DocDate"

                }
                ,
                {
                    data: "customerId",
                    name: "CustomerId"

                }
                ,
                {
                    data: "netPremium",
                    name: "NetPremium"

                } 
                ,
                {
                    data: "sumInsured",
                    name: "SumInsured"

                } 
                ,
                {
                    data: "vatAmount",
                    name: "VatAmount"

                } 
                ,
                {
                    data: "stampAmount",
                    name: "StampAmount"

                } 
                ,
                {
                    data: "producerCode",
                    name: "ProducerCode"

                } 
                ,
                {
                    data: "businessStatus",
                    name: "BusinessStatus"

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




        $("#DocumentWiseEditLogList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#DocumentWiseEditLogList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    return {
        init: init
    }


}(CommonService, DocumentWiseEditLogService);