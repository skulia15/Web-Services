const express = require("express");
const bodyParser = require("body-parser");
const prettier = require("prettier"); //?
const moment = require("moment");
const app = express();
app.use(bodyParser.json());

app.get('/', (req, res) => {
    res.json({message: 'Home Page, nothing to display'});
});
    
// GET /api/companies
// Returns a list of all registered companies
app.get('/api/companies', (req, res) => {
    res.json(companies);
});

// POST /api/companies
// Adds a new company. The required properties are "name" and "punchCount",
// indicating how many punches a user needs to collect in order to get a discount.
app.post('/api/companies', (req, res) => {
    let newCompany = {};
    newCompany.id = req.body.id;
    newCompany.name = req.body.name;
    newCompany.punchCount = req.punchCount;
    companies.push(newCompany);
    res.status(201).send('Company Created!');
});

//  GET /api/companies/{id}
// Returns a given company by id
app.get('/api/companies/:id', (req, res) => {
    let result = companies.filter(c => c.id == req.params.id);
    if(result.length){
        res.status(200).json(result);
    }
    else{
        res.status(404).send('Company Not Found');
    }
});

//   GET /api/users/
app.get('/api/users/', (req, res) => {
    res.json(users);
});

//   POST /api/users/
// Adds a new user to the system. The following properties must be specified: name,
// email
app.post('/api/users/', (req, res) => {
    let newUser = {};
    newUser.id = req.body.id;
    newUser.name = req.body.name;
    newUser.email = req.body.email;
    users.push(newUser);
    res.status(201).send('User Created!');
});

//   GET /api/users/{id}/punches
// Returns a list of all punches registered for the given user. Each punch contains
// information about what company it was added to, and when it was created. It should
// be possible to filter the list by adding a "?company={id}" to the query.
app.get('/api/users/:id/punches', (req, res) => {
    let user = users.filter(c => c.id == req.params.id);
    // Find the user
    if(!user){
        res.status(404).send('User not found');
    }
    // Check if user has any punches
    else if(!user[0].punches.length){
        res.json({message: 'User has no punches to roll with'});
    }
    // If company is provided
    if(req.query.company){
        let allPunches = user[0].punches;
        let companyPunches = allPunches.filter(p => p.companyId == req.query.company); 
        if(companyPunches.length){
            res.json(companyPunches);
        }
        else{
            res.json({message: 'User has no punches from the provided company'});
        }
    }
    else{
        // Return all punches from the user
        res.json(user[0].punches);
    }
});

// POST /api/users/{id}/punches
// Adds a new punch to the user account. The only information needed is the id of the
// company
app.post('/api/users/:id/punches', (req, res) => {
    let user = users.filter(c => c.id == req.params.id);
    // Find the user
    if(!user){
        res.status(404).send('User not found');
    }
    let newPunch = {};
    newPunch.companyId = req.body.companyId;
    newPunch.date = moment().format('LL');
    user[0].punches.push(newPunch);
    res.status(201).send('Punch Added!');
});

app.listen(3000, () => console.log('listening on port 3000'));


let users = [
    {
        "id": 0,
        "name": "John Johnson",
        "email": "john@gmail.com",
        "punches": [
            {
                "companyId": 1,
                "date": moment().subtract(10, 'days').format('LL')
            },
            {
                "companyId": 1,
                "date": moment().subtract(115, 'days').format('LL')
            },
            {
                "companyId": 2, 
                "date": moment().subtract(10, 'years').format('LL')
            }
        ]

    },
    {
        "id": 1,
        "name": "Jim Bob",
        "email": "jim@gmail.com",
        "punches": [
            {
                "companyId": 1,
                "date": moment().subtract(5, 'days').format('LL')
            },
            {
                "companyId": 0,
                "date": moment().subtract(70, 'days').format('LL')
            }
        ]
    },
    {
        "id": 2,
        "name": "Bobba Fett",
        "email": "bobba@gmail.com",
        "punches": [
            {
                "companyId": 0,
                "date": moment().subtract(2, 'days').format('LL')
            }
        ]
    }
];

let companies = [
    {
        "id": 0,
        "name": "Bob's Burgers",
        "punchCount": 10
    },
    {
        "id": 1,
        "name": "Black Mesa",
        "punchCount": 9
    },
    {
        "id": 2,
        "name": "Blue Waffles",
        "punchCount": 9
    }
];