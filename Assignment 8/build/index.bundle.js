'use strict';

var _express = require('express');

var _express2 = _interopRequireDefault(_express);

var _bodyParser = require('body-parser');

var _bodyParser2 = _interopRequireDefault(_bodyParser);

var _mongoose = require('mongoose');

var _mongoose2 = _interopRequireDefault(_mongoose);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var uuidv4 = require('uuid/v4'); /*
                                     In order for this to successfully run, there are 3 steps that need to be taken
                                       1. npm install
                                       2. npm run babel
                                       3. npm start
                                 
                                     You can also do this in 2 steps, if you are in a big big hurry
                                       1. npm install
                                       2. npm run execute
                                 
                                     After that the server runs on http://localhost:5001
                                 */

var ObjectId = require('mongodb').ObjectID;
//var api = require('./api');

// entities.js
var Schema = _mongoose2.default.Schema;

var userSchema = Schema({
    name: String,
    token: String,
    gender: String //Bara einn character samt!!!!
});
var companySchema = Schema({
    name: String,
    punchCount: Number // default 10
});
var punchSchema = Schema({
    //company_id: ObjectId, //fatta ekki
    //user_id: ObjectId, // fatta ekki
    created: Date,
    used: Boolean //Initial value false
});

_mongoose2.default.Promise = global.Promise;
_mongoose2.default.connect('mongodb://skulia15:Jokull128@ds119685.mlab.com:19685/assignment8', { useMongoClient: true }).then(function (db) {
    console.log('Mongo connected');
    var User = db.model('User', userSchema);
    var Company = db.model('Company', companySchema);
    var Punch = db.model('Punch', punchSchema);

    //const routes = require('./Routes/api.js');
    var app = (0, _express2.default)();
    app.use(_bodyParser2.default.json());
    //app.use('/api', routes);

    // Initialize listen for app to listen on a specific port, either provided or hardcoded
    app.listen(process.env.PORT || 5001);

    // api.js

    // Defining data structures for companies and users in punchcard.com
    var companies = [];
    var users = [];
    var punches = new Map();

    // Returns all companies, registered in the punchcard.com service
    app.get('/api/companies', function (req, res) {
        // No Authentication
        //Done!
        Company.find({}).exec(function (err, data) {
            var filteredData = data.map(function (company) {
                return {
                    name: company.name,
                    punchCount: company.punchCount,
                    id: company._id
                };
            });
            res.json({ companies: filteredData });
        });
    });

    // Registers a new company to the punchcard.com service
    app.post('/api/companies', function (req, res) {
        // should be authenticated using the Authorization header, using the hardcoded admin token.
        // AUTHENTICATE HERE
        var name = req.body.name;
        var punchCount = req.body.punchCount;
        if (!punchCount) {
            punchCount = 10;
        }
        if (!req.body.hasOwnProperty('name')) {
            res.statusCode = 400;
            return res.send('Post syntax error');
        } else {
            new Company({ name: name, punchCount: punchCount }).save(function (err, company) {
                if (err) {
                    res.status(500).json({ error: "Failed to save to database" });
                }
                console.log(company);
                var name = company.name,
                    punchCount = company.punchCount,
                    _id = company._id;

                res.statusCode = 201;
                res.json({ company_id: _id });
            });
        }
    });

    // Gets a specific company, given a valid id
    app.get('/api/companies/:id', function (req, res) {
        var findCompany = Company.find({ _id: req.params.id }).exec(function (err, companies) {
            console.log(companies);
            if (findCompany) {
                var filteredData = companies.map(function (company) {
                    return {
                        name: company.name,
                        punchCount: company.punchCount,
                        id: company._id
                    };
                });
                return res.json(filteredData);
            } else {
                res.statusCode = 404;
                return res.send('Company with given id was not found');
            }
        });
    });

    // Gets all users in the system
    app.get('/api/users', function (req, res) {
        User.find({}).exec(function (err, data) {
            var filteredData = data.map(function (user) {
                return {
                    name: user.name,
                    gender: user.gender,
                    id: user._id
                };
            });
            res.json({ user: filteredData });
        });
    });

    // Creates a new user in the system
    app.post('/api/users', function (req, res) {
        // Requires Admin Token
        //AUTHORIZE HERE!
        var _req$body = req.body,
            name = _req$body.name,
            gender = _req$body.gender;

        var token = uuidv4();
        console.log(token);
        if (!req.body.hasOwnProperty('name') || !req.body.hasOwnProperty('gender')) {
            res.statusCode = 400;
            return res.send('User was not properly formatted');
        } else if (gender !== "m" || gender !== "f" || gender !== "o") {
            res.statusCode = 400;
            return res.send('Gender should be m, f or o');
        } else {
            new User({ name: name, token: token, gender: gender }).save(function (err, user) {
                if (err) {
                    res.status(500).json({ error: "Failed to save to database" });
                }
                console.log(user);
                var name = user.name,
                    gender = user.gender,
                    _id = user._id;

                res.json({ token: token });
            });
        }
    });

    // Returns a list of all punches, registered for the given user
    app.get('/api/users/:id/punches', function (req, res) {
        if (!isValidUser(req.params.id)) {
            res.statusCode = 404;
            return res.send('User with given id was not found in the system.');
        }
        // There was a ?company query provided
        if (req.query.company) {
            var filteredPunches = punches.get(req.params.id);
            if (filteredPunches) {
                // The user already has some punches in his list
                var returnList = [];
                filteredPunches.forEach(function (value, idx) {
                    if (value.companyId == req.query.company) {
                        returnList.push(value);
                    }
                });
                return res.json(returnList);
            } else {
                return res.json([]);
            }
        } else {
            var retrievedPunches = punches.get(req.params.id) === undefined ? [] : punches.get(req.params.id);
            return res.json(retrievedPunches);
        }
    });

    // Creates a punch, associated with a user
    app.post('/api/users/:id/punches', function (req, res) {
        if (!req.body.hasOwnProperty('companyId')) {
            res.statusCode = 400;
            return res.send('Company Id is missing');
        } else if (!isValidUser(req.params.id)) {
            res.statusCode = 404;
            return res.send('The user was not found in the system.');
        } else if (!isValidCompany(req.body.companyId)) {
            res.statusCode = 404;
            return res.send('The company with the given id was not found in the system.');
        }

        // We have valid data
        var oldPunches = punches.get(req.params.id) === undefined ? [] : punches.get(req.params.id);
        oldPunches.push({
            companyId: req.body.companyId,
            companyName: getCompanyNameById(req.body.companyId),
            created: new Date().toLocaleString()
        });
        punches.set(req.params.id, oldPunches);

        res.json(true);
    });

    // Helper functions

    function isValidUser(userId) {
        for (var i = 0; i < users.length; i++) {
            if (users[i].id == userId) {
                return true;
            }
        }
        return false;
    }

    function isValidCompany(companyId) {
        for (var i = 0; i < companies.length; i++) {
            if (companies[i].id == companyId) {
                return true;
            }
        }
        return false;
    }

    function getCompanyNameById(companyId) {
        for (var i = 0; i < companies.length; i++) {
            if (companies[i].id == companyId) {
                return companies[i].name;
            }
        }
        return "";
    }
});
