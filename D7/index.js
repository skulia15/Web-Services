const express = require("express");
const bodyParser = require("body-parser");
const prettier = require("prettier"); //?
const app = express();
app.use(bodyParser.json());

app.get('/', (req, res) => {
    res.json({message: 'Hello World'});
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
    var result = companies.find(c => c.id === req.body.id);
    console.log(result);
    res.json(result);
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
    res.json({message: 'Returns a list of all punches registered for the given user'});
});

// POST /api/users/{id}/punches
// Adds a new punch to the user account. The only information needed is the id of the
// company
app.get('/api/users/:id/punches', (req, res) => {
    res.json({message: 'Adds a new punch to the user account'});
});

app.listen(3000, () => console.log('listening on port 3000'));


let users = [
    {
        "id": 0,
        "name": "John Johnson",
        "email": "john@gmail.com"
    },
    {
        "id": 1,
        "name": "Jim Bob",
        "email": "jim@gmail.com"
    },
    {
        "id": 2,
        "name": "Bobba Fett",
        "email": "bobba@gmail.com"
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