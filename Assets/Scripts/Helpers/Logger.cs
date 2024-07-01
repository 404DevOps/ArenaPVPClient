using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    public static class Logger
    {
        public static void Log(string message)
        {
            StackTrace stackTrace = new StackTrace();
            UnityEngine.Debug.Log($"{stackTrace.GetFrame(1).GetType()}::{stackTrace.GetFrame(1).GetMethod().Name}::{message}");
        }
    }
}
