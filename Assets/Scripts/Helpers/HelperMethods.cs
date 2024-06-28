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
        public static string GetKeyBindNameShort(KeyCode[] keycodes)
        {
            string fullStr = "";
            foreach (KeyCode key in keycodes)
            {
                var str = key.ToString();

                str = str.Replace("Keypad", "");
                str = str.Replace("None", "");
                str = str.Replace("Alpha", "");
                str = str.Replace("LeftShift", "s");
                str = str.Replace("LeftControl", "c");
                str = str.Replace("LeftAlt", "a");
                str = str.Replace("Mouse0", "ML");
                str = str.Replace("Mouse1", "MR");
                str = str.Replace("Mouse2", "M3");
                str = str.Replace("Mouse3", "M4");
                str = str.Replace("Mouse4", "M5");
                str = str.Replace("Space", "SB");

                fullStr += str;
            }

            return fullStr;
        }
    }
}
