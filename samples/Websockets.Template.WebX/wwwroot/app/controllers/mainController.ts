/// <reference path="../../lib/definitions/angularjs/angular.d.ts" />

class MainController
{
    private socketId: string;
    private applicationId: string;
    private socket: SocketWrapper;
    private hand: any = [];
    private money: number = 500;
    private currentBet: number = 50;

    constructor(private scope: any, private $http: ng.IHttpService)
    {
        scope.dataType = "message";
        scope.dataTitle = "update";
        scope.dataValue = "getcard";
        scope.vm = this;
        this.socket = new SocketWrapper();
        this.initalizeSocketMessages();
    }

    private initalizeSocketMessages()
    {
        this.socket.on("connect", (data: any) =>
        {
            this.socket.send("message", "addplayer", "");
        });

        this.socket.on("addplayer", (data: IDataTransferObject) =>
        {
            this.socketId = data.socketId;
            this.applicationId = data.applicationId;
            this.scope.$apply();
            console.log("game joined");
        });

        this.socket.on("gamestarted", (data: any) =>
        {

        });

        this.socket.on("update", (data: any) =>
        {
            var card = JSON.parse(data.data);
            if (card)
                this.hand.push(card);

            this.scope.$apply();
        });

        this.socket.on("reset", () =>
        {
            this.hand.length = 0;
            this.scope.$apply();
        });
    }

    private submitButtonClick(dataType: any, dataTitle: any, dataValue: any)
    {
        this.socket.send(dataType, dataTitle, dataValue);
        console.log("sending...");
    }

    private resetDeck()
    {
        this.socket.send("message", "update", "resetdeck");
        this.hand.length = 0;
    }

    private bet(currentBet: number)
    {
        this.currentBet = currentBet;
        this.socket.send("message", "bet", currentBet);
    }
}

app.controller("mainController", ["$scope", "$http", MainController]);
