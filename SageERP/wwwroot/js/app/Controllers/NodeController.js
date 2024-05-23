var NodeController = function (CommonService, NodeService) {



    var init = function () {



        if ($("#UserId").length) {
            LoadCombo("UserId", '/Common/UserId');
        }
        if ($("#ModulId").length) {
            LoadCombo("ModulId", '/Common/ModulId');
        }
        if ($("#NodeName").length) {
            LoadCombo("NodeName", '/Common/NodeName');
        }
        

        var indexTable = NodeTable();


        $(".btnsave").click(function () {
            save();
        });  



        $("#NodeName").on("change", function (e) {

            $.ajax({
                url: '/Node/GetNodeInfo?nodeId=' + $(this).val(),

            })
                .done((result) => {

                    $("#Url").val(result.node.url)
                    $("#Node").val(result.node.node)
                    $("#ActionName").val(result.node.actionName)
                    $("#ControllerName").val(result.node.controllerName)

                })
                .fail();


        });



    }

    /*init end*/


//Delete Node


    $('.btnNodeDelete').click('click', function () {

        Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
            console.log(result);

            if (result) {
         
                var form = $("#frm_Node")[0];
                var formData = new FormData(form);             
                NodeService.deleteNode(formData, deleteDoneNode, deleteFail);

            }
        });

    });


    function deleteDoneNode(result) {
        if (result.status == "200") {

            ShowNotification(1, result.message);
            $('#AuditUser').modal('hide');

        }
        else if (result.status == "400") {          
            ShowNotification(3, result.message);
        }
    }

    function deleteFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }



//end




    function save() {


        var validator = $("#frm_Node").validate();
        var node = serializeInputs("frm_Node");

        //var result = validator.form();

        //if (!result) {
        //    validator.focusInvalid();
        //    return;
        //}
        var selectedText = $("#UserId option:selected").text();
        node.UserId = selectedText;

        NodeService.save(node, saveDone, saveFail);



    }


    function saveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);
                $(".btnsave").html('Update');

                $(".btnSave").addClass('sslUpdate');

                $("#Id").val(result.data.id);
                $("#Code").val(result.data.code);

                $("#divUpdate").show();

                $("#divSave").hide();

                $("#SavePost").show();

                result.data.operation = "update";
                $("#Operation").val(result.data.operation);

            } else {
                ShowNotification(1, result.message);

                $("#divSave").hide();

                $("#divUpdate").show();


            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error); 
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message || result.error); 
        }
    }


    function saveFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    var NodeTable = function () {

        $('#NodeList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#NodeList thead');


        var dataTable = $("#NodeList").DataTable({
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
                url: '/Node/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                           

                            "userName": $("#md-UserName").val(),
                            "node": $("#md-Node").val()
                            
                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/Node/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> " //+

                           // "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"
  
                            ;
                   

                    },
                    "width": "9%",
                    "orderable": false
                },                           
                {
                    data: "userId",
                    name: "UserId"

                }
                ,
                {
                    data: "node",
                    name: "Node"

                }
                ,
                {
                    data: "url",
                    name: "Url"

                }
                ,
                {
                    data: "actionName",
                    name: "ActionName"

                }
                ,
                {
                    data: "controllerName",
                    name: "ControllerName"

                }
                ,
                {
                    data: "isAllowByUser",
                    name: "IsAllowByUser"

                }
                
                
                

            ],


           //order: [1, "desc"],
           order: [1, "asc"],


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




        $("#NodeList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#NodeList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    


    return {
        init: init
    }


}(CommonService, NodeService);