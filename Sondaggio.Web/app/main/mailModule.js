(function (window, angular) {
	'use-strict';
  angular.module('mailModule', ['ui.router', 'mainModule', 'ui.bootstrap'])
    .config(function ($stateProvider, $urlRouterProvider) {
      $urlRouterProvider.otherwise('/pageNotFound');
      $stateProvider
        .state('mail',
          {
            url: '/mail/:id',
            templateUrl: 'app/main/mail.html',
            controller: 'domandeCtrl'
          })
        ;
    })
    .factory('domandeService',
      function ($http) {
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
      })

    .controller('domandeCtrl',
      function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService) {
         

        sondaggiService.list().then(function (result) {
          $scope.Sondaggi = result.data;
        }).catch(function () {
          $state.go("errore");
        });

       domandeService.list().then(function (result) {
          $scope.Domande = result.data;
        }).catch(function () {
          $state.go("errore");
          });

	      risposteService.detail($stateParams.id).then(function (result) {
					$scope.Domanda = result.data;
	      }).catch(function () {
		      $state.go("errore");
	      });
        //da implementare lista utenti
        $scope.Utenti = null;

        $scope.updatedomande = function () {
          //$scope.Domande = Sondaggio.Domande;
          alert("ciao");
        };

        $scope.save = function () {
          domandeService.save($scope.Domanda).then(function (data) {
            window.alert("Mail inviata.");
          }).catch(function () {
            $state.go("errore");
          });
        }
      });

})(window, window.angular);
