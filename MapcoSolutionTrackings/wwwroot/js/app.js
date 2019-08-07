let app = (function () {

    let public = {};

    public.init = () => {
        events();
    };

    let events = function () {
        console.log("CARGANDO...");
        $("#desde,#hasta").datepicker({
            dateFormat: "yy-mm-dd",
            dayNames: ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"],
            dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
            monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
            maxDate: new Date(),
            minDate: "-2m"
        });
        $("#btnClean").click(function () {
            $("#frmSearch")[0].reset();
            $("#resultContainer").addClass("hide");
        });
        $("#btnSend").click(function () {
            $("#loading").removeClass("hide");
            setTimeout(function () {
                $("#resultContainer").removeClass("hide");
                $("#loading").addClass("hide");
            }, 1000);
        });
    }

    return public;

})();
setTimeout(function () {
    app.init();
}, 1000);
