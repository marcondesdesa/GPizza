
//objeto literal
var indexPesquisar = {


    pesquisar: function () {

        var tbodyRes = fd.getById("tbodyRes");

        if (fd.getValById("txtNome").length < 2) {
            fd.getById("tabResultado").style.display = "none";
            //tbodyRes.innerHTML = "<tr><td colspan=\"3\">Dados não encontrado</td></tr>";
            return;
        }
        var url = "Pesquisar?cli_nome=" +fd.getValById("txtNome");


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
                tr.setAttribute("data-id", item.ped_codigo);

                var tdped_codigo = document.createElement("td");
                tdped_codigo.innerHTML = item.ped_codigo;
                tdped_codigo.align = 'center';

                var tdped_data = document.createElement("td");
                tdped_data.innerHTML = RetiraHoraData(item.ped_data);

                var tdcli_nome = document.createElement("td");
                tdcli_nome.innerHTML = item.cliente.cli_nome;
                                             
                var tdped_valor_total = document.createElement("td");
                tdped_valor_total.innerHTML = item.ped_valor_total;
               
                var tdAcoes = document.createElement("td")
                tdAcoes.innerHTML = "<a class='btn mini blue-stripe' style='color:blue' align='center' onclick=\"indexPesquisar.editar(" + item.ped_codigo + ")\">Visualizar</a>";
                //tdAcoes.innerHTML += "<a onclick=\"indexPesquisar.excluir(" + item.fun_codigo + ")\">excluir</a>";

                tr.appendChild(tdped_codigo);
                tr.appendChild(tdped_data);
                tr.appendChild(tdcli_nome);
                tr.appendChild(tdped_valor_total);             
                tr.appendChild(tdAcoes);
                tbodyRes.appendChild(tr);
            });

        }, function () {

            alert("Erro na consulta");

        });


    },

    editar: function (ped_codigo) {

        //janela que abrir a pesquisa
        window.parent.index.editar(ped_codigo);
    }
}




