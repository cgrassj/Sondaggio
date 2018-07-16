(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.config(function ($stateProvider) {
			$stateProvider
				.state('private.utenti',
					{
						url: '/utenti',
						templateUrl: 'app/main/utenti.html',
						controller: 'utentiCtrl'
					});
		})
		.controller('utentiCtrl',
			function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {

				if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
					utentiService.list().then(function (result) {
						$scope.Utenti = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				} else {
					utentiService.detail($stateParams.id).then(function (result) {
						$scope.Utente = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				}
				$scope.save = function (result) {
					utentiService.save(result).then(function () {
					}).catch(function () {
						$state.go("errore");
					});
				}
			});
})(window, window.angular);
