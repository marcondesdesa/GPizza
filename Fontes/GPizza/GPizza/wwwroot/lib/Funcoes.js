function MascaraCNPJ(name) {
    var mcnpj = fd.getValById(name);
    if (mascaraInteiro(mcnpj) == false) {
        event.returnValue = false;
    }
    return formataCampo(name, mcnpj, '00.000.000/0000-00', event);
}

function MascaraCep(name) {
    var cep = fd.getValById(name);
    if (mascaraInteiro(cep) == false) {
        event.returnValue = false;
    }
    return formataCampo(name, cep, '00.000-000', event);
}

function MascaraData(name) {
    var data = fd.getValById(name);
    if (mascaraInteiro(data) == false) {
        event.returnValue = false;
    }
    return formataCampo(name, data, '00/00/0000', event);
}

function MascaraTelefone(name) {
    var tel = fd.getValById(name);
    if (mascaraInteiro(tel) == false) {
        event.returnValue = false;
    }
    return formataCampo(name, tel, '(00) 0000-0000', event);
}

function MascaraCelular(name) {
    var tel = fd.getValById(name);
    if (mascaraInteiro(tel) == false) {
        event.returnValue = false;
    }
    return formataCampo(name, tel, '(00) 00000-0000', event);
}

function MascaraCPF(name) {
    var mcpf = fd.getValById(name);
    if (mascaraInteiro(mcpf) == false) {
        event.returnValue = false;
    }
    return formataCampo(name, mcpf, '000.000.000-00', event);
}

function MascaraRG(name) {
    var rg = fd.getValById(name);
    if (mascaraInteiro(rg) == false) {
        event.returnValue = false;
    }
    return formataCampo(name, rg, '00.000.000-0', event);
}



function ValidaTelefone(name) {
    var tel = fd.getValById(name);
    exp = /\(\d{2}\)\ \d{4}\-\d{4}/
    if (!exp.test(tel.value)) {
        alert('Numero de Telefone Invalido!');
        fd.getValById(name).value = '';
        //fd.getById(name).focus();
    }
}

function ValidaCep(name) {
    var cep = fd.getValById(name);
    exp = /\d{2}\.\d{3}\-\d{3}/
    if (!exp.test(cep.value)) {
        alert('Numero de Cep Invalido!');
        fd.getValById(name).value = '';
        //fd.getById(name).focus();
    }
}

function ValidaData(name) {
    var data = fd.getValById(name);
    exp = /\d{2}\/\d{2}\/\d{4}/
    if (!exp.test(data.value)) {
        alert('Data Invalida!');
        fd.getValById(name).value = '';
        //fd.getById(name).focus();
    }
}

function ValidarCPF(name) {
    var cpf = fd.getValById(name);
    var exp = /\.|\-/g
    cpf = cpf.replace(exp, "");
    var digitoDigitado = eval(cpf.charAt(9) + cpf.charAt(10));
    var soma1 = 0, soma2 = 0;
    var vlr = 11;

    for (i = 0; i < 9; i++) {
        soma1 += eval(cpf.charAt(i) * (vlr - 1));
        soma2 += eval(cpf.charAt(i) * vlr);
        vlr--;
    }
    soma1 = (((soma1 * 10) % 11) == 10 ? 0 : ((soma1 * 10) % 11));
    soma2 = (((soma2 + (2 * soma1)) * 10) % 11);

    var digitoGerado = (soma1 * 10) + soma2;
    if (digitoGerado != digitoDigitado) {
        alert('CPF Invalido!');
        fd.getValById(name).value = '';
        //fd.getById(name).focus();
    }
}

function ValidarCNPJ(name) {
    var cnpj = fd.getValById(name);
    var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
    var dig1 = new Number;
    var dig2 = new Number;

    exp = /\.|\-|\//g
    cnpj = cnpj.toString().replace(exp, "");
    var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

    for (i = 0; i < valida.length; i++) {
        dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
        dig2 += cnpj.charAt(i) * valida[i];
    }
    dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
    dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

    if (((dig1 * 10) + dig2) != digito) {
        alert('CNPJ Invalido!');
        fd.getValById(name).value = '';
        //fd.getById(name).focus();
    }
}

