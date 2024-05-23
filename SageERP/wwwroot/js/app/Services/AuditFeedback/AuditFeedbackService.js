var AuditFeedbackService = function () {
    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/CreateEdit',
            method: 'post',
            data: masterObj,

            processData: false,
            contentType: false,

        })
            .done(done)
            .fail(fail);

    };


    //var saveFeed = function (masterObj, done, fail) {
    //    $.ajax({
    //        url: '/AuditFeedback/CreateEdit',
    //        method: 'post',
    //        data: masterObj
    //    })
    //        .done(done)
    //        .fail(fail);
    //};


    var FeedbackBranchSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/CreateEdit',
            method: 'post',
            data: masterObj,

            processData: false,
            contentType: false,

        })
            .done(done)
            .fail(fail);

    };

    // { id: fileId, filePath: filePath }
    var deleteFile = function (obj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/DeleteFile',
            type: 'POST',
            data: obj,
        })
            .done(done)
            .fail(fail);

    };


    //change for auditFeedBack Branch

    var deleteFileBranch = function (obj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/DeleteFile',
            type: 'POST',
            data: obj,
        })
            .done(done)
            .fail(fail);

    };



    //for branch feedback


    var AuditBranchFeedbackPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/MultiplePost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    //end



    //For AuditFeedback Post

    var AuditFeedbackPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/MultiplePost',
            method: 'post',
            data: masterObj

           

        })
            .done(done)
            .fail(fail);

    };

    //End Of AuditFeedback Post





    //for branch feedback

    var AuditBranchBranchFeedbackUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    //end


    //For UnPost Of BranchFeedback
    var AuditFeedbackUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };





    return {
        save, deleteFile, FeedbackBranchSave, deleteFileBranch, AuditBranchFeedbackPost, AuditBranchBranchFeedbackUnPost, AuditFeedbackPost, AuditFeedbackUnPost
    }


}();