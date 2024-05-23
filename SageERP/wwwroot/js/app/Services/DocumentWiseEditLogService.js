var DocumentWiseEditLogService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/DocumentWiseEditLog/CreateEdit',
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