var CollectionEditLogService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/CollectionEditLog/CreateEdit',
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