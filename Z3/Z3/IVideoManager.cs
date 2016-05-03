using System;
using System.Collections.Generic;
using System.Text;

namespace Z3.View {
    interface IVideoManager {
        void EnableVideo(bool p);
        void EnableVideoCalib(bool p);
        void Dispose();
        void Loading();
        void Calibrate(double value, int zoom);
        void EnableCounting(bool p);
        void Resize();
        Zoopomatic2.Controls.Measurer Measurer { get; }
    }
}
