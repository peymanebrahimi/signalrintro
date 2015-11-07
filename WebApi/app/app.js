(function () {
    'use strict';
    var app = angular.module('app', ['ui.bootstrap', 'ui.bootstrap.persian.datepicker']);
    var uri = 'api/complaints';
    var errorMessage = function (data, status) {
        return 'Error: ' + status + (data.message !== undefined ? (' ' + data.message) : '');
    };
    var hub = $.connection.myHub;

    //$(function () {
    //    $.connection.hub.logging = true;
    //    $.connection.hub.start();
    //});

    //$.connection.hub.error(function (err) {
    //    console.log('An error occurred: ' + err);
    //});

    //app.value('proxy', $.connection.hub);//proxy is now the proxy object

    app.controller('myController', [
        '$http', '$scope',
        function ($http, $scope) {
            $scope.complaints = [];
            $scope.customerIdSubscribed = "";
            $scope.getAllFromCustomer = function () {
                if ($scope.customerId.length == 0) {
                    return;
                }
                $http.get(uri + '/' + $scope.customerId)
                    .success(function (data, status) {
                        $scope.complaints = data;
                        if ($scope.customerIdSubscribed &&
                            $scope.customerIdSubscribed.length > 0 &&
                            $scope.customerIdSubscribed !== $scope.customerId) {
                            hub.server.unSubscribe($scope.customerIdSubscribed);
                        }
                        hub.server.subscribe($scope.customerId);
                        $scope.customerIdSubscribed = $scope.customerId;
                    })
                    .error(function (data, status) {
                        $scope.complaints = [];
                        $scope.errorToSearch = errorMessage(data, status);
                    });
            };

            $scope.postOne = function () {
                $http.post(uri, {
                    id: 0,
                    customerId: $scope.customerId,
                    description: $scope.descToAdd
                })
                    .success(function (data, status) {
                        $scope.errorToAdd = null;
                        $scope.descToAdd = null;
                    })
                    .error(function (data, status) {
                        $scope.errorToAdd = errorMessage(data, status);
                    });
            };

            $scope.putOne = function () {
                $http.put(uri + '/' + $scope.idToUpdate, {
                    id: $scope.idToUpdate,
                    customerId: $scope.customerId,
                    description: $scope.descToUpdate
                })
                    .success(function (data, status) {
                        $scope.errorToUpdate = null;
                        $scope.idToUpdate = null;
                        $scope.descToUpdate = null;
                    })
                    .error(function (data, status) {
                        $scope.errorToUpdate = errorMessage(data, status);
                    });
            };

            $scope.deleteOne = function (item) {
                $http.delete(uri + '/' + item.id)
                    .success(function (data, status) {
                        $scope.errorToDelete = null;
                    })
                    .error(function (data, status) {
                        $scope.errorToDelete = errorMessage(data, status);
                    });
            };

            $scope.editIt = function (item) {
                $scope.idToUpdate = item.id;
                $scope.descToUpdate = item.description;
            };

            $scope.toShow = function () {
                return $scope.complaints && $scope.complaints.length > 0;
            };

            // at initial page load
            $scope.orderProp = 'id';

            // signalr client functions
            hub.client.addItem = function (item) {
                $scope.complaints.push(item);
                $scope.$apply(); // this is outside of angularjs, so need to apply
                console.log('added: ' + item);
            };

            hub.client.deleteItem = function (item) {
                var array = $scope.complaints;
                for (var i = array.length - 1; i >= 0; i--) {
                    if (array[i].id === item.id) {
                        array.splice(i, 1);

                    }
                }
                $scope.$apply();
                console.log('deleted: ' + item);
            };

            hub.client.updateItem = function (item) {
                var array = $scope.complaints;
                for (var i = array.length - 1; i >= 0; i--) {
                    if (array[i].id === item.id) {
                        array[i].description = item.description;
                        $scope.$apply();
                    }
                }
                console.log('updated: ' + item);
            };

            $.connection.hub.start(); // connect to signalr hub

            $scope.openPersian = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.persianIsOpen = true;
                $scope.gregorianIsOpen = false;
            };
            $scope.openGregorian = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.gregorianIsOpen = true;
                $scope.persianIsOpen = false;
            };

            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 6
            };

        }
    ]);
}());