var SocketWrapper = (function () {
    function SocketWrapper() {
        var _this = this;
        this.registeredFuncs = {};
        this.socket = new WebSocket("ws://localhost:8095");
        this.on("guid", function (data) {
            _this.guid = data;
        });
        this.socket.onopen = function () {
            console.log("connection opened...");
            _this.send("guid", "guid", null);
            var func = _this.registeredFuncs["connect"];
            if (func)
                func();
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
    SocketWrapper.prototype.on = function (name, func) {
        name = name.toLowerCase();
        this.registeredFuncs[name] = func;
    };
    SocketWrapper.prototype.send = function (dataType, dataTitle, data) {
        var dataObj = { dataType: dataType, dataTitle: dataTitle, data: data };
        var jsonString = JSON.stringify(dataObj);
        this.socket.send(jsonString);
    };
    SocketWrapper.prototype.close = function () {
        this.socket.close();
    };
    SocketWrapper.prototype.getGuid = function () {
        return this.guid;
    };
    return SocketWrapper;
})();
///<reference path="../lib/definitions/jquery/jquery.d.ts/"/>
///<reference path="./socketWrapper.ts"/>
var socketWrapper = new SocketWrapper();
socketWrapper.on("ABCD", function () {
    console.log("ok");
});
socketWrapper.on("action1", function () {
    console.log("on \"clicked\" event hit");
});
socketWrapper.on("connect", function () {
    console.log("game on");
    socketWrapper.send("message", "addplayer", "");
});
$("#button1").click(function () {
    var dataType = $("#dataType").val();
    var dataTitle = $("#dataTitle").val();
    var dataValue = $("#dataValue").val();
    socketWrapper.send(dataType, dataTitle, dataValue);
});
//# sourceMappingURL=appCombined.js.map