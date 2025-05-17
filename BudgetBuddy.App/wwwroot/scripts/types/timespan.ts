namespace BudgetBuddy.App.Frontend.Types
{
    export class TimeSpan {
        private milliseconds: number;

        constructor(milliseconds: number) {
            this.milliseconds = milliseconds;
        }

        static fromDays(days: number): TimeSpan {
            return new TimeSpan(days * 24 * 60 * 60 * 1000);
        }

        static fromHours(hours: number): TimeSpan {
            return new TimeSpan(hours * 60 * 60 * 1000);
        }

        static fromMinutes(minutes: number): TimeSpan {
            return new TimeSpan(minutes * 60 * 1000);
        }

        static fromSeconds(seconds: number): TimeSpan {
            return new TimeSpan(seconds * 1000);
        }

        static fromMilliseconds(milliseconds: number): TimeSpan {
            return new TimeSpan(milliseconds);
        }

        get totalDays(): number {
            return this.milliseconds / (24 * 60 * 60 * 1000);
        }

        get totalHours(): number {
            return this.milliseconds / (60 * 60 * 1000);
        }

        get totalMinutes(): number {
            return this.milliseconds / (60 * 1000);
        }

        get totalSeconds(): number {
            return this.milliseconds / 1000;
        }

        get totalMilliseconds(): number {
            return this.milliseconds;
        }

        add(timeSpan: TimeSpan): TimeSpan {
            return new TimeSpan(this.milliseconds + timeSpan.totalMilliseconds);
        }

        subtract(timeSpan: TimeSpan): TimeSpan {
            return new TimeSpan(this.milliseconds - timeSpan.totalMilliseconds);
        }

        toString(): string {
            const hours = Math.floor(this.totalHours);
            const minutes = Math.floor(this.totalMinutes % 60);
            const seconds = Math.floor(this.totalSeconds % 60);
            const milliseconds = this.totalMilliseconds % 1000;
            return `${hours}:${minutes}:${seconds}.${milliseconds}`;
        }
    }

}

