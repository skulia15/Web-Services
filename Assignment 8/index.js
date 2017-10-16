/*
    In order for this to successfully run, there are 3 steps that need to be taken
      1. npm install
      2. npm run babel
      3. npm start

    You can also do this in 2 steps, if you are in a big big hurry
      1. npm install
      2. npm run execute

    After that the server runs on http://localhost:5001
*/

import express from 'express';
import bodyParser from 'body-parser';
import mongoose from 'mongoose';
const uuidv4 = require('uuid/v4');
var ObjectId = require('mongodb').ObjectID;
//var api = require('./api');

// entities.js
const {Schema} = mongoose;
const userSchema = Schema({
    name: String,
    token: String,
    gender: String //Bara einn character samt!!!!
});
const companySchema = Schema({
    name: String,
    punchCount: Number // default 10
});
const punchSchema = Schema({
    //company_id: ObjectId, //fatta ekki
    //user_id: ObjectId, // fatta ekki
    created: Date,
    used: Boolean //Initial value false
});


mongoose.Promise = global.Promise;
mongoose
    .connect('mongodb://skulia15:Jokull128@ds119685.mlab.com:19685/assignment8', {useMongoClient: true})
    .then(db => {
        console.log('Mongo connected');
        const User = db.model('User', userSchema);
        const Company = db.model('Company', companySchema);
        const Punch = db.model('Punch', punchSchema);

        //const routes = require('./Routes/api.js');
        var app = express();
        app.use(bodyParser.json());
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
            Company.find({}).exec((err, data) => {
                const filteredData = data.map(company => ({
                    name: company.name,
                    punchCount: company.punchCount,
                    id: company._id
                }));
                res.json({companies: filteredData});
            });
        });
        
        // Registers a new company to the punchcard.com service
        app.post('/api/companies', function (req, res) {
            // should be authenticated using the Authorization header, using the hardcoded admin token.
            // AUTHENTICATE HERE
            const name = req.body.name;
            var punchCount = req.body.punchCount;
            if (!punchCount){
                punchCount = 10;
            }
            if (!req.body.hasOwnProperty('name')) {
                res.statusCode = 400;
                return res.send('Post syntax error');
            }
            else{
                new Company({name, punchCount}).save((err, company) => {
                    if(err){
                        res.status(500).json({error: "Failed to save to database"});
                    }
                    console.log(company);
                    const {name, punchCount, _id} = company;
                    res.statusCode = 201;
                    res.json({company_id: _id});
                });
            }
        });
        
        // Gets a specific company, given a valid id
        app.get('/api/companies/:id', function (req, res) {
            let findCompany = Company.find({_id: req.params.id}).exec((err, companies) =>{
                console.log(companies);
                if (findCompany) {
                    const filteredData = companies.map(company => ({
                        name: company.name,
                        punchCount: company.punchCount,
                        id: company._id
                    }));
                    return res.json(filteredData);
                } else {
                    res.statusCode = 404;
                    return res.send('Company with given id was not found');
                }
            });
        });
        
        // Gets all users in the system
        app.get('/api/users', function (req, res) {
            User.find({}).exec((err, data) => {
                const filteredData = data.map(user => ({
                    name: user.name,
                    gender: user.gender,
                    id: user._id
                }));
                res.json({user: filteredData});
            })
        });
        
        // Creates a new user in the system
        app.post('/api/users', function (req, res) {
            // Requires Admin Token
            //AUTHORIZE HERE!
            const {name, gender} = req.body;
            const token = uuidv4();
            console.log(token);
            if (!req.body.hasOwnProperty('name') || !req.body.hasOwnProperty('gender')){
                res.statusCode = 400;
                return res.send('User was not properly formatted');
            }
            else if(gender !== "m" || gender !== "f" || gender !== "o"){
                res.statusCode = 400;
                return res.send('Gender should be m, f or o');
            }
            else{
                new User({name, token, gender}).save((err, user) => {
                    if(err){
                        res.status(500).json({error: "Failed to save to database"});
                    }   
                    console.log(user);
                    const {name, gender, _id} = user;
                    res.json({token});
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
    })
