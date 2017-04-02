(function () {
    var ctrlName = "categoriesController";

    ngApplication.controller(ctrlName, function ($scope) {

        $scope.categories = { [{ id: 1, Name: 'name', Description: 'some MSDescription' }, { Id: 2, Name: 'Other', Description: 'Another Category' }]};

        $scope.EditItem = function (id) {

            for (var i = 0; i < $scope.categories.Length

        }
    }






    });

}());