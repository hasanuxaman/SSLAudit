var BranchProfileService = function () {
    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/BranchProfile/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    var deleteBranch = function (masterObj, done, fail) {
        $.ajax({
            url: '/BranchProfile/BranchProfileDelete',
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
        deleteBranch: deleteBranch,
    
    }
}();