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

//function postDeleteAllSelected(controller, action) {

//    var ids = [];

//    $('.deleteCHB input:checked').each(function () {
//        ids.push(this.id.split("_")[1]);
//    });

//    if (ids.length == 0)
//        return;

//    $.ajax({
//        url: controller + '/' + action,
//        type: 'POST',
//        cache: false,
//        contentType: 'application/json; charset=utf-8',
//        data: JSON.stringify({ entityIds: ids, rd: new Date().getTime() }),
//        success: function (data) {
//            document.open();
//            document.write(data);
//            document.close();
//        }
//    });
//}