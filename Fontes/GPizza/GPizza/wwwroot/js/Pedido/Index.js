var index = {

    dados: [],

  
    carregarItem: function () {
        var pro_codigo = fd.getById("pro_codigo").value;      
        if (pro_codigo > 0){
            var url = "/Produto/obter/" + pro_codigo;
            fd.ajax("GET", url, null, function (retServ) {
                var pi_quant = parseInt(fd.getById("pi_quant").value);
                var vl_unit = Number(retServ.pro_preco);
                var vl_tot = Number(pi_quant * vl_unit);
                fd.getById("pi_valor_unit").value = vl_unit;
                fd.getById("pi_valor_tot").value = vl_tot;

            }, function () {

                alert("Não foi possível obter o Produto.");
            });
        }
    },

    calculaTotais: function () {
        if ((fd.getById("ped_valor_total").value != "") & (fd.getById("ped_valor_total").value != "0,00")) {
            var ped_valor_total = Number(fd.getById("ped_valor_total").value);
            var ped_desconto = 0.00;

            if ((fd.getById("ped_desconto").value != "") & (fd.getById("ped_desconto").value != "0,00")) {
                ped_desconto = Number(fd.getById("ped_desconto").value);
            }
            fd.getById("ped_valor_bruto").value = ped_valor_total;
            fd.getById("ped_valor_total").value = ped_valor_total - ped_desconto;
        }
    },


    obterProduto: function (id) {

        var li = document.querySelector("li[data-id='" + id +"']")
        var id = li.getAttribute("data-id");
        var nome = li.innerHTML;

        fd.getById("hfPatrimonioId").value = id;
        fd.getById("txtPatrimonio").value = nome;
        fd.getById("btnAdd").classList.remove("disabled");

    },


    adicionar: function () {

        if (fd.getById("pro_codigo").value == "0" || fd.getById("pro_codigo").value == "") {
            alert("Selecione um Produto.");
            return;
        }

        if (fd.getById("cli_codigo").value.trim() == "") {
            alert("Informe o cliente no Pedido.");
            return;
        }

        var url = "/Produto/obter/" + fd.getById("pro_codigo").value;
        fd.ajax("GET", url, null, function (retServ) {         
            if (retServ.pro_codigo > 0) {
                var item = {
                    pro_codigo: retServ.pro_codigo,
                    pro_descricao: retServ.pro_descricao,
                    pi_quant: fd.getById("pi_quant").value,
                    pi_valor_unit: fd.getById("pi_valor_unit").value,
                    pi_valor_tot: fd.getById("pi_valor_tot").value,
                    edicao: false
                };

                var pos = index.dados.findIndex(function (linha) {
                    if (linha.pro_codigo == item.pro_codigo) {
                        return true;
                    }
                });

                if (pos > -1) {

                    if (!index.dados[pos].edicao)
                        alert("Este item já foi incluído.");
                    else {
                        index.dados[pos] = item;
                        fd.getById("pro_codigo").value = "";
                        fd.getById("pi_quant").value = "1";
                        fd.getById("pi_valor_unit").value = "0,00";
                        fd.getById("pi_valor_tot").value = "0,00";
                        index.carregarItems();
                    }
                }
                else {
                    index.dados.push(item);
                    //fd.getById("hfProdutoId").value = "";      
                    fd.getById("pro_codigo").value = "";
                    fd.getById("pi_quant").value = "1";
                    fd.getById("pi_valor_unit").value = "0,00";
                    fd.getById("pi_valor_tot").value = "0,00";
                    index.carregarItems();
                }
            }
        }, function () {});
        
    },

    carregarItems: function () {

        var tbodyItems = fd.getById("tbodyItems");
        tbodyItems.innerHTML = "";
        var trs = "";
        var ped_valor_total = 0.00;
        var pi_valor_tot = 0.00;
        var ped_desconto = 0.00;
        index.dados.forEach(function (linha) {
            trs += "<tr data-id=\"" + linha.pro_codigo + "\" > " +
                "  <td>" + linha.pro_codigo + "</td>" +
                "  <td>" + linha.pro_descricao + "</td>" +
                "  <td>" + linha.pi_quant + "</td>" +
                "  <td>" + linha.pi_valor_unit + "</td>" +
                "  <td>" + linha.pi_valor_tot + "</td>" +              
                "  <td> <a class='btn mini blue-stripe' style='color:blue' align='center' onclick=\"index.editarItem(" + linha.pro_codigo + ")\">Editar</a>" +
                "       <a class='btn mini blue-stripe' style='color:blue' align='center' onclick=\"index.excluirItem(" + linha.pro_codigo + ")\">Excluir</a>" +
                "   </td > " +
                "</tr > "
            pi_valor_tot = Number(linha.pi_quant * linha.pi_valor_unit);
            ped_valor_total = ped_valor_total + pi_valor_tot;         
        });
        if ((fd.getById("ped_desconto").value != "") & (fd.getById("ped_desconto").value != "0,00")) {
            ped_desconto = Number(fd.getById("ped_desconto").value);
        }
        fd.getById("ped_valor_bruto").value = ped_valor_total;
        fd.getById("ped_valor_total").value = ped_valor_total - ped_desconto;

        tbodyItems.innerHTML = trs;

    },

    excluirItem(pro_codigo) {

        if (!confirm("Deseja remover o Item ?")) {
            return;
        }
        var pos = index.dados.findIndex(function (linha) {
            if (linha.pro_codigo == pro_codigo) {
                return true;
            }
        });

        if (pos > -1) {
            index.dados.splice(pos, 1);
            index.carregarItems();
        }
    },


    editarItem(pro_codigo) {
        var item = index.dados.find(function (linha) {

            if (linha.pro_codigo == pro_codigo) {
                linha.edicao = true;
                return true;
            }
        });
        if (item != null) {
            fd.getById("pro_codigo").value = item.pro_codigo;
            //fd.getById("pro_descricao").value = item.pro_descricao;
            fd.getById("pi_quant").value = item.pi_quant;
            fd.getById("pi_valor_unit").value = item.pi_valor_unit;
            fd.getById("pi_valor_tot").value = item.pi_valor_tot;
            //fd.getById("btnAdd").classList.remove("disabled");   
        }
    },

    gravar() {    
        if (fd.getById("cli_codigo").value == "0" || fd.getById("cli_codigo").value == "") {
            alert("Informe o Cliente.");
            fd.getById("cli_codigo").focus();
            return;
        }

        if (fd.getById("ped_data").value == "") {
            alert("Data não informada.");
            return;
        }

        if (fd.getById("ped_valor_total").value == "0,00" || fd.getById("ped_valor_total").value == "0") {
            alert("O Valor do Pedido está zerado.");
            return;
        }
       
        var dados = {
            ped_codigo: parseInt(fd.getById("ped_codigo").value),
            ped_data: fd.getById("ped_data").value,
            cli_codigo: parseInt(fd.getById("cli_codigo").value),
            ped_valor_total: Number(fd.getById("ped_valor_total").value),
            ped_desconto: Number(fd.getById("ped_desconto").value),
            ped_observacao: fd.getById("ped_observacao").value,
            items: index.dados
        };

        fd.ajax("POST", "/Pedido/Gravar", dados, function (retServ) {

            if (retServ.operacao) {
                if (retServ.ped_codigo == 0) {
                    alert("Dados gravados com sucesso !");               
                }
                else if (retServ.msg == ""){
                    alert("Dados alterados com sucesso !");
                }
                window.location.href = window.location.href; 
            }
            else {
                alert("Não foi possível gravar. " + retServ.msg);
            }

        }, function () {



        });
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
    editar: function (ped_codigo) {
        $.fancybox.close();
        var url = "/Pedido/obter/" + ped_codigo
        fd.ajax("GET", url, null, function (retServ) {
            fd.getById("ped_codigo").value = retServ.ped_codigo;
            fd.getById("ped_data").value = RetiraHoraData(retServ.ped_data);           
            fd.getById("cli_codigo").value = retServ.cliente.cli_codigo;
            if (retServ.cliente.cli_codigo != "")
                index.carregaClientes(retServ.cliente.cli_codigo);
            index.dados = [];
            for (var i = 0; i < retServ.itensPed.length; i++) {
                var itemPed = {
                    pro_codigo: retServ.itensPed[i].produto.pro_codigo,
                    pro_descricao: retServ.itensPed[i].produto.pro_descricao,
                    pi_quant: retServ.itensPed[i].pi_quant,
                    pi_valor_unit: retServ.itensPed[i].pi_valor_unit,
                    pi_valor_tot: retServ.itensPed[i].pi_valor_tot,
                    edicao: false 
                };
                index.dados.push(itemPed);
            };
            if (index.dados.length > 0) {
                index.carregarItems();
            }       
            var ped_valor_total = Number(retServ.ped_valor_total);
            var ped_desconto = Number(retServ.ped_desconto);
            var ped_valor_bruto = (ped_valor_total + ped_desconto);         
            fd.getById("ped_valor_bruto").value = ped_valor_bruto;
            fd.getById("ped_desconto").value = retServ.ped_desconto;
            fd.getById("ped_valor_total").value = retServ.ped_valor_total;
            fd.getById("ped_observacao").value = retServ.ped_observacao;
        }, function () {
            alert("Não foi possível obter o Pedido.");
        });
    },
    Limpar: function () {
        var dataAtual = new Date().toISOString();
        fd.getById("ped_codigo").value = "";
        fd.getById("ped_data").value = RetiraHoraData(dataAtual);
        fd.getById("cli_codigo").value = "";
        fd.getById("pro_codigo").value = "";
        fd.getById("pi_quant").value = "1";
        fd.getById("pi_valor_unit").value = "0,00";
        fd.getById("pi_valor_tot").value = "0,00";
        fd.getById("ped_valor_bruto").value = "0,00";
        fd.getById("ped_desconto").value = "0,00";
        fd.getById("ped_valor_total").value = "0,00";
        fd.getById("ped_observacao").value = "";
        fd.getById("tbodyItems").innerHTML = "";    
        fd.getById("divMsg").innerHTML = "";
        fd.getById("cli_codigo").focus();
    },

    carregaClientes: function (cli_codigo) {
        var select = fd.getById("cli_codigo");
        if (cli_codigo != "") {
            var options = select.getElementsByTagName('OPTION');
            for (var i = 0; i < options.length; i++) {
                select.removeChild(options[i]);
                i--;
            }
        }
        var url = "/Cliente/Pesquisar?cli_nome=%";
        fd.ajax("GET", url, null,
            function (retServ) {
                if (retServ.length == 0) {
                    return;
                }
                var cont = 0;
                var index = "-1";
                for (var i = 0; i < retServ.length; i++) {
                    var opt = document.createElement("option");
                    if (cli_codigo == retServ[i].cli_codigo) {
                        index = cont;                       
                    }
                    opt.value = retServ[i].cli_codigo;
                    opt.innerHTML = retServ[i].cli_nome +" - CPF:"+retServ[i].cli_cpf;
                    select.appendChild(opt);
                    cont++;
                };
                select.selectedIndex = index;
            },
            function () {
                alert("Erro ao carregar Clientes !");
            })

    },
    carregaProdutos: function (pro_codigo) {
        var select = fd.getById("pro_codigo");
        if (pro_codigo != "") {
            var options = select.getElementsByTagName('OPTION');
            for (var i = 0; i < options.length; i++) {
                select.removeChild(options[i]);
                i--;
            }
        }
        var url = "/Produto/Pesquisar?pro_descricao=%";
        fd.ajax("GET", url, null,
            function (retServ) {
                if (retServ.length == 0) {
                    return;
                }
                var cont = 0;
                var index = "-1";
                for (var i = 0; i < retServ.length; i++) {
                    var opt = document.createElement("option");
                    if (pro_codigo == retServ[i].pro_codigo) {
                        index = cont;
                    }
                    opt.value = retServ[i].pro_codigo;
                    opt.innerHTML = retServ[i].pro_codigo + " - " + retServ[i].pro_descricao;
                    select.appendChild(opt);
                    cont++;
                };
                select.selectedIndex = index;
            },
            function () {
                alert("Erro ao carregar Produtos !");
            })

    }


}


document.addEventListener("DOMContentLoaded", function () {

    //index.init();
});