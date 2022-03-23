var fodaoJS = {

    getById: function (id) {
        return document.getElementById(id);
    },

     getValById: function (id) {
        return document.getElementById(id).value;
    },

    toggleLoading: function (show) {

        var html = '<div id="divLoading">' +
            '<img src="/images/loading.gif" />' +
            '</div>';

        if (show) {
            var div = document.createElement("div");
            div.id = "divContainerLoading";
            div.innerHTML = html;
            document.body.appendChild(div);
        }
        else {


            var div = document.getElementById("divContainerLoading");
            document.body.removeChild(div);
        }
    },

    ajax: function (method, action, dados, success, fail) {

        fd.toggleLoading(true);
        var xhttp = new XMLHttpRequest();
        //method, action, assincrono
        xhttp.open(method, action, true);
        //configurando o formato de ida dos dados (JSON)
        xhttp.setRequestHeader('Content-Type', 'application/json');

        var json = JSON.stringify(dados); //serializando o obj literal

        //https://developer.mozilla.org/pt-BR/docs/Web/API/XMLHttpRequest/readyState
        //é um evento que é disparado toda vez que a requisição 
        //muda seu status (0...4 (done))
        xhttp.onreadystatechange = function () {
            if (xhttp.readyState == 4) {

                //status -> códigos HTTP
                if (xhttp.status == 200) {

                    //deserializando os dados enviados para o servidor.
                    var retornoServer = JSON.parse(xhttp.responseText);
                    success(retornoServer);

                }
                else {
                    fail();
                }
                fd.toggleLoading(false);

            }

        }

        xhttp.send(json);

    }


}

var fd = fodaoJS;