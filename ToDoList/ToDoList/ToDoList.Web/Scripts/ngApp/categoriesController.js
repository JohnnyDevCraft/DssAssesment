
var ctrlName = "categoriesController";

var catagoriesController = ngApplication.controller(ctrlName,
    ["$scope", "$http", "dlservice", function ($scope, $http, dlservice) {

        $scope.categories = {
            "categories": [
                {
                    "Id": 1,
                    "Name": "name",
                    "Description": "some Description"
                },
                {
                    "Id": 2,
                    "Name": "Other",
                    "Description": "Another Category"
                }
            ]
        };

        $scope.itemEditing = {};

        $scope.GetEditItem = function(id) {
            $scope.itemEditing = {};
            for (var i = 0; i < $scope.categories.categories.length; i++) {
                if ($scope.categories.categories[i].Id === id) {
                    $scope.itemEditing = $scope.categories.categories[i];
                }
            }
        };

        $scope.view = "List";

        $scope.itemCreating = {};

        $scope.ErrorsReturned = false;

        $scope.Errors = ["Error 1", "Error 2"];

        $scope.EditItem = function(id) {
            $scope.GetEditItem(id);
            $scope.view = "Edit";
        };

        $scope.NewItem = function() {
            $scope.itemCreating.Name = "";
            $scope.itemCreating.Description = "";
            $scope.view = "Create";
        };

        $scope.CancelEdit = function() {
            $scope.view = "List";
            $scope.ErrorsReturned = false;
        };

        $scope.CompleteEdit = function() {
            dlservice.Categories.Update($scope.itemEditing, $scope.GetCategories, $scope.SetErrors);
        }

        $scope.CancelNew = function() {
            $scope.view = "List";
            $scope.ErrorsReturned = false;
        };

        $scope.CompleteNew = function () {
            dlservice.Categories.Create($scope.itemCreating, $scope.GetCategories, $scope.SetErrors);
        };

        $scope.GetCategories = function() {
            dlservice.Categories.Get($scope.SetCategories);
            $scope.view = "List";
            $scope.ErrorsReturned = false;
        };

        $scope.SetCategories = function(data) {
            $scope.categories.categories = data;
        };

        $scope.SetErrors = function(errors) {
            $scope.Errors = errors;
            $scope.ErrorsReturned = true;
        };

        $scope.ClearErrors = function() {
            $scope.ErrorsReturned = false;
        };

        $scope.Delete = function(id) {
            dlservice.Categories.Delete(id, $scope.GetCategories, $scope.SetErrors);
        };

        $scope.GotoTasks = function () {
            window.location = ngCurrentLoc;
        }

        $scope.GetCategories();

        

    }]
);
