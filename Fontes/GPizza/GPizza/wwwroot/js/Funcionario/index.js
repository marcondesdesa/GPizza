
//objeto literal
var index = {

    gravar: function () {     
        var obj = {
            fun_codigo: fd.getValById("fun_codigo"),
            fun_nome: fd.getValById("fun_nome"),
            fun_nivel: fd.getValById("fun_nivel"),
            fun_usuario: fd.getValById("fun_usuario"),
            fun_senha: fd.getValById("fun_senha")
        }

        if ((obj.fun_nome.trim() == "") || (obj.fun_nivel.trim() == "") || (obj.fun_usuario.trim() == "")) {
            alert("Preencha corretamente todos os campos !");
            fd.getById("fun_nome").focus();
            return;
        }

        if (((obj.fun_codigo == 0)||(obj.fun_codigo.trim() == "0")) && (obj.fun_senha.trim() == "")) {
            alert("Preencha corretamente todos os campos !");
            fd.getById("fun_nome").focus();
            return;
        }

        fd.ajax("POST", "/Funcionario/Gravar", obj, index.gravarSuccess, index.gravarFail);
    },

    gravarSuccess: function (retornoServer) {
        //fd.getById("divMsg").innerHTML = "";
        //fd.getById("divMsg").innerHTML = retornoServer.msg;
        //fd.getById("fun_codigo").value = retornoServer.fun_codigo;

        if (retornoServer.msg != "") {
            alert(retornoServer.msg);
            fd.getById("fun_nome").focus();
            return;
        }

        if (retornoServer.fun_codigo == 0) {        
            alert("Dados gravados com sucesso !");
        }
        else if (retornoServer.msg == ""){ 
            alert("Dados alterados com sucesso !");
        }
        window.location.href = window.location.href; 
        //fd.getById("fun_codigo").value = "";
        //fd.getById("fun_nome").value = "";
        //fd.getById("fun_nivel").value = "";
        //fd.getById("fun_usuario").value = "";
        //fd.getById("fun_senha").value = "";
        //fd.getById("divMsg").innerHTML = "";
        //fd.getById("fun_nome").focus();
    },

    gravarFail: function () {
        alert("Erro na requisição");
    },

    novo: function () {

        window.location.href = window.location.href;
    },

    abrirPesquisa: function () {

        $.fancybox.open({
            src: "IndexPesquisar",
            type: 'iframe',
            smallBtn: true
        });
    },
    editar: function (id) {

        $.fancybox.close();

        var url = "/Funcionario/obter/" + id
        fd.ajax("GET", url, null, function (retServ) {

            //fd.getById("hId").value = retServ.id;
            fd.getById("fun_codigo").value = retServ.fun_codigo;
            fd.getById("fun_nome").value = retServ.fun_nome;
            fd.getById("fun_nivel").value = retServ.fun_nivel;
            fd.getById("fun_usuario").value = retServ.fun_usuario;
            fd.getById("fun_senha").value = retServ.fun_senha;

        }, function () {

            alert("Não foi possível obter o funcionário.");
        });

    },
    limpar: function () {
        fd.getById("fun_codigo").value = "";
        fd.getById("fun_nome").value = "";
        fd.getById("fun_nivel").value = "";
        fd.getById("fun_usuario").value = "";
        fd.getById("fun_senha").value = "";
        fd.getById("divMsg").innerHTML = "";
        fd.getById("fun_nome").focus();
    },
    excluir: function () {

        if ((fd.getById("fun_codigo").value == "") || (fd.getById("fun_codigo").value == "0")) {
            alert("Localize um Funcionário !");         
             return;
            }

            if (!confirm("Deseja excluir?")) {
                return;
            }

            var dados = {
                fun_codigo: fd.getValById("fun_codigo")
            };

        fd.ajax("POST", "/Funcionario/Excluir", dados, function (retServ) {
                if (retServ.operacao) {
                    fd.getById("fun_codigo").value = "";
                    fd.getById("fun_nome").value = "";
                    fd.getById("fun_nivel").value = "";
                    fd.getById("fun_usuario").value = "";
                    fd.getById("fun_senha").value = "";
                    fd.getById("divMsg").innerHTML = "";
                    fd.getById("divMsg").innerHTML = retServ.msg;
                    fd.getById("fun_nome").focus();
                }
            }, function () {
                alert("Não foi possível excluir.")
                });
    }
}




