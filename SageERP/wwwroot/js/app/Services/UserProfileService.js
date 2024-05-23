var UserProfileService = function () {
    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/UserProfile/CreateEdit',
            method: 'post',
            data: masterObj,

            //NewAdd
            processData: false,
            contentType: false,


        })
            .done(done)
            .fail(fail);

    };



    var DeActiveUser = function (masterObj, done, fail) {
        $.ajax({
            url: '/UserProfile/DeActiveUser',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false
        })
            .done(done)
            .fail(fail);

    };




    return {
        save: save,
        DeActiveUser: DeActiveUser

    }
}();