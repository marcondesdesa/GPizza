﻿
//objeto literal
var index = {

    gravar: function () {
        var obj = {
            cid_codigo: fd.getValById("cid_codigo"),
            cid_nome: fd.getValById("cid_nome"),
            cid_uf: fd.getValById("cid_uf")
        }

        if ((obj.cid_nome.trim() == "") || (obj.cid_uf.trim() == "")) {
            alert("Preencha corretamente todos os campos !");
            fd.getById("cid_nome").focus();
            return;
        }
        fd.ajax("POST", "/Cidade/Gravar", obj, index.gravarSuccess, index.gravarFail);
    },

    gravarSuccess: function (retornoServer) {
        if (retornoServer.msg != "") {
            alert(retornoServer.msg);
            fd.getById("cid_nome").focus();
            return;
        }
        if (retornoServer.cid_codigo == 0) {
            alert("Dados gravados com sucesso !");
        }
        else if (retornoServer.msg == "") {
            alert("Dados alterados com sucesso !");
        }
        window.location.href = window.location.href; 
        //fd.getById("cid_codigo").value = "";
        //fd.getById("cid_nome").value = "";
        //fd.getById("cid_uf").value = "25";
        //fd.getById("divMsg").innerHTML = "";
        //fd.getById("cid_nome").focus();
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
    editar: function (cid_codigo) {

        $.fancybox.close();
     
        var url = "/Cidade/obter/" + cid_codigo
        fd.ajax("GET", url, null, function (retServ) {

            //fd.getById("hId").value = retServ.id;
            fd.getById("cid_codigo").value = retServ.cid_codigo;
            fd.getById("cid_nome").value = retServ.cid_nome;
            fd.getById("cid_uf").value = retServ.cid_uf;

        }, function () {

            alert("Não foi possível obter a Cidade.");
        });

    },
    limpar: function () {
        fd.getById("cid_codigo").value = "";
        fd.getById("cid_nome").value = "";
        fd.getById("cid_uf").value = "25"; 
        fd.getById("divMsg").innerHTML = "";
        fd.getById("cid_nome").focus();
    },
    excluir: function () {

        if ((fd.getById("cid_codigo").value == "") || (fd.getById("cid_codigo").value == "0")) {
            alert("Localize uma Cidade !");   
            return;
        }

        if (!confirm("Deseja excluir?")) {
            return;
        }
        var dados = {
            cid_codigo: fd.getValById("cid_codigo")
        };

        fd.ajax("POST", "/Cidade/Excluir", dados, function (retServ) {
            if (retServ.operacao) {
                fd.getById("cid_codigo").value = "";
                fd.getById("cid_nome").value = "";
                fd.getById("cid_uf").value = "25"; 
                fd.getById("divMsg").innerHTML = "";
                fd.getById("cid_nome").focus();
            }
        }, function () {
            alert("Não foi possível excluir.")
        });
    }
}




