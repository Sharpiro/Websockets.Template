var socket = new WebSocket("ws://localhost:8095");
socket.onopen = function () {
    console.log("connection opened...");
    socket.send("MDN");
};
socket.onmessage = function (message) {
    console.log("message received: " + message.data);
};
socket.onclose = function () {
    console.log("connection closed...");
};
//# sourceMappingURL=mysocket.js.map