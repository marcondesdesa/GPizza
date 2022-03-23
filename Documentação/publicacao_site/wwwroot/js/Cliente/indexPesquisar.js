
//objeto literal
var indexPesquisar = {


    pesquisar: function () {

        var tbodyRes = fd.getById("tbodyRes");

        if (fd.getValById("txtNome").length <= 2) {
            fd.getById("tabResultado").style.display = "none";
            //tbodyRes.innerHTML = "<tr><td colspan=\"3\">Dados não encontrado</td></tr>";
            return;
        }
        var url = "Pesquisar?nome=" +
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
                tr.setAttribute("data-id", item.cli_codigo);

                var tdcli_codigo = document.createElement("td");
                tdcli_codigo.innerHTML = item.cli_codigo;

                var tdcli_nome = document.createElement("td");
                tdcli_nome.innerHTML = item.cli_nome;
                
                var tdcli_endereco = document.createElement("td");
                tdcli_endereco.innerHTML = item.cli_endereco;

                var tdcli_numero = document.createElement("td");
                tdcli_numero.innerHTML = item.cli_numero;

                var tdcli_bairro = document.createElement("td");
                tdcli_bairro.innerHTML = item.cli_bairro;

                var tdcid_codigo = document.createElement("td");
                tdcid_codigo.innerHTML = item.cid_codigo;

                var tdcli_cep = document.createElement("td");
                tdcli_cep.innerHTML = item.cli_cep;

                var tdcli_telefone = document.createElement("td");
                tdcli_telefone.innerHTML = item.cli_telefone;

                var tdcli_celular = document.createElement("td");
                tdcli_celular.innerHTML = item.cli_celular;

                var tdcli_cpf = document.createElement("td");
                tdcli_cpf.innerHTML = item.cli_cpf;

                var tdcli_rg = document.createElement("td");
                tdcli_rg.innerHTML = item.cli_rg;

                var tdcli_email = document.createElement("td");
                tdcli_email.innerHTML = item.cli_email;

                var tdcli_observacao = document.createElement("td");
                tdcli_observacao.innerHTML = item.cli_observacao;

                var tdAcoes = document.createElement("td")
                tdAcoes.innerHTML = "<a class='btn mini blue-stripe' style='color:blue' align='center' onclick=\"indexPesquisar.editar(" + item.cli_codigo + ")\">Visualizar</a>";
                //tdAcoes.innerHTML += "<a onclick=\"indexPesquisar.excluir(" + item.fun_codigo + ")\">excluir</a>";

                tr.appendChild(tdcli_codigo);
                tr.appendChild(tdcli_nome);      
                tr.appendChild(tdcli_endereco);  
                tr.appendChild(tdcli_numero);    
                tr.appendChild(tdcli_bairro);    
                tr.appendChild(tdcid_codigo);    
                tr.appendChild(tdcli_cep);       
                tr.appendChild(tdcli_telefone);  
                tr.appendChild(tdcli_celular);   
                tr.appendChild(tdcli_cpf);       
                tr.appendChild(tdcli_r);g        
                tr.appendChild(tdcli_email);                          
                tr.appendChild(tdcli_observacao);
                tr.appendChild(tdAcoes);
                tbodyRes.appendChild(tr);

            });

        }, function () {

            alert("Erro na consulta");

        });


    },

    editar: function (id) {

        //janela que abrir a pesquisa
        window.parent.index.editar(id);
    }
}




