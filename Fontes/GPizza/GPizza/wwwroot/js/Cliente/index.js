
//objeto literal
var index = {
    gravar: function () {
        var obj = {
            cli_codigo: fd.getValById("cli_codigo"),          
            cli_nome: fd.getValById("cli_nome"),
            cli_endereco: fd.getValById("cli_endereco"),
            cli_numero: fd.getValById("cli_numero"),
            cli_bairro: fd.getValById("cli_bairro"),
            cid_codigo: fd.getValById("cid_codigo"),
            cli_cep: fd.getValById("cli_cep"),
            cli_telefone: fd.getValById("cli_telefone"),          
            cli_celular: fd.getValById("cli_celular"),
            cli_dt_nascimento: fd.getValById("cli_dt_nascimento"),
            cli_cpf: fd.getValById("cli_cpf"),
            cli_rg: fd.getValById("cli_rg"),
            cli_email: fd.getValById("cli_email"),
            cli_observacao: fd.getValById("cli_observacao")
        }

        if ((obj.cli_nome.trim() == "") ||
            (obj.cli_endereco.trim() == "") ||      
            (obj.cid_codigo == 0) ||
            (obj.cid_codigo.trim() == "0") ||
            (obj.cli_telefone.trim() == "") ||
            (obj.cli_dt_nascimento.trim() == "") ||
            (obj.cli_email.trim() == ""))
        {
            if (obj.cli_nome.trim() == "") {
                alert("Informe o Nome do Cliente.");
                fd.getById("cli_nome").focus();         
            }
            else if (obj.cli_dt_nascimento.trim() == "") {
                alert("Informe a data de nascimento do Cliente.");
                fd.getById("cli_dt_nascimento").focus();
            }
            else if (obj.cli_endereco.trim() == "") {
                alert("Informe o Endereço do Cliente.");
                fd.getById("cli_endereco").focus(); 
            }
            else if ((obj.cid_codigo == 0) || (obj.cid_codigo.trim() == "0")) {
                alert("Informe a Cidade do Cliente.");
                fd.getById("cid_codigo").focus();
            }
            else if (obj.cli_telefone.trim() == "") {
                alert("Informe o Telefone do Cliente.");
                fd.getById("cli_telefone").focus();
            }         
            else if (obj.cli_email.trim() == "") {
                alert("Informe o E-mail do Cliente.");
                fd.getById("cli_email").focus();
            };
            return;
        }
        fd.ajax("POST", "/Cliente/Gravar", obj, index.gravarSuccess, index.gravarFail);
    },

    gravarSuccess: function (retornoServer) {
        if (retornoServer.msg != "") {
            alert(retornoServer.msg);
            fd.getById("cli_nome").focus();
            return;
        }
        if (retornoServer.cli_codigo == 0) {
            alert("Dados gravados com sucesso !");
        }
        else if(retornoServer.msg == ""){
            alert("Dados alterados com sucesso !");
        }
        window.location.href = window.location.href; 
        //fd.getById("cli_codigo").value = "";
        //fd.getById("cli_nome").value = "";
        //fd.getById("cli_endereco").value = "";
        //fd.getById("cli_numero").value = "";
        //fd.getById("cli_bairro").value = "";
        //fd.getById("cid_codigo").value = "";
        //fd.getById("cli_cep").value = "";
        //fd.getById("cli_telefone").value = "";
        //fd.getById("cli_celular").value = "";
        //fd.getById("cli_dt_nascimento").value = "";
        //fd.getById("cli_cpf").value = "";
        //fd.getById("cli_rg").value = "";
        //fd.getById("cli_email").value = "";
        //fd.getById("cli_observacao").value = "";
        //fd.getById("divMsg").innerHTML = "";
        //fd.getById("cli_nome").focus();

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
    editar: function (cli_codigo) {

        $.fancybox.close();

        var url = "/Cliente/obter/" + cli_codigo
        fd.ajax("GET", url, null, function (retServ) {

            //fd.getById("hId").value = retServ.id;
            fd.getById("cli_codigo").value = retServ.cli_codigo;
            fd.getById("cli_nome").value = retServ.cli_nome;
            fd.getById("cli_endereco").value = retServ.cli_endereco;
            fd.getById("cli_numero").value = retServ.cli_numero;
            fd.getById("cli_bairro").value = retServ.cli_bairro;
            fd.getById("cid_codigo").value = retServ.cidade.cid_codigo;
            if (retServ.cidade.cid_codigo!="")
                index.carregaCidades(retServ.cidade.cid_codigo);
            fd.getById("cli_cep").value = retServ.cli_cep;
            fd.getById("cli_telefone").value = retServ.cli_telefone;
            fd.getById("cli_celular").value = retServ.cli_celular;        
            fd.getById("cli_dt_nascimento").value = RetiraHoraData(retServ.cli_dt_nascimento);
            fd.getById("cli_cpf").value = retServ.cli_cpf;
            fd.getById("cli_rg").value = retServ.cli_rg;
            fd.getById("cli_email").value = retServ.cli_email;
            fd.getById("cli_observacao").value = retServ.cli_observacao;
            

        }, function () {
            alert("Não foi possível obter a Cidade.");
        });

    },
    limpar: function () {
        fd.getById("cli_codigo").value = "";
        fd.getById("cli_nome").value = "";
        fd.getById("cli_endereco").value = "";
        fd.getById("cli_numero").value = "";
        fd.getById("cli_bairro").value = "";
        fd.getById("cid_codigo").value = "";
        fd.getById("cli_cep").value = "";
        fd.getById("cli_telefone").value = "";
        fd.getById("cli_celular").value = "";
        fd.getById("cli_dt_nascimento").value = "";
        fd.getById("cli_cpf").value = "";
        fd.getById("cli_rg").value = "";
        fd.getById("cli_email").value = "";
        fd.getById("cli_observacao").value = "";
        fd.getById("divMsg").innerHTML = "";
        fd.getById("cli_nome").focus();
    },
    carregaCidades: function (cid_codigo) {
        var select = fd.getById("cid_codigo");
        if (cid_codigo != "") {           
            var options = select.getElementsByTagName('OPTION');
            for (var i = 0; i < options.length ; i++) {
                select.removeChild(options[i]);
                i--; 
           }       
        }       
        var url = "/Cidade/Pesquisar?cid_nome=%";
        fd.ajax("GET", url, null,
            function (retServ) {
                if (retServ.length == 0) {
                    return;
                }
                var cont = 0; 
                var index = "-1"; 
                for (var i = 0; i < retServ.length; i++) {
                    var opt = document.createElement("option");
                    if (cid_codigo == retServ[i].cid_codigo) {
                        index = cont;
                        //opt.value = retServ[i].cid_codigo + " selected";
                    }                    
                    opt.value = retServ[i].cid_codigo;
                    opt.innerHTML = retServ[i].cid_nome;
                    select.appendChild(opt);
                    cont++;
                };
                select.selectedIndex = index;
            },
            function () {
                alert("Erro ao carregar Cidades !");
            })
        
    },
    excluir: function () {

        if ((fd.getById("cli_codigo").value == "") || (fd.getById("cli_codigo").value == "0")) {
            alert("Localize um Cliente !");   
            return;
        }

        if (!confirm("Deseja excluir?")) {
            return;
        }
        var dados = {
            cli_codigo: fd.getValById("cli_codigo")
        };

        fd.ajax("POST", "/Cliente/Excluir", dados, function (retServ) {
            if (retServ.operacao) {
                fd.getById("cli_codigo").value = "";
                fd.getById("cli_nome").value = "";
                fd.getById("cli_endereco").value = "";
                fd.getById("cli_numero").value = "";
                fd.getById("cli_bairro").value = "";
                fd.getById("cid_codigo").value = "";
                fd.getById("cli_cep").value = "";
                fd.getById("cli_telefone").value = "";
                fd.getById("cli_celular").value = "";
                fd.getById("cli_dt_nascimento").value = "";
                fd.getById("cli_cpf").value = "";
                fd.getById("cli_rg").value = "";
                fd.getById("cli_email").value = "";
                fd.getById("cli_observacao").value = "";
                fd.getById("divMsg").innerHTML = "";
                fd.getById("cli_nome").focus();
            }
        }, function () {
            alert("Não foi possível excluir.")
        });
    }
}




