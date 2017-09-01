/// <reference path="../lib/definitions/angularjs/angular.d.ts" />

let app = angular.module("app", ["ui.router"]);

app.config(["$stateProvider", "$urlRouterProvider", ($stateProvider: any, $urlRouterProvider: any) =>
{
    $urlRouterProvider.otherwise("/main");

    $stateProvider
        .state("main", {
            url: "/main",
            templateUrl: "app/templates/mainTemplate.html",
            controller: "mainController"
        });
}]);


interface IDataTransferObject
{
    dataType: string;
    dataTitle: string;
    data: string;
    applicationId: string;
    socketId: string;
    socketNumber: number;
}