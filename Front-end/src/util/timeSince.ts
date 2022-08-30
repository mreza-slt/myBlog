// refrence link
// https://stackoverflow.com/questions/3177836/how-to-format-time-since-xxx-e-g-4-minutes-ago-similar-to-stack-exchange-site

export default function timeSince(date: string): string {
  const lastdate: Date = new Date(date);

  var seconds = Math.floor((+new Date() - +lastdate) / 1000);

  var interval = seconds / 31536000;

  if (interval > 1) {
    return Math.floor(interval) + " سال قبل";
  }
  interval = seconds / 2592000;
  if (interval > 1) {
    return Math.floor(interval) + " ماه قبل";
  }
  interval = seconds / 86400;
  if (interval > 1) {
    return Math.floor(interval) + " روز قبل";
  }
  interval = seconds / 3600;
  if (interval > 1) {
    return Math.floor(interval) + " ساعت قبل";
  }
  interval = seconds / 60;
  if (interval > 1) {
    return Math.floor(interval) + " دقیقه قبل";
  }
  return Math.floor(seconds) + " ثانیه قبل";
}
