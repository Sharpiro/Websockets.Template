class SocketWrapper
{
    private socket: WebSocket;
    private guid: string;
    private registeredFuncs: any = {};

    constructor()
    {
        this.socket = new WebSocket("ws://localhost:8095");

        this.on("guid", (data: any) =>
        {
            this.guid = data;
        });

        this.socket.onopen = () =>
        {
            console.log("connection opened...");
            this.send("guid", "guid", null);
        };

        this.socket.onmessage = (message) =>
        {
            console.log("message received:");
            var jsonObject: any = undefined;
            try
            {
                jsonObject = JSON.parse(message.data);

            } catch (ex)
            {

            }

            if (!jsonObject) return;
            console.log(jsonObject);
            var functionName: any = jsonObject.dataTitle;
            var func = this.registeredFuncs[functionName];
            if (func)
                func(jsonObject.data);
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

    public send(dataType: string, dataTitle: string, data: any)
    {
        const dataObj = { dataType: dataType, dataTitle: dataTitle, data: data };
        const jsonString = JSON.stringify(dataObj);
        this.socket.send(jsonString);
    }

    public close()
    {
        this.socket.close();
    }

    public getGuid()
    {
        return this.guid;
    }
}