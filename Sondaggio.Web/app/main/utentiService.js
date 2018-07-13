(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.factory('utentiService',
			function ($http) {
				return {
					list: function () {
						return $http.get("/api/utenti");
					},
					detail: function (id) {
						return $http.get("/api/utenti/" + id);
					},
					create: function (utenti) {
						var req = {
							method: 'POST',
							url: '/api/utenti',
							headers: {
								'Content-Type': 'application/json'
							},
							data: utenti
						};
						return $http(req);
					},
					save: function (utente) {
						var req = {
							method: 'PATCH',
							url: '/api/utenti/' + utente.IdUtente,
							headers: {
								'Content-Type': 'application/json'
							},
							data: utente
						};
						return $http(req);
					}
				}
			});
})(window, window.angular);
