import {add, loop, throws} from './index';
import * as module from './index';

jest.mock('./errorFunction');

describe('add', () => {
  test('Add two valid numbers', () => {
    expect(module.add(1,1)).toBe(2);
  });

  test('Add two large valid numbers', () => {
    expect(module.add(3000000,10000)).toBe(3010000);
  });

  test('Add two negative numbers', () => {
    expect(module.add(-3, -7)).toBe(-10);
  });

  test('Adding with null, Should return 1, ignore the null?', () => {
    expect(module.add(null, 1)).toBe(1);
  });

  test('Add without providing numbers, should return NaN?', () => {
    expect(module.add()).toBeNaN();
  });
});

describe('throws', () => {
  test('Should return 9 and should not throw an error', () => {
    expect(throws(9)).toBe(9);
  });
});

describe('loop', () => {
  test('Check if loop is called n times', () => {

    const addSpy = jest.spyOn(module, 'add');
    module.loop(100);
    expect(addSpy).toHaveBeenCalledTimes(100);
  });
});