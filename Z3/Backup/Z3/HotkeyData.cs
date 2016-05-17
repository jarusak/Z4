
using System.Windows.Forms;
using Z3.State;
using Z3.Logic;
using Z3.View;

namespace Z3.Util
{
    public class HotkeyContext
    {
        public HotkeyContext(IView v, ALogic l, ProgramState s)
        {
            View = v;
            Logic = l;
            State = s;
        }

        private IView _view;

        public IView View
        {
            get { return _view; }
            set { _view = value; }
        }
        private ALogic _logic;

        public ALogic Logic
        {
            get { return _logic; }
            set { _logic = value; }
        }
        private ProgramState _state;

        public ProgramState State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}