(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.config(function ($stateProvider) {
			$stateProvider
				.state('public.dettagliRisposta',
					{
						url: '/dettagliRisposta/:id',
						templateUrl: 'app/main/main.html',
            controller: 'risposteDettaglioCtrl'
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

        //popola lista sondaggi
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

        //popola lista Utenti
				$scope.Utenti = null;
				utentiService.list().then(function (result) {
					$scope.Utenti = result.data;
				}).catch(function () {
					$state.go("errore");
				});

        //popola lista domande del sondaggio selezionato
				$scope.updatedomande = function (sondaggio) {
					domandeService.updateDomande(sondaggio.IdSondaggio).then(function (result) {
						$scope.Domande = result.data;
					});
				}

        $scope.save = function () {
          risposteService.create($scope.selectedDomanda.IdDomanda, $scope.selectedUtente.IdUtente).then(function (result) {
           
            

              window.alert("Mail inviata.");

          });
        }
     })
    .controller('risposteDettaglioCtrl',
      function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {

        $scope.nuovaRisposta = null;
        if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
          $state.go("errore");
        } else {
          loadReview($stateParams.id)
        }

        $scope.save = function () {
          if ($scope.rispostaDtAgg) {
            if (confirm("Sicuro di voler modificare la recensione?"))
              risposteService.save($scope.Risposta).then(function (result) {
                //window.alert("Recensione modificata.");
                $scope.nuovaRisposta = 1;
              });
            else {
              loadReview($scope.Risposta.IdRisposta);
               
            }
          }
          else
            risposteService.save($scope.Risposta).then(function (result) {
              //window.alert("Recensione pubblicata.");
              $scope.nuovaRisposta = 1;
            });
        }

        $scope.annulla = function () { loadReview($scope.Risposta.IdRisposta); $scope.nuovaRisposta = null; }

        function loadReview(idRisposta) {
          risposteService.detail(idRisposta).then(function (result) {
            $scope.Risposta = result.data;
            $scope.rispostaDtAgg = $scope.Risposta.dtAgg;
          }).catch(function () {
            $state.go("errore");
          });
        }

      });
})(window, window.angular);

