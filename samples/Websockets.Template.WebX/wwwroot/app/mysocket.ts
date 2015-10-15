class Wrapper
{
    private socket: WebSocket;
    private registeredFuncs = {};

    constructor()
    {
        this.socket = new WebSocket("ws://localhost:8095");
        this.socket.onopen = () =>
        {
            console.log("connection opened...");
            this.socket.send("ABCD");
        };

        this.socket.onmessage = (message) =>
        {
            console.log(`message received: ${message.data}`);
            var functionName = message.data.toLowerCase();
            var func = this.registeredFuncs[functionName];
            if (func)
                func();
        }

        this.socket.onclose = () =>
        {
            console.log("connection closed...");
        }
    }
    public on(name: string, func: Function)
    {
        name = name.toLowerCase();
        this.registeredFuncs[name] = func;
    }
}

var wrapper = new Wrapper();
wrapper.on("ABCD", () =>
{
    console.log("ok");
});
wrapper.on("nothing", () =>
{
    console.log(`on nothing event hit`);
});

