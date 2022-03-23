var index = {

    logar: function () {

        var dados = {
            fun_usuario: fd.getValById("txtUsuario"), 
            fun_senha: fd.getValById("txtSenha")
        }

        fd.ajax("POST", "login/logar", dados, function (retornoServ) {

            //success

            if (retornoServ.operacao) {
                //redirect
                window.location.href = "home/index";
                
            }
            else {
                alert("Dados inválidos");
            }

        }, function () {//fail
            alert("Não foi possível processar sua requisição.");
        });
    }


}