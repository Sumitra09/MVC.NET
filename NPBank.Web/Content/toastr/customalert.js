function MessageBoxTop(type, title, msg) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-right",
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr[type](msg, title);
}

function ShowMessage(type, title, msg) {
    
    var notific = $('html body div#toast-container');
    if (notific.html()) {
        $(notific).remove(); // remove previously shown notifications msg
    }
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-right",
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    toastr[type](msg, title);
}

function showNotification(type) {
    switch (type) {
        case 'success':
            ShowMessage(type, 'Success', 'Data saved successfully');
            break;
        case 'duplicate':
            ShowMessage('warning', 'Warning', 'Data already exists');
            break;
        case 'error':
            ShowMessage('warning', 'Warning', 'Error while saving data');
            break;
        default:
            ShowMessage('error', 'Error', 'Data not saved');
            break;
    }
}