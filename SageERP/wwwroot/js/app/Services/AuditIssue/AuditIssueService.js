﻿var AuditIssueService = function () {
    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/CreateEdit',
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
            url: '/AuditIssue/DeleteFile',
            type: 'POST',
            data: obj,
        })
            .done(done)
            .fail(fail);

    };



    var AuditIssueMultiplePost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/MultiplePost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var AuditIssueMultipleUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var FollowUpAuditIssues = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/FollowUpAuditIssueEamil',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };
    var IssuedeadLineLapsed = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/IssuedeadLineLapsed',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };
    var TotalPendingIssuesReview = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/TotalPendingIssuesReview',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };
    


    return {
        save, deleteFile, AuditIssueMultiplePost, AuditIssueMultipleUnPost, FollowUpAuditIssues, IssuedeadLineLapsed, TotalPendingIssuesReview
    }


}();