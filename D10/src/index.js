import express from "express";
import bodyParser from "body-parser";
import mongoose from "mongoose";
import app from "./api";
import createProducer from "./producer";
import {userTopic, punchTopic, discountTopic} from "./mqTopics";

mongoose.Promise = global.Promise;
const mongoConnection = mongoose.connect(
  "mongodb://skulia:skulia@ds119685.mlab.com:19685/assignment8",
  {
    useMongoClient: true
  }
);

const userProducer = createProducer(userTopic);
const PunchProducer = createProducer(punchTopic);
const discountProducer = createProducer(discountTopic);
Promise.all([mongoConnection, userProducer, PunchProducer, discountProducer]).then(([db, userMq, punchMq, discountMq]) => {
  const server = app(db, userMq, punchMq, discountMq);
  server.listen(5001), () => console.log("Listening on port 5001");
});
