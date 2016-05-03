using System;
using System.Collections.Generic;
using System.Text;

namespace Z3.Leger {
    public abstract class Z3ViewState {
        protected Z3View view;
        protected bool[] stats;

        public Z3ViewState(Z3View c, bool[] arr) {
            view = c;
            stats = arr;
        }

        protected void transition(Z3ViewState s) {
            view.setState(s);
            s.enforce();
        }

        protected void refresh() {
            enforce();
        }

        protected void error() {
            throw new InvalidOperationException("Illegal view state transition detected.");
        }

        public abstract void enforce();

        public virtual void sourceVideo() { }
        public virtual void unsourceVideo() { }
        public virtual void calibrateVideo() { }
        public virtual void decalibrateVideo() { }

        public bool isReady() { return stats[0]; }
        public bool isVideoSourced() { return stats[1]; }
        public bool isVideoCalibrated() { return stats[2]; }

        public class NoVideoState : Z3ViewState {
            public NoVideoState(Z3View c)
                : base(c, new bool[] { false, false, false }) {
            }

            public override void enforce() {
                view.vs_enableVideoControls(false);
                view.vs_enableVideoCalibControls(false);
                view.vs_ready(false);
            }

            public override void sourceVideo() {
                transition(new VideoSourcedState(view));
            }

            public override void calibrateVideo() { error(); }
            public override void decalibrateVideo() { error(); }
        }

        public class VideoSourcedState : Z3ViewState {
            public VideoSourcedState(Z3View c)
                : base(c, new bool[] { false, true, false }) {
            }

            public override void enforce() {
                view.vs_ready(false);
                view.vs_enableVideoCalibControls(false);
                view.vs_enableVideoControls(true);
            }

            public override void unsourceVideo() {
                transition(new NoVideoState(view));
            }

            public override void calibrateVideo() {
                transition(new VideoCalibratedState(view));
            }

            public override void decalibrateVideo() {
                refresh();
            }
        }

        public class VideoCalibratedState : Z3ViewState {
            public VideoCalibratedState(Z3View c)
                : base(c, new bool[] { true, true, true}) {
            }

            public override void enforce() {
                view.vs_enableVideoControls(false);
                view.vs_enableVideoCalibControls(true);
                view.vs_ready(true);
            }

            public override void sourceVideo() {
                transition(new VideoSourcedState(view));
            }

            public override void unsourceVideo() {
                transition(new NoVideoState(view));
            }

            public override void calibrateVideo() {
                refresh();
            }

            public override void decalibrateVideo() {
                transition(new VideoSourcedState(view));
            }
        }
    }
}
