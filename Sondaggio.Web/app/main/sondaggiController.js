(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.config(function ($stateProvider) {
			$stateProvider
					.state('private.sondaggi',
						{
							url: '/sondaggi/:id?',
							templateUrl: 'app/main/sondaggi.html',
							controller: 'sondaggiCtrl'
						})
				;
		})
		.controller('sondaggiCtrl',
			function ($scope, $state, $stateParams, sondaggiService) {

				$scope.aggiungiCognomeNome = function() {
					$scope.Sondaggio.TestoEmail += '{{CognomeNome}}';
				}
				$scope.aggiungiSottoTitoloSondaggio = function () {
					$scope.Sondaggio.TestoEmail += '{{SottoTitoloSondaggio}}';
				}
				$scope.aggiungiDescrizioneSondaggio = function () {
					$scope.Sondaggio.TestoEmail += '{{DescrizioneSondaggio}}';
				}
				$scope.aggiungiUrl = function () {
					$scope.Sondaggio.TestoEmail += '<a href="{{URL}}">link</a>';
				}

				$scope.save = function () {
					sondaggiService.save($scope.Sondaggio).then(function (result) {
						window.alert("Sondaggio salvato.");
						load(result.data.IdSondaggio);
					});
				};

				$scope.create = function () {
					sondaggiService.create({}).then(function (result) {
						load(result.data.IdSondaggio);
					});
				};

				$scope.delete = function () {
					sondaggiService.delete($scope.Sondaggio).then(function () {

					});
				};

				$scope.close = function () {
					loadList();
					$scope.Sondaggio = null;
					$scope.idSondaggioSelezionato = null;
				};


				$scope.idSondaggioSelezionato = null;
				$scope.detail = function (value) {
					$scope.idSondaggioSelezionato = value;
					sondaggiService.detail(value).then(function (result) {
						$scope.Sondaggio = result.data;
					});
				};

				load = function (value) {
					sondaggiService.detail(value).then(function (result) {
						$scope.Sondaggio = result.data;
					});
				};

				loadList = function () {
          sondaggiService.list().then(function (result) {
						$scope.ListaSondaggi = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				};

				$scope.isReadonly = false;

				if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
					sondaggiService.list().then(function (result) {
						$scope.ListaSondaggi = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				}
				else {
					sondaggiService.detail($stateParams.id).then(function (result) {
						$scope.Sondaggio = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				}
			});
})(window, window.angular);