function ValidarEmail(name) {
    var field = fd.getValById(name);
    usuario = field.substring(0, field.indexOf("@"));
    dominio = field.substring(field.indexOf("@") + 1, field.length);
    if ((usuario.length >= 1) &&
        (dominio.length >= 3) &&
        (usuario.search("@") == -1) &&
        (dominio.search("@") == -1) &&
        (usuario.search(" ") == -1) &&
        (dominio.search(" ") == -1) &&
        (dominio.search(".") != -1) &&
        (dominio.indexOf(".") >= 1) &&
        (dominio.lastIndexOf(".") < dominio.length - 1)) {
        return;
    }
    else {
        alert("E-mail invalido");
        fd.getValById(name).value = '';
        //fd.getById(name).focus();
        return;
    }
}

function mascaraInteiro() {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.returnValue = false;
        return false;
    }
    return true;
}

//formata de forma generica os campos
function formataCampo(nome, campo, Mascara, evento) {
    var boleanoMascara;

    var Digitato = evento.keyCode;
    exp = /\-|\.|\/|\(|\)| /g
    campoSoNumeros = campo.replace(exp, "");

    var posicaoCampo = 0;
    var NovoValorCampo = "";
    var TamanhoMascara = campoSoNumeros.length;;

    if (Digitato != 8) { // backspace 
        for (i = 0; i <= TamanhoMascara; i++) {
            boleanoMascara = ((Mascara.charAt(i) == "-") || (Mascara.charAt(i) == ".")
                || (Mascara.charAt(i) == "/"))
            boleanoMascara = boleanoMascara || ((Mascara.charAt(i) == "(")
                || (Mascara.charAt(i) == ")") || (Mascara.charAt(i) == " "))
            if (boleanoMascara) {
                NovoValorCampo += Mascara.charAt(i);
                TamanhoMascara++;
            } else {
                NovoValorCampo += campoSoNumeros.charAt(posicaoCampo);
                posicaoCampo++;
            }
        }
        fd.getById(nome).value = NovoValorCampo;
        return true;
    } else {
        return true;
    }
}


function dataFormatadaBR(mdata) {
    dia = mdata.getDate().toString(),
        diaF = (dia.length == 1) ? '0' + dia : dia,
        mes = (mdata.getMonth() + 1).toString(), //+1 pois no getMonth Janeiro começa com zero.
        mesF = (mes.length == 1) ? '0' + mes : mes,
        anoF = mdata.getFullYear();
    return diaF + "/" + mesF + "/" + anoF;
}

function dataFormatadaUS(mdata) {
    dia = mdata.getDate().toString(),
        diaF = (dia.length == 1) ? '0' + dia : dia,
        mes = (mdata.getMonth() + 1).toString(), //+1 pois no getMonth Janeiro começa com zero.
        mesF = (mes.length == 1) ? '0' + mes : mes,
        anoF = mdata.getFullYear();
    return anoF + "/" + mesF + "/" + diaF;
}

function RetiraHoraData(data) {
    return data.substring(0, 10);
}

function formatReal(valor) {
    if (valor != "") {
        var exp = /\D/g
        var numero = valor;
        numero = numero.replace(exp, "");
        exp = /(\d)(\d{8})$/
        numero = numero.replace(exp, "$1.$2");
        exp = /(\d)(\d{5})$/
        numero = numero.replace(exp, "$1.$2");
        exp = /(\d)(\d{2})$/
        numero = numero.replace(exp, "$1,$2");
        return numero;
    }
    else return 0,00;
}

function formataValorBR(name) {
    var objvalor = fd.getValById(name);
    objvalor = objvalor.replace(/\D/g, "");
    objvalor = objvalor.replace(/(\d)(\d{8})$/, "$1.$2");
    objvalor = objvalor.replace(/(\d)(\d{5})$/, "$1.$2");
    objvalor = objvalor.replace(/(\d)(\d{2})$/, "$1,$2");
    fd.getValById(name).value = objvalor;
}

