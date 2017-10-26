import mongoose from "mongoose";

module.exports = function(db) {
  const { Schema } = mongoose;

  tables.user = db.model(
    "User",
    Schema({
      name: String,
      token: String,
      gender: String
    })
  );

  tables.company = db.model(
    "Company",
    Schema({
      name: String,
      punchCount: Number // default 10
    })
  );

  tables.punch = db.model(
    "Punch",
    Schema({
      company_id: { type: mongoose.Schema.Types.ObjectId, ref: "Company" },
      user_id: { type: mongoose.Schema.Types.ObjectId, ref: "User" },
      created: Date,
      used: Boolean // Initial value false
    })
  );

  return tables;
};
