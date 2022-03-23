//objeto literal
var indexPesquisar = {

    pesquisar: function () {

        var tbodyRes = fd.getById("tbodyRes");

        if (fd.getValById("txtNome").length < 2) {
            fd.getById("tabResultado").style.display = "none";
            //tbodyRes.innerHTML = "<tr><td colspan=\"3\">Dados não encontrado</td></tr>";
            return;
        }
        var url = "Pesquisar?pro_descricao=" +
            fd.getValById("txtNome");


        fd.ajax("GET", url, null, function (retServ) {

            var linhas = "";//"<tr><td colspan=\"2\">Dados não encontrado</td></tr>";

            tbodyRes.innerHTML = "";
            if (retServ.length == 0) {
                fd.getById("tabResultado").style.display = "none";
                //tbodyRes.innerHTML = "<tr><td colspan=\"3\">Dados não encontrado</td></tr>";
            }
            else fd.getById("tabResultado").style.display = "table";

            retServ.forEach(function (item, index) {

                var tr = document.createElement("tr");
                tr.setAttribute("data-id", item.pro_codigo);

                var tdpro_codigo = document.createElement("td");
                tdpro_codigo.innerHTML = item.pro_codigo;
                tdpro_codigo.align = 'center';

                var tdpro_descricao = document.createElement("td");
                tdpro_descricao.innerHTML = item.pro_descricao;
              
                var tdpro_tipo = document.createElement("td");
                switch (item.pro_tipo) {
                    case 1:
                        tdpro_tipo.innerHTML = 'Pizzas';
                        break;
                    case 2:
                        tdpro_tipo.innerHTML = 'Bebidas';
                        break;
                    case 3:
                        tdpro_tipo.innerHTML = 'Outros';
                        break;                  
                    default:
                        tdpro_tipo.innerHTML = 'Nenhum';
                }

                var tdpro_preco = document.createElement("td");
                tdpro_preco.innerHTML = item.pro_preco;

                var tdAcoes = document.createElement("td")
                tdAcoes.innerHTML = "<a class='btn mini blue-stripe' style='color:blue' align='center' onclick=\"indexPesquisar.editar(" + item.pro_codigo + ")\">Visualizar</a>";
                //tdAcoes.innerHTML += "<a onclick=\"indexPesquisar.excluir(" + item.fun_codigo + ")\">excluir</a>";

                tr.appendChild(tdpro_codigo);
                tr.appendChild(tdpro_descricao);
                tr.appendChild(tdpro_tipo);
                tr.appendChild(tdpro_preco);
                tr.appendChild(tdAcoes);
                tbodyRes.appendChild(tr);
            });

        }, function () {

            alert("Erro na consulta");

        });
    },
    editar: function (pro_codigo) {
        //janela que abrir a pesquisa
        window.parent.index.editar(pro_codigo);
    }
}




