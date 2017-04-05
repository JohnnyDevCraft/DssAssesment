var TaskControllerName = "TaskController";

var TaskController = ngApplication.controller(TaskControllerName,
[
    "$scope", "dlservice", function($scope, dlservice) {

        $scope.AllTasks = [];

        $scope.TaskEditing = {};

        $scope.TaskCreating = {
            "Name": "",
            "Description": ""
        }


    }
]);