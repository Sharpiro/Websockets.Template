///<reference path="./wrapper.ts"/>
///<reference path="../lib/definitions/jquery/jquery.d.ts/"/>

var wrapper = new Wrapper();
wrapper.on("ABCD", () =>
{
    console.log("ok");
});
wrapper.on("action1", () =>
{
    console.log(`on "clicked" event hit`);
});


$("#button1").click(() =>
{
    var inputText = $("#input").val();
    wrapper.send("broadcast", "action1", inputText);
});