var UsersPermissionController = function (CommonService, UsersPermissionService) {

    var init = function () {

        var table = $('#RoleDetails');     
        if ($("#UserId").length) {
            LoadCombo("UserId", '/Common/UserId');
        }
        var indexTable = UsersTable();
        $(".btnsave").click(function () {
            save();
        });  
        
        var nodesTable = null;

        $("#ModuleList").on("click", ".td-Module-Name", function () {

            if ($.fn.DataTable.isDataTable("#NodesList")) {
                $("#NodesList").DataTable().destroy();              
            }
            var selectedValue = $("#UserId").val();
            var UserName = $("#UserId option:selected").text();
            var row = $(this).closest("tr");
            var moduleId = row.find("[id^='moduleId_']").text();
            var isChecked = $(this).prop('checked');
            //console.log("Module Id: " + moduleId);
            //var indexTable = UsersTable();
            //var nodesTable = NodesTable() + '?' + new Date().getTime();
            nodesTable = NodesTable(moduleId, UserName);
            //$("#NodesList").DataTable().destroy();

        });

        $("#CheckBox1").on("click", function () {
            var check = $("#CheckBox1").is(":checked");
        });
    }

    /*init end*/

    function save() {
        var validator = $("#frm_UsersPermission").validate();
        var userPermission = serializeInputs("frm_UsersPermission");
        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        var ModuleList = [];
        var NodeList = [];

        debugger;
        $("#ModuleList tbody tr").each(function () {
            var rowData = {
                Id: $(this).find("td:first").text(),
                UserPermissionId: $(this).find("td:nth-child(2)").text(),
                Modul: $(this).find("td:nth-child(3)").text(),
                IsActive: $(this).find("input[type=checkbox]").prop("checked")
            };

            ModuleList.push(rowData);
        });

        $("#NodesList tbody tr").each(function () {
            var rowData = {
                Id: $(this).find("td:first").text(),
                Node: $(this).find("td:nth-child(2)").text(),
                //Modul: $(this).find("td:nth-child(3)").text(),
                IsAllowByUser: $(this).find("input[type=checkbox]").prop("checked")
            };

            NodeList.push(rowData);
        });
        

        userPermission.ModuleList = ModuleList;
        userPermission.NodeList = NodeList;

        var selectedText = $("#UserId option:selected").text();
        userPermission.UserName = selectedText;
        UsersPermissionService.save(userPermission, saveDone, saveFail);

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




    //Node
    var NodesTable = function (moduleId, UserName) {
        
        var dataTable = $("#NodesList").DataTable({
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
                url : '/UsersPermission/_indexNodes?edit=' + moduleId + '&UserName=' + UserName,
                type: 'POST',
                data: function (payLoad) {

                    return $.extend({},
                        payLoad,
                        {
                            "username": $("#md-UsersName").val()
                        });
                }
            },

            columns: [

                {
                    data: "id",
                    name: "Id",
                    //visible: false,
                    "width": "7%",
                    "orderable": false,
                    
                }            
                ,
                {
                    data: "node",
                    name: "Node"

                }
                ,
                {
                    data: "isAllowByUser",
                    render: function (data, type, row) {
                        
                        var module = row.module;                     
                        var isChecked = data ? 'checked' : '';                    
                        return '<input type="checkbox" class="form-check-input module-checkbox" style="margin-left: 15px; transform: scale(1.9);" id="CheckBox' + data + '" ' + isChecked + ' />';
                    },
                    "width": "9%",
                    "orderable": false
                },
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

        $("#NodesList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#NodesList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        return dataTable;
    }




    var UsersTable = function () {

        $('#UsersList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#UsersList thead');


        var dataTable = $("#UsersList").DataTable({
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
                url: '/UsersPermission/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "username": $("#md-UsersName").val()

                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/UsersPermission/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> " //+

                            //"<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"



                            ;


                    },
                    "width": "9%",
                    "orderable": false
                }
                //,                           
                //{
                //    data: "code",
                //    name: "Code"

                //}
                ,
                {
                    data: "userName",
                    name: "UserName"

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




        $("#UsersList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#UsersList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    return {
        init: init
    }


}(CommonService, UsersPermissionService);