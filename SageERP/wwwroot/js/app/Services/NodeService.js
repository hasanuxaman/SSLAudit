var NodeService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/Node/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    
    var deleteNode = function (masterObj, done, fail) {

        $.ajax({
            url: '/Node/Delete',
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
        deleteNode: deleteNode,



    }
}();