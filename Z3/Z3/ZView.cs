/*
using System;
using System.Collections.Generic;
using System.Text;
using Z3.Model;

namespace Z3.Leger {
    /// <summary>
    /// A view of Z3.  See MVC design pattern.<br />
    /// The View is the aspect that interacts with the user.
    /// All data access should be handled by the Model (ZSchema) and the 
    /// view should interact with the Controller in logical operations
    /// instead of direct data manipulation.
    /// </summary>
    public interface ZView {
        /// <summary>
        /// Sends a list of valid measurement types to the view.
        /// </summary>
        /// <param name="types">A list of Z3Measurements to display.</param>
        void setMeasurementTypes(List<ZMeasurement> types);

        void setFileName(string filename);

        /// <summary>
        /// Sets the view state.
        /// </summary>
        /// <param name="z"></param>
        void setState(ZViewState z);

        /// <summary>
        /// Enables the video source management controls, when the video source
        /// is chosen.
        /// </summary>
        /// <param name="p"></param>
        void vs_enableVideoControls(bool p);

        /// <summary>
        /// Enables the video recalibration controls, when the video is calibrated.
        /// If this is FALSE, the "calibration" controls should be displayed.
        /// If this is TRUE, the "normal" controls should be displayed.
        /// </summary>
        /// <param name="p"></param>
        void vs_enableVideoCalibControls(bool p);

        /// <summary>
        /// Signals that the viewstate is ready for measurement.
        /// </summary>
        /// <param name="p"></param>
        void vs_ready(bool p);

        /// <summary>
        /// Enables the data management controls, when a file is open.
        /// </summary>
        /// <param name="p"></param>
        void cs_enableDataControls(bool p);

        /// <summary>
        /// Signals that the controller is ready for measurement, but only
        /// with hotkeys.
        /// </summary>
        /// <param name="p"></param>
        void cs_hotkeyReady(bool p);

        /// <summary>
        /// Signals that the controller is ready for measurement.
        /// </summary>
        /// <param name="p"></param>
        void cs_ready(bool p);

        void Close();

        void ShowFileName();
    }
}
*/