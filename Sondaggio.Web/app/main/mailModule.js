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
		})
	  .factory('utentiService',
		  function ($http) {
			  return {
				  list: function () {
					  return $http.get("/api/utenti");
				  },
				  detail: function (id) {
					  return $http.get("/api/utenti/" + id);
				  },
					create: function (utenti) {
					  var req = {
						  method: 'POST',
							url: '/api/utenti',
						  headers: {
							  'Content-Type': 'application/json'
						  },
							data: utenti
					  };

					  return $http(req);
          },
         
					save: function (utente) {
					  var req = {
						  method: 'PATCH',
							url: '/api/utenti/' + utente.IdUtente,
						  headers: {
							  'Content-Type': 'application/json'
						  },
							data: utente
					  };
					  return $http(req);
				  }
			  }
		  })

    .controller('domandeCtrl',
      function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {
         

        sondaggiService.list().then(function (result) {
          $scope.Sondaggi = result.data;
        }).catch(function () {
          $state.go("errore");
        });

	      if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
		      domandeService.list().then(function(result) {
			      $scope.ListaDomande = result.data;
		      }).catch(function() {
			      $state.go("errore");
		      });
	      } else {
		      risposteService.detail($stateParams.id).then(function(result) {
			      $scope.Domanda = result.data;
		      }).catch(function() {
			      $state.go("errore");
		      });
	      }
	      //da implementare lista utenti
        $scope.Utenti = null;
	      utentiService.list().then(function (result) {
					$scope.Utenti = result.data;
	      }).catch(function () {
		      $state.go("errore");
					});

        $scope.updatedomande = function (sondaggio) {
          domandeService.updateDomande(sondaggio.IdSondaggio).then(function (result) {
            $scope.Domande = result.data;
            alert("ciao");
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
