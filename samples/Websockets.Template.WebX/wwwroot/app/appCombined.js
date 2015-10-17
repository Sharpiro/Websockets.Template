var Wrapper = (function () {
    function Wrapper() {
        var _this = this;
        this.registeredFuncs = {};
        this.socket = new WebSocket("ws://localhost:8095");
        this.on("guid", function (data) {
            _this.guid = data;
        });
        this.socket.onopen = function () {
            console.log("connection opened...");
            _this.send("guid", "guid", null);
        };
        this.socket.onmessage = function (message) {
            console.log("message received:");
            var jsonObject = undefined;
            try {
                jsonObject = JSON.parse(message.data);
            }
            catch (ex) {
            }
            if (!jsonObject)
                return;
            console.log(jsonObject);
            var functionName = jsonObject.dataTitle;
            var func = _this.registeredFuncs[functionName];
            if (func)
                func(jsonObject.data);
        };
        this.socket.onclose = function () {
            console.log("connection closed...");
        };
    }
    Wrapper.prototype.on = function (name, func) {
        name = name.toLowerCase();
        this.registeredFuncs[name] = func;
    };
    Wrapper.prototype.send = function (dataType, dataTitle, data) {
        var dataObj = { dataType: dataType, dataTitle: dataTitle, data: data };
        var jsonString = JSON.stringify(dataObj);
        this.socket.send(jsonString);
    };
    Wrapper.prototype.close = function () {
        this.socket.close();
    };
    Wrapper.prototype.getGuid = function () {
        return this.guid;
    };
    return Wrapper;
})();
///<reference path="./wrapper.ts"/>
///<reference path="../lib/definitions/jquery/jquery.d.ts/"/>
var wrapper = new Wrapper();
wrapper.on("ABCD", function () {
    console.log("ok");
});
wrapper.on("action1", function () {
    console.log("on \"clicked\" event hit");
});
$("#button1").click(function () {
    var inputText = $("#input").val();
    wrapper.send("broadcast", "action1", inputText);
});
//# sourceMappingURL=appCombined.js.map