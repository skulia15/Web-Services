import app from './app';
import mongoose, {Schema} from 'mongoose';

mongoose.Promise = global.Promise;

mongoose
  .connect('mongodb://skulia15:skulia15@ds125365.mlab.com:25365/veft-testing', {
    useMongoClient: true,
  })
  .then(db => {
    const server = app(db);
    server.listen(3000, () => console.log('Server running on port 3000'));
  });