/// <reference path="../lib/definitions/angularjs/angular.d.ts" />
var app = angular.module("app", ["ui.router"]);
app.config(["$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise("/main");
        $stateProvider
            .state("main", {
            url: "/main",
            templateUrl: "app/templates/mainTemplate.html",
            controller: "mainController"
        });
    }]);
var SocketWrapper = (function () {
    function SocketWrapper() {
        var _this = this;
        this.registeredFuncs = {};
        var hostUrl = window.location.href.split("/")[2];
        this.socket = new WebSocket("ws://" + hostUrl + "/socket");
        this.on("guid", function (data) {
            _this.guid = data;
        });
        this.socket.onopen = function () {
            console.log("connection opened...");
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
                func(jsonObject);
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
/// <reference path="../../lib/definitions/angularjs/angular.d.ts" />
var MainController = (function () {
    function MainController(scope, $http) {
        this.scope = scope;
        this.$http = $http;
        this.hand = [];
        this.money = 500;
        this.currentBet = 50;
        scope.dataType = "message";
        scope.dataTitle = "update";
        scope.dataValue = "getcard";
        scope.vm = this;
        this.socket = new SocketWrapper();
        this.initalizeSocketMessages();
    }
    MainController.prototype.initalizeSocketMessages = function () {
        var _this = this;
        this.socket.on("connect", function (data) {
            _this.socket.send("message", "addplayer", "");
        });
        this.socket.on("addplayer", function (data) {
            _this.socketId = data.socketId;
            _this.applicationId = data.applicationId;
            _this.scope.$apply();
            console.log("game joined");
        });
        this.socket.on("gamestarted", function (data) {
        });
        this.socket.on("update", function (data) {
            var card = JSON.parse(data.data);
            if (card)
                _this.hand.push(card);
            _this.scope.$apply();
        });
        this.socket.on("reset", function () {
            _this.hand.length = 0;
            _this.scope.$apply();
        });
    };
    MainController.prototype.submitButtonClick = function (dataType, dataTitle, dataValue) {
        this.socket.send(dataType, dataTitle, dataValue);
        console.log("sending...");
    };
    MainController.prototype.resetDeck = function () {
        this.socket.send("message", "update", "resetdeck");
        this.hand.length = 0;
    };
    MainController.prototype.bet = function (currentBet) {
        this.currentBet = currentBet;
        this.socket.send("message", "bet", currentBet);
    };
    return MainController;
})();
app.controller("mainController", ["$scope", "$http", MainController]);
//# sourceMappingURL=appCombined.js.map