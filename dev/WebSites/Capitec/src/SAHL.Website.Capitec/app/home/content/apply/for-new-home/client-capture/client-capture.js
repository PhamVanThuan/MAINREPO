angular.module('capitecApp.home.content.apply.for-new-home.client-capture',
    ['capitecApp.home.apply.client-capture'])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-new-home.new-home.client-capture', {
        url: '/client-capture',
        templateUrl: 'client-capture.tpl.html',
        data: {
            breadcrumb: 'Capture',
            pageHeading: 'Please answer all questions',
            next: 'home.content.apply.application-precheck-for-new-home.new-home.application-submit'
        },
        controller: 'clientCaptureCtrl'
    }).state('home.content.apply.application-precheck-for-new-home.new-home.client-capture.personal', {
        url: '/personal',
        templateUrl: 'client-capture.tpl.html',
        data: {
            breadcrumb: 'Personal',
            pageHeading: 'Please complete all personal details',
            next: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture.address',
            step: 0
        },
        controller: 'clientCaptureCtrl'
    }).state('home.content.apply.application-precheck-for-new-home.new-home.client-capture.address', {
        url: '/address',
        templateUrl: 'client-capture.tpl.html',
        data: {
            breadcrumb: 'Address',
            pageHeading: 'Please complete all address details',
            next: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture.employment',
            step: 1,
            previous: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture.personal'
        },
        controller: 'clientCaptureCtrl'
    }).state('home.content.apply.application-precheck-for-new-home.new-home.client-capture.employment', {
        url: '/employment',
        templateUrl: 'client-capture.tpl.html',
        data: {
            breadcrumb: 'Employment',
            pageHeading: 'Please complete all employment details',
            next: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture.declarations',
            step: 2,
            previous: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture.address'
        },
        controller: 'clientCaptureCtrl'
    }).state('home.content.apply.application-precheck-for-new-home.new-home.client-capture.declarations', {
        url: '/declarations',
        templateUrl: 'client-capture.tpl.html',
        data: {
            breadcrumb: 'Declarations',
            pageHeading: 'Please complete all declarations',
            next: 'home.content.apply.application-precheck-for-new-home.new-home.application-submit',
            step: 3,
            previous: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture.employment'
        },
        controller: 'clientCaptureCtrl'
    });
}])