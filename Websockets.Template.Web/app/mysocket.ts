var socket = new WebSocket("ws://localhost:8095");

socket.onopen = () =>
{
    console.log("connection opened...");
    socket.send("MDN");
};

socket.onmessage = (message) =>
{
    console.log(`message received: ${message.data}`);
}

socket.onclose = () =>
{
    console.log("connection closed...");
}