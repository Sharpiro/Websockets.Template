export class SocketWrapper {
    private socket: WebSocket;
    private guid: string;
    private registeredFuncs: any = {};

    constructor() {
        // const baseUrl = window.location.href.split("/")[2];
        const baseUrl = "localhost:55024";
        const socketUrl = `ws://${baseUrl}/socket`;
        this.socket = new WebSocket(socketUrl);

        this.on("guid", (data: any) => {
            this.guid = data;
        });

        this.socket.onopen = () => {
            console.log("connection opened...");
            // this.send("guid", "guid", null);
            const func = this.registeredFuncs["connected"];
            if (func)
                func();
        };

        this.socket.onmessage = (message) => {
            // console.log("message received:");
            let jsonObject: any = undefined;
            try {
                jsonObject = JSON.parse(message.data);

            } catch (ex) {

            }

            if (!jsonObject) return;
            // console.log(jsonObject);
            const functionName: any = jsonObject.dataTitle;
            const func = this.registeredFuncs[functionName];
            if (func)
                func(jsonObject);
        };

        this.socket.onclose = () => {
            console.log("connection closed...");
        };
    }
    public on(name: string, func: Function) {
        name = name.toLowerCase();
        this.registeredFuncs[name] = func;
    }

    public send(dataType: string, dataTitle: string, data: any) {
        const dataObj = { dataType: dataType, dataTitle: dataTitle, data: data };
        const jsonString = JSON.stringify(dataObj);
        this.socket.send(jsonString);
    }

    public close() {
        this.socket.close();
    }

    public getGuid() {
        return this.guid;
    }
}