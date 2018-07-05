$(function () {
    app.initialize();

    // Attiva Knockout
    ko.validation.init({ grouping: { observable: false } });
    ko.applyBindings(app);
});
