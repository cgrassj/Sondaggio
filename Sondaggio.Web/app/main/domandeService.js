(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.factory('domandeService', function ($http) {
			return {
				list: function () {
					return $http.get("/api/domande");
				},
				detail: function (id) {
					return $http.get("/api/domande/" + id);
				},
				create: function (domande) {
					var req = {
						method: 'POST',
						url: '/api/domande',
						headers: {
							'Content-Type': 'application/json'
						},
						data: domanda
					};
					return $http(req);
				},
				updateDomande: function (id) {
					return $http.get("/api/updateDomande/" + id);
				},
				save: function (domande) {
					var req = {
						method: 'PATCH',
						url: '/api/domande/' + domanda.IdDomande,
						headers: {
							'Content-Type': 'application/json'
						},
						data: domanda
					};
					return $http(req);
				}
			}
		});

})(window, window.angular);
