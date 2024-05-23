var DateWisePolicyEditLogController = function (CommonService, DateWisePolicyEditLogService) {


    var init = function () {




        var dateWisePolicyEditLogTable = DateWisePolicyEditLogTable();
        $(".btnSaveDateWisePolicyEditLog").click(function () {
             
            save($table);
        });
        var $table = $('#DateWisePolicyEditLogDetails');
        $('#DateWisePolicyEditLogAddRow').on('click', function () {
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

        var DateWisePolicyEditLogMaster = {};
        var DateWisePolicyEditLogDetails = serializeTable($table);

        DateWisePolicyEditLogMaster.DateWisePolicyEditLogDetails = DateWisePolicyEditLogDetails;
        DateWisePolicyEditLogMaster.Operation = operation;
        DateWisePolicyEditLogMaster.Id = id;

        var size = DateWisePolicyEditLogDetails.length;

        for (var i = 0; i < size; i++) {

            if (DateWisePolicyEditLogDetails[i].DocNo == " " || DateWisePolicyEditLogDetails[i].DocNo == "") {
                ShowNotification(3, "Please Enter Doc No");
                return;
            }
            if (DateWisePolicyEditLogDetails[i].PCName == " " || DateWisePolicyEditLogDetails[i].PCName == "") {
                ShowNotification(3, "Please Enter PC Name");
                return;
            }
            if (DateWisePolicyEditLogDetails[i].UserId == " " || DateWisePolicyEditLogDetails[i].UserId == "") {
                ShowNotification(3, "Please Enter User Id");
                return; 
            }
            if (DateWisePolicyEditLogDetails[i].BranchCode == " " || DateWisePolicyEditLogDetails[i].BranchCode == "") {
                ShowNotification(3, "Please Enter Entry Date"); 
                return; 
            }
            if (DateWisePolicyEditLogDetails[i].EntryDate == " " || DateWisePolicyEditLogDetails[i].EntryDate == "  " || DateWisePolicyEditLogDetails[i].EntryDate == "") {
                ShowNotification(3, "Please Enter Edit Date");
                return;
            }

        }
 

        DateWisePolicyEditLogService.save(DateWisePolicyEditLogMaster, DateWisePolicyEditLogSaveDone, DateWisePolicyEditLogSaveFail);


    }

    function DateWisePolicyEditLogSaveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);          
                result.data.operation = "update";
                $(".btnSaveDateWisePolicyEditLog").html('Update');
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


    function DateWisePolicyEditLogSaveFail(result) {
        console.log(result);
        //ShowNotification(3, "Something gone wrong");
        ShowNotification(3, "Please Fill All Field First");
    }

    var DateWisePolicyEditLogTable = function () {

        $('#DateWisePolicyEditLogList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#DateWisePolicyEditLogList thead');


        var dataTable = $("#DateWisePolicyEditLogList").DataTable({
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
                url: '/DateWisePolicyEditLog/_index',
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

                        return "<a href=/DateWisePolicyEditLog/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> ";


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
                    data: "branchCode",
                    name: "BranchCode"

                }
                ,
                {
                    data: "class",
                    name: "Class"

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
                    data: "produeerCode",
                    name: "ProdueerCode"

                }
                ,
                {
                    data: "business",
                    name: "Business"

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




        $("#DateWisePolicyEditLogList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#DateWisePolicyEditLogList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    return {
        init: init
    }


}(CommonService, DateWisePolicyEditLogService);