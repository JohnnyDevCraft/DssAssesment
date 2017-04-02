(function () {
    var ctrlName = "categoriesController";

    ngApplication.controller(ctrlName, function ($scope) {

        $scope.categories = { [{ id: 1, Name: 'name', Description: 'some Description' }, { Id: 2, Name: 'Other', Description: 'Another Category' }]};

        $scope.itemEditing = { id: 1, Name: 'name', Description: 'Some Description' };

        $scope.GetEditItem = function (id) {

            for (var i = 0; i < $scope.categories.Length; i++) {
                if ($scope.categories[i].Id == id) {
                    $scope.itemEditing = $scope.categories[i];
                }
            }
        }

        $scope.view = "List";

        $scope.itemCreating = {};

        $scope.EditItem = function (id) {
            $scope.GetEditItem(id);
            $scope.view = "Edit";
        }

        $scope.NewItem = function () {
            $scope.itemCreating.Name = '';
            $scope.itemCreating.Description = '';
            $scope.view = "Create";
        }

        $scope.CancelEdit = function () {
            $scope.view = "List";
        }

        $scope.CancelNew = function () {
            $scope.view = "List";
        }
    }






    });

}());