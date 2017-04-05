(function() {
    ngApplication.service("dlservice",
    [
        "$http", function($http) {

            this.Categories =
            {
                "Get": function(callback) {
                    $http.get(ngCurrentLoc + "/Categories/GetAllAsync").then(function(response) {
                        callback(response.data);
                    }, function(response) {
                        alert(response.data);
                    });
                },
                "Create": function(obj, callback, error) {
                    $http.post(ngCurrentLoc + "/Categories/CreateAsync", obj).then(function (response) {
                        if (response.data.Result === "Success") {
                            callback();
                        } else {
                            error(response.data.ValidationResults);
                        }
                        
                    }, function(error) {
                        alert(error.data);
                    });
                },
                "Update": function(obj, callback, error) {
                    $http.post(ngCurrentLoc + "/Categories/UpdateAsync", obj).then(function(response) {
                        if (response.data.Result === "Success") {
                            callback();
                        } else {
                            error(response.data.ValidationResults);
                        }
                    }, function(response) {
                        alert(response.data);
                    });
                },
                "Delete": function(id, callback, error) {
                    $http.get(ngCurrentLoc + "/Categories/DeleteAsync/" + id).then(function(response) {
                        if (response.data.Result === "Success") {
                            callback();
                        } else {
                            error(response.data.ValidationResults);
                        }
                    }, function(response) {
                        alert(response.data);
                    });
                }
            };

            this.Tasks = {
                "Get": function(callback) {
                    
                },
                "Find": function(id, callback, error) {
                    
                },
                "Search": function(searchStr, callback, error) {
                    
                },
                "Update": function(obj, callback, error) {
                    
                },
                "Create": function(obj, callback, error) {
                    
                },
                "Delete": function(id, callback, error) {
                    
                }
            }


        }
    ]);
}());

