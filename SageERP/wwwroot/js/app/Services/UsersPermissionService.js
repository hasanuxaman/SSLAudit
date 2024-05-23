var UsersPermissionService = function () {

    var save = function (masterObj, done, fail) {
        debugger;
        $.ajax({
            url: '/UsersPermission/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };


    return {
        save: save,

    }
}();