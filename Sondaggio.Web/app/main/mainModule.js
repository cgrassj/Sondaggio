(function (window, angular) {
	'use-strict';
	angular.module('mainModule', ['ui.router', 'ngAnimate', 'ngSanitize', 'ui.bootstrap'])
		.config([
			'$stateProvider', function ($stateProvider) {
				$stateProvider
					.state('dettagliRisposta',
					{
						url: '/dettagliRisposta/:id',
						templateUrl: 'app/main/main.html',
						controller: 'detailCtrl'
					}).state('errore',
					{
						url: '/pageNotFound',
						templateUrl: 'app/main/error.html',
						controller: 'errorCtrl'
					});
			}
		])
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

    .controller('detailCtrl', function ($scope, $state, $stateParams, risposteService) {


      $scope.save = function () {
        risposteService.save($scope.Risposta).then(function (data) {
          load(data.Id);
        });
      };

      $scope.rate = 0;
      $scope.max = 5;
      $scope.isReadonly = false;

      $scope.hoveringOver = function (value) { $scope.overStar = value; };

	     $scope.ratingStates =
	     [
	           { stateOn: 'glyphicon-star', stateOff: 'glyphicon-star-empty' },
	           { stateOff: 'glyphicon-off' }
	     ];

	     			if ($stateParams.id === '')
					$state.go("error");

			risposteService.detail($stateParams.id).then(function (result)
			{
				if (result.data === '')
					$state.go("error");
				else
					$scope.Risposta = result.data;
			});
	}
	);

})(window, window.angular);
