let login = (function () {

    let public = {};

    public.init = () => {
        events();
        console.log("WOLOLOOOOOOO");
    };

    let events = function () {
        $("#btn_send").click(function (e) {
            e.preventDefault();
            console.log("Preparando el Ajax");
            let _user = getUser();
            let jsonData = JSON.stringify(_user);
           
            $.ajax({
                url: "/Home/LoginLog",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: "POST",

            }).done(function (data) {
                console.log("Respuesta: ", data);
                //Validando la respuesta
                if (data.code == 500) {
                    swal({
                        title: data.message,
                        text: "",
                        icon: "error",
                        button: "Aceptar",
                    });
                } else if (data.code == 200) {
                    location.assign(location.origin + "/Home/GoReports");
                }
               
            }).fail(function (data) {
                console.log("Respuesta: ", data);
              
            });
        });
    }

    function getUser() {
        let _user = $("#inputUsuario").val();
        let _pass = $("#inputContraseña").val();
        return (_user == "") ? null : { user: _user, password: _pass };
    }

    return public;

})();
setTimeout(function () {
    login.init();
}, 1000);
