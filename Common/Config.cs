//////////////////////
// Original Writer: Ben Anderson.
// Reviewed by:
//////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class Config
    {
        private static Dictionary<string, StreamReader> files = new Dictionary<string, StreamReader>();

        public Config(string path)
        {
        }

        public object Get(string name)
        {
            return null;
        }

        public object Get(string name, object def)
        {
            return null;
        }
    }
}
