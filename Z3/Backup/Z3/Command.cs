using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Z3.Util
{
    delegate void HotkeyCommandHandle(HotkeyContext context, Hotkeyable value);

    sealed class HotkeyCommand
    {
        private HotkeyContext _context;
        private HotkeyCommandHandle _keyCommand;
        private Hotkeyable _target;

        public HotkeyCommand(HotkeyCommandHandle cmd, HotkeyContext ctxt, Hotkeyable target)
        {
            _context = ctxt;
            _keyCommand = cmd;
            _target = target;
        }

        public void Perform()
        {
            if (_keyCommand != null)
                _keyCommand(_context, _target);
        }

        public string Name
        {
            get
            {
                return _target.Name;
            }
        }

        public Keys Hotkey
        {
            get
            {
                return _target.Hotkey;
            }
            set
            {
                _target.Hotkey = value;
            }
        }
    }
}
