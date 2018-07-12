(function (window, angular) {
	'use-strict';
  angular.module('mainModule', ['ui.router', 'mailModule', 'ngAnimate', 'ngSanitize', 'ui.bootstrap'])
		.config(function($stateProvider, $urlRouterProvider) {
			$urlRouterProvider.otherwise('/pageNotFound');
			$stateProvider
				.state('public.dettagliRisposta',
					{
						url: '/dettagliRisposta/:id',
						templateUrl: 'app/main/main.html',
						controller: 'detailCtrl'
				})
				.state('private.dettagliRisposta',
					{
						url: '/dettagliRisposta/:id',
						templateUrl: 'app/main/main.html',
						controller: 'detailCtrl'
					})
				.state('private',
					{
						url: '/private',
						templateUrl: 'app/main/private.html',
						controller: function ($scope, $state) { $scope.state = $state; }
				})
				.state('public',
					{
						url: '/public',
						templateUrl: 'app/main/public.html',
						controller: function ($scope, $state) { $scope.state = $state; }
					})
				.state('errore',
					{
						url: '/errore',
						templateUrl: 'app/main/error.html'
					}).state('notFound',
					{
						url: '/pageNotFound',
						templateUrl: 'app/main/error404.html'
				}).state('private.sondaggio',
					{
						url: '/sondaggio/:id?',
						templateUrl: 'app/main/sondaggio.html',
						controller: 'sondaggiCtrl'
				}).state('private.risposte',
					{
						url: '/risposte',
						templateUrl: 'app/main/mail.html',
            controller: 'domandeCtrl'
				}).state('private.statistiche',
				  {
						url: '/statistiche',
						templateUrl: 'app/main/statistiche.html',
					  controller: 'domandeCtrl'
				  });;
		})
		.factory('risposteService',
			function($http) {
				return {
					list: function() {
						return $http.get("/api/risposte");
					},
					detail: function(id) {
						return $http.get("/api/risposte/" + id);
					},
					create: function(risposta) {
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
					save: function(risposta) {
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
		.factory('sondaggiService',
			function($http) {
				return {
					list: function() {
						return $http.get("/api/sondaggi");
					},
					detail: function(id) {
						return $http.get("/api/sondaggi/" + id);
					},
					delete: function(cmp) {
						var req = {
							method: 'DELETE',
							url: '/api/sondaggi/' + cmp.IdSondaggio,
							headers: {
								'Content-Type': 'application/json'
							}
						};
						return $http(req);
					},
					create: function(cmp) {
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
					save: function(cmp) {
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
	  .controller('detailCtrl',
			function($scope, $state, $stateParams, risposteService) {

				$scope.save = function() {
					risposteService.save($scope.Risposta).then(function(data) {

						window.alert("Grazie per aver risposto.");

						risposteService.detail($scope.Risposta.IdRisposta).then(function(result) {
							$scope.Risposta = result.data;
							original = angular.copy(result.data);
							$scope.rispostaDtAgg = $scope.Risposta.dtAgg;

						}).catch(function() {
							$state.go("errore");
						});
					});
				};

				$scope.annulla = function() {
					$scope.Risposta = angular.copy(original);
				}

				$scope.rate = 0;
				$scope.max = 5;
				$scope.isReadonly = false;
				$scope.rispostaDtAgg = null;

				$scope.hoveringOver = function(value) { $scope.overStar = value; };

				$scope.ratingStates =
				[
					{ stateOn: 'glyphicon-star', stateOff: 'glyphicon-star-empty' },
					{ stateOff: 'glyphicon-off' }
				];

        if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {  
          risposteService.list().then(function (result) {
            $scope.ListaRisposte = result.data;
          }).catch(function () {
            $state.go("errore");
          });
        }
        else {

          risposteService.detail($stateParams.id).then(function (result) {
            $scope.Risposta = result.data;
            original = angular.copy(result.data);
            $scope.rispostaDtAgg = $scope.Risposta.dtAgg;
          }).catch(function () {
            $state.go("errore");
          });
        }
				
			}
		)
		.controller('sondaggiCtrl',
			function($scope, $state, $stateParams, sondaggiService) {

				$scope.save = function() {
          sondaggiService.save($scope.Sondaggio).then(function (result) {
            window.alert("Sondaggio salvato.");
						load(result.data.IdSondaggio);
					});
				};

				$scope.create = function() {
					sondaggiService.create({}).then(function(result) {
						load(result.data.IdSondaggio);
					});
				};

				$scope.delete = function() {
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

        if (typeof $stateParams.id === "undefined" || $stateParams.id ==="") {
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
