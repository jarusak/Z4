using System;
using System.Windows.Forms;

using Z3.View.Floating;

namespace Z3 {
    class Program : ApplicationContext
    {
        private Program()
        {
            try
            {
                Z3.View.IView v = Z3.View.Floating.FloatingViewFactory.Build();
                Z3.State.ProgramState s = new Z3.State.ProgramState();
                Z3.Logic.ALogic l = new Z3.Logic.LogicLayer();
                l.Initialize(v, s);
                v.Display.ViewClosed += new EventHandler<FormClosedEventArgs>(v_ViewClosed);
                v.Display.Show();

                String lastFile = Options.LastWorkspace;
                if (lastFile != null)
                {
                    try
                    {
                        s.CurrentWorkspace.Value = Z3.Workspace.Factory.Load(lastFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Encountered a " + ex.GetType().Name + " (" + ex.Message + ") in file:\n" + lastFile, "Unable to open most recent file");
                        Options.LastWorkspace = null;
                        s.CurrentWorkspace.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unhandled error is forcing Z3 to close.  This is the error message:\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Z3 Application Error");
            }
        }

        void v_ViewClosed(object sender, FormClosedEventArgs e)
        {
            ExitThread();
        }

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            
            ApplicationContext myAppContext = new Program();
            Application.Run(myAppContext);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            using (ErrorForm ef = new ErrorForm())
            {
                ef.Message = "An unhandled error is forcing Z3 to close.  This is the error message:\n\n" + e.Exception.Message + "\n\n" + e.Exception.StackTrace;
                ef.ShowDialog();
            }
        }
    }
}