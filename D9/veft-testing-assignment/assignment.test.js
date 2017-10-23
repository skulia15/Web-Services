import {loop, throws} from './index';
import {add} from './add';
import * as addModule from './add';

jest.mock('./errorFunction');

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