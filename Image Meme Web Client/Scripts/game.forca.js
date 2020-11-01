$(function () {

    var forcajogoloader = "#forcajogoloader";
    var forcasegredo = "#forcasegredo";
    var forcajogadores = "#forcajogadores";
    var forcajogocomvencedor = "#forcajogocomvencedor";
    var forcajogosemvencedor = "#forcajogosemvencedor";

    var htmlJogoIniciando = "JogoIniciando.html";
    var htmlJogoSegredo = "JogoSegredo.html";
    var htmlJogoParticipantes = "JogoParticipantes.html";
    var htmlJogoParticipante = "JogoParticipante.html";
    var htmlJogoSemVencedor = "JogoSemVencedor.html";
    var htmlJogoComVencedor = "JogoComVencedor.html";

    var segredo = "#segredo";
    var jogadores = "#jogadores";
    var divjogador = "#jogador";
    var vidajogador = ".vidajogador";
    var nomejogador = ".nomejogador";
    var tentativasjogador = ".tentativasjogador";
    var fimjogosemvencedortexto = "#fimjogosemvencedortexto";
    var fimjogocomvencedortexto = "#fimjogocomvencedortexto";

    $.connection.hub.url = "http://localhost:8080/signalr";
    var forca = $.connection.forcaHub;

    var resetarcontainers = function () {
        $(forcajogosemvencedor).fadeOut().empty();
        $(forcajogocomvencedor).fadeOut().empty();
        $(forcajogadores).fadeOut().empty();
        $(forcasegredo).fadeOut().empty();
        $(forcajogoloader).fadeOut().empty();
    };



    forca.client.iniciando = function () {
        resetarcontainers();
        $(forcajogoloader)
            .fadeOut(300, function () {
                $(this).empty();
                $(forcajogoloader)
                    .load(htmlJogoIniciando)
                    .fadeIn(300);
            });
    };

    forca.client.iniciado = function (jogo) {
        $(forcajogoloader).fadeOut(300, function () {
            $(this).empty();
            $(forcasegredo).load(htmlJogoSegredo, function () {
                $(segredo).html(jogo.Segredo);
                $(this).fadeIn(300, function () {
                    $(forcajogadores).load(htmlJogoParticipantes).fadeIn(300);
                });
            });
        });
    };

    forca.client.adicionajogador = function (jogador) {
        $('<div id="jogador' + jogador.Nome + '"/>')
            .load(htmlJogoParticipante, function () {
                $(this).find(nomejogador).html(jogador.Nome);
                $(this).find(tentativasjogador).html('Tentativas:' + jogador.Tentativas);
                $(this).appendTo(jogadores);
            });
    };

    forca.client.respostacerta = function (resposta) {
        $(segredo).html(resposta);
    };

    forca.client.respostaerrada = function (jogador) {
        var divJogadorParticipante = $(divjogador + jogador.Nome);

        switch (true) {
            case (jogador.Tentativas < 7 && jogador.Tentativas >= 5):
                $(divJogadorParticipante)
                    .find(vidajogador)
                    .removeClass('bg-success')
                    .addClass('bg-warning');
                break;
            case (jogador.Tentativas < 5):
                $(divJogadorParticipante)
                    .find(vidajogador)
                    .removeClass('bg-warning')
                    .addClass('bg-danger');
                break;
            default:
        }

        if (jogador.Tentativas > 0) {

            $(divJogadorParticipante)
                .find(tentativasjogador)
                .html('Tentativas: ' + jogador.Tentativas);

            $(divJogadorParticipante)
                .find(vidajogador)
                .width(jogador.Tentativas + '0%');
        }
        else {

            $(divJogadorParticipante)
                .find(tentativasjogador)
                .addClass('text-danger')
                .html('Tentativas: 0');

            $(divJogadorParticipante)
                .find(vidajogador).width('0%');

            $(divJogadorParticipante)
                .find(nomejogador)
                .addClass('text-danger');
        }
    };

    forca.client.encerradosemvencedor = function (mensagem) {
        resetarcontainers();
        $(forcajogosemvencedor)
            .load(htmlJogoSemVencedor, function () {
                $(forcajogosemvencedor)
                    .find(fimjogosemvencedortexto)
                    .empty()
                    .html(mensagem);
                $(forcajogosemvencedor).fadeIn(300);
            });
    };

    forca.client.encerradocomvencedor = function (mensagem) {
        resetarcontainers();
        $(forcajogocomvencedor)
            .load(htmlJogoComVencedor, function () {
                $(forcajogocomvencedor)
                    .find(fimjogocomvencedortexto)
                    .empty()
                    .html(mensagem);
                $(forcajogocomvencedor).fadeIn(300);
            });
    };

    $.connection.hub.start().done(function () { });
});