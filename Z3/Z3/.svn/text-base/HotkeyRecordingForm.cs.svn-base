using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Z3.Util;

namespace Z3
{
    public partial class HotkeyRecordingForm : Form
    {
        public HotkeyRecordingForm()
        {
            InitializeComponent();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            Recording = !Recording;
        }

        private Keys _keychar;
        private bool _recording;
        public bool Recording
        {
            get
            {
                return _recording;
            }
            set
            {
                _recording = value;
                ctrlBox.Enabled = !_recording;
                altBox.Enabled = !_recording;
                shiftBox.Enabled = !_recording;
                okButton.Enabled = !_recording;
                cancelButton.Enabled = !_recording;
                
                if (_recording)
                {
                    keyBox.Text = "RECORDING";
                    recordButton.Text = "Stop";
                }
                else
                {
                    if (_keychar > 0)
                    {
                        keyBox.Text = _keychar.ToString();
                    }
                    else
                    {
                        keyBox.Text = "";
                    }
                    recordButton.Text = "Record";
                }
            }
        }

        public bool Ctrl
        {
            get
            {
                return ((_keychar & Keys.Control) != 0);
            }
            set
            {
                _keychar = (_keychar | Keys.Control);
                if (!value) _keychar -= Keys.Control;
                refresh();
            }
        }

        public bool Alt
        {
            get
            {
                return ((_keychar & Keys.Alt) != 0);
            }
            set
            {
                _keychar = (_keychar | Keys.Alt);
                if (!value) _keychar -= Keys.Alt;
                refresh();
            }
        }

        public bool Shift
        {
            get
            {
                return ((_keychar & Keys.Shift) != 0);
            }
            set
            {
                _keychar = (_keychar | Keys.Shift);
                if (!value) _keychar -= Keys.Shift;
                refresh();
            }
        }

        public Keys Key
        {
            get
            {
                return _keychar;
            }
            set
            {
                if (!Keyboard.IsMeaningful(value)) return;

                _keychar = value;
                Recording = false;

                refresh();
            }
        }

        public void ClearKey()
        {
            _keychar = 0;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Recording = false;
            ClearKey();
            refresh();
        }

        private void HotkeyRecordingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Recording) return;

            Shift = e.Shift;
            Alt = e.Alt;
            Ctrl = e.Control;

            Key = e.KeyData;
        }

        private void refresh()
        {
            ctrlBox.Checked = Ctrl;
            altBox.Checked = Alt;
            shiftBox.Checked = Shift;

            if (Keyboard.GetBaseKey(_keychar) == Keys.None)
            {
                keyBox.Text = "";
            }
            else
            {
                keyBox.Text = Keyboard.GetReadableKey(_keychar);
            }

            valueBox.Text = ((int)_keychar).ToString();
        }
    }
}
