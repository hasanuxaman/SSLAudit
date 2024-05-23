var AuditService = function () {   

    var ReportDataUpdate = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportDataUpdate',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    //EndReport

    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var DbUpdatesaveSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/Settings/DbUpdate',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var sendEmailSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/SendEmailCreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    //var ExvelSave = function (masterObj, done, fail) {
    //var ExvelSave = function (auditMasterList, done, fail) {
    //    var masterObj = {
    //        AuditMasterList: auditMasterList  // Assuming auditMasterList is an array of AuditMaster data
    //    };
    //    $.ajax({
    //        //url: '/Audit/ExcelCreateEdit',
    //        url: '/Audit/ExcelSaveCreateEdit',
    //        method: 'post',
    //        //data: masterObj,
    //        data: auditMasterList,
    //        //data: JSON.stringify(masterObj),
    //        //contentType: 'application/json',
    //        processData: false,
    //        contentType: false,
    //    })
    //        .done(done)
    //        .fail(fail);
    //};

    var ExvelSave = function (auditMaster, done, fail) {


        console.log("Sending auditMasterList:", auditMaster);
        debugger;

        $.ajax({
            url: '/Audit/ExcelSaveCreateEdit',         
            method: 'post',
            data: JSON.stringify(auditMaster),
            contentType: 'application/json',


        })
            .done(done)
            .fail(fail);

    };



    var ReportStatus = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportCreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var AuditStatus = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditStatusCreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var PendingAuditApproval = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/PendingAuditApproval',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };
    
    var AuditIssueComplete = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/IssueComplete',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    var AuditFeedbackComplete = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/FeedbackComplete',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    var AuditBranchFeedbackComplete = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/BranchFeedbackComplete',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

 
    var AuditMultiplePost = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/MultiplePost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var MultipleIssueSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/MultipleIssueSave',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    

    var AuditMultipleUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };


    //END

    //SeeALl


    var saveSeeAllReport = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportSeeAllCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    //EndSeeAll

    //ReportHeading


    var saveReportHeading = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportHeadingCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };




    //EndOfReportHeading




    //SecondReportHeading
    var saveSecondReportHeading = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/SecondReportHeadingCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };
    //EndOfSecondReportHeading


    //AuditAnnexureReport
    var saveAnnexureReport = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AnnexureReportCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };
    //EndOfSecondReportHeading




    var saveArea = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AreaCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };



    var saveEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditUserCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };
    var deleteEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/Delete',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var SendToHOD = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/SendToHOD',
            method: 'post',
            data: masterObj
            
        })
            .done(done)
            .fail(fail);
    };


    var deleteIssueEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/Delete',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var deleteReportEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/ReportDelete',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var IssuesaveEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditIssueUserCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };

    //ReporUserInsert
    var ReportInserUserEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditReportInserUser',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    function saveDoneEmail(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveAuditIssueUser").html('Update');
                $("#AuditUserId").val(result.data.id);
                //$("#AuditUserId").val(result.data.auditIssueId);

                result.data.operation = "update";

                $("#AuditUserOperation").val(result.data.operation);

            } else {

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


    var seeAllModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/SeeAllModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {

            $('#auditSeeAllModal').on('hidden.bs.modal', function () {
                //alert("closed")

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();



            $('.auditSeeAllSummerNote').summernote({
                height: 550,

            });

            if ($("#AuditSeeAllDetails").length) {
            
                var encodedData = $("#AuditSeeAllDetails").val();             
                $('.auditSeeAllSummerNote').summernote('code', encodedData);

            }
           

            //if ($("#AuditSeeAllDetails").length) {
            //    var encodedData = $("#AuditSeeAllDetails").val();
            //    if (encodedData) {
            //        // Ensure padding
            //        var encodedDataWithPadding = encodedData + '='.repeat((4 - encodedData.length % 4) % 4);

            //        // Perform URI decoding first
            //        var decodedURIComponentData = decodeURIComponent(encodedDataWithPadding);

            //        // Now, decode Base64
            //        var decodedData = atob(decodedURIComponentData);

            //        $('.auditSeeAllSummerNote').summernote('code', decodedData);
            //    }
            //}

            setEvents();
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditSeeAllModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditSeeAllModal").modal({ backdrop: 'static', keyboard: false }, "show");
     
        }


        function setEvents() {

            $(".btnSaveSeeAllReport").on("click", function (e) {
                addCallBack(e);
            });
        }
    };


    var auditReportModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditReportModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#auditReportModal').on('hidden.bs.modal', function () {
                //alert("closed")
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();



            $('.auditReportSummerNote').summernote({
                height: 550,

            });

            if ($("#AuditReportDetails").length) {

                var encodedData = $("#AuditReportDetails").val();
                var decodedData = atob(encodedData);
                $('.auditReportSummerNote').summernote('code', decodedData);
                
            }


            setEvents();
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditReportModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditReportModal").modal({ backdrop: 'static', keyboard: false }, "show");

        }


        function setEvents() {

            $(".btnSaveReport").on("click", function (e) {

                addCallBack(e);

            });
        }
    };


    var auditSecondReportModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditSecondReportModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {
            $('#auditSecondReportModal').on('hidden.bs.modal', function () {
              
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);
            }

            modalCloseEvent();

            $('.auditSecondReportSummerNote').summernote({
                height: 550,

            });

            if ($("#AuditSecondReportDetails").length) {

                var encodedData = $("#AuditSecondReportDetails").val();
                //var decodedData = atob(encodedData);
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.auditSecondReportSummerNote').summernote('code', decodedData);
                
            }

            setEvents();
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditSecondReportModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditSecondReportModal").modal({ backdrop: 'static', keyboard: false }, "show");
            //$("#AuditAreas").validate();
        }


        function setEvents() {

            $(".btnSaveSecondReport").on("click", function (e) {
                addCallBack(e);
            });
        }
    };

    var annexureReportModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AnnexureReportModal ',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {
            $('#auditAnnexureReportModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            var summernoteOpened = false;

            $('.auditAnnexureSummerNote').summernote({
                height: 550,              
            });

            if ($("#AuditAnnexureDetails").length) {

                var encodedData = $("#AuditAnnexureDetails").val();
                //var decodedData = atob(encodedData);
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.auditAnnexureSummerNote').summernote('code', decodedData);
                
            }


            setEvents();
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditAnnexureReportModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditAnnexureReportModal").modal({ backdrop: 'static', keyboard: false }, "show");
            //$("#AuditAreas").validate();
        }

        function setEvents() {

            $(".btnSaveAnnexureReport").on("click", function (e) {

                addCallBack(e);

            });
        }
    };

    //AuditReportUserModal

    var AuditReportUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditReportUserModal ',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#auditReportUserModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }
        function onSuccess(result) {
            showModal(result);
            if (typeof done == "function") {
                done(result);

            }
            modalCloseEvent();


            //var summernoteOpened = false;
            //$('.auditAnnexureSummerNote').summernote({
            //    height: 550,
            //});
            //if ($("#AuditAnnexureDetails").length) {
            //    var encodedData = $("#AuditAnnexureDetails").val();
            //    var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
            //    $('.auditAnnexureSummerNote').summernote('code', decodedData);
            //}


            var auditReportUserTable;
            if ($("#auditReportUser").length) {
                var tableConfigs = getAuditReportUserTableConfig()
                auditReportUserTable = $("#auditReportUser").DataTable(tableConfigs);
            }

            setEvents(auditReportUserTable);
            $("#addAuditReportUser").on("click", function (e) {
                auditReportInsertUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditReportInsertUserAdd(result, auditReportUserTable) }, null, null, () => { auditReportUserTable.draw() });
            })

            $('#auditReportUser').on('click', '.reportEmail', function () {
                debugger;
                var rowData = auditReportUserTable.row($(this).closest('tr')).data();
                var AuditReportUserList = [];
                if (rowData) {
                    var userId = rowData.id;
                    var userName = rowData.userName;
                    var emailAddress = rowData.emailAddress;
                    var auditId = rowData.auditId;

                    AuditReportUserList.push({
                        Id: rowData.id,
                        UserName: rowData.userName,
                        EmailAddress: rowData.emailAddress,
                        AuditId: rowData.auditId
                    });
                }
                var AuditId = $("#AuditId").val();
                var masterObj = {
                    AuditReportUserList: AuditReportUserList,
                    AuditId: AuditId
                };

                AuditReportSaveEmail(masterObj, SaveReportEmailDone, ReportEmailFail);

            });


            var AuditReportSaveEmail = function (masterObj, done, fail) {
                console.log("Sending auditReportList:", masterObj);
                debugger;
                $.ajax({
                    url: '/Audit/AuditReportEmailSend',
                    method: 'post',
                    data: masterObj

                })
                    .done(done)
                    .fail(fail);

            };


            function SaveReportEmailDone(result) {
                if (result.status == "200") {
                    ShowNotification(1, "Email has been send successfully ");                   
                }
                else {
                    ShowNotification(3, "Item Is Not Found");
                }
            }
            function ReportEmailFail(result) {
                console.log(result);
                ShowNotification(3, "Item Is Not Found");
            }

        }

        //EndOfSuccess

        function getAuditReportUserTableConfig() {
            debugger;
            return {

                "processing": true,
                serverSide: true,
                "info": false,
                ajax: {
                    //url: '/Audit/_indexAuditReportUser?id=' + $("#IssueId").val(),
                    url: '/Audit/_indexAuditReportUser?id=' + $("#Id").val(),
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
                        data: "userName",
                        name: "UserName"
                    },
                    {
                        data: "emailAddress",
                        name: "emailAddress"
                    },
                    {
                        data: "id",
                        render: function (data) {
                      
                            return "<a   data-id='" + data + "' class='edit auditReportUserEdit' ><i data-id='" + data + "' class='material-icons auditReportUserEdit' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  " +                            
                                "<a data-id='" + data + "' class='reportEmail' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                                ;

                        },
                        "width": "7%",
                        "orderable": false
                    }

                ],
                order: [1, "asc"],

            }
        }

        function onFail(result) {
            fail(result);
        }
        function showModal(html) {
            $("#auditReportUserModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditReportUserModal").modal({ backdrop: 'static', keyboard: false }, "show");        
        }

        function setEvents(auditReportUserTable) {

            $(".btnSaveAnnexureReport").on("click", function (e) {
                addCallBack(e);
            });


            $("#auditReportUser").on("click", ".auditReportUserEdit", function (e) {
                debugger;
                var id = $(this).data("id");
                auditReportInsertUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: id }, (result) => { onAuditReportInsertUserAdd(result, auditReportUserTable) }, null, null, () => { auditReportUserTable.draw() });

            });
        }
    };

    //EndFirst


    var auditReportInsertUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({     
            url: '/Audit/AuditReportInsertUserModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();
            setEvents();


            //if ($("#IssuePriority").length) {
            //    LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            //}

            if ($("#UserId").length) {
                LoadCombo("UserId", '/Common/GetAllUserName');
            }
            //change for Audit Issue Email for delete
            $('.btnDeleteAuditReportUser').click('click', function () {
                Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
                    console.log(result);

                    if (result) {

                        var form = $("#frm_Audit_Report_User")[0];
                        var formData = new FormData(form);
                        AuditService.deleteReportEmail(formData, deleteReportDoneEmail, deleteReportFail);

                    }
                });

            });

            function deleteReportDoneEmail(result) {
                if (result.status == "200") {
                    ShowNotification(1, result.message);
                    //$('#AuditUser').modal('hide');
                    $('#auditReportAddModal').modal('hide');
                }
                else if (result.status == "400") {
                    ShowNotification(3, "Something gone wrong");
                }
            }
            function deleteReportFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
            }
        }

        var modalCloseEvent = function (callBack) {

            //$('#AuditIssueUser').on('hidden.bs.modal', function () {
            $('#auditReportAddModal').on('hidden.bs.modal', function () {
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }


        function onFail(result) {
            if (typeof fail == "function") {
                fail(result);
            }

        }

        function showModal(html) {

            //$("#AuditIssueUser").html(html);
            $("#auditReportAddModal").html(html);
            $('.draggable').draggable({
                handle: ".modal-header"
            });
        
            $("#auditReportAddModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {
          
            $(".btnSaveAuditReportUser").on("click", function (e) {
                addCallBack();               
            });

            $("#UserId").on("change", function (e) {

                $.ajax({
                    url: '/Audit/GetUserInfo?userId=' + $(this).val(),
                })
                    .done((result) => {
                        $("#EmailAddress").val(result.email)
                    })
                    .fail();
            });
        }

    };


    function onAuditReportInsertUserAdd(result) {
        var validator = $("#frm_Audit_Report_User").validate({ 
            rules: {

                EmailAddress: {
                    required: true,
                    email: true 
                },
                IssuePriority: {
                    required: true
                }
                
            },
            messages: {

                EmailAddress: {
                    required: "Email address is required",
                    email: "Please enter a valid email address"
                }

                
            }
        });
        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var form = $("#frm_Audit_Report_User")[0];
        var formData = new FormData(form);       
        formData.set('AuditIssueId', $('#IssueId').val())
        debugger;
        ReportInserUserEmail(formData, saveDoneReportEmail, saveReportFail);


    }


    function saveDoneReportEmail(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveAuditReportUser").html('Update');
                $("#AuditUserId").val(result.data.id);
                result.data.operation = "update";
                $("#AuditUserOperation").val(result.data.operation);

            } else {

                ShowNotification(1, result.message);
            }



        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }

    function saveReportFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    //EndAuditReportUserModal



    var auditAreaModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditAreaModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {


            $('#areaModal').on('hidden.bs.modal', function () {
                //alert("closed")

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();



            $('.auditAreaSummerNote').summernote({
                height: 300,

            });

            if ($("#AreaDetails").length) {

                var encodedData = $("#AreaDetails").val();
                var decodedData = atob(encodedData);
                $('.auditAreaSummerNote').summernote('code', decodedData);


                //CKEDITOR.replace("AreaDetails");
                //CKEDITOR.instances['AreaDetails'].setData(decodeBase64($("#AreaDetails").val()));
            }


            setEvents();
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#areaModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#areaModal").modal({ backdrop: 'static', keyboard: false }, "show");


            //$("#AuditAreas").validate();
        }


        function setEvents() {

            $(".btnSaveArea").on("click", function (e) {

                addCallBack(e);

            });
        }
    };


    //var auditStatusModal = function (masterObj, addCallBack, done, fail, closeCallback) {

    //    $.ajax({
    //        url: '/Audit/AuditStatusModal',
    //        method: 'post',
    //        data: masterObj

    //    })
    //        .done(onSuccess)
    //        .fail(onFail);



    //    var modalCloseEvent = function (callBack) {


    //        $('#areaModal').on('hidden.bs.modal', function () {
    //            //alert("closed")

    //            if (typeof closeCallback == "function") {
    //                closeCallback();
    //            }

    //        });

    //    }

    //    function onSuccess(result) {
    //        showModal(result);

    //        if (typeof done == "function") {
    //            done(result);

    //        }

    //        modalCloseEvent();


    //        //if ($("#AreaDetails").length) {
    //        //    CKEDITOR.replace("AreaDetails");
    //        //    CKEDITOR.instances['AreaDetails'].setData(decodeBase64($("#AreaDetails").val()));
    //        //}


    //        setEvents();
    //    }


    //    function onFail(result) {
    //        fail(result);
    //    }


    //    function showModal(html) {
    //        $("#areaModal").html(html);

    //        $('.draggable').draggable({
    //            handle: ".modal-header"
    //        });

    //        $("#areaModal").modal({ backdrop: 'static', keyboard: false }, "show");


    //        //$("#AuditAreas").validate();
    //    }


    //    function setEvents() {

    //        $(".btnSaveAudit").on("click", function (e) {

    //            addCallBack(e);

    //        });
    //    }
    //};

    var auditIssueModal = function (masterObj, addCallBack, done, fail, closeCallback) {
        debugger;
        

        $.ajax({
            url: '/Audit/AuditIssueModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        //$(document).ready(function () {
        //    $('.issueDetailsTextArea').summernote();
        //});

        //$(document).keydown(function (e) {
        //    if (e.keyCode === 27) {
        //        closeModal();
        //    }
        //});
        //$('.sslUnPost').on('click', function () {
        //    closeModal();
        //});


        var modalCloseEvent = function (callBack) {

            $('#IssueModal').on('hidden.bs.modal', function () {
                //alert("closed")
                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            var auditUserTable;
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }
            modalCloseEvent();

            // 
            InitDateRange();


            if ($("#IssuePriority").length) {
                LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            }
            if ($("#IssueStatus").length) {
                LoadCombo("IssueStatus", '/Common/GetIssueStatus');
            }
            if ($("#AuditType").length) {
                LoadCombo("AuditType", '/Common/GetAuditTypes');
            }


            debugger;
            //$('.issueDetailsTextArea').summernote({
            //    height: 300,
            //});


            $('.issueDetailsTextArea').summernote({
                
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],               

                toolbar: [
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['fontsize', ['fontsize']], 
                    ['fontname', ['fontname']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['height', ['height']],
                    ['view', ['fullscreen', 'codeview']],
                    ['insert', ['link', 'picture', 'video']],
                ],


                height: 300,

               
                callbacks: {
                    onInit: function () {
                        $('.note-editable').css('text-align', 'left');
                    },
                    onKeyup: function (e) {
                        $('.note-editable').css('text-align', 'left');
                    },
                    onPaste: function (e) {
                        setTimeout(function () {
                            $('.note-editable').css('text-align', 'left');
                        }, 10);
                    }
                }

                
               
            });





            if ($("#IssueDetails").length) {
                debugger;
                var encodedData = $("#IssueDetails").val();             
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
             
                $('.issueDetailsTextArea').summernote('code', decodedData);

            }

            if ($("#issueUserAudit").length) {
                var tableConfigs = getAuditUserTableConfig()
                auditUserTable = $("#issueUserAudit").DataTable(tableConfigs);
            }

            setEvents(auditUserTable);
            ///-----

            $("#addIssueAuditUser").on("click", function (e) {
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            })


            //ForAllEmail


            //ForSingle

            $('#issueUserAudit').on('click', '.displayInfo', function () {
                debugger;
                var rowData = auditUserTable.row($(this).closest('tr')).data();
                var AuditIssueUserList = [];


                if (rowData) {
                    var userId = rowData.id;
                    var userName = rowData.userName;
                    var emailAddress = rowData.emailAddress;

                    AuditIssueUserList.push({
                        Id: rowData.id,
                        UserName: rowData.userName,
                        EmailAddress: rowData.emailAddress
                    });
                }

                var AuditId = $("#AuditId").val();

                var masterObj = {
                    AuditIssueUserList: AuditIssueUserList,
                    AuditId: AuditId
                };

                AuditIssueSaveAllEmail(masterObj, SaveAllEmailDone, AllEmailFail);



            });

            $("#AllIssueAuditUserEmail").on("click", function (e) {
                debugger;

                var masterObj = {};
                var AuditIssueUserList = [];

                
                auditUserTable.rows().every(function (index, element) {
                    var data = this.data();                  
                    AuditIssueUserList.push({
                        Id: data.id,
                        UserName: data.userName,
                        EmailAddress: data.emailAddress
                    });
                    return true;
                });


                var AuditId = $("#AuditId").val();

                masterObj.AuditIssueUserList = AuditIssueUserList;
                masterObj.AuditId = AuditId;

                AuditIssueSaveAllEmail(masterObj, SaveAllEmailDone, AllEmailFail);
            })


            var AuditIssueSaveAllEmail = function (masterObj, done, fail) {

                console.log("Sending auditMasterList:", masterObj);
                debugger;

                $.ajax({
                    url: '/Audit/AuditIssueUserAllEmailCreateEdit',
                    method: 'post',
                    data: masterObj

                })
                    .done(done)
                    .fail(fail);

            };

            function SaveAllEmailDone(result) {
                debugger;
                if (result.status == "200") {

                    //if (result.data.operation == "add") {

                    ShowNotification(1, "Email has been sent successfully");

                    //ShowNotification(1, result.message);
                    //$(".btnSaveAuditUser").html('Update');
                    //$("#AuditUserId").val(result.data.id);
                    //result.data.operation = "update";
                    //$("#AuditUserOperation").val(result.data.operation);
                    //} else {
                    //ShowNotification(1, result.message);
                    //}

                }

                else if (result.status == "400") {
                    ShowNotification(3, "Feedback is not completed,Please complete feedback first");
                }
                else {
                    ShowNotification(3, "Feedback is not completed,Please complete feedback first");
                }
            }
            function AllEmailFail(result) {
                console.log(result);
                ShowNotification(3, "Feedback is not completed,Please complete feedback first");
            }

            //EndOfAllEmail

            //Change for audit issue
          
            $('.AuditIssuePost').click('click', function () {

                Confirmation("Are you sure? Do You Want to Post Data For Audit Issue?", function (result) {
                    console.log(result);
                    if (result) {


                        var issue = serializeInputs("frm_Audit_Issue");
                        if (issue.IsPost == "Y") {
                            ShowNotification(3, "Data has already been Posted.");
                        }
                        else {
                            issue.IDs = issue.Id;
                            AuditIssueService.AuditIssueMultiplePost(issue, AuditIssueMultiplePosts, AuditIssueMultiplePostFail);
                        }
                    }


                });

            });


            function AuditIssueMultiplePosts(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('Y');
                    $(".btnUnPost").show();
                    //$(".btnReject").show();
                    //$(".btnApproved").show();
                    //$(".btnPush").show();
                    //Visibility(true);

                    var dataTable = $('#AuditIssueDetails').DataTable();
                    dataTable.draw();

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message);
                }
            }

            function AuditIssueMultiplePostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditIssueDetails').DataTable();
                dataTable.draw();

            }

            $('.IssueSubmit').click('click', function () {

                UnPostReasonOfIssue = $("#UnPostReasonOfIssue").val();

                var issue = serializeInputs("frm_Audit_Issue");

                issue["UnPostReasonOfIssue"] = UnPostReasonOfIssue;

                Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
                    if (UnPostReasonOfIssue === "" || UnPostReasonOfIssue === null) {
                        ShowNotification(3, "Please Write down Reason Of UnPost");
                        $("#UnPostReasonOfIssue").focus();
                        return;
                    }

                    if (result) {


                        issue.IDs = issue.Id;
                        AuditIssueService.AuditIssueMultipleUnPost(issue, AuditIssueMultipleUnPost, AuditIssueMultipleUnPostFail);
                    }
                });
            });
            function AuditIssueMultipleUnPost(result) {
                console.log(result.message);

                if (result.status == "200") {
                    ShowNotification(1, result.message);
                    $("#IsPost").val('N');
                    //Visibility(false);
                    $("#divReasonOfUnPost").hide();
                    $(".btnUnPost").hide();
                    // $(".btnReject").hide();
                    //$(".btnApproved").hide();

                    var dataTable = $('#AuditIssueDetails').DataTable();

                    dataTable.draw();


                    $('#modal-default').modal('hide');


                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message); // <-- display the error message here
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message); // <-- display the error message here
                }
            }

            function AuditIssueMultipleUnPostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditIssueDetails').DataTable();

                dataTable.draw();
            }
            //end
        }

        function onAuditUserAdd(result) {
            var validator = $("#frm_Audit_Issue_User").validate({ 
                rules: {

                    EmailAddress: {
                        required: true,
                        email: true // ensure the input is email
                    },
                    IssuePriority: {
                        required: true
                    }
                    //Remarks: {
                    //    required: true
                    //}
                },
                messages: {

                    EmailAddress: {
                        required: "Email address is required",
                        email: "Please enter a valid email address"
                    }
                    //Remarks: {
                    //    required: "Remarks is required"
                    //}
                }
            });
            var result = validator.form();

            if (!result) {
                ShowNotification(2, "Please complete the form");
                return;
            }

            var form = $("#frm_Audit_Issue_User")[0];
            var formData = new FormData(form);


            //formData.forEach((value, key) => {
            //    console.log(`key: ${key}, value: ${value}`);
            //});
            formData.set('AuditIssueId', $('#IssueId').val())
            IssuesaveEmail(formData, saveDoneEmail, saveFail);

        }

        function getAuditUserTableConfig() {

            return {

                "processing": true,
                serverSide: true,
                "info": false,
                ajax: {
                    url: '/Audit/_indexAuditIssueUser?id=' + $("#IssueId").val(),
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
                        data: "userName",
                        name: "UserName"
                    },
                    {
                        data: "emailAddress",
                        name: "emailAddress"
                    },
                    {
                        data: "id",
                        render: function (data) {

                            return "<a   data-id='" + data + "' class='edit auditIssueUserEdit' ><i data-id='" + data + "' class='material-icons auditIssueUserEdit' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  " +

                                //"<a data-id='" + data + "' class='displayInfo' data-toggle='tooltip' title='' data-original-title='Display Info'><i data-id='" + data + "' class='material-icons displayInfo'>info</i></a>"
                                //"<a data-id='" + data + "' class='displayInfo' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>info</i>    </a>"
                               "<a data-id='" + data + "' class='displayInfo' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                                ;
                        },
                        "width": "7%",
                        "orderable": false
                    }

                ],
                order: [1, "asc"],

            }
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#IssueModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#IssueModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }


        function setEvents(auditUserTable) {
  
            $(".btnSaveIssue").on("click", function (e) {

                addCallBack(auditUserTable);
                //$("#IssueModal").modal("hide");

            });

            $("#newButton").on("click", function (e) {

            });

            $("#issueUserAudit").on("click", ".auditIssueUserEdit", function (e) {

                var id = $(this).data("id");
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: id }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            });
        }

    };


    //for email icon add


    //function displayInfo(userId, userName, emailAddress) {

    //    console.log("User ID: " + userId);
    //    console.log("User Name: " + userName);
    //    console.log("Email Address: " + emailAddress);
    //}  
    //$(document).ready(function () {    
    //    $('#issueUserAudit').on('click', '.displayInfo', function () {

    //        var $icon = $(this).find('i.material-icons');


    //        if (!$(this).data('clicked')) {

    //            $icon.off('click');
    //            $(this).data('clicked', true);


    //            var rowData = auditUserTable.row($(this).closest('tr')).data();
    //            var AuditIssueUserList = [];

    //            if (rowData) {
    //                var userId = rowData.id;
    //                var userName = rowData.userName;
    //                var emailAddress = rowData.emailAddress;

    //                AuditIssueUserList.push({
    //                    Id: rowData.id,
    //                    UserName: rowData.userName,
    //                    EmailAddress: rowData.emailAddress
    //                });
    //            }

    //            var AuditId = $("#AuditId").val();

    //            var masterObj = {
    //                AuditIssueUserList: AuditIssueUserList,
    //                AuditId: AuditId
    //            };

    //            AuditIssueSaveAllEmail(masterObj, SaveAllEmailDone, AllEmailFail);

    //        }


    //    });
    //});

    //var AuditIssueSaveAllEmail = function (masterObj, done, fail) {

    //    console.log("Sending auditMasterList:", masterObj);
    //    debugger;

    //    $.ajax({
    //        url: '/Audit/AuditIssueUserAllEmailCreateEdit',
    //        method: 'post',
    //        data: masterObj

    //    })
    //        .done(done)
    //        .fail(fail);



    //};


    //function SaveAllEmailDone(result) {

    //    if (result.status == "200") {




    //        ShowNotification(1, "Successfully Done");

    //    }



    //    else {
    //        ShowNotification(3, "Item Is Not Found");

    //    }

    //}
    //function AllEmailFail(result) {
    //    console.log(result);
    //    ShowNotification(3, "Item Is Not Found");
    //}

    //end email icon add



    var auditReportStatusEditModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditReportStatusModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {

            $('#auditReportStatusModal').on('hidden.bs.modal', function () {
               //alert("closed")
                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            var auditUserTable;
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#ReportStatusModal").length) {
                LoadCombo("ReportStatusModal", '/Common/GetReportStatus');
            }
            if ($("#IssuePriorityUpdate").length) {
                LoadCombo("IssuePriorityUpdate", '/Common/GetIssuePriority');
            }

            InitDateRange();
        }

        function setEvents() {
            //$(".btnSaveFeedback").on("click", function (e) {
            $(".btnSaveReport").on("click", function (e) {
                addCallBack();
                //$("#IssueModal").modal("hide");
            });
        }



        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditReportStatusModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditReportStatusModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

    };

    var auditStatusModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditStatusModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {

            $('#forauditstatusModal').on('hidden.bs.modal', function () {
                //alert("closed")
                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            var auditUserTable;
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#AuditStatus").length) {
                LoadCombo("AuditStatus", '/Common/GetAuditStatus');
            }


            if ($("#BranchIDStatus").length) {
                LoadCombo("BranchIDStatus", '/Common/Branch');
            }
            InitDateRange();

        }

        function setEvents() {
            //$(".btnSaveFeedback").on("click", function (e) {
            $(".btnSaveStatus").on("click", function (e) {
                addCallBack();
                //$("#IssueModal").modal("hide");
            });
        }

        function onFail(result) {
            fail(result);
        }

        function showModal(html) {
            $("#forauditstatusModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#forauditstatusModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }




    };



    var auditIssueUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditIssueUserModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#IssuePriority").length) {
                LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            }
            if ($("#UserId").length) {
                LoadCombo("UserId", '/Common/GetAllUserName');
            }

            //ChangeForAuditIssueEmailForDelete

            $('.btnDeleteAuditIssueUser').click('click', function () {

                Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
                    console.log(result);

                    if (result) {
                        var form = $("#frm_Audit_Issue_User")[0];
                        var formData = new FormData(form);
                        AuditService.deleteIssueEmail(formData, deleteIssueDoneEmail, deleteIssueFail);
                    }
                });

            });

            function deleteIssueDoneEmail(result) {
                if (result.status == "200") {
                    ShowNotification(1, result.message);
                    $('#AuditUser').modal('hide');
                }
                else if (result.status == "400") {
                    ShowNotification(3, "Something gone wrong");
                }
            }

            function deleteIssueFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
            }
        }

        var modalCloseEvent = function (callBack) {

            $('#AuditIssueUser').on('hidden.bs.modal', function () {
                //alert("closed")

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });
        }

        function onFail(result) {
            if (typeof fail == "function") {
                fail(result);
            }

        }

        function showModal(html) {
            $("#AuditIssueUser").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#AuditIssueUser").modal({ backdrop: 'static', keyboard: false }, "show");
        }


        function setEvents() {

            $(".btnSaveAuditIssueUser").on("click", function (e) {

                addCallBack();
                //$("#IssueModal").modal("hide");

            });

            $("#UserId").on("change", function (e) {
                $.ajax({
                    url: '/Audit/GetUserInfo?userId=' + $(this).val(),

                })
                    .done((result) => {

                        $("#EmailAddress").val(result.email)

                    })
                    .fail();
            });
        }

    };


    var auditFeedbackModal = function (masterObj, addCallBack, done, fail, closeCallback, { auditId }) {

        $.ajax({
            url: '/Audit/AuditFeedbackModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        var modalCloseEvent = function (callBack) {

            $('#FeedbackModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            $("#seeAuditIssuePreviewForFeedback").on("click", function (e) {

                var IssueIdForFeedback = $('#AuditIssueId').val();

                if (IssueIdForFeedback == null) {

                    ShowNotification(3, "You Have to Select a Item");
                    return false;

                }          
                auditIssuePreviewModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: IssueIdForFeedback });
            })

            //End Of Audit Issue Preview Modal

            //InitDateRange();

            if ($("#AuditIssueId").length) {

                if (auditId) {
                    LoadCombo("AuditIssueId", '/Common/GetIssues?auditid=' + auditId);

                }
                else {
                    LoadCombo("AuditIssueId", '/Common/GetIssues?auditid=' + auditId);

                }
            }

            $('.issueFeedbackSummerNote').summernote({
                height: 300,

            });
            
            if($("#FeedbackDetails").length) {

                debugger;               
                var encodedData = $("#FeedbackDetails").val();

                //var decodedData = atob(encodedData);
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.issueFeedbackSummerNote').summernote('code', decodedData);

            }

            //For Post Change Of AuditFeedback
            
            $('.AuditFeedback').click('click', function () {

                Confirmation("Are you sure? Do You Want to Post Data for AuditFeedback?", function (result) {
                    console.log(result);
                    if (result) {


                        var feedback = serializeInputs("frm_Audit_feedback");
                        if (feedback.IsPost == "Y") {
                            ShowNotification(3, "Data has already been Posted.");
                        }
                        else {
                            feedback.IDs = feedback.Id;
                            feedback.Id = feedback.Id;
                            AuditFeedbackService.AuditFeedbackPost(feedback, feedbackMultiplePost, feedbackMultiplePostFail);
                        }
                    }
                });

            });


            function feedbackMultiplePost(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('Y');
                    $(".btnUnPost").show();
                    
                    var dataTable = $('#AuditFeedbackDetails').DataTable();
                    dataTable.draw();

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message);
                }
            }

            function feedbackMultiplePostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditFeedbackDetails').DataTable();
                dataTable.draw();

            }



            $('.FeedbackSubmit').click('click', function () {

                UnPostReasonOfFeedback = $("#UnPostReasonOfFeedback").val();

                var feedback = serializeInputs("frm_Audit_feedback");

                feedback["UnPostReasonOfFeedback"] = UnPostReasonOfFeedback;

                Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
                    if (UnPostReasonOfFeedback === "" || UnPostReasonOfFeedback === null) {
                        ShowNotification(3, "Please Write down Reason Of UnPost");
                        $("#UnPostReasonOfFeedback").focus();
                        return;
                    }

                    if (result) {

                        feedback.IDs = feedback.Id;
                        AuditFeedbackService.AuditFeedbackUnPost(feedback, feedbackMultipleUnPost, feedbackMultipleUnPostFail);

                    }
                });
            });
            function feedbackMultipleUnPost(result) {
                console.log(result.message);

                if (result.status == "200") {
                    ShowNotification(1, result.message);
                    $("#IsPost").val('N');
                    //Visibility(false);
                    $("#divReasonOfUnPost").hide();
                    $(".btnUnPost").hide();
                    // $(".btnReject").hide();
                    //$(".btnApproved").hide();

                    var dataTable = $('#AuditFeedbackDetails').DataTable();

                    dataTable.draw();
                    $('#modal-default').modal('hide');

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message); 
                }
            }

            function feedbackMultipleUnPostFail(result) {
                ShowNotification(3, result.message);
                var dataTable = $('#AuditFeedbackDetails').DataTable();

                dataTable.draw();
            }
        
            $('.btnFeedback').click('click', function () {

                var validator = $("#frm_Audit_feedback").validate({
                    rules: {
                        AuditIssueId: {
                            required: true
                        },
                        Heading: {
                            required: true
                        },
                        IssueDetails: {
                            required: true
                        }
                    },
                    messages: {
                        AuditIssueId: {
                            required: "Please select the audit issue."
                        },
                        Heading: {
                            required: "Please enter the feedback heading."
                        },
                        IssueDetails: {
                            required: "Please provide the feedback details."
                        }
                    }
                }
                );
                var result = validator.form();

                if (!result) {
                    ShowNotification(2, "Please complete the form");
                    return;
                }

                //if (!CKEDITOR.instances['IssueDetailsFeedback'].getData()) {
                //    ShowNotification(2, "Please Enter Issue Details");
                //    return;
                //}

                var form = $("#frm_Audit_feedback")[0];
                var formData = new FormData(form);


                //Remarks Change

                $('.issueFeedbackSummerNote').summernote();
                var summernotes = $('.issueFeedbackSummerNote').summernote('code');
                //var encodedSummernotes = btoa(summernotes);
                var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

                //End

                //formData.set("IssueDetails", encodeBase64(CKEDITOR.instances['IssueDetailsFeedback'].getData()));
                //formData.set("FeedbackDetails", encodeBase64(CKEDITOR.instances['IssueDetailsFeedback'].getData()));
                formData.set("FeedbackDetails", encodedSummernotes);

                //formData.append("feedbackOperation", "add");
                formData.set("Operation", "add");

                AuditFeedbackService.save(formData, saveDoneFeedback, saveFail);

            });


            function saveDoneFeedback(result) {
                if (result.status == "200") {
                    if (result.data.operation == "add") {

                        ShowNotification(1, result.message);
                        $(".btnSaveFeedback").html('Update');
                        $("#feedbackId").val(result.data.id);
                        result.data.operation = "update";
                        $("#feedbackOperation").val(result.data.operation);
                        addListItemFeedBack(result);

                       
                    } else {

                        //addListItem(result);
                        //change
                         addListItemFeedBack(result);
                        //end
                         ShowNotification(1, result.message);
                    }

                    $("#fileToUpload").val('');

                }
                else if (result.status == "400") {
                    //ShowNotification(3, "Something gone wrong");
                    ShowNotification(3, result.message);
                }
            }

            function saveFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
            }

            function addListItemFeedBack(result) {
                var list = $(".fileGroup");

                result.data.attachmentsList.forEach(function (item) {

                    var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                        item.displayName +
                        '</span><a target="_blank" href="/AuditFeedback/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                        '<button onclick="AuditController.deleteFeedbackFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                        '</li>';

                    list.append(item);
                });
            }
        }

        function onFail(result) {

            //function onFail(result) {
            //    fail(result);
            //    console.log(result);
            // }
            if (typeof fail == "function") {
                fail(result);
            }
            console.log(result)
        }


        function showModal(html) {
            $("#FeedbackModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });
            $("#FeedbackModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {     
            $(".btnSaveFeedback").on("click", function (e) {
                addCallBack();
                //$("#IssueModal").modal("hide");
            });
        }
    };


    var auditBranchFeedbackModal = function (masterObj, addCallBack, done, fail, closeCallback, { auditId }) {

        $.ajax({
            url: '/Audit/AuditBranchFeedbackModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#BranchFeedbackModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });
        }

        function onSuccess(result) {

            showModal(result);

            if (typeof done == "function") {
                done(result);

            }
            modalCloseEvent();
            setEvents();

            debugger;
            $("#showTestField").click(function () {
                $("#testFieldContainer").toggle();
            });

            $("#seeAuditIssuePreview").on("click", function (e) {

                var IssueId = $('#AuditBranchIssueId').val();

                if (IssueId == null) {

                    ShowNotification(3, "You Have to Select a Item");
                    return false;
                }     
                auditIssuePreviewModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: IssueId });               
            })

            InitDateRange();

            if ($("#AuditBranchIssueId").length) {

                if (auditId) {                
                    LoadCombo("AuditBranchIssueId", '/Common/GetBranchFeedbackIssues?auditid=' + auditId);
                }
                else {
                    
                    LoadCombo("AuditBranchIssueId", '/Common/GetBranchFeedbackIssues?auditid=' + auditId);
                }
            }


            //if ($("#Status").length) {
            //    LoadCombo("Status", '/Common/GetIssueStatus');
            //}
            //if ($("#IssueStatus").length) {
                LoadCombo("IssueStatus", '/Common/GetIssueStatus');
            //}

            debugger;
            $('.issueBranchFeedbackSummerNote').summernote({
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI'],
                height: 300,
            });

            if ($("#IssueBranchDetailsFeedback").length) {

                debugger;
                var encodedData = $("#IssueBranchDetailsFeedback").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.issueBranchFeedbackSummerNote').summernote('code', decodedData);
               
            }

            $('.AuditBranchFeedback').click('click', function () {

                Confirmation("Are you sure? Do You Want to Post Data for BranchFeedback?", function (result) {
                    console.log(result);
                    if (result) {

                        var branchfeedback = serializeInputs("frm_Audit_Branch_feedback");
                        if (branchfeedback.IsPost == "Y") {
                            ShowNotification(3, "Data has already been Posted.");
                        }
                        else {
                            branchfeedback.IDs = branchfeedback.Id;
                            AuditFeedbackService.AuditBranchFeedbackPost(branchfeedback, BranchFeedbackMultiplePost, BranchFeedbackMultiplePostFail);
                        }
                    }
                });

            });

            function BranchFeedbackMultiplePost(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('Y');
                    $(".btnUnPost").show();

                    var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                    dataTable.draw();

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message);
                }
            }

            function BranchFeedbackMultiplePostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                dataTable.draw();

            }
           
            $('.BranchFeedbackSubmit').click('click', function () {

                UnPostReasonOfBranchFeedback = $("#UnPostReasonOfBranchFeedback").val();
                var branchfeedback = serializeInputs("frm_Audit_Branch_feedback");
                branchfeedback["UnPostReasonOfBranchFeedback"] = UnPostReasonOfBranchFeedback;

                Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
                    if (UnPostReasonOfBranchFeedback === "" || UnPostReasonOfBranchFeedback === null) {
                        ShowNotification(3, "Please Write down Reason Of UnPost");
                        $("#UnPostReasonOfBranchFeedback").focus();
                        return;
                    }
                    if (result) {

                        branchfeedback.IDs = branchfeedback.Id;
                        AuditFeedbackService.AuditBranchBranchFeedbackUnPost(branchfeedback, BranchFeedbackMultipleUnPost, BranchFeedbackMultipleUnPostFail);

                    }
                });
            });
            function BranchFeedbackMultipleUnPost(result) {
                console.log(result.message);

                if (result.status == "200") {
                    ShowNotification(1, result.message);

                    $("#IsPost").val('N');                    
                    $("#divReasonOfUnPost").hide();
                    $(".btnUnPost").hide();
                    
                    var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                    dataTable.draw();
                    $('#modal-default').modal('hide');

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message); 
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message); 
                }
            }

            function BranchFeedbackMultipleUnPostFail(result) {
                ShowNotification(3, result.message);
                var dataTable = $('#AuditBranchFeedbackDetails').DataTable();

                dataTable.draw();
            }
       
            $('.btnBranchFeedback').click('click', function () {

                var validator = $("#frm_Audit_Branch_feedback").validate({
                    rules: {

                        //AuditIssueId: {
                        //    required: true
                        //},
                        AuditBranchIssueId: {
                            required: true
                        },

                        Heading: {
                            required: true
                        },
                        IssueDetails: {
                            required: true
                        }
                    },
                    messages: {

                        //AuditIssueId: {
                        //    required: "Please select the audit issue."
                        //},
                        AuditBranchIssueId: {
                            required: "Please select the audit issue."
                        },

                        Heading: {
                            required: "Please enter the feedback heading."
                        },
                        IssueDetails: {
                            required: "Please provide the feedback details."
                        }
                    }
                }
                );
                var result = validator.form();

                if (!result) {
                    ShowNotification(2, "Please complete the form");
                    return;
                }

                var IssueStatus = $("#IssueStatus").val();

                var form = $("#frm_Audit_Branch_feedback")[0];
                var formData = new FormData(form);

                $('.issueBranchFeedbackSummerNote').summernote();
                var summernotes = $('.issueBranchFeedbackSummerNote').summernote('code');              
                var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));
              
                //formData.set("IssueDetails", encodeBase64(CKEDITOR.instances['IssueDetailsFeedback'].getData()));
                //formData.set("IssueDetails", encodeBase64(CKEDITOR.instances['IssueBranchDetailsFeedback'].getData()));
                //formData.set("IssueDetails", encodedSummernotes);

                formData.set("BranchFeedbackDetails", encodedSummernotes);
                formData.set("IssueStatus", IssueStatus);


                var ForBranchEmail = $('#AuditBranchIssueId').val();

                formData.set("Operation", "add");
                formData.set("TeamCheck", "Team");
                formData.set("BranchEmailIssueId", ForBranchEmail);
                AuditFeedbackService.FeedbackBranchSave(formData, saveDoneBranchFeedback, saveFail);

                //}
            });

            function saveDoneBranchFeedback(result) {
                if (result.status == "200") {
                    if (result.data.operation == "add") {

                        ShowNotification(1, result.message);                     
                        $(".btnBranchSaveFeedback").html('Update');
                        $("#BranchfeedbackId").val(result.data.id);
                        $("#Id").val(result.data.auditId);
                        $("#divFeedback").show();
                        result.data.operation = "update";

                        $("#feedbackBranchOperation").val(result.data.operation);
                        //$("#feedbackOperation").val(result.data.operation);
                        //addListItem(result);

                        addListItemBranchFeedBack(result);

                    } else {

                        //addListItem(result);
                        addListItemBranchFeedBack(result);
                        ShowNotification(1, result.message);
                    }

                    $("#fileToUpload").val('');

                }
                else if (result.status == "400") {
                    //ShowNotification(3, "Something gone wrong");
                    ShowNotification(3, result.message);
                }
            }

            function saveFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
            }

            function addListItemBranchFeedBack(result) {
                var list = $(".fileGroup");

                result.data.attachmentsList.forEach(function (item) {

                    var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                        item.displayName +
                        '</span><a target="_blank" href="/AuditBranchFeedback/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                        '<button onclick="AuditController.deleteBranchFeedbackFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                        '</li>';
                    list.append(item);
                });
            }
        }

        function onFail(result) {
            //function onFail(result) {
            //    fail(result);
            //    console.log(result);
            // }
            if (typeof fail == "function") {
                fail(result);
            }
            console.log(result)
        }

        function showModal(html) {
            $("#BranchFeedbackModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });
            $("#BranchFeedbackModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {
            //$(".btnSaveFeedback").on("click", function (e) {
            $(".btnBranchSaveFeedback").on("click", function (e) {
                addCallBack();            
            });
       
            //For Getting ImplementationDate from Issue
            $("#AuditBranchIssueId").on("change", function (e) {

                $.ajax({
                    url: '/Audit/GetIssueDeadLine?issueId=' + $(this).val(),

                })
                    .done((result) => {
                      
                        $("#ImplementationDate").val(result.implementationDate)
                        $("#DeadLineDate").val(result.issueDeadLine)
                        $("#Status").val(result.status)

                    })
                    .fail();
            });

        }

    };



    //ModalForAuditIssuePreview

    var auditIssuePreviewModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditIssuePreviewModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        var modalCloseEvent = function (callBack) {
            $('#AuditIssuePreview').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            //if ($("#FeedbackDetailsPreview").length) {
            //    CKEDITOR.replace("FeedbackDetailsPreview");
            //    CKEDITOR.instances['FeedbackDetailsPreview'].setData(decodeBase64($("#FeedbackDetailsPreview").val()));
            //}

            $('.auditIssuePreviewSummernote').summernote({
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI'],
                height: 300,
            });
            if ($("#FeedbackDetailsPreview").length) {

                debugger;
                var encodedData = $("#FeedbackDetailsPreview").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.auditIssuePreviewSummernote').summernote('code', decodedData);

            }

            
            setEvents();

            //UserEamil Part

            $("#addIssueAuditUser").on("click", function (e) {
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            })

            //End Of UserEmail
        }

        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#AuditIssuePreview").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#AuditIssuePreview").modal({ backdrop: 'static', keyboard: false }, "show");

        }

        function setEvents() {

            $(".btnSaveArea").on("click", function (e) {

                addCallBack(e);

            });
        }
    };

    //End Of Modal Of Audit Issue Preview


    var auditUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditUserModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {


            $('#AuditUser').on('hidden.bs.modal', function () {
                //alert("closed")
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#IssuePriority").length) {
                LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            }
            if ($("#UserId").length) {
                LoadCombo("UserId", '/Common/GetAllUserName');
            }

            //Change for email for delete

            $('.btnDeleteAuditUser').click('click', function () {

                Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
                    console.log(result);

                    if (result) {

                        var teamValue = $("#TeamValue").val();
                        var form = $("#frm_Audit_User")[0];
                        var formData = new FormData(form);
                        //formData.TeamId = teamId;
                        formData.append("TeamValue", teamValue);

                        AuditService.deleteEmail(formData, deleteDoneEmail, deleteFail);

                        //var issue = serializeInputs("frm_Audit_Issue");
                        //if (tours.IsPost == "Y") {
                        //    ShowNotification(3, "Data has already been Posted.");
                        //}
                        //else {
                        //    issue.IDs = issue.Id;
                        //    ToursMultiplePost.ToursMultiplePost(issue, ToursMultiplePosts, ToursMultiplePostFail);
                        //}

                    }
                });

            });

            function deleteDoneEmail(result) {
                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    //$("#userModal").modal('hide'); 
                    $('#AuditUser').modal('hide');

                }
                else if (result.status == "400") {
                    //ShowNotification(3, "Something gone wrong");
                    ShowNotification(3, result.message);
                }
            }
            function deleteFail(result) {
                console.log(result);
                //ShowNotification(3, "Something gone wrong");
                ShowNotification(3, result.message);
            }

            //end
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#AuditUser").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#AuditUser").modal({ backdrop: 'static', keyboard: false }, "show");
        }


        function setEvents() {

            $(".btnSaveAuditUser").on("click", function (e) {
                addCallBack();
                //$("#IssueModal").modal("hide");
            });

            $("#UserId").on("change", function (e) {
                $.ajax({
                    url: '/Audit/GetUserInfo?userId=' + $(this).val(),
                })
                    .done((result) => {

                        $("#EmailAddress").val(result.email)

                    })
                    .fail();

            });
        }

    };

    return {
        save, auditAreaModal, auditIssueModal
        , auditFeedbackModal, saveArea, auditUserModal, saveEmail
        , AuditMultiplePost, AuditMultipleUnPost, auditBranchFeedbackModal
        , deleteEmail, auditStatusModal, auditReportStatusEditModal
        , ReportStatus, AuditStatus, deleteIssueEmail, ExvelSave, sendEmailSave, AuditIssueComplete, AuditBranchFeedbackComplete, AuditFeedbackComplete,
         auditIssuePreviewModal, SendToHOD, auditReportModal, saveReportHeading, saveSecondReportHeading, auditSecondReportModal, annexureReportModal, saveAnnexureReport, seeAllModal, saveSeeAllReport
        , ReportDataUpdate, PendingAuditApproval, AuditReportUserModal, auditReportInsertUserModal, deleteReportEmail, DbUpdatesaveSave, MultipleIssueSave
    }

}();