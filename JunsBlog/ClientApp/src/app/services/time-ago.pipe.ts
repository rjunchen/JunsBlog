import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeAgo',
})
export class TimeAgoPipe implements PipeTransform {

  transform(value: string): unknown {
    if (value) {
      const seconds = Math.floor((new Date().getTime() -new Date(value).getTime()) / 1000);
      if (seconds < 60) {
        return 'Just now'
      }

      const intervals = {
        'year': 60 * 60 * 24 * 7 * 365,
        'month': 60 * 60 * 24 * 7 * 30,
        'week': 60 * 60 * 24 * 7,
        'day': 60 * 60 * 24,
        'hour': 60 * 60,
        'minute': 60,
        'second': 1
      };

      let counter
      for (const i in intervals) {
        counter = Math.floor(seconds / intervals[i]);
        if (counter > 0) {
          return `${counter} ${i}${counter === 1 ? '' : 's'} ago`;
        }
      }

    }
    return value;
  }
}