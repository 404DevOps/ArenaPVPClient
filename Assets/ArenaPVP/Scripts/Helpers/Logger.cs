using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    public static class Logger
    {
        public static void Log(string message)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            UnityEngine.Debug.Log($"{methodInfo.ReflectedType.Name}::{methodInfo.Name}::{message}");
        }
    }
}
