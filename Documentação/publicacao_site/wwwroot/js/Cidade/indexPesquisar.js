
//objeto literal
var indexPesquisar = {

    pesquisar: function () {

        var tbodyRes = fd.getById("tbodyRes");

        if (fd.getValById("txtNome").length <= 2) {
            fd.getById("tabResultado").style.display = "none";
            //tbodyRes.innerHTML = "<tr><td colspan=\"3\">Dados não encontrado</td></tr>";
            return;
        }
        var url = "Pesquisar?cid_nome=" +
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
                tr.setAttribute("data-id", item.cid_codigo);

                var tdcid_codigo = document.createElement("td");
                tdcid_codigo.innerHTML = item.cid_codigo;

                var tdcid_nome = document.createElement("td");
                tdcid_nome.innerHTML = item.cid_nome;

                var tdcid_uf = document.createElement("td");
                tdcid_uf.innerHTML = item.cid_uf;

                var tdcid_uf = document.createElement("td");
                switch (item.cid_uf) {                 
                    case '1':
                        tdcid_uf.innerHTML = 'Acre (AC)';
                        break;
                    case '2':
                        tdcid_uf.innerHTML = 'Alagoas (AL)';
                        break;
                    case '3':
                        tdcid_uf.innerHTML = 'Amapá (AP)';
                        break;
                    case '4':
                        tdcid_uf.innerHTML = 'Amazonas (AM)';
                        break;
                    case '5':
                        tdcid_uf.innerHTML = 'Bahia (BA)';
                        break;
                    case '6':
                        tdcid_uf.innerHTML = 'Ceará (CE)';
                        break;
                    case '7':
                        tdcid_uf.innerHTML = 'Distrito Federal (DF)';
                        break;
                    case '8':
                        tdcid_uf.innerHTML = 'Espírito Santo (ES)';
                        break;
                    case '9':
                        tdcid_uf.innerHTML = 'Goiás (GO)';
                        break;
                    case '10':
                        tdcid_uf.innerHTML = 'Maranhão (MA)';
                        break;
                    case '11':
                        tdcid_uf.innerHTML = 'Mato Grosso (MT)';
                        break;
                    case '12':
                        tdcid_uf.innerHTML = 'Mato Grosso do Sul (MS)';
                        break;
                    case '13':
                        tdcid_uf.innerHTML = 'Minas Gerais (MG)';
                        break;
                    case '14':
                        tdcid_uf.innerHTML = 'Pará (PA)';
                        break; 
                    case '15':
                        tdcid_uf.innerHTML = 'Paraíba (PB)';
                        break;
                    case '16':
                        tdcid_uf.innerHTML = 'Paraná (PR)';
                        break;
                    case '17':
                        tdcid_uf.innerHTML = 'Pernambuco (PE)';
                        break;
                    case '18':
                        tdcid_uf.innerHTML = 'Piauí (PI)';
                        break;
                    case '19':
                        tdcid_uf.innerHTML = 'Rio de Janeiro (RJ)';
                        break;
                    case '20':
                        tdcid_uf.innerHTML = 'Rio Grande do Norte (RN)';
                        break;
                    case '21':
                        tdcid_uf.innerHTML = 'Rio Grande do Sul (RS)';
                        break;
                    case '22':
                        tdcid_uf.innerHTML = 'Rondônia (RO)';
                        break;
                    case '23':
                        tdcid_uf.innerHTML = 'Roraima (RR)';
                        break;
                    case '24':
                        tdcid_uf.innerHTML = 'Santa Catarina (SC)';
                        break;
                    case '25':
                        tdcid_uf.innerHTML = 'São Paulo (SP)';
                        break;
                    case '26':
                        tdcid_uf.innerHTML = 'Sergipe (SE)';
                        break;
                    case '27':
                        tdcid_uf.innerHTML = 'Tocantins (TO)';
                        break;
                    default:
                        tdcid_uf.innerHTML = 'Não identificado';                   
                }    
                    
                var tdAcoes = document.createElement("td")
                tdAcoes.innerHTML = "<a class='btn mini blue-stripe' style='color:blue' align='center' onclick=\"indexPesquisar.editar(" + item.cid_codigo + ")\">Visualizar</a>";
                //tdAcoes.innerHTML += "<a onclick=\"indexPesquisar.excluir(" + item.fun_codigo + ")\">excluir</a>";

                tr.appendChild(tdcid_codigo);
                tr.appendChild(tdcid_nome);
                tr.appendChild(tdcid_uf);
                tr.appendChild(tdAcoes);

                tbodyRes.appendChild(tr);
            });

        }, function () {

            alert("Erro na consulta");

        });
    },
    editar: function (cid_codigo) {
        //janela que abrir a pesquisa
        window.parent.index.editar(cid_codigo);
    }
}




