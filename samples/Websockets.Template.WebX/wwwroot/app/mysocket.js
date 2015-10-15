var Wrapper = (function () {
    function Wrapper() {
        var _this = this;
        this.registeredFuncs = {};
        this.socket = new WebSocket("ws://localhost:8095");
        this.socket.onopen = function () {
            console.log("connection opened...");
            _this.socket.send("ABCD");
        };
        this.socket.onmessage = function (message) {
            console.log("message received: " + message.data);
            var functionName = message.data.toLowerCase();
            var func = _this.registeredFuncs[functionName];
            if (func)
                func();
        };
        this.socket.onclose = function () {
            console.log("connection closed...");
        };
    }
    Wrapper.prototype.on = function (name, func) {
        name = name.toLowerCase();
        this.registeredFuncs[name] = func;
    };
    return Wrapper;
})();
var wrapper = new Wrapper();
wrapper.on("ABCD", function () {
    console.log("ok");
});
wrapper.on("nothing", function () {
    console.log("on nothing event hit");
});
