var ForgetPasswordService = function () {

    var SendEamil = function (masterObj, done, fail) {

        $.ajax({
            url: '/ForgetPassword/MailSent',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };
    
    var ChangePasswords = function (masterObj, done, fail) {

        $.ajax({
            url: '/ForgetPassword/CreateEdit',
            method: 'post',
            data: masterObj

            //processData: false,
            //contentType: false,

        })
            .done(done)
            .fail(fail);

    };

    return {
        SendEamil: SendEamil,
        ChangePasswords: ChangePasswords
    }

}();