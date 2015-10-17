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


$("#button1").click(() =>
{
    var dataType = $("#dataType").val();
    var inputText = $("#input").val();
    socketWrapper.send(dataType, "action1", inputText);
});