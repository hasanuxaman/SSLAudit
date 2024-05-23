var DateWisePolicyEditLogService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/DateWisePolicyEditLog/CreateEdit',
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