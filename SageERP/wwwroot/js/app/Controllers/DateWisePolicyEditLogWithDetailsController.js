var DateWisePolicyEditLogWithDetailsController = function (CommonService, DateWisePolicyEditLogWithDetailsService) {


    var init = function () {




        var dateWisePolicyEditLogWithDetailsTable = DateWisePolicyEditLogWithDetailsTable();
        $(".btnSaveDateWisePolicyEditLogWithDetails").click(function () {             
            save($table);
        });
        var $table = $('#DateWisePolicyEditLogWithDetails');
        $('#DateWisePolicyEditLogWithDetailsAddRow').on('click', function () {
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

        var DateWisePolicyEditLogWithDetailsMaster = {};
        var DateWisePolicyEditLogDetailsData = serializeTable($table);

        DateWisePolicyEditLogWithDetailsMaster.DateWisePolicyEditLogDetailsData = DateWisePolicyEditLogDetailsData;
        DateWisePolicyEditLogWithDetailsMaster.Operation = operation;
        DateWisePolicyEditLogWithDetailsMaster.Id = id;

        var size = DateWisePolicyEditLogDetailsData.length;

        for (var i = 0; i < size; i++) {

            if (DateWisePolicyEditLogDetailsData[i].DocNo == " " || DateWisePolicyEditLogDetailsData[i].DocNo == "") {
                ShowNotification(3, "Please Enter Doc No");
                return;
            }
            if (DateWisePolicyEditLogDetailsData[i].PCName == " " || DateWisePolicyEditLogDetailsData[i].PCName == "") {
                ShowNotification(3, "Please Enter PC Name");
                return;
            }
            if (DateWisePolicyEditLogDetailsData[i].UserId == " " || DateWisePolicyEditLogDetailsData[i].UserId == "") {
                ShowNotification(3, "Please Enter User Id");
                return; 
            }
            if (DateWisePolicyEditLogDetailsData[i].BranchCode == " " || DateWisePolicyEditLogDetailsData[i].BranchCode == "") {
                ShowNotification(3, "Please Enter Entry Date"); 
                return; 
            }
            if (DateWisePolicyEditLogDetailsData[i].EntryDate == " " || DateWisePolicyEditLogDetailsData[i].EntryDate == "  " || DateWisePolicyEditLogDetailsData[i].EntryDate == "") {
                ShowNotification(3, "Please Enter Edit Date");
                return;
            }

        }
 

        DateWisePolicyEditLogWithDetailsService.save(DateWisePolicyEditLogWithDetailsMaster, DateWisePolicyEditLogWithDetailsSaveDone, DateWisePolicyEditLogWithDetailsSaveFail);


    }

    function DateWisePolicyEditLogWithDetailsSaveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);          
                result.data.operation = "update";
                $(".btnSaveDateWisePolicyEditLogWithDetails").html('Update');
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


    function DateWisePolicyEditLogWithDetailsSaveFail(result) {
        console.log(result);
        //ShowNotification(3, "Something gone wrong");
        ShowNotification(3, "Please Fill All Field First");
    }

    var DateWisePolicyEditLogWithDetailsTable = function () {

        $('#DateWisePolicyEditLogWithDetailsList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#DateWisePolicyEditLogWithDetailsList thead');


        var dataTable = $("#DateWisePolicyEditLogWithDetailsList").DataTable({
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
                url: '/DateWisePolicyEditLogWithDetails/_index',
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

                        return "<a href=/DateWisePolicyEditLogWithDetails/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> ";


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
                    data: "vat",
                    name: "Vat"

                }
                ,
                {
                    data: "stamp",
                    name: "Stamp"

                }
                ,
                {
                    data: "gross",
                    name: "Gross"

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




        $("#DateWisePolicyEditLogWithDetailsList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#DateWisePolicyEditLogWithDetailsList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    return {
        init: init
    }


}(CommonService, DateWisePolicyEditLogWithDetailsService);