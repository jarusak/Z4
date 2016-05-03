using System;
using System.Collections.Generic;
using System.Text;

namespace Z3.Leger {
    public abstract class Z3ControllerState {
        protected Z3Controller controller;
        protected bool[] stats;

        public Z3ControllerState(Z3Controller c, bool[] stats) {
            controller = c;
            this.stats = stats;
        }

        protected void transition(Z3ControllerState s) {
            controller.State = (s);
            s.enforce();
        }

        protected void refresh() {
            enforce();
        }

        protected void error() {
            throw new InvalidOperationException("Illegal state transition requested.");
        }

        public abstract void enforce();

        public virtual void loadFile() { }
        public virtual void unloadFile() { }
        public virtual void selectContainer() { }
        public virtual void deselectContainer() { }
        public virtual void selectCountable() { }
        public virtual void deselectCountable() { }

        public bool isReady() { return stats[0]; }
        public bool isFileLoaded() { return stats[1]; }
        public bool isContainerSelected() { return stats[2]; }
        public bool isCountableSelected() { return stats[3]; }

        public class NoFileState : Z3ControllerState {
            public NoFileState(Z3Controller controller)
                : base(controller, new bool[] { false, false, false, false }) {
            }

            public override void enforce() {
                controller.cs_enableDataControls(false);
                controller.cs_hotkeyReady(false);
                controller.cs_ready(false);
            }

            public override void loadFile() {
                transition(new FileState(controller));
            }

            public override void selectContainer() { error(); }
            public override void deselectContainer() { error(); }
            public override void selectCountable() { error(); }
            public override void deselectCountable() { error(); }
        }

        public class FileState : Z3ControllerState {
            public FileState(Z3Controller controller)
                : base(controller, new bool[] { false, true, false, false }) {
            }

            public override void enforce() {
                controller.cs_enableDataControls(true);
                controller.cs_hotkeyReady(false);
                controller.cs_ready(false);
            }

            public override void loadFile() {
                refresh();
            }

            public override void unloadFile() {
                transition(new NoFileState(controller));
            }

            public override void selectContainer() {
                transition(new FileContainerState(controller));
            }

            public override void selectCountable() {
                transition(new FileCountableState(controller));
            }
        }

        public class FileContainerState : Z3ControllerState {
            public FileContainerState(Z3Controller controller)
                : base(controller, new bool[] { false, true, true, false }) {
            }

            public override void enforce() {
                controller.cs_enableDataControls(true);
                controller.cs_hotkeyReady(true);
                controller.cs_ready(false);
            }

            public override void loadFile() {
                transition(new FileState(controller));
            }

            public override void unloadFile() {
                transition(new NoFileState(controller));
            }

            public override void selectContainer() {
                refresh();
            }

            public override void deselectContainer() {
                transition(new FileContainerState(controller));
            }

            public override void selectCountable() {
                transition(new ReadyState(controller));
            }
        }
        
        public class FileCountableState : Z3ControllerState {
            public FileCountableState(Z3Controller controller)
                : base(controller, new bool[] { false, true, false, true }) {
            }

            public override void enforce() {
                controller.cs_enableDataControls(true);
                controller.cs_hotkeyReady(false);
                controller.cs_ready(false);
            }

            public override void loadFile() {
                transition(new FileState(controller));
            }

            public override void unloadFile() {
                transition(new NoFileState(controller));
            }

            public override void selectContainer() {
                transition(new ReadyState(controller));
            }

            public override void selectCountable() {
                refresh();
            }

            public override void deselectCountable() {
                transition(new FileState(controller));
            }
        }
        
        public class ReadyState : Z3ControllerState {
            public ReadyState(Z3Controller controller)
                : base(controller, new bool[] { true, true, true, true }) {
            }

            public override void enforce() {
                controller.cs_enableDataControls(true);
                controller.cs_hotkeyReady(true);
                controller.cs_ready(true);
            }

            public override void loadFile() {
                transition(new FileState(controller));
            }

            public override void unloadFile() {
                transition(new NoFileState(controller));
            }

            public override void selectContainer() {
                refresh();
            }

            public override void deselectContainer() {
                transition(new FileCountableState(controller));
            }

            public override void selectCountable() {
                refresh();
            }

            public override void deselectCountable() {
                transition(new FileContainerState(controller));
            }
        }
    }
}