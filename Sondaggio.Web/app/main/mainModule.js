(function (window, angular) {
	'use-strict';
	angular.module('mainModule', ['ui.router', 'ngAnimate', 'ngSanitize', 'ui.bootstrap'])
		.config(function ($stateProvider, $urlRouterProvider) {
			$urlRouterProvider.otherwise('/pageNotFound');
			$stateProvider
				.state('dettagliRisposta',
					{
						url: '/dettagliRisposta/:id',
						templateUrl: 'app/main/main.html',
						controller: 'detailCtrl'
					}).state('errore',
						{
							url: '/errore',
							templateUrl: 'app/main/error.html'
						}).state('notFound',
							{
								url: '/pageNotFound',
								templateUrl: 'app/main/error404.html'
							}).state('sondaggio',
								{
									url: '/sondaggio/:id',
									templateUrl: 'app/main/sondaggio.html',
									controller: 'sondaggiCtrl'
								})
				;
		})
		.factory('risposteService', function ($http) {
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
		})
		.factory('sondaggiService', function ($http) {
			return {
				list: function () {
					return $http.get("/api/sondaggi");
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
		})

		.controller('detailCtrl', function ($scope, $state, $stateParams, risposteService) {

			$scope.save = function () {
				risposteService.save($scope.Risposta).then(function (data) {

					window.alert("Grazie per aver risposto.");

					risposteService.detail($scope.Risposta.IdRisposta).then(function (result) {
						$scope.Risposta = result.data;
						original = angular.copy(result.data);
						$scope.rispostaDtAgg = $scope.Risposta.dtAgg;

					}).catch(function () {
						$state.go("errore");
					});
				});
			};

			$scope.annulla = function () {
				$scope.Risposta = angular.copy(original);
			}

			$scope.rate = 0;
			$scope.max = 5;
			$scope.isReadonly = false;
			$scope.rispostaDtAgg = null;

			$scope.hoveringOver = function (value) { $scope.overStar = value; };

			$scope.ratingStates =
				[
					{ stateOn: 'glyphicon-star', stateOff: 'glyphicon-star-empty' },
					{ stateOff: 'glyphicon-off' }
				];

			if ($stateParams.id === '')
				$state.go("error");

			risposteService.detail($stateParams.id).then(function (result) {
				$scope.Risposta = result.data;
				original = angular.copy(result.data);
				$scope.rispostaDtAgg = $scope.Risposta.dtAgg;
			}).catch(function () {
				$state.go("errore");
			});
		}
		)

		.controller('sondaggiCtrl', function ($scope, $state, $stateParams, sondaggiService) {

			$scope.save = function () {
				sondaggiService.save($scope.Sondaggio).then(function (result) {
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

			$scope.isReadonly = false;

			sondaggiService.detail($stateParams.id).then(function (result) {
				$scope.Sondaggio = result.data;
			}).catch(function () {
				$state.go("errore");
			});

			sondaggiService.list().then(function (result) {
				$scope.ListaSondaggi = result.data;
			}).catch(function () {
				$state.go("errore");
			});
		});
})(window, window.angular);
