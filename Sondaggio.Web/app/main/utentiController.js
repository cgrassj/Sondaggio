(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.config(function ($stateProvider) {
			$stateProvider
				.state('private.utenti',
					{
						url: '/utenti',
						templateUrl: 'app/main/utenti.html',
						controller: 'utentiCtrl'
					});
		})
		.controller('utentiCtrl',
			function ($scope, $state, $stateParams, domandeService, risposteService, sondaggiService, utentiService) {

				if (typeof $stateParams.id === "undefined" || $stateParams.id === "") {
					utentiService.list().then(function (result) {
						$scope.Utenti = result.data.value;
					}).catch(function () {
						$state.go("errore");
					});
				} else {
					utentiService.detail($stateParams.id).then(function (result) {
						$scope.Utente = result.data;
					}).catch(function () {
						$state.go("errore");
					});
				}
				$scope.save = function (result) {
					utentiService.save(result).then(function (result) {
						//Controllo da effettuare su nuovo, su salvataggio viene cambiata la chiave e deve essere messo in sola lettura
						var cf = result.data.IdUtente.toUpperCase();
						var cfReg = /^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$/;
						if (!cfReg.test(cf))
							alert("Codice Fiscale errato");
						var set1 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
						var set2 = "ABCDEFGHIJABCDEFGHIJKLMNOPQRSTUVWXYZ";
						var setpari = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
						var setdisp = "BAKPLCQDREVOSFTGUHMINJWZYX";
						var s = 0;
						for (i = 1; i <= 13; i += 2)
							s += setpari.indexOf(set2.charAt(set1.indexOf(cf.charAt(i))));
						for (i = 0; i <= 14; i += 2)
							s += setdisp.indexOf(set2.charAt(set1.indexOf(cf.charAt(i))));
						if (s % 26 != cf.charCodeAt(15) - 'A'.charCodeAt(0))
							alert("Codice Fiscale errato");

					}).catch(function () {
						$state.go("errore");
					});
				}
			});
})(window, window.angular);
