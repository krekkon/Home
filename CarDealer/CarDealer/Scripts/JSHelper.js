function showMessageBox(messageContent, messageHeader) {
            alert(messageContent);
}

function submitFormGetPartView(fromId, containerId) {
    var createFrom = $("#" + fromId);
    $.ajax({
        type: 'POST',
        url: createFrom.attr("action"),
        data: createFrom.serialize(),
        dataType: 'html',
        success: function (data) {
            $('#' + containerId).html(data);
        }
    });
}