$( document ).ready(function() {
    $(document).ajaxStart(function () {
        $("#loading").removeClass("hide");
    });
    $(document).ajaxStop(function () {
        $("#loading").addClass("hide");
    });
}); 
