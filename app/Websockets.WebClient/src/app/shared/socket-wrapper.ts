export class SocketWrapper {
    private socket: WebSocket;
    private _id: string;
    private _applicationId: string;
    private registeredFuncs: any = {};

    public get id() {
        return this._id;
    }

    public get applicationId() {
        return this._applicationId;
    }

    constructor() {

    }

    public connect(): void {
        // const baseUrl = window.location.href.split("/")[2];
        const baseUrl = "localhost:55024";
        const socketUrl = `ws://${baseUrl}/socket`;
        this.socket = new WebSocket(socketUrl);

        this.socket.onopen = (data: any) => {
            console.log("connection opened...");
            // this.send("guid", "guid", null);
            const func = this.registeredFuncs["connected"];
            if (func)
                func();
        };

        this.socket.onmessage = (message) => {
            const jsonObject = JSON.parse(message.data);
            if (!jsonObject) return;
            if (!this.id || !this.applicationId) {
                this._id = jsonObject.socketId;
                this._applicationId = jsonObject.applicationId;
            }
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
}