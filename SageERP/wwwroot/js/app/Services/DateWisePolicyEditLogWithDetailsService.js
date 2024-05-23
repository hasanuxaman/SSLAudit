var DateWisePolicyEditLogWithDetailsService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/DateWisePolicyEditLogWithDetails/CreateEdit',
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