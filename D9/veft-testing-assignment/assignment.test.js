import request from 'supertest';
import mongo from 'mongodb-memory-server';
import app from './app';
import {Employee} from './app';
import mongoose from 'mongoose';
import {loop, throws} from './app';
import {add} from './add';
import * as addModule from './add';

jest.mock('./errorFunction');

mongoose.Promise = global.Promise;
let mongoServer;
let server;

beforeAll(() => {
  return new Promise((resolve,reject) => {
    mongoServer = new mongo();
    mongoServer.getConnectionString().then((mongoUri) => {
      mongoose
        .connect(mongoUri, {
          useMongoClient: true,
        })
        .then(db => {
          server = app(db);
          resolve();
        });
      });
  });
});


afterEach(() => {
  return new Promise((resolve,reject) => {
    Employee.deleteMany({}, (err,data) => {
      resolve();
    });
  });
});

describe('add', () => {
  test('Add two valid numbers', () => {
    expect(add(1,1)).toBe(2);
  });

  test('Add two large valid numbers', () => {
    expect(add(3000000,10000)).toBe(3010000);
  });

  test('Add two negative numbers', () => {
    expect(add(-3, -7)).toBe(-10);
  });

  test('Adding with null, Should return 1, ignore the null?', () => {
    expect(add(null, 1)).toBe(1);
  });

  test('Add without providing numbers, should return NaN?', () => {
    expect(add()).toBeNaN();
  });
});

describe('throws', () => {
  test('Should return 9 and should not throw an error', () => {
    expect(throws(9)).toBe(9);
  });
});

describe('loop', () => {
  test('Check if loop is called n times', () => {
    const addSpy = jest.spyOn(addModule, 'add');
    loop(10);
    expect(addSpy).toHaveBeenCalledTimes(10);
  });
});

const createEmployees = (n, cb) => {
  let promises = [];
  for (let i = 0; i < n; i++){
    promises.push(
      new Promise((resolve,reject) => {
        request(server)
          .post('/')
          .send({name : `employee ${i}`})
          .then(res => {
            resolve();
          })
      })
    )
  }
  Promise.all(promises).then(cb);
};

describe('server', () => {
  test('should return a list of 3 employees', done => {
    createEmployees(3,() => {
      request(server)
        .get('/')
        .expect(200)
        .then(res => {
          const resultWithoutIds = res.body.data.map(({name,jobTitles}) => ({
            name,
            jobTitles
          }));
          expect(resultWithoutIds).toMatchSnapshot();
          done();
        });
    });
  });
})

describe('server', () => {
  test('should return a list of 5 employees', done => {
    createEmployees(5,() => {
      request(server)
        .get('/')
        .expect(200)
        .then(res => {
          const resultWithoutIds = res.body.data.map(({name,jobTitles}) => ({
            name,
            jobTitles
          }));
          expect(resultWithoutIds).toMatchSnapshot();
          done();
        });
    });
  });
})

describe('server', () => {
  test('should return list an empty list of employees', done => {
    request(server)
      .get('/')
      .expect(200)
      .then(res => {
        expect(res.body).toEqual({data: []});
        done();
      });
  });
})

