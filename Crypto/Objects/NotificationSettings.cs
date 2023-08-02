using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    public class NotificationSettings
    {
        public double DefaultStandardDifference { get; set; }
        public double DefaultSoundDifference { get; set; }
        public List<string> IgnoredSymbolNames { get; set; } = new List<string>();
        public List<string> MutedSymbolNames { get; set; } = new List<string>();
        public List<NotificationGroup> Groups { get; set; } = new List<NotificationGroup>();

        public NotificationSettings(double standardDifference, double soundDifference)
        {
            DefaultStandardDifference = standardDifference;
            DefaultSoundDifference = soundDifference;
        }
    }

    public class NotificationGroup
    {
        public List<string> Symbols { get; set; }
        public double StandardDifference { get; set; } = 0.00015;
        public double SoundDifference { get; set; } = 0.0003;
        public NotificationGroup(List<string> symbols, double diff, double soundDiff)
        {
            Symbols = symbols;
            StandardDifference = diff;
            SoundDifference = soundDiff;
        }
        public NotificationGroup()
        {
            Symbols = new();
        }
    }
}
