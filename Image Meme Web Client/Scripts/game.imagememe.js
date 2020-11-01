$(function () {
    $.connection.hub.url = "http://localhost:8080/signalr";
    var imgContainer = "#imgMeme";
    var imageMeme = $.connection.imagemMemeHub;

    imageMeme.client.exibirimagem = function (imgUrl) {
        console.log(imgUrl);
        $(imgContainer).attr('src', imgUrl).fadeIn(300);
        window.setTimeout(escnderImagem, 5000);
    };

    var escnderImagem = function () {
        $(imgContainer).fadeOut(300);
    }

    $.connection.hub.start().done(function () { });
});