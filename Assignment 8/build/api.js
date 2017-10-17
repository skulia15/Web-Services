'use strict';

var _moment = require('moment');

var _moment2 = _interopRequireDefault(_moment);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var uuidv4 = require('uuid/v4');

var adminToken = '598d24f1-3d87-4a0d-980a-d00d461be53b';

module.exports = function (application, app) {
    // Returns all companies, registered in the punchcard.com service
    application.get('/api/companies', function (req, res) {
        app.company.find({}).exec(function (err, companies) {
            if (err) {
                res.status(500).json({ error: "Failed to load from database" });
            }
            var filteredData = companies.map(function (company) {
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
    application.post('/api/companies', function (req, res) {
        if (!req.header('Authorization')) {
            res.statusCode = 401;
            return res.send('Token missing in header fields');
        }
        if (req.header('Authorization') !== adminToken) {
            res.statusCode = 403;
            return res.send('Forbidden');
        }
        var name = req.body.name;
        //handle error
        var punchCount = req.body.punchCount;
        if (!punchCount) {
            punchCount = 10;
        }
        if (!req.body.hasOwnProperty('name')) {
            res.statusCode = 412;
            return res.send('Data not structured correctly');
        } else {
            new app.company({ name: name, punchCount: punchCount }).save(function (err, company) {
                if (err) {
                    res.status(500).json({ error: "Failed to save to database" });
                }
                var name = company.name,
                    punchCount = company.punchCount,
                    _id = company._id;

                res.statusCode = 201;
                res.json({ company_id: _id });
            });
        }
    });

    // Gets a specific company, given a valid id
    application.get('/api/companies/:id', function (req, res) {
        var findCompany = app.company.find({ _id: req.params.id }).exec(function (err, companies) {
            if (err) {
                res.status(500).json({ error: "Failed to load from database" });
            }
            if (findCompany.length) {
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
    application.get('/api/users', function (req, res) {
        app.user.find({}).exec(function (err, data) {
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
    application.post('/api/users', function (req, res) {
        if (!req.header('Authorization')) {
            res.statusCode = 401;
            return res.send('Token missing in header fields');
        }
        if (req.header('Authorization') !== adminToken) {
            res.statusCode = 403;
            return res.send('Forbidden');
        }
        var _req$body = req.body,
            name = _req$body.name,
            gender = _req$body.gender;

        var token = uuidv4();
        if (!req.body.hasOwnProperty('name') || !req.body.hasOwnProperty('gender')) {
            res.statusCode = 412;
            return res.send('User was not properly formatted');
        }
        if (gender !== 'm' && gender !== 'f' && gender !== 'o') {
            res.statusCode = 412;
            return res.send('Gender should be m, f or o');
        } else {
            new app.user({ name: name, token: token, gender: gender }).save(function (err, user) {
                if (err) {
                    res.status(500).json({ error: "Failed to save to database" });
                }
                var name = user.name,
                    gender = user.gender,
                    _id = user._id;

                res.json({ userId: _id, token: token });
            });
        }
    });

    // Creates a punch, associated with a user
    application.post('/api/my/punches', function (req, res) {
        // Check if token is provided
        if (!req.header('Authorization')) {
            res.statusCode = 401;
            return res.send('Token missing in header fields');
        }
        // Check if company id is provided
        if (!req.body.hasOwnProperty('companyId')) {
            //case sensitive
            res.statusCode = 412;
            return res.send('Company Id is missing');
        } else {
            app.user.findOne({ token: req.header('Authorization') }).exec(function (err, user) {
                if (err || !user) {
                    res.statusCode = 401;
                    return res.send('User was not found');
                }
                app.company.findOne({ _id: req.body.companyId }).exec(function (err, company) {
                    if (err) {
                        res.statusCode = 404;
                        return res.send('Company was not found with the provided Id');
                    } else {
                        if (!company) {
                            // For fixing when company returns null but no error
                            res.statusCode = 404;
                            return res.send('Company was not found with the provided Id');
                        }
                        app.punch.find({ company_id: req.body.companyId, user_id: user._id }).exec(function (err, punches) {
                            if (punches.length >= company.punchCount - 1) {
                                app.punch.updateMany({
                                    company_id: req.body.companyId, user_id: user._id }, { used: true }).exec(function (err, data) {
                                    if (err) {
                                        res.status(500).json({ error: "Failed to update database" });
                                    }
                                    res.json({ discount: true });
                                });
                            } else {
                                //Create new punch
                                new app.punch({
                                    user_id: user._id, company_id: req.body.companyId, created: (0, _moment2.default)(), used: false }).save(function (err, punch) {
                                    if (err) {
                                        res.status(500).json({ error: "Failed to create punch" });
                                    }
                                    var user_id = punch.user_id,
                                        company_id = punch.company_id,
                                        created = punch.created,
                                        used = punch.used,
                                        _id = punch._id;

                                    res.statusCode = 201;
                                    res.json({ punch_id: _id });
                                });
                            }
                        });
                    }
                });
            });
        }
    });
};