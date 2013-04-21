//////////////////////
// Original Writer: Ben Anderson.
// Reviewed by:
//////////////////////

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Server.Gui
{
    /// <summary>
    /// A simple class designed to make writing to the server log easier.
    /// </summary>
    public sealed class Logger
    {
        // Inline comment documentation incoming - see Network.cs
        // The variable that stores the singleton instance
        private static volatile Logger instance;

        // Locking object used for thread synchronisation
        private static readonly object syncRoot = new Object();

        // Empty constructor is empty.
        private Logger() { 
            
        }

        // The variable returns the logger instance, and can create it if needed
        // Danger: Cool code.
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

        // Stores the textbox the logger should be writing to
        private TextBox target = null;
        private bool consoleOnly = true;  // flag for whether should be writing to console or TextBox(s)

        // Changes the output textbox. It'd be cool to allow for multiple output sources, so this would
        // need to be expanded
        public void SetOutput(TextBox tb)
        {
            this.target = tb;
            this.consoleOnly = false;
            this.writerDelegate = new WriteDelegate(this.WriteDelegated);
        }

        // Used for invoking the change to a WinForms component, on the main thread
        private delegate void WriteDelegate(string s);
        private WriteDelegate writerDelegate;

        // The actual delegate that does the work. This will probably need changing to support
        // multiple outputs. We'll have to figure it out.      
        public void WriteDelegated(string s)
        {
            this.target.Text += s;
        }

        // Performs the actual write to the output. Again, we'll need to be changed
        // to support proper logging.
        public void _Write(string s, params object[] values)
        {
            // Obviously, we need a textbox to write too. Error out if one isn't available
            if (target == null && !consoleOnly)
                throw new Exception("Logger has not had a output set");

            // This'll parse any params out of the s.
            s = String.Format(s, values);

            // and now format the string nicely.
            s = String.Format("[{0}] {1}", DateTime.Now.ToString(), s);

            // write message to console if consoleOnly is true
            if (consoleOnly)
            {
                Console.Write(s);
                return;
            }

            // Detectings if this function is being executed by the main thread
            if (this.target.InvokeRequired)
                this.target.Text += s;
            else // if it's not, we need to invoke it.
                this.target.Invoke(writerDelegate, s);
        }

        // Wrapper method which automatically adds a newline
        public void _WriteLine(string s, params object[] values)
        {
            this._Write(s + "\r\n", values);
        }



        // Static version which was actually intended to be used.
        public static void Write(string s, params object[] values)
        {
            Logger.Instance._Write(s, values);
        }

        // Static version of the WriteLine
        public static void WriteLine(string s, params object[] values)
        {
            Logger.Instance._WriteLine(s, values);
        }
    }
}
