using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtDb.Extensions
{
    public static class IntExtension
    {
        public static char ToExcelColumn(this int value)
        {
            const int AValue = 'A';
            const int ZValue = 'Z';
            const char OUT_OF_RANGE = '!';

            //value is 0 based
            value += AValue;

            char output;
            if (AValue <= value && value <= ZValue)
            {
                output = (char)value;
            }
            else
            {
                output = OUT_OF_RANGE;
            }
            return output;
        }
    }
}