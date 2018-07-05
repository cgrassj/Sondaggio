angular.module('ui.sinrating', ['ngAnimate', 'ngSanitize', 'ui.bootstrap']);
angular.module('ui.sinrating').controller('RatingController', function ($scope) {
  $scope.rate = 0;
  $scope.max = 5;
  $scope.isReadonly = false;

  $scope.hoveringOver = function(value) {
    $scope.overStar = value;
  };

  $scope.ratingStates = [
    {stateOn: 'glyphicon-star', stateOff: 'glyphicon-star-empty'},
    {stateOff: 'glyphicon-off'}
  ];
});