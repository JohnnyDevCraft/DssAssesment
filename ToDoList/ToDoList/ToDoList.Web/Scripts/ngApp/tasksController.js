var TaskControllerName = "TaskController";

var TaskController = ngApplication.controller(TaskControllerName,
[
    "$scope", "dlservice", function($scope, dlservice) {

        $scope.SortTasksFinal = function () {
            switch ($scope.CurrentSortBy) {
                case "Name":
                    if ($scope.CurrentDirectionAsc) {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj1.Name - obj2.Name;
                        });
                    } else {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj2.Name - obj1.Name;
                        });
                    }
                    break;
                case "Direction":
                    if ($scope.CurrentDirectionAsc) {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj1.Direction - obj2.Direction;
                        });
                    } else {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj2.Direction - obj1.Direction;
                        });
                    }
                    break;
                case "Priority":
                    if ($scope.CurrentDirectionAsc) {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj1.Priority - obj2.Priority;
                        });
                    } else {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj2.Priority - obj1.Priority;
                        });
                    }
                    break;
                case "DueDate":
                    if ($scope.CurrentDirectionAsc) {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            if (obj1.DueDate > obj2.DueDate) return 1;
                            if (obj1.DueDate < obj2.DueDate) return -1;
                            return 0;
                        });
                    } else {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            if (obj1.DueDate > obj2.DueDate) return -1;
                            if (obj1.DueDate < obj2.DueDate) return 1;
                            return 0;
                        });
                    }
                    break;
                case "Completed":
                    if ($scope.CurrentDirectionAsc) {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj1.Completed - obj2.Completed;
                        });
                    } else {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj2.Completed - obj1.Completed;
                        });
                    }
                    break;
                case "CompletedAt":
                    if ($scope.CurrentDirectionAsc) {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj1.CompletedAt - obj2.CompletedAt;
                        });
                    } else {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj2.CompletedAt - obj1.CompletedAt;
                        });
                    }
                    break;
                case "Category":
                    if ($scope.CurrentDirectionAsc) {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj1.Category - obj2.Category;
                        });
                    } else {
                        $scope.FilteredTasks.sort(function (obj1, obj2) {
                            return obj2.Category - obj1.Category;
                        });
                    }
                    break;

                default:
            }
        }

        $scope.SearchString = "";

        $scope.Search = function() {
            if ($scope.SearchString === "") {
                $scope.ShowAll();
            } else {
                dlservice.Tasks.Search($scope.SearchString, $scope.LoadTasksViaSearch, $scope.ShowErrors);
            }
        }

        $scope.Categories = {};

        $scope.ViewType = "List";

        $scope.AllTasks = [];
        $scope.FilteredTasks = [];

        $scope.TaskEditing = {};

        $scope.EditTask = function(id) {
            $scope.AllTasks.forEach(function(item) {
                if (item.Id === id) {
                    $scope.TaskEditing = item;
                }
            });
            $scope.ViewType = "Edit";
        }
        $scope.CancelEdit = function() {
            $scope.ViewType = "List";
        }
        $scope.CompleteEdit = function () {
            $("#ProcessingModal").modal('show');
            dlservice.Tasks.Update($scope.TaskEditing, $scope.ShowAll, $scope.ShowErrors);
            
        }

        $scope.NewTask = function() {
            $scope.TaskCreating = {};
            $scope.ViewType = "Create";
        }
        $scope.CancelCreate =function() {
            $scope.ViewType = "List";
        }
        $scope.CompleteCreate = function() {
            $("#ProcessingModal").modal('show');
            dlservice.Tasks.Create($scope.TaskCreating, $scope.ShowAll, $scope.ShowErrors);
        }

        $scope.UnCancel = function (id) {
            $scope.AllTasks.forEach(function (item) {
                if (item.Id === id) {
                    $scope.TaskEditing = item;
                }
            });
            $scope.TaskEditing.Canceled = false;
            $scope.TaskEditing.CancelReason = "";
            dlservice.Tasks.Update($scope.TaskEditing, $scope.ShowAll, $scope.ShowErrors);
        }

        $scope.Cancel = function(id) {
            $scope.AllTasks.forEach(function (item) {
                if (item.Id === id) {
                    $scope.TaskEditing = item;
                }
            });
            $("#CancelModal").modal("show");
        }
        $scope.CancelCancellation = function(){
            $("#CancelModal").modal("hide");
        }
        $scope.CommitCancel = function() {
            $scope.TaskEditing.Canceled = true;
            dlservice.Tasks.Update($scope.TaskEditing, $scope.ShowAll, $scope.ShowErrors);
            $("#CancelModal").modal("hide");
        }



        $scope.UnComplete = function (id) {
            var cItem = {};
            $scope.AllTasks.forEach(function (item) {
                if (item.Id === id) {
                    cItem = item;
                }
            });

            cItem.Completed = false;
            cItem.CompletedAt = null;
            $("#ProcessingModal").modal('show');
            dlservice.Tasks.Update(cItem, $scope.ShowAll, $scope.ShowErrors);

        }

        $scope.Complete = function(id) {
            var cItem = {};
            $scope.AllTasks.forEach(function (item) {
                if (item.Id === id) {
                    cItem = item;
                }
            });

            cItem.Completed = true;
            date = new Date();
            cItem.CompletedAt = date.toUTCString();
            $("#ProcessingModal").modal('show');
            dlservice.Tasks.Update(cItem, $scope.ShowAll, $scope.ShowErrors);

        }

        $scope.Delete = function(id) {
            $("#ProcessingModal").modal('show');
            dlservice.Tasks.Delete(id, $scope.ShowAll, $scope.ShowErrors);
        }

        $scope.TaskCreating = {
            "Name": "",
            "Description": "",
            "Priority": 0,
            "DueDate": "",
            "Completed": false,
            "CompletedAt": "",
            "CategoryId": 0
        };

        $scope.SortTasks = function(sortBy) {

            if ($scope.CurrentSortBy === sortBy) {
                $scope.CurrentDirectionAsc = !$scope.CurrentDirectionAsc;
                $scope.SortTasksFinal();
            } else {
                $scope.CurrentSortBy = sortBy;
                $scope.CurrentDirectionAsc = true;
                $scope.SortTasksFinal();
            }

        }

        $scope.GotoCategories = function() {
            window.location = ngCurrentLoc + "/Categories";
        }

        $scope.CurrentSortBy = "";
        $scope.CurrentDirectionAsc = true;

        $scope.Errors = [];
        $scope.ErrorsActive = false;

        $scope.ClearNew = function() {
            $scope.TaskCreating = {
                "Name": "",
                "Description": "",
                "Priority": 0,
                "DueDate": "",
                "Completed": false,
                "CompletedAt": "",
                "CategoryId": 0
            };
        }

        $scope.LoadTasks = function(data) {
            $scope.AllTasks = data;
            $scope.CurrentSortBy = "";
            $scope.SortTasks("Name");
            $scope.ErrorsActive = false;
            $scope.ViewType = "List";
            $scope.ViewActive();
            $("#ProcessingModal").modal('hide');
        }


        $scope.LoadTasksViaSearch = function (data) {
            $scope.AllTasks = data;
            $scope.CurrentSortBy = "";
            $scope.SortTasks("Name");
            $scope.ErrorsActive = false;
            $scope.ViewType = "List";
            $scope.ViewAll();
            $("#ProcessingModal").modal('hide');
        }


        $scope.ShowErrors = function(errors) {
            $scope.Errors = errors;
            $scope.ErrorsActive = true;
            $("#ProcessingModal").modal('hide');
        }


        $scope.ViewActive = function () {
            $scope.FilteredTasks = $scope.AllTasks.filter(function (value) {
                $scope.ResultType = "Active";
                return value.Completed === false && value.Canceled === false;
            });
        }
        $scope.ViewComplete = function () {
            $scope.FilteredTasks = $scope.AllTasks.filter(function (value) {
                $scope.ResultType = "Complete";
                return value.Completed === true && value.Canceled === false;
            });
        }
        $scope.ViewAll = function () {
            $scope.FilteredTasks = $scope.AllTasks.filter(function (value) {
                $scope.ResultType = "All";
                return true;
            });
        }
        $scope.ViewCanceled = function () {
            $scope.FilteredTasks = $scope.AllTasks.filter(function (value) {
                $scope.ResultType = "Canceled";
                return value.Canceled === true;
            });
        }

        $scope.ResultType = "";


        $scope.ShowAll = function() {
            dlservice.Tasks.Get($scope.LoadTasks);
            
        }

        $scope.LoadCategories = function(data) {
            $scope.Categories = data;
        }

        $("#ProcessingModal").modal('show');

        dlservice.Categories.Get($scope.LoadCategories);

        $scope.ShowAll();
        
    }
]);