(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.factory('sondaggiService',
			function ($http) {
				return {
					list: function () {
						return $http.get("/api/sondaggi");
          },
          listValidi: function () {
            //return $http.get("/api/sondaggivalidi");
            var req = {
              method: 'GET',
              url: '/api/sondaggivalidi',
              headers: {
                'Content-Type': 'application/json'
              }
            };
            return $http(req);
          },
					detail: function (id) {
						return $http.get("/api/sondaggi/" + id);
					},
					delete: function (cmp) {
						var req = {
							method: 'DELETE',
							url: '/api/sondaggi/' + cmp.IdSondaggio,
							headers: {
								'Content-Type': 'application/json'
							}
						};
						return $http(req);
					},
					create: function (cmp) {
						var req = {
							method: 'POST',
							url: '/api/sondaggi',
							headers: {
								'Content-Type': 'application/json'
							},
							data: cmp
						};
						return $http(req);
					},
					save: function (cmp) {
						var req = {
							method: 'PATCH',
							url: '/api/sondaggi/' + cmp.IdSondaggio,
							headers: {
								'Content-Type': 'application/json'
							},
							data: cmp
						};
						return $http(req);
					}
				}
			});
})(window, window.angular);
