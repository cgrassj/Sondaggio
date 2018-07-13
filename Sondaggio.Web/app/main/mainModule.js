(function (window, angular) {
	'use-strict';
	angular.module('mainModule')
		.config(function($stateProvider, $urlRouterProvider) {
			$urlRouterProvider.otherwise('/pageNotFound');
			$stateProvider
				.state('private',
					{
						url: '/private',
						templateUrl: 'app/main/private.html',
						controller: function($scope, $state) { $scope.state = $state; }
					})
				.state('public',
					{
						url: '/public',
						templateUrl: 'app/main/public.html',
						controller: function($scope, $state) { $scope.state = $state; }
					})
				.state('errore',
					{
						url: '/errore',
						templateUrl: 'app/main/error.html'
					})
				.state('notFound',
					{
						url: '/pageNotFound',
						templateUrl: 'app/main/error404.html'
					})
				.state('private.statistiche',
					{
						url: '/statistiche',
						templateUrl: 'app/main/statistiche.html',
						controller: 'domandeCtrl'
					});
		});
})(window, window.angular);
