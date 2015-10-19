///<reference path="../lib/definitions/jquery/jquery.d.ts/"/>
///<reference path="./socketWrapper.ts"/>

var socketWrapper = new SocketWrapper();
socketWrapper.on("ABCD", () =>
{
    console.log("ok");
});
socketWrapper.on("action1", () =>
{
    console.log(`on "clicked" event hit`);
});

socketWrapper.on("connect", () =>
{
    console.log("game on");
    socketWrapper.send("message", "addplayer", "");
});

$("#button1").click(() =>
{
    var dataType = $("#dataType").val();
    var dataTitle = $("#dataTitle").val();
    var dataValue = $("#dataValue").val();
    socketWrapper.send(dataType, dataTitle, dataValue);
});