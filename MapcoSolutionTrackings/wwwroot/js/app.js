let app = (function () {

    let public = {};

    public.init = () => {
        events();
        fillData();
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

    function fillData() {
        console.log("Llenando los datos");
        $.ajax({
            url: "/Search/GetData",
            contentType: "application/json; charset=utf-8",
            type: "POST",
        }).done(function (response) {
            console.log("Respuesta: ", response);
            //Ciclando el data
            let region = [];
            let subregion = [];
            let tienda = [];
            for (let i = 0; i < response.data.length; i++) {
                let _element = response.data[i];
                region.push(_element.Region);
                subregion.push(_element.Subregion);
                tienda.push(_element.Tienda);
            }
            //Se borran los Duplicados
            region = eliminateDuplicates(region);
            subregion = eliminateDuplicates(subregion);
            tienda = eliminateDuplicates(tienda);
            //Se pintan
            for (r = 0; r < region.length; r++) {
                $("#region").append('<option value= "' + region[r] + '">'+region[r]+'</>');
            }
            for (s = 0; s < subregion.length; s++) {
                $("#subregion").append('<option value= "' + subregion[s] + '">' + subregion[s] + '</>');
            }
            for (t = 0; t < tienda.length; t++) {
                $("#tienda").append('<option value= "' + tienda[t] + '">' + tienda[t] + '</>');
            }
            
        }).fail(function (data) {
            console.log("Respuesta: ", data);
        });
    }

    function eliminateDuplicates(arr) {
        var i,
            len = arr.length,
            out = [],
            obj = {};

        for (i = 0; i < len; i++) {
            obj[arr[i]] = 0;
        }
        for (i in obj) {
            out.push(i);
        }
        return out;
    }


    return public;

})();
setTimeout(function () {
    app.init();
}, 1000);
