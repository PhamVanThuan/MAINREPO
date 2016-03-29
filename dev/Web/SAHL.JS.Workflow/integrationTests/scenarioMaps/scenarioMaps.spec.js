describe('x2 Service', function() {
    describe('Scenario Maps', function() {

        describe('these tests', function(){
          it('should be ignored', function(){
            expect(true).toBeTruthy();
          });
        });


        xdescribe('create instance', function() {

            it('should create an instance', function(done) {
                browser.get('index.html');

                var describe = this.suite.description.replace(new RegExp(' ', 'g'), '_');
                var it = this.description.replace(new RegExp(' ', 'g'), '_');
                var id = describe + '_' + it;

                var idBecause = id + '_because';


                expect(element(by.id(idBecause)).isPresent()).toBe(true);

                element(by.id(idBecause)).click();

                var idError = id + '_error';
                expect(element(by.id(idError)).isPresent()).toBe(true);

                var errorElement = element(by.id(idError));

                errorElement.getInnerHtml().then(function(text) {
                    expect(text.trim()).toEqual('');
                });

                var idSuccess = id + '_success';
                expect(element(by.id(idSuccess)).isPresent()).toBe(true);

                var successElement = element(by.id(idSuccess));
                successElement.getInnerHtml().then(function(text) {
                    expect(JSON.parse(text).data.ReturnData.InstanceId).not.toEqual('0');
                    expect(JSON.parse(text).data.ReturnData.IsErrorResponse).toBe(false);
                });
                done();
            });
        });

        xdescribe('complete create instance activity', function() {

            it('should complete a create instance activity and move the case to Application Created', function() {

                var describe = this.suite.description.replace(new RegExp(' ', 'g'), '_');
                var it = this.description.replace(new RegExp(' ', 'g'), '_');
                var id = describe + '_' + it;

                var idBecause = id + '_because';

                expect(element(by.id(idBecause)).isPresent()).toBe(true);

                element(by.id(idBecause)).click();


                var idError = id + '_error';
                expect(element(by.id(idError)).isPresent()).toBe(true);

                var errorElement = element(by.id(idError));

                errorElement.getInnerHtml().then(function(text) {
                    expect(text.trim()).toEqual('');
                });

                var idSuccess = id + '_success';
                expect(element(by.id(idSuccess)).isPresent()).toBe(true);

                var successElement = element(by.id(idSuccess));
                successElement.getInnerHtml().then(function(text) {
                    expect(JSON.parse(text).data.ReturnData.InstanceId).not.toEqual('0');
                    expect(JSON.parse(text).data.ReturnData.IsErrorResponse).toBe(false);
                });
            });
        });

        xdescribe('start activity', function() {

            it('should start an activity', function() {

                var describe = this.suite.description.replace(new RegExp(' ', 'g'), '_');
                var it = this.description.replace(new RegExp(' ', 'g'), '_');
                var id = describe + '_' + it;

                var idBecause = id + '_because';

                expect(element(by.id(idBecause)).isPresent()).toBe(true);

                element(by.id(idBecause)).click();

                var idError = id + '_error';
                expect(element(by.id(idError)).isPresent()).toBe(true);

                var errorElement = element(by.id(idError));

                errorElement.getInnerHtml().then(function(text) {
                    expect(text.trim()).toEqual('');
                });

                var idSuccess = id + '_success';
                expect(element(by.id(idSuccess)).isPresent()).toBe(true);

                var successElement = element(by.id(idSuccess));
                successElement.getInnerHtml().then(function(text) {
                    expect(JSON.parse(text).data.ReturnData.InstanceId).not.toEqual('0');
                    expect(JSON.parse(text).data.ReturnData.IsErrorResponse).toBe(false);
                });
            });
        });

        xdescribe('cancel activity', function() {

            it('should cancel an activity', function() {

                var describe = this.suite.description.replace(new RegExp(' ', 'g'), '_');
                var it = this.description.replace(new RegExp(' ', 'g'), '_');
                var id = describe + '_' + it;

                var idBecause = id + '_because';

                expect(element(by.id(idBecause)).isPresent()).toBe(true);

                element(by.id(idBecause)).click();

                var idError = id + '_error';
                expect(element(by.id(idError)).isPresent()).toBe(true);

                var errorElement = element(by.id(idError));

                errorElement.getInnerHtml().then(function(text) {
                    expect(text.trim()).toEqual('');
                });

                var idSuccess = id + '_success';
                expect(element(by.id(idSuccess)).isPresent()).toBe(true);

                var successElement = element(by.id(idSuccess));
                successElement.getInnerHtml().then(function(text) {
                    expect(JSON.parse(text).data.ReturnData.InstanceId).not.toEqual('0');
                    expect(JSON.parse(text).data.ReturnData.IsErrorResponse).toBe(false);
                });
            });
        });

        xdescribe('start activity move application', function() {

            it('should start an activity and move to Application Moved', function() {

                var describe = this.suite.description.replace(new RegExp(' ', 'g'), '_');
                var it = this.description.replace(new RegExp(' ', 'g'), '_');
                var id = describe + '_' + it;

                var idBecause = id + '_because';

                expect(element(by.id(idBecause)).isPresent()).toBe(true);

                element(by.id(idBecause)).click();

                var idError = id + '_error';
                expect(element(by.id(idError)).isPresent()).toBe(true);

                var errorElement = element(by.id(idError));

                errorElement.getInnerHtml().then(function(text) {
                    expect(text.trim()).toEqual('');
                });

                var idSuccess = id + '_success';
                expect(element(by.id(idSuccess)).isPresent()).toBe(true);

                var successElement = element(by.id(idSuccess));
                successElement.getInnerHtml().then(function(text) {
                    expect(JSON.parse(text).data.ReturnData.InstanceId).not.toEqual('0');
                    expect(JSON.parse(text).data.ReturnData.IsErrorResponse).toBe(false);
                });
            });
        });

        xdescribe('complete activity', function() {

            it('should complete an activity', function() {

                var describe = this.suite.description.replace(new RegExp(' ', 'g'), '_');
                var it = this.description.replace(new RegExp(' ', 'g'), '_');
                var id = describe + '_' + it;

                var idBecause = id + '_because';

                expect(element(by.id(idBecause)).isPresent()).toBe(true);

                element(by.id(idBecause)).click();

                var idError = id + '_error';
                expect(element(by.id(idError)).isPresent()).toBe(true);

                var errorElement = element(by.id(idError));

                errorElement.getInnerHtml().then(function(text) {
                    expect(text.trim()).toEqual('');
                });

                var idSuccess = id + '_success';
                expect(element(by.id(idSuccess)).isPresent()).toBe(true);

                var successElement = element(by.id(idSuccess));
                successElement.getInnerHtml().then(function(text) {
                    expect(JSON.parse(text).data.ReturnData.InstanceId).not.toEqual('0');
                    expect(JSON.parse(text).data.ReturnData.IsErrorResponse).toBe(false);
                });
            });
        });
    });
});
