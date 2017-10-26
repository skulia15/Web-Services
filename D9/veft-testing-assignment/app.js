import express from 'express';
import mongoose, {Schema} from 'mongoose';
import bodyParser from 'body-parser';
import errorFunction from './errorFunction';
import {add} from './add';

/* DO NOT REFACTOR THIS CODE */

export const throws = n => {
  errorFunction();
  return n;
};

export const loop = n => {
  let sum = 0;
  for (let i = 0; i < n; i++) {
    sum += add(n, n - 1);
  }
  return sum;
};
/* DO NOT REFACTOR THIS CODE */

/* SERVER CODE TO REFACTOR */
export const Employee = mongoose.model(
  'employee',
  Schema({
    name: String,
    jobTitles: {type: [String]}
  })
);

export default db => {
  const app = express();

  app.use(bodyParser.json());
  
  app.get('/', (req, res) => {
    Employee.find({}).exec((err, data) => res.json({data}));
  });

  app.post('/', (req,res) => {
    const employee_name = req.body.name;
    new Employee({name: employee_name}).save((err,employee) => {
      res.statusCode = 201;
      res.json({name: employee_name});
    });
  });

  return app;
};

/* SERVER CODE TO REFACTOR */