using System;
using System.Collections.Generic;
using System.Linq;

namespace WinningTemp
{
    public class TollCalculator
    {
        private const int ChargeLimit = 60;

        /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
        public int GetTotalTollFee(IVehicle vehicle, DateTime[] dates)
        {
            var totalFee = 0;
            if (dates is {Length: > 0})
            {
                DateTime? previousChargeDate = null;
                foreach (var date in dates)
                {
                    if (previousChargeDate.HasValue &&
                        !((date - previousChargeDate.Value).TotalMinutes >= 60)) continue;

                    totalFee += GetTollFee(vehicle, date);
                    previousChargeDate = date;
                }
            }

            if (totalFee > ChargeLimit) totalFee = ChargeLimit;
            return totalFee;
        }

        private int GetTollFee(IVehicle vehicle, DateTime date)
        {
            if (IsTollFreeDate(date) || vehicle.IsTollFree()) return 0;

            //Get time slot
            var charge = GetChargesTable()
                .LastOrDefault(x => date.TimeOfDay >= x.Item1)
                .Item2;

            return charge;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            var month = date.Month;
            var day = date.Day;

            if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) return true;

            return month == 1 && day == 1 ||
                   month == 3 && day is 28 or 29 ||
                   month == 4 && day is 1 or 30 ||
                   month == 5 && day is 1 or 8 or 9 ||
                   month == 6 && day is 5 or 6 or 21 ||
                   month == 7 ||
                   month == 11 && day == 1 ||
                   month == 12 && day is 24 or 25 or 26 or 31;
        }

        //TODO: read table from external source
        private static IEnumerable<(TimeSpan, int)> GetChargesTable() =>
            new List<(TimeSpan, int)>
                {
                    (new TimeSpan(0, 0, 0), 0),
                    (new TimeSpan(6, 00, 0), 9),
                    (new TimeSpan(6, 30, 0), 16),
                    (new TimeSpan(7, 00, 0), 22),
                    (new TimeSpan(8, 00, 0), 16),
                    (new TimeSpan(8, 30, 0), 9),
                    (new TimeSpan(15, 00, 0), 16),
                    (new TimeSpan(15, 30, 0), 22),
                    (new TimeSpan(17, 00, 0), 16),
                    (new TimeSpan(18, 00, 0), 9),
                    (new TimeSpan(18, 30, 0), 0)
                }
                .OrderBy(x => x.Item1);
    }
}