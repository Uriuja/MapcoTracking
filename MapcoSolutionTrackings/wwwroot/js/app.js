let app = (function () {

    let public = {};

    public.init = () => {
        validateForm();
        events();
        fillData();
        fillStatus();
    };

    let events = function () {
        console.log("CARGANDO...");

        $('#tableContainer,#tableContainerPrecalification').DataTable({
            //dom: 'Bfrtip',
            //buttons: [
            //    "copy", "excel", "csv", "pdf"
            //],
            "language": {
                "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
            }
        });

        $("#desde,#hasta,#desdeP,#hastaP").datepicker({
            dateFormat: "yy-mm-dd",
            dayNames: ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"],
            dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
            monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
            maxDate: new Date(),
            minDate: "-3m"
        });
        $('#tableContainer').on('autoFill', function (e, datatable, cells) {
            alert((cells.length * cells[0].length) + ' cells were updated');
        });
        $("#btnClean").click(function () {
            $("#frmSearch")[0].reset();
            $("#resultContainer").addClass("hide");
        });
        $("#btnSendPrecalifications").click(function(){
            let _data = {
                desde: $("#desdeP").val(),
                hasta: $("#hastaP").val(),
                aprobado: $("#aprobado").val(),
            };
            let jsonData = JSON.stringify(_data);
            $.ajax({
                url: "/Search/Generar_Precalificacion",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: "POST",

            }).done(function (data) {
                console.log("Respuesta: ", data);
                let _collection = data.data;
                let _tabla = $('#tableContainerPrecalification').DataTable();
                _tabla.destroy();
                $("#rowsP").html("");
                for (let i = 0; i < _collection.length; i++) {
                    let _row = _collection[i];
                    $("#rowsP").append('<tr><td>' + _row.noConsulta + '</td><td>' + _row.preCalificación + '</td><td>'
                        + _row.confirmado + '</td><td>' + _row.fecha + '</td><td>' + _row.nombre + '</td><td>'
                        + _row.apePat + '</td><td>' + _row.apeMat + '</td><td>'
                        + _row.fechaNacimiento + '</td><td>' + _row.ciudad + '</td><td>' + _row.municipio + '</td><td>'
                        + _row.estado + '</td><td>' + _row.promotor + '</td><td>' + _row.tienda + '</td></tr>');
                }

                $('#tableContainerPrecalification').DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        "copy", "excel", "csv", "pdf", //"print"
                    ],
                    "language": {
                        "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
                    }
                });
            }).fail(function (data) {
                console.log("Respuesta: ", data);
            });
        })

        $("#btnSend").click(function () {
            let _data = {
                desde: $("#desde").val(),
                hasta: $("#hasta").val(),
                name: $("#name").val(),
                Amaterno: $("#ap_mat").val(),
                APaterno: $("#ap_pat").val(),
                Ddl_Estatus: $("#status").val(),
                solicitud: $("#solicitud").val(),
                promotor: $("#promotor").val(),
                tienda: $("#tienda").val(),
            };
            let jsonData = JSON.stringify(_data);
            let _level = sessionStorage.getItem("PrincipalLevel");
            let _url = "";
            if (_level == "Administrador") {
                _url = "/Search/Search";
            } else {
                _url = "/Search/Generar_ReporteNormal";
            }
            $.ajax({
                url: _url,
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: "POST",

            }).done(function (data) {
                console.log("Respuesta: ", data);
                let _collection = data.data;
                let _tabla = $('#tableContainer').DataTable();
                _tabla.destroy();
                $("#rows").html("");
                for (let i = 0; i < _collection.length; i++){
                    let _row = _collection[i];
                    $("#rows").append('<tr><td>' + _row.noConsulta + '</td><td>' + _row.fecha + '</td><td>'
                        + _row.nombre + '</td><td>' + _row.apePat + '</td><td>' + _row.apeMat + '</td><td>'
                        + _row.estatus + '</td><td>' + _row.motivoStatus + '</td><td>'
                        + _row.estatusRecepcion + '</td><td>' + _row.promotor + '</td><td>' + _row.tienda + '</td></tr>');
                }
         
                $('#tableContainer').DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        "copy", "excel", "csv", "pdf", //"print"
                    ],
                    "language": {
                        "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
                    }
                });
                   
            
             }).fail(function (data) {
                console.log("Respuesta: ", data);
             });
            
        });

        $("#btnPrecalifications").click(function () {
            $("#container1").addClass("hide");
            $("#container2").removeClass("hide");
        });

        $("#btnReturn").click(function () {
            $("#container2").addClass("hide");
            $("#container1").removeClass("hide");
        });

        $("#btnCloseSesion").click(function () {
            sessionStorage.clear();
            location.assign(location.origin + "");
        });
    }

   

    function fillStatus() {
        $.ajax({
            url: "/Search/GetStatus",
            contentType: "application/json; charset=utf-8",
            type: "POST",
        }).done(function (response) {
            console.log("Respuesta: ", response);
            for (r = 0; r < response.data.length; r++) {
                $("#status").append('<option value= "' + response.data[r].status + '">' + response.data[r].status.toUpperCase() + '</>');
            }
          
        }).fail(function (data) {
            console.log("Respuesta: ", data);
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
                $("#region").append('<option value= "' + region[r] + '">' + region[r].toUpperCase() + '</>');
            }
            for (s = 0; s < subregion.length; s++) {
                $("#subregion").append('<option value= "' + subregion[s] + '">' + subregion[s].toUpperCase() + '</>');
            }
            for (t = 0; t < tienda.length; t++) {
                $("#tienda").append('<option value= "' + tienda[t] + '">' + tienda[t].toUpperCase() + '</>');
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

    function validateForm() {
        let _level = sessionStorage.getItem("PrincipalLevel");
        if (_level != "Administrado") {
            $("#filtrosCascada,#btnPrecalifications,#btnAll").addClass("hide");
        }
    }


    return public;

})();
setTimeout(function () {
    app.init();
}, 1000);
