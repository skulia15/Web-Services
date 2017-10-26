import moment from "moment";
import express from "express";
import mongoose from "mongoose";
import bodyParser from "body-parser";
//import {tables} from './entities';
import { userTopic } from "./mqTopics";
const { Schema } = mongoose;

const userSchema = Schema({
  name: String,
  token: String,
  gender: String
});

const companySchema = Schema({
  name: String,
  punchCount: Number // default 10
});

const punchSchema = Schema({
  company_id: { type: mongoose.Schema.Types.ObjectId, ref: "Company" },
  user_id: { type: mongoose.Schema.Types.ObjectId, ref: "User" },
  created: Date,
  used: Boolean // Initial value false
});

export default (db, userMq) => {
  console.log("Mongo Connected");
  const uuidv4 = require("uuid/v4");
  const adminToken = "598d24f1-3d87-4a0d-980a-d00d461be53b";
  const app = express();
  app.use(bodyParser.json());
  app.use(bodyParser.urlencoded({ extended: true }));

  const User = db.model("User", userSchema);
  const Company = db.model("Company", companySchema);
  const Punch = db.model("Punch", punchSchema);

  // Returns all companies, registered in the punchcard.com service
  app.get("/api/companies", function(req, res) {
    Company.find({}).exec((err, companies) => {
      if (err) {
        res.status(500).json({ error: "Failed to load from database" });
      }
      const filteredData = companies.map(company => ({
        name: company.name,
        punchCount: company.punchCount,
        id: company._id
      }));
      res.json({ companies: filteredData });
    });
  });

  // Registers a new company to the punchcard.com service
  app.post("/api/companies", function(req, res) {
    if (!req.header("Authorization")) {
      res.statusCode = 401;
      return res.send("Token missing in header fields");
    }
    if (req.header("Authorization") !== adminToken) {
      res.statusCode = 403;
      return res.send("Forbidden");
    }
    const name = req.body.name;
    //handle error
    var punchCount = req.body.punchCount;
    if (!punchCount) {
      punchCount = 10;
    }
    if (!req.body.hasOwnProperty("name")) {
      res.statusCode = 412;
      return res.send("Data not structured correctly");
    } else {
      new Company({ name, punchCount }).save((err, company) => {
        if (err) {
          res.status(500).json({ error: "Failed to save to database" });
        }
        const { name, punchCount, _id } = company;
        res.statusCode = 201;
        res.json({ company_id: _id });
      });
    }
  });

  // Gets a specific company, given a valid id
  app.get("/api/companies/:id", function(req, res) {
    let findCompany = Company.find({
      _id: req.params.id
    }).exec((err, companies) => {
      if (err) {
        res.status(500).json({ error: "Failed to load from database" });
      }
      if (findCompany.length) {
        const filteredData = companies.map(company => ({
          name: company.name,
          punchCount: company.punchCount,
          id: company._id
        }));
        return res.json(filteredData);
      } else {
        res.statusCode = 404;
        return res.send("Company with given id was not found");
      }
    });
  });

  // Gets all users in the system
  app.get("/api/users", function(req, res) {
    User.find({}).exec((err, data) => {
      const filteredData = data.map(user => ({
        name: user.name,
        gender: user.gender,
        id: user._id
      }));
      res.json({ user: filteredData });
    });
  });

  // Creates a new user in the system
  app.post("/api/users", function(req, res) {
    if (!req.header("Authorization")) {
      res.statusCode = 401;
      return res.send("Token missing in header fields");
    }
    if (req.header("Authorization") !== adminToken) {
      res.statusCode = 403;
      return res.send("Forbidden");
    }
    const { name, gender } = req.body;
    const token = uuidv4();
    if (
      !req.body.hasOwnProperty("name") ||
      !req.body.hasOwnProperty("gender")
    ) {
      res.statusCode = 412;
      return res.send("User was not properly formatted");
    }
    if (gender !== "m" && gender !== "f" && gender !== "o") {
      res.statusCode = 412;
      return res.send("Gender should be m, f or o");
    } else {
      new User({ name, token, gender }).save((err, user) => {
        if (err) {
          res.status(500).json({ error: "Failed to save to database" });
        } else {
          const { name, gender, _id } = user;
          userMq.sendToQueue(
            userTopic,
            new Buffer(JSON.stringify({ name, token, gender, id: _id }))
          );
          res.json({ userId: _id, token });
        }
      });
    }
  });

  // Creates a punch, associated with a user
  app.post("/api/my/punches", function(req, res) {
    // Check if token is provided
    if (!req.header("Authorization")) {
      res.statusCode = 401;
      return res.send("Token missing in header fields");
    }
    // Check if company id is provided
    if (!req.body.hasOwnProperty("companyId")) {
      //case sensitive
      res.statusCode = 412;
      return res.send("Company Id is missing");
    } else {
      User.findOne({ token: req.header("Authorization") }).exec((err, user) => {
        if (err || !user) {
          res.statusCode = 401;
          return res.send("User was not found");
        }
        Company.findOne({ _id: req.body.companyId }).exec((err, company) => {
          if (err) {
            res.statusCode = 404;
            return res.send("Company was not found with the provided Id");
          } else {
            if (!company) {
              // For fixing when company returns null but no error
              res.statusCode = 404;
              return res.send("Company was not found with the provided Id");
            }
            Punch.find({
              company_id: req.body.companyId,
              user_id: user._id
            }).exec((err, punches) => {
              if (punches.length >= company.punchCount - 1) {
                Punch.updateMany(
                  {
                    company_id: req.body.companyId,
                    user_id: user._id
                  },
                  { used: true }
                ).exec((err, data) => {
                  if (err) {
                    res
                      .status(500)
                      .json({ error: "Failed to update database" });
                  }
                  res.json({ discount: true });
                });
              } else {
                //Create new punch
                new Punch({
                  user_id: user._id,
                  company_id: req.body.companyId,
                  created: moment(),
                  used: false
                }).save((err, punch) => {
                  if (err) {
                    res.status(500).json({ error: "Failed to create punch" });
                  }
                  const { user_id, company_id, created, used, _id } = punch;
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

  return app;
};
