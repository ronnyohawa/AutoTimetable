using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Africana_TimeTable_Generator.Forms;
using Africana_TimeTable_Generator.Forms.Configuaration;

namespace Africana_TimeTable_Generator
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Rooms());
        }
    }
}
