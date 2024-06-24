using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class HelperMethods
    {
        public static string GetKeyBindNameShort(KeyCode[] primary)
        {
            string fullStr = "";
            foreach (KeyCode key in primary)
            {
                var str = key.ToString();

                str = str.Replace("Keypad", "");
                str = str.Replace("Alpha", "");
                str.Replace("Shift", "s");

                fullStr += str;
            }

            return fullStr;
        }
    }
}
