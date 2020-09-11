import { TimeAgoPipe } from './time-ago.pipe';

describe('TimeAgoPipe', () => {
  it('create an instance', () => {
    const pipe = new TimeAgoPipe();
    expect(pipe).toBeTruthy();
  });

  it('should transform the date to yesterday', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setHours(testDate.getHours() - 25); // Subtract 25 hours from now
    expect( pipe.transform(testDate.toUTCString())).toEqual('yesterday');
  });

  it('should transform the date to 2 days ago', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setHours(testDate.getHours() - 49); // Subtract 49 hours from now
    expect( pipe.transform(testDate.toUTCString())).toEqual('2 days ago');
  });

  it('should transform the date to just now', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setSeconds(testDate.getSeconds() - 59); // Subtract 59 seconds from now
    expect(pipe.transform(testDate.toUTCString())).toEqual('Just now');
  });

  it('should transform the date to 1 minute ago', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setSeconds(testDate.getSeconds() - 60); // Subtract 60 seconds from now
    expect(pipe.transform(testDate.toUTCString())).toEqual('1 minute ago');
  });

  it('should transform the date to 1 month ago', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setMonth(testDate.getMonth() - 1); // Subtract 1 month from now
    expect(pipe.transform(testDate.toUTCString())).toEqual('1 month ago');
  });
  it('should transform the date to 2 months ago', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setMonth(testDate.getMonth() - 2); // Subtract 2 months from now
    expect(pipe.transform(testDate.toUTCString())).toEqual('2 months ago');
  });

  it('should transform the date to 1 year ago', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setMonth(testDate.getMonth() - 13); // Subtract 13 months from now
    expect(pipe.transform(testDate.toUTCString())).toEqual('1 year ago');
  });

  it('should transform the date to 2 years ago', () => {
    const pipe = new TimeAgoPipe();
    var testDate = new Date();
    testDate.setMonth(testDate.getMonth() - 25); // Subtract 25 months from now
    expect(pipe.transform(testDate.toUTCString())).toEqual('2 years ago');
  });

});
