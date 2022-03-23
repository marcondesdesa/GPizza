//objeto literal
var index = {

    gravar: function () {
        var obj = {
            pro_codigo: fd.getValById("pro_codigo"),
            pro_descricao: fd.getValById("pro_descricao"),
            pro_preco: fd.getValById("pro_preco"),
            pro_estoque: fd.getValById("pro_estoque"),
            pro_tipo: fd.getValById("pro_tipo"),
            pro_observacao: fd.getValById("pro_observacao")
        }

        if ((obj.pro_descricao.trim() == "") ||
            ((obj.pro_preco.trim() == "0") || (obj.pro_preco == 0)) ||
            ((obj.pro_tipo.trim() == "0") || (obj.pro_tipo == 0))) {
            alert("Preencha corretamente todos os campos !");
     
            if (obj.pro_descricao.trim() == "")
                fd.getById("pro_descricao").focus();
            else if ((obj.pro_preco.trim() == "0") || (obj.pro_preco == 0))
                fd.getById("pro_preco").focus();
            else if ((obj.pro_tipo.trim() == "0") || (obj.pro_tipo == 0))
                fd.getById("pro_tipo").focus();
            return;
        }

        fd.ajax("POST", "/Produto/Gravar", obj, index.gravarSuccess, index.gravarFail);
    },

    gravarSuccess: function (retornoServer) {
        if (retornoServer.msg != "") {
            alert(retornoServer.msg);
            fd.getById("pro_descricao").focus();
            return;
        }
        if (retornoServer.pro_codigo == 0) {
            alert("Dados gravados com sucesso !");
        }
        else {
            alert("Dados alterados com sucesso !");
        }
        fd.getById("pro_codigo").value = "";
        fd.getById("pro_descricao").value = "";
        fd.getById("pro_preco").value = 0;
        fd.getById("pro_estoque").value = 0;
        fd.getById("pro_tipo").value = 3;
        fd.getById("pro_observacao").value = "";
        fd.getById("divMsg").innerHTML = "";
        fd.getById("pro_descricao").focus();
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
        var url = "/Produto/obter/" + id
        fd.ajax("GET", url, null, function (retServ) {
            //fd.getById("hId").value = retServ.id;
            fd.getById("pro_codigo").value = retServ.pro_codigo;
            fd.getById("pro_descricao").value = retServ.pro_descricao;
            fd.getById("pro_preco").value = retServ.pro_preco;
            fd.getById("pro_estoque").value = retServ.pro_estoque;
            fd.getById("pro_tipo").value = retServ.pro_tipo;
            fd.getById("pro_observacao").value = retServ.pro_observacao;
        }, function () {

            alert("Não foi possível obter o Produto.");
        });

    },
    limpar: function () {
        fd.getById("pro_codigo").value = "";
        fd.getById("pro_descricao").value = "";
        fd.getById("pro_preco").value = 0;
        fd.getById("pro_estoque").value = 0;
        fd.getById("pro_tipo").value = 3;
        fd.getById("pro_observacao").value = "";
        fd.getById("divMsg").innerHTML = "";
        fd.getById("pro_descricao").focus();
    },
    excluir: function () {

        if ((fd.getById("pro_codigo").value == "") || (fd.getById("pro_codigo").value == "0")) {
            alert("Localize um Produto !");
            return;
        }

        if (!confirm("Deseja excluir?")) {
            return;
        }

        var dados = {
            pro_codigo: fd.getValById("pro_codigo")
        };

        fd.ajax("POST", "/Produto/Excluir", dados, function (retServ) {
            if (retServ.operacao) {
                fd.getById("pro_codigo").value = "";
                fd.getById("pro_descricao").value = "";
                fd.getById("pro_preco").value = 0;
                fd.getById("pro_estoque").value = 0;
                fd.getById("pro_tipo").value = 3;
                fd.getById("pro_observacao").value = "";
                fd.getById("divMsg").innerHTML = "";
                fd.getById("pro_descricao").focus();
            }
        }, function () {
            alert("Não foi possível excluir.")
        });
    }
}




