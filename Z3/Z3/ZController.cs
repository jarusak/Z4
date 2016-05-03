/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Z3.Model;

namespace Z3.Leger
{
    /// <summary>
    /// The Z3 Controller.  Negotiates between the View and the Model.
    /// Encapsulates the business logic of Z3: Counting, Measuring, 
    /// handling Countable and Container levels.
    /// </summary>
    public class BadZController
    {


        /// <summary>
        /// Closes the controller.
        /// </summary>
 */
        /*public void Close()
        {
            while (_views.Count > 0)
            {
                ZView v = _views[0];
                v.Close();
                _views.Remove(v);
            }
            if (ZSchema.getInstance() != null)
                ZSchema.getInstance().Close();
        }*/
        /*
        /// <summary>
        /// Opens a workspace file.
        /// </summary>
        /// <param name="filename">The workspace or schema file to open.</param>
        public void openWorkspace(String filename) {
            if (_fn != null && filename.ToUpper().Equals(_fn.ToUpper())) return;
            _fn = filename;
            ZSchema.open(filename);
            
            _state.loadFile();
            resetView();
        }

        /// <summary>
        /// Creates a new workspace and returns a x associated with it.
        /// </summary>
        /// <param name="schema">The schema file to base the workspace on.</param>
        /// <param name="filename">The workspace file to create.</param>
        public void newWorkspace(string schema, string filename) {
            _fn = filename;
            ZSchema.newWorkspace(schema, filename);
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "RecentFile", filename);
            _state.loadFile();
            resetView();
        }*/
/*
    }
}
 */
