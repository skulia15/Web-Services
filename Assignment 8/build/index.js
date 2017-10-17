'use strict';

var _express = require('express');

var _express2 = _interopRequireDefault(_express);

var _bodyParser = require('body-parser');

var _bodyParser2 = _interopRequireDefault(_bodyParser);

var _mongoose = require('mongoose');

var _mongoose2 = _interopRequireDefault(_mongoose);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var application = (0, _express2.default)();
var app;

_mongoose2.default.Promise = global.Promise;
_mongoose2.default.connect('mongodb://skulia:skulia@ds119685.mlab.com:19685/assignment8', { useMongoClient: true }).then(function (db) {
    application.use(_bodyParser2.default.json());
    application.use(_bodyParser2.default.urlencoded({ extended: true }));
    require(__dirname + '/api.js')(application, require(__dirname + '/entities.js')(db));
    application.listen(5001);
    console.log('Mongo connected');
    console.log('Server listening on port 5001');
});
module.exports = application;