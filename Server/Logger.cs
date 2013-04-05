using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server
{
    /// <summary>
    /// A simple class designed to make writing to the server log easier.
    /// </summary>
    public sealed class Logger
    {
        // Inline comment documentation incoming - see Network.cs
        private static volatile Logger instance;
        private static object syncRoot = new Object();

        private Logger() { }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }
                return instance;
            }
        }

        private TextBox target = null;

        public void SetOutput(TextBox tb)
        {
            this.target = tb;
        }

        private delegate void WriteDelegate(string s);
        private WriteDelegate writerDelegate = new WriteDelegate(WriteDelegated);

        public static void WriteDelegated(string s)
        {

            Logger.instance.target.Text += s;
        }

        public void _Write(string s, params object[] values)
        {
            if (target == null)
                throw new Exception("Logger has not had a output set");

            s = String.Format(s, values);
            s = String.Format("[{0}] {1}", DateTime.Now.ToString(), s);

            if (!this.target.InvokeRequired)
                this.target.Text += s;
            else
            {
                this.target.Invoke(writerDelegate, s);
            }
        }

        public void _WriteLine(string s, params object[] values)
        {
            this._Write(s + "\r\n", values);
        }

        public static void Write(string s, params object[] values)
        {
            Logger.Instance._Write(s, values);
        }

        public static void WriteLine(string s, params object[] values)
        {
            Logger.Instance._WriteLine(s, values);
        }
    }
}
