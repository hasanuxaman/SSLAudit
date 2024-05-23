var ForgetPasswordController = function (CommonService,ForgetPasswordService) {

    var init = function () {


        $(".btnsave").click(function () {        
            save();
        });

        $(".changePassword").click(function () {
            ChangePassword();
        });
        

    }

    /*init end*/

    function ChangePassword() {
        debugger;
        var validator = $("#frm_ChangePassword").validate();

        var result = validator.form();
        if (!result) {
            validator.focusInvalid();
            return;
        }

        var Cp = serializeInputs("frm_ChangePassword");

        if (Cp.Password == null || Cp.Password == "" || Cp.ConfirmPassword == null || Cp.ConfirmPassword == "") {
            ShowNotification(3, "Please Set Password Or ConfirmPassword First");
            return false;
        }

        Cp.Operation = "update";
        ForgetPasswordService.ChangePasswords(Cp, CpsaveDone, CpsaveFail);

    }

    function CpsaveDone(result) {
        debugger
        if (result.status == "200") {   
            ShowNotification(1, "Password Change Successfully");
            debugger;

            setTimeout(function () {
                
                $.ajax({
                    url: '/Login/Index',
                    method: 'get',
                    success: function (response) {                      
                        $('body').html(response);
                    },
                    error: function (xhr, status, error) {
                       
                    }
                });
            }, 800); 


            //$.ajax({
            //    url: '/Login/Index',
            //    method: 'get',
            //    success: function (response) {                  
            //        $('body').html(response);
            //    },
            //    error: function (xhr, status, error) {           
            //    }
            //});

        }
        else if (result.status == "400") {       
            ShowNotification(3, "Please Set Password Or ConfirmPassword First");
        }
        else if (result.status == "199") {  
            ShowNotification(3, "Please Set Password Or ConfirmPassword First");
        }
    }
    function CpsaveFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    function save() {

        debugger;
        var validator = $("#frm_ForgetPassword").validate();
        var masterObj = $("#frm_ForgetPassword").serialize();
        masterObj = queryStringToObj(masterObj);


        if (masterObj.Email == null || masterObj.Email == "" || masterObj.UserName == null || masterObj.UserName=="") {
            ShowNotification(3, "Please Set Email Address First");
            return false;
        }

        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        ForgetPasswordService.SendEamil(masterObj, saveDone, saveFail);

    }

    function saveDone(result) {
        debugger
        if (result.status == "200") {          
            ShowNotification(1, "Email Send Successfully");
        }
        else if (result.status == "400") {          
            ShowNotification(3, "UserName or Password Is Not Corrent");
        }
        else if (result.status == "199") {
            ShowNotification(3, "UserName or Password Is Not Corrent");
        }
    }

    function saveFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    return {
        init: init
    }

}(CommonService,ForgetPasswordService);