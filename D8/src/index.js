import express from 'express';
import bodyParser from 'body-parser';
import mongoose from 'mongoose';

var application = express();
var app;

mongoose.Promise = global.Promise;
mongoose.connect('mongodb://skulia:skulia@ds119685.mlab.com:19685/assignment8', {useMongoClient: true}).then((db) => {
    application.use(bodyParser.json());
    application.use(bodyParser.urlencoded({ extended: true }));
    require(__dirname + '/api.js')(application, require(__dirname + '/entities.js')(db));
    application.listen(5001);
    console.log('Mongo connected');
    console.log('Server listening on port 5001');
});
module.exports = application;