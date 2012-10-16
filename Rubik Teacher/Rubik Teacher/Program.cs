using System;
using System.Windows.Forms;

namespace Rubik_Teacher {
#if WINDOWS || XBOX
    public static class Program {
		public static MainForm form;
        public static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(form = new MainForm());
        }
    }
#endif
}

