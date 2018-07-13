(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.config(function ($stateProvider) {
			$stateProvider
				.state('public.dettagliRisposta',
					{
						url: '/dettagliRisposta/:id',
						templateUrl: 'app/main/main.html',
						controller: 'risposteCtrl'
					})
				.state('private.dettagliRisposta',
					{
						url: '/dettagliRisposta/:id',
						templateUrl: 'app/main/main.html',
						controller: 'risposteCtrl'
					})
				.state('private.risposte',
					{
						url: '/risposte',
						templateUrl: 'app/main/domande.html',
						controller: 'risposteCtrl'
					});
		})
		.controller('risposteCtrl',
			function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {

				sondaggiService.list().then(function (result) {
					$scope.Sondaggi = result.data;
				}).catch(function () {
					$state.go("errore");
				});

				if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
					domandeService.list().then(function (result) {
						$scope.ListaDomande = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				} else {
					domandeService.detail($stateParams.id).then(function (result) {
						$scope.Domanda = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				}

				$scope.Utenti = null;
				utentiService.list().then(function (result) {
					$scope.Utenti = result.data;
				}).catch(function () {
					$state.go("errore");
				});

				$scope.updatedomande = function (sondaggio) {
					domandeService.updateDomande(sondaggio.IdSondaggio).then(function (result) {
						$scope.Domande = result.data;
					});
				}

				$scope.save = function () {
					domandeService.save($scope.Domanda).then(function (data) {
						window.alert("Mail inviata.");
					}).catch(function () {
						$state.go("errore");
					});
				}
			});
})(window, window.angular);

