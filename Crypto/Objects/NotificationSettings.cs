using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    public class NotificationSettings
    {
        public double StandardDifference { get; set; }
        public double SoundDifference { get; set; }
        public NotificationType Type { get; set; }
        public List<string> IgnoredSymbolNames { get; set; } = new List<string>();
        public List<string> WatchedSymbolNames { get; set; } = new List<string>();

        public NotificationSettings(double standardDifference, double soundDifference)
        {
            StandardDifference = standardDifference;
            SoundDifference = soundDifference;
        }
    }

    public enum NotificationType
    {
        All,
        Specified,
        AllExcept
    }
}
