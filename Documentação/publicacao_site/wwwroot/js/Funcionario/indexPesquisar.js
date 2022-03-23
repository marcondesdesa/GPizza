
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
                tr.setAttribute("data-id", item.fun_codigo);

                var tdfun_codigo = document.createElement("td");
                tdfun_codigo.innerHTML = item.fun_codigo;

                var tdfun_nome = document.createElement("td");
                tdfun_nome.innerHTML = item.fun_nome;

                var tdfun_nivel = document.createElement("td");
                switch (item.fun_nivel) {
                    case '1':
                        tdfun_nivel.innerHTML = 'Administrador';
                        break;
                    case '2':
                        tdfun_nivel.innerHTML = 'Funcionário';
                        break;
                    default:
                        tdfun_nivel.innerHTML = 'Outro';
                }    

                var tdfun_usuario = document.createElement("td");
                tdfun_usuario.innerHTML = item.fun_usuario


                var tdAcoes = document.createElement("td")
                tdAcoes.innerHTML = "<a class='btn mini blue-stripe' style='color:blue' align='center' onclick=\"indexPesquisar.editar(" + item.fun_codigo + ")\">Visualizar</a>";
                //tdAcoes.innerHTML += "<a onclick=\"indexPesquisar.excluir(" + item.fun_codigo + ")\">excluir</a>";
         
                tr.appendChild(tdfun_codigo);
                tr.appendChild(tdfun_nome);
                tr.appendChild(tdfun_nivel);
                tr.appendChild(tdfun_usuario);
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
    },

    excluir: function (id) {

        if (!confirm("Deseja excluir?")) {
            return;
        }

        var dados = {
            id: id
        };

        fd.ajax("POST", "/GerenciadorFuncionario/Excluir", dados, function (retServ) {

            if (retServ.operacao) {

                var trExcluir = document.querySelector("tr[data-id='" + id + "']");

                trExcluir.parentNode.removeChild(trExcluir);
            }


        }, function () {

            alert("Não foi possível excluir.")

        });


    }


}




