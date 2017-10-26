import express from "express";
import bodyParser from "body-parser";
import mongoose from "mongoose";
import app from "./api";
import createProducer from "./producer";
import { userTopic } from "./mqTopics";

mongoose.Promise = global.Promise;
const mongoConnection = mongoose.connect(
  "mongodb://skulia:skulia@ds119685.mlab.com:19685/assignment8",
  {
    useMongoClient: true
  }
);

const userProducer = createProducer(userTopic);
Promise.all([mongoConnection, userProducer]).then(([db, userMq]) => {
  const server = app(db, userMq);
  server.listen(5001), () => console.log("Listening on port 5001");
});
