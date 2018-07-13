(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.factory('risposteService',
			function ($http) {
				return {
					list: function () {
						return $http.get("/api/risposte");
					},
					detail: function (id) {
						return $http.get("/api/risposte/" + id);
					},
					create: function (risposta) {
						var req = {
							method: 'POST',
							url: '/api/risposte',
							headers: {
								'Content-Type': 'application/json'
							},
							data: risposta
						};
						return $http(req);
					},
					save: function (risposta) {
						var req = {
							method: 'PATCH',
							url: '/api/risposte/' + risposta.IdRisposta,
							headers: {
								'Content-Type': 'application/json'
							},
							data: risposta
						};
						return $http(req);
					}
				}
			});
})(window, window.angular);
