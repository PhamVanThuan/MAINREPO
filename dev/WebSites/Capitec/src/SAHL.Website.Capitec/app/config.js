'use strict';

angular.module('capitecApp.config', [])
.factory('$config', function () {
    return {
        CapitecAppVersion: '0.0.0.0', // This is a place holder value that gets transformed into the actual build version number during the build.
        AlphaHousingIncomeLimit: 18000,
        AlphaHousingCategory: 7,
        NonAlphaHousingCategory: 4,
        PtiRatio: 0.3,
        PrintConsentForm: true
    };
});