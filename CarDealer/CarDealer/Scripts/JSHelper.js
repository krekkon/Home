function showMessageBox(messageHeader, messageContent) {
    $('<div id="dialog-message" title="' + messageHeader + '">' +
         '<p>' + messageContent + '</p>'+
      '</div>').dialog({
          modal: true,
          show: {
              effect: "fadeIn",
              duration: 1000
          },
          hide: {
              effect: "fadeOut",
              duration: 1000
          },
          
          buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
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