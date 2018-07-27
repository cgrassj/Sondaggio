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
					if ($scope.Sondaggio.IsNew) {
						sondaggiService.create($scope.Sondaggio).then(function(result) {
							load(result.data.IdSondaggio);
							window.swal({
								title: "Salvataggio",
								text: "Nuovo sondaggio salvato",
								icon: "success"
							});
						});
					} else {
						sondaggiService.save($scope.Sondaggio).then(function(result) {
							window.swal({
								title: "Salvataggio",
								text: "Sondaggio salvato",
								icon: "success"
							});
							load(result.data.IdSondaggio);
						});
					}
				};

				$scope.create = function () {
					$scope.Sondaggio = { IsNew: true };
					$scope.ListaSondaggi.push($scope.Sondaggio);
				};

				$scope.delete = function () {
					sondaggiService.delete($scope.Sondaggio).then(function () {
						window.swal({
							title: "Elimina",
							text: "Sondaggio eliminato con successo",
							icon: "info"
						});
						loadList();
						$scope.Sondaggio = null;
						$scope.idSondaggioSelezionato = null;
					});
				};

				$scope.close = function () {
					loadList();
					$scope.Sondaggio = null;
					$scope.idSondaggioSelezionato = null;
				};


				$scope.idSondaggioSelezionato = null;
				$scope.detail = function (value) {
					$scope.Sondaggio = value;

					//$scope.idSondaggioSelezionato = value;
					//sondaggiService.detail(value).then(function (result) {
					//	$scope.Sondaggio = result.data;
					//});
				};

				load = function (value) {
					sondaggiService.detail(value).then(function (result) {
						$scope.Sondaggio = result.data;
						$scope.VisualizzaLista = true;
						$scope.VisualizzaDettaglio = false;
					});
				};

				loadList = function () {
          sondaggiService.list().then(function (result) {
						$scope.ListaSondaggi = result.data;
						$scope.VisualizzaLista = true;
	          $scope.VisualizzaDettaglio = false;
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
