(function (window, angular) {
  'use-strict';
  angular.module('mainModule')
    .config(function ($stateProvider) {
      $stateProvider
        .state('public.inizioRisposta',
          {
            url: '/dettagliRisposta/:id',
            templateUrl: 'app/main/landing.html',
            controller: 'risposteInizioCtrl'
          })
        .state('public.inserisciRisposta',
          {
            url: '/inserisciRisposta/:id',
            templateUrl: 'app/main/main.html',
            controller: 'risposteDettaglioCtrl',
          })
        .state('public.fineRisposta',
          {
            url: '/fineRisposta',
            templateUrl: 'app/main/end.html',
            controller: 'risposteFineCtrl'
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
		function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService, toaster) {

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
	        toaster.pop('info', "Sondaggio", "Mail inviata correttamente");
	        $scope.MailInviata = true;
        });
        }
      })
    .controller('risposteDettaglioCtrl',
      function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {

        $scope.nuovaRisposta = null;
        if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
          $state.go("errore");
        } else {
	        loadReview($stateParams.id);
        }
        $scope.save = function () {
					if ($scope.rispostaDtAgg) {
						window.swal({
							title: "Salvare?",
							text: "Sicuro di voler modificare la recensione?",
							icon: "warning",
							buttons: ["Annulla", "Continua"],
								
								dangerMode: true
							})
							.then((ok) => {
								if (ok) {
									risposteService.save($scope.Risposta).then(function(result) {
										$scope.nuovaRisposta = 1;
										$state.go("public.fineRisposta");
									});
									window.swal("Recensione aggiornata con successo", {
										icon: "success"
									});
								} else {
									loadReview($scope.$id);
									window.swal("Non sono state fatte modifiche alla recensione!");
								}
							});
            //if (confirm("Sicuro di voler modificare la recensione?"))
            //  risposteService.save($scope.Risposta).then(function (result) {
            //    $scope.nuovaRisposta = 1;
            //    $state.go("public.fineRisposta");
            //  });
            //else {
            //  //loadReview($scope.Risposta.IdRisposta);
            //  loadReview($scope.$id);
            //}
          }
          else
            risposteService.save($scope.Risposta).then(function (result) {
              $scope.nuovaRisposta = 1;
              $state.go("public.fineRisposta");
            });
        }

        $scope.annulla = function () {
          loadReview($scope.Risposta.IdRisposta);
          $scope.nuovaRisposta = null;
        }

        function loadReview(idRisposta) {
          risposteService.detail(idRisposta).then(function (result) {
            $scope.Risposta = result.data;
            $scope.rispostaDtAgg = $scope.Risposta.dtAgg;
          }).catch(function () {
            $state.go("errore");
          });
        }

      })
    .controller('risposteInizioCtrl',
      function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {
        if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
          $state.go("errore");
        } else 
          $scope.stateParams = $stateParams;
        $scope.idRisposta = $stateParams.id;
          $scope.state = $state;
        $scope.successivo = function () { $state.go("public.inserisciRisposta", { 'IdRispostaDaCaricare': $stateParams.id }); }
      })
    .controller('risposteFineCtrl',
      function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {
        $scope.successivo = function () { $state.go("public.inserisciRisposta"); }
      });
})(window, window.angular);

