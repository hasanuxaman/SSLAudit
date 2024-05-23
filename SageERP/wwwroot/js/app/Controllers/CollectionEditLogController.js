var CollectionEditLogController = function (CommonService, CollectionEditLogService) {


    var init = function () {




        var collectionEditLogTable = CollectionEditLogTable();
        $(".btnSaveCollectionEditLog").click(function () {             
            save($table);
        });
        var $table = $('#CollectionEditLogDetails');
        $('#CollectionEditLogAddRow').on('click', function () {
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

        var CollectionEditLogMaster = {};
        var CollectionEditLogDetails = serializeTable($table);

        CollectionEditLogMaster.CollectionEditLogDetails = CollectionEditLogDetails;
        CollectionEditLogMaster.Operation = operation;
        CollectionEditLogMaster.Id = id;

        var size = CollectionEditLogDetails.length;

        for (var i = 0; i < size; i++) {

            if (CollectionEditLogDetails[i].MR == " " || CollectionEditLogDetails[i].MR == "") {
                ShowNotification(3, "Please Enter MR");
                return;
            }
            if (CollectionEditLogDetails[i].PCName == " " || CollectionEditLogDetails[i].PCName == "") {
                ShowNotification(3, "Please Enter PC Name");
                return;
            }
            if (CollectionEditLogDetails[i].UserId == " " || CollectionEditLogDetails[i].UserId == "") {
                ShowNotification(3, "Please Enter User Id");
                return; 
            }
            if (CollectionEditLogDetails[i].Status == " " || CollectionEditLogDetails[i].Status == "") {
                ShowNotification(3, "Please Enter Entry Date"); 
                return; 
            }
            if (CollectionEditLogDetails[i].EditDate == " " || CollectionEditLogDetails[i].EntryDate == "  " || CollectionEditLogDetails[i].EntryDate == "") {
                ShowNotification(3, "Please Enter Edit Date");
                return;
            }

        }
 

        CollectionEditLogService.save(CollectionEditLogMaster, CollectionEditLogSaveDone, CollectionEditLogSaveFail);


    }

    function CollectionEditLogSaveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);          
                result.data.operation = "update";
                $(".btnSaveCollectionEditLog").html('Update');
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


    function CollectionEditLogSaveFail(result) {
        console.log(result);
        //ShowNotification(3, "Something gone wrong");
        ShowNotification(3, "Please Fill All Field First");
    }

    var CollectionEditLogTable = function () {

        $('#CollectionEditLogList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#CollectionEditLogList thead');


        var dataTable = $("#CollectionEditLogList").DataTable({
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
                url: '/CollectionEditLog/_index',
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

                        return "<a href=/CollectionEditLog/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> ";


                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "mr",
                    name: "MR"

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
                    data: "slNo",
                    name: "SlNo"

                }
                ,
                {
                    data: "deposit",
                    name: "Deposit"

                }
                ,
                {
                    data: "depositDate",
                    name: "DepositDate"

                }
                ,
                {
                    data: "record",
                    name: "Record"

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




        $("#CollectionEditLogList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#CollectionEditLogList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    return {
        init: init
    }


}(CommonService, CollectionEditLogService);