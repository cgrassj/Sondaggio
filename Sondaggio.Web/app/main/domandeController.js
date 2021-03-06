(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.config(function ($stateProvider) {
			$stateProvider
				.state('domande',
					{
						url: '/domande/:id',
						templateUrl: 'app/domande/domande.html',
						controller: 'domandeCtrl'
					})
				;
		})
		.controller('domandeCtrl',
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
						window.swal({
							title: "Invio",
							text: "Mail inviata con successo",
							icon: "success"
						});

					}).catch(function () {
						$state.go("errore");
					});
				}
			});
})(window, window.angular);
