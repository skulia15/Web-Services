'use strict';

var _mongoose = require('mongoose');

var _mongoose2 = _interopRequireDefault(_mongoose);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

module.exports = function (db) {
    var Schema = _mongoose2.default.Schema;


    var app = {};

    app.user = db.model('User', Schema({
        name: String,
        token: String,
        gender: String
    }));

    app.company = db.model('Company', Schema({
        name: String,
        punchCount: Number // default 10
    }));

    app.punch = db.model('Punch', Schema({
        company_id: { type: _mongoose2.default.Schema.Types.ObjectId, ref: 'Company' },
        user_id: { type: _mongoose2.default.Schema.Types.ObjectId, ref: 'User' },
        created: Date,
        used: Boolean // Initial value false
    }));

    return app;
};