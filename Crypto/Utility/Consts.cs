using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
    public static class Consts
    {
        // to do: implement a proper cell value class that would not use any specific numbers
        // but an enum and could keep a reason of an error
        public static float Error { get; } = -100f;
        public static float Unknown { get; } = -99f;
    }
}
