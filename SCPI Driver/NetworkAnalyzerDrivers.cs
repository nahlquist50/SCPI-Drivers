﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using Ivi.Visa.Interop;

namespace SCPI {

    namespace NetworkAnalyzerDrivers {

        public class testc {
            private PNAX myPNAX;
            public testc()
            {
                myPNAX = new PNAX();
                myPNAX.Initialize("ad", 3000);

            }
        }

        public class PNAX : SCPIDriver {

            // Nested Classes
            public class CapabilitiesClass {

                // Variables
                private PNAX _parentPNAX;

                // Properties
                /// <summary>
                /// Maximum number of channels allowed.
                /// </summary>
                public int MaximumChannels
                { get; private set; }
                /// <summary>
                /// Maximum number of windows allowed.
                /// </summary>
                public int MaximumWindows
                { get; private set; }
                /// <summary>
                /// Maximum number of traces per window.
                /// </summary>
                public int MaximumTracesPerWindow
                { get; private set; }
                /// <summary>
                /// Maximum number of points allowed.
                /// </summary>
                public int MaximumPoints
                { get; private set; }
                /// <summary>
                /// Minimum number of points allowed
                /// </summary>
                public int MinimumPoints
                { get; private set; }
                /// <summary>
                /// Maximum frequency allowed.
                /// </summary>
                public double MaximumFrequency
                { get; private set; }
                /// <summary>
                /// Minimum frequency allowed.
                /// </summary>
                public double MinimumFrequency
                { get; private set; }

                // Constructor
                internal CapabilitiesClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                }

                // Private Methods
                private void FindMaximumChannels()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:CHANnels:MAXimum:COUNt?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    MaximumChannels = (int)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                }
                private void FindMaximumWindows()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:WINDows:MAXimum:COUNt?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    MaximumWindows = (int)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                }
                private void FindMaximumTracesPerWindow()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:WINDows:TRACes:MAXimum:COUNt?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    MaximumTracesPerWindow = (int)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                }
                private void FindMaximumPoints()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:POINts:MAXimum?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    MaximumPoints = (int)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                }
                private void FindMinimumPoints()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:POINts:Minimum?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    MinimumPoints = (int)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                }
                private void FindMaximumFrequency()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:PRESet:FREQuency:MAXimum?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    MaximumFrequency = (double)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                }
                private void FindMinimumFrequency()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:PRESet:FREQuency:MINimum?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    MinimumFrequency = (double)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                }

                // Internal Methods
                internal void FindCapabilities()
                {
                    FindMaximumChannels();
                    FindMaximumWindows();
                    FindMaximumTracesPerWindow();
                    FindMaximumPoints();
                    FindMinimumPoints();
                    FindMaximumFrequency();
                    FindMinimumFrequency();
                }
                internal string[] FindInternalPortNames()
                {
                    object[] retVals;
                    List<string> portNames = new List<string>();
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CAPability:HARDware:PORTs:INTernal:CATalog?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVals = (object[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_Any, ",;");
                    foreach (object retVal in retVals) {
                        portNames.Add(Convert.ToString(retVal));
                    }
                    return portNames.ToArray();
                }
                internal int ConvertPortNameToNumber(string PortName)
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString(String.Format("SYSTem:CAPability:HARDware:PORTs:PNUMber? '{0}'", PortName));
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    return (int)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                }

            }
            public class CorrectionClass {

                // Nested Classes
                public class CalSetsClass {

                    // Variables
                    private PNAX _parentPNAX;

                    // Properties
                    /// <summary>
                    /// Returns the names of Cal Sets currently stored on the PNA.
                    /// </summary>
                    public string[] CalSetNames
                    {
                        get { return this.GetCalSetNames(); }
                    }

                    // Constructor
                    internal CalSetsClass(PNAX ParentPNAX)
                    {
                        _parentPNAX = ParentPNAX;
                    }

                    // Private Methods
                    private string[] GetCalSetNames()
                    {
                        object[] retVals;
                        List<string> calSetNames = new List<string>();
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString("CSET:CATalog?");
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        retVals = (object[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_Any, ",;");
                        foreach (object retVal in retVals) {
                            calSetNames.Add(Convert.ToString(retVal));
                        }
                        return calSetNames.ToArray();
                    }

                    // Public Methods
                    /// <summary>
                    /// Creates a new cal set and copies the current cal set data into it.
                    /// </summary>
                    public void Copy(string CalSetToCopy, string CalSetToCreate)
                    {
                        _parentPNAX.WriteString(String.Format("CSET:COPY '{0}', '{1}'", CalSetToCopy, CalSetToCreate));
                    }
                    /// <summary>
                    /// Returns whether or not the specified CalSet exists on the PNA.
                    /// </summary>
                    /// <param name="CalSet"></param>
                    /// <returns></returns>
                    public bool Exists(string CalSet)
                    {
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString(String.Format("CSET:EXISts? '{0}'", CalSet));
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        return (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    }
                    /// <summary>
                    /// Deletes the Cal Set from the PNA.
                    /// If the Cal Set is currently in use by a channel, the Cal Set is deleted and correction for that channel is turned off.
                    /// </summary>
                    /// <param name="CalSet"></param>
                    public void Delete(string CalSet)
                    {
                        _parentPNAX.WriteString(String.Format("CSET:DEL '{0}'", CalSet));
                    }
                    /// <summary>
                    /// Deletes all cal sets from the PNA, including phase reference and Global Delta Match Cal Sets.
                    /// </summary>
                    public void DeleteAll()
                    {
                        _parentPNAX.WriteString("CSET:DALL");
                    }
                    /// <summary>
                    /// Returns the date that the specified Cal Set was last saved.
                    /// If operation failed, returns:
                    /// new DateTime(1,1,1,0,0,0);
                    /// </summary>
                    public DateTime LastSavedDateAndTime(string CalSet)
                    {
                        DateTime date;
                        int[] retVals1;
                        int[] retVals2;
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString(String.Format("CSET:DATE? '{0}'", CalSet));
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        retVals1 = (int[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_I4, ",;");
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString(String.Format("CSET:TIME? '{0}'", CalSet));
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        retVals2 = (int[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_I4, ",;");
                        if (retVals1.Length > 2 && retVals2.Length > 2) {
                            date = new DateTime(retVals1[0], retVals1[1], retVals1[2], retVals2[0], retVals2[1], retVals2[0]);
                        } else {
                            date = new DateTime(1,1,1,0,0,0);
                        }
                        return date;
                    }

                }

                // Variables
                private PNAX _parentPNAX;
                private bool _channelMode;
                private GuidedCalibrationSavePreferenceEnum _savePreference;

                // Properties
                /// <summary>
                /// Determines whether or not the channel calibration will be honored (true)
                /// or if legacy behavior will be active (false).
                /// This should be set to true when using this driver. This property is only here
                /// for advanced users.
                /// </summary>
                public bool ChannelMode
                {
                    get { return this.GetChannelMode(); }
                    set { this.SetChannelMode(value); }
                }
                /// <summary>
                /// Specifies the default manner in which calibrations performed using this driver are saved.
                /// Cal data is ALWAYS stored to the channel Cal Register regardless of this setting.
                /// CalRegister = Each cal is saved ONLY to the channel Cal Register. If the error terms from a new Cal can co-exist with those in the Cal Register, they are appended.
                /// UserCalSet = Each cal is saved to its own new User Cal Set file. The Cal Set name is automatically generated.
                /// Reuse = The cal is saved to the cal set that is currently selected on the channel. If the channel does not yet have a selected Cal Set, the cal will be saved to a new User Cal Set.
                /// </summary>
                public GuidedCalibrationSavePreferenceEnum SavePreference
                {
                    get { return this.GetSavePreference(); }
                    set { this.SetSavePreference(value); }
                }
                /// <summary>
                /// Returns an array of valid connectors based on the connector descriptions of the available cal kits.
                /// Use an item from this array to specify a connector for Channel.Correction.Guided.Port("item").
                /// An example of a connector type is "APC 3.5 female"
                /// </summary>
                public string[] ValidConnectors
                {
                    get { return this.GetValidConnectors(); }
                }
                /// <summary>
                /// Manage Cal Sets on the PNA
                /// </summary>
                public CalSetsClass CalSets
                { get; private set; }

                // Constructor
                internal CorrectionClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                    CalSets = new CalSetsClass(_parentPNAX);
                }

                // Private Methods
                private bool GetChannelMode()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SENSe:CORRection:COLLect:GUIDed:CHANnel:MODE?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _channelMode = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _channelMode;
                }
                private void SetChannelMode(bool ChannelMode)
                {
                    _channelMode = ChannelMode;
                    _parentPNAX.WriteString(String.Format("SENSe:CORRection:COLLect:GUIDed:CHANnel:MODE {0}", _channelMode ? "ON" : "OFF"));
                }
                private GuidedCalibrationSavePreferenceEnum GetSavePreference()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SENSe:CORRection:PREFerence:CSET:SAVE?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("CALR")) {
                        _savePreference = GuidedCalibrationSavePreferenceEnum.CalRegister;
                    } else if (retVal.Contains("USER")) {
                        _savePreference = GuidedCalibrationSavePreferenceEnum.UserCalSet;
                    } else {
                        _savePreference = GuidedCalibrationSavePreferenceEnum.Reuse;
                    }
                    return _savePreference;
                }
                private void SetSavePreference(GuidedCalibrationSavePreferenceEnum SavePreference)
                {
                    _savePreference = SavePreference;
                    switch (_savePreference) {
                        case GuidedCalibrationSavePreferenceEnum.CalRegister:
                            _parentPNAX.WriteString("SENSe:CORRection:PREFerence:CSET:SAVE CALRegister");
                            break;
                        case GuidedCalibrationSavePreferenceEnum.UserCalSet:
                            _parentPNAX.WriteString("SENSe:CORRection:PREFerence:CSET:SAVE USER");
                            break;
                        case GuidedCalibrationSavePreferenceEnum.Reuse:
                            _parentPNAX.WriteString("SENSe:CORRection:PREFerence:CSET:SAVE REUSe");
                            break;
                    }
                }
                private string[] GetValidConnectors()
                {
                    object[] retVals;
                    List<string> validConnectors = new List<string>();
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SENSe:CORRection:COLLect:GUIDed:CONNector:CATalog?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVals = (object[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_Any, ",;");
                    foreach (object retVal in retVals) {
                        validConnectors.Add(Convert.ToString(retVal));
                    }
                    return validConnectors.ToArray();
                }

                // Public Methods
                /// <summary>
                /// Returns a string array of valid cal kits that use the specified connector type.
                /// Find the connector types using PNAX.Correction.ValidConnectors.
                /// </summary>
                public string[] FindCalKitsUsingConnector(string Connector)
                {
                    object[] retVals;
                    List<string> calKits = new List<string>();
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString(String.Format("SENSe:CORRection:COLLect:GUIDed:CKIT:CATalog? '{0}'", Connector));
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVals = (object[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_Any, ",;");
                    foreach (object retVal in retVals) {
                        calKits.Add(Convert.ToString(retVal));
                    }
                    return calKits.ToArray();
                }
                /// <summary>
                /// Main = Launches the Calibration Wizard which matches the active channel.
                /// CalKit = Launches the Version 2 Calibration Kit File Manager dialog box.
                /// </summary>
                /// <param name="CalibrationWizard"></param>
                public void LaunchCalibrationWizard(CalibrationWizardEnum CalibrationWizard)
                {
                    switch (CalibrationWizard) {
                        case CalibrationWizardEnum.Main:
                            _parentPNAX.WriteString("SYSTem:CORRection:WIZard:IMMediate MAIN");
                            break;
                        case CalibrationWizardEnum.CalKit:
                            _parentPNAX.WriteString("SYSTem:CORRection:WIZard:IMMediate MAIN");
                            break;
                    }
                }

            }
            public class DisplayClass {

                // Nested Classes
                public class WindowClass {

                    // Nested Classes
                    public class TraceClass {

                        // Variables
                        internal WindowClass _parentWindow;
                        private bool _memory;
                        private bool _display;
                        private string _title;
                        private bool _titleState;
                        private double _perDivision;
                        private double _minimumPerDivision;
                        private double _maximumPerDivision;
                        private double _referenceLevel;
                        private double _minimumReferenceLevel;
                        private double _maximumReferenceLevel;
                        private int _referencePosition;
                        private int _minimumReferencePosition;
                        private int _maximumReferencePosition;

                        // Properties
                        /// <summary>
                        /// Number of the trace within the window. Not the measurement number on the PNAX display.
                        /// </summary>
                        public int Number
                        { get; private set; }
                        /// <summary>
                        /// The measurement attached to this trace.
                        /// </summary>
                        public PNAX.ChannelClass.MeasurementClass AttachedMeasurement
                        { get; private set; }
                        /// <summary>
                        /// Enable or disable the memory trace.
                        /// </summary>
                        public bool Memory
                        {
                            get { return this.GetMemory(); }
                            set { this.SetMemory(value); }
                        }
                        /// <summary>
                        /// Turn on or off the trace on the display.
                        /// </summary>
                        public bool Display
                        {
                            get { return this.GetDisplay(); }
                            set { this.SetDisplay(value); }
                        }
                        /// <summary>
                        /// Title of the trace that shows up on the PNAX display.
                        /// </summary>
                        public string Title
                        {
                            get { return this.GetTitle(); }
                            set { this.SetTitle(value); }
                        }
                        /// <summary>
                        /// Enable or disable the displaying of the title.
                        /// </summary>
                        public bool TitleState
                        {
                            get { return this.GetTitleState(); }
                            set { this.SetTitleState(value); }
                        }
                        /// <summary>
                        /// Y Axis Per Division Value.
                        /// </summary>
                        public double PerDivision
                        {
                            get { return this.GetPerDivision(); }
                            set { this.SetPerDivision(value); }
                        }
                        /// <summary>
                        /// Current minimum Y Axis Per Division Value
                        /// </summary>
                        public double MinimumPerDivision
                        {
                            get { return this.GetMinimumPerDivision(); }
                        }
                        /// <summary>
                        /// Current maximum Y Axis Per Division Value
                        /// </summary>
                        public double MaximumPerDivision
                        {
                            get { return this.GetMaximumPerDivision(); }
                        }
                        /// <summary>
                        /// Y Axis Reference Level
                        /// </summary>
                        public double ReferenceLevel
                        {
                            get { return this.GetReferenceLevel(); }
                            set { this.SetReferenceLevel(value); }
                        }
                        /// <summary>
                        /// Current Minimum Y Axis Reference Level
                        /// </summary>
                        public double MinimumReferenceLevel
                        {
                            get { return this.GetMinimumReferenceLevel(); }
                        }
                        /// <summary>
                        /// Current Maximum Y Axis Reference Level
                        /// </summary>
                        public double MaximumReferenceLevel
                        {
                            get { return this.GetMaximumReferenceLevel(); }
                        }
                        /// <summary>
                        /// Y Axis Reference Position
                        /// </summary>
                        public int ReferencePosition
                        {
                            get { return this.GetReferencePosition(); }
                            set { this.SetReferencePosition(value); }
                        }
                        /// <summary>
                        /// Current Minimum Y Axis Reference Position
                        /// </summary>
                        public int MinimumReferencePosition
                        {
                            get { return this.GetMinimumReferencePosition(); }
                        }
                        /// <summary>
                        /// Current Maximum Y Axis Reference Position
                        /// </summary>
                        public int MaximumReferencePosition
                        {
                            get { return this.GetMaximumReferencePosition(); }
                        }

                        // Constructor
                        internal TraceClass(WindowClass ParentWindow, int Number, PNAX.ChannelClass.MeasurementClass AttachedMeasurment)
                        {
                            _parentWindow = ParentWindow;
                            this.Number = Number;
                            this.AttachedMeasurement = AttachedMeasurement;
                        }

                        // Private Methods
                        private bool GetMemory()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:MEMory:STATe?", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _memory = (((byte)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _memory;
                        }
                        private void SetMemory(bool Memory)
                        {
                            _memory = Memory;
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:MEMory:STATe {2}", _parentWindow.Number, Number, _memory ? "ON" : "OFF"));
                        }
                        private bool GetDisplay()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:STATe?", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _display = (((byte)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _display;
                        }
                        private void SetDisplay(bool Display)
                        {
                            _display = Display;
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:STATe {2}", _parentWindow.Number, Number, _display ? "ON" : "OFF"));
                        }
                        private string GetTitle()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:DATA?", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _title = _parentWindow._parentPNAX.ReadString();
                            return _title;
                        }
                        private void SetTitle(string Title)
                        {
                            _title = Title;
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:DATA {2}", _parentWindow.Number, Number, _title));
                        }
                        private bool GetTitleState()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:STATe?", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _titleState = (((byte)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _titleState;
                        }
                        private void SetTitleState(bool TitleState)
                        {
                            _titleState = TitleState;
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:STATe {2}", _parentWindow.Number, Number, _titleState ? "ON" : "OFF"));
                        }
                        private double GetPerDivision()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision?", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _perDivision = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _perDivision;
                        }
                        private void SetPerDivision(double PerDivision)
                        {
                            _perDivision = PerDivision;
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision {2}", _parentWindow.Number, Number, _perDivision));
                        }
                        private double GetMinimumPerDivision()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision? MINimum", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _minimumPerDivision = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _minimumPerDivision;
                        }
                        private double GetMaximumPerDivision()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision? MAXimum", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _maximumPerDivision = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _maximumPerDivision;
                        }
                        private double GetReferenceLevel()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel?", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _referenceLevel = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _referenceLevel;
                        }
                        private void SetReferenceLevel(double ReferenceLevel)
                        {
                            _referenceLevel = ReferenceLevel;
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel {2}", _parentWindow.Number, Number, _referenceLevel));
                        }
                        private double GetMinimumReferenceLevel()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel? MINimum", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _minimumReferenceLevel = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _minimumReferenceLevel;
                        }
                        private double GetMaximumReferenceLevel()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel? MAXimum", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _maximumReferenceLevel = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _maximumReferenceLevel;
                        }
                        private int GetReferencePosition()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition?", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _referencePosition = (int)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                            return _referencePosition;
                        }
                        private void SetReferencePosition(int ReferencePosition)
                        {
                            _referencePosition = ReferencePosition;
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition {2}", _parentWindow.Number, Number, _referencePosition));
                        }
                        private int GetMinimumReferencePosition()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition? MINimum", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _minimumReferencePosition = (int)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                            return _minimumReferencePosition;
                        }
                        private int GetMaximumReferencePosition()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition? MAXimum", _parentWindow.Number, Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            _maximumReferencePosition = (int)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                            return _maximumReferencePosition;
                        }

                        // Public Methods
                        /// <summary>
                        /// Select this trace as the active trace.
                        /// </summary>
                        public void Select()
                        {
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:SELect", _parentWindow.Number, Number));
                        }
                        /// <summary>
                        /// Auto scale Y axis.
                        /// </summary>
                        public void AutoScaleY()
                        {
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:AUTO", _parentWindow.Number, Number));
                        }

                    }
                    public class TraceCollectionClass {

                        // Variables
                        private WindowClass _parentWindow;
                        private Dictionary<int, TraceClass> _traces;

                        // Properties
                        /// <summary>
                        /// Number of traces in this window managed by this driver.
                        /// </summary>
                        public int Count
                        {
                            get { return _traces.Count; }
                        }
                        /// <summary>
                        /// Trace Numbers in this window managed by this driver.
                        /// </summary>
                        public int[] ManagedTraceNumbers
                        {
                            get { return this._traces.Keys.ToArray(); }
                        }
                        /// <summary>
                        /// All open trace numbers in the window in the PNAX. Not all traces are necessarily managed by this driver.
                        /// i.e. If a trace was created locally using the front panel, this driver does not manage its use.
                        /// </summary>
                        public int[] OpenTraceNumbers
                        {
                            get { return this.GetOpenTraceNumbers(); }
                        }

                        // Constructor
                        internal TraceCollectionClass(WindowClass ParentWindow)
                        {
                            _parentWindow = ParentWindow;
                        }

                        // Indexer
                        public TraceClass this[int TraceNumber]
                        {
                            get
                            {
                                if (!_traces.ContainsKey(TraceNumber))
                                    throw new System.ArgumentException("Trace does not exist", "TraceNumber");

                                return _traces[TraceNumber];
                            }
                        }

                        // Protected Methods
                        protected virtual int[] GetOpenTraceNumbers()
                        {
                            _parentWindow._parentPNAX.ClearEventRegisters();
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:CATalog?", _parentWindow.Number));
                            _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                            return (int[])_parentWindow._parentPNAX.ReadList(IEEEASCIIType.ASCIIType_I4, ",;");

                        }

                        // Public Methods
                        /// <summary>
                        /// Adds a new trace with the next available trace number and attaches the measurement.
                        /// </summary>
                        /// <returns>
                        /// The trace number of the newly created trace.
                        /// Returns -1, if the maximum number of traces is reached.
                        /// </returns>
                        public int Add(PNAX.ChannelClass.MeasurementClass AttachedMeasurement)
                        {
                            int availableTraceNumber = 1;
                            int[] openTraceNumbers = OpenTraceNumbers;

                            if (openTraceNumbers.Length == _parentWindow._parentPNAX.Capabilities.MaximumTracesPerWindow)
                                return -1;

                            for (int i = 0; i < openTraceNumbers.Length; i++) {
                                if (!openTraceNumbers.Contains(i + 1)) {
                                    availableTraceNumber = i + 1;
                                    break;
                                }
                            }

                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:FEED {2}", _parentWindow.Number, availableTraceNumber, AttachedMeasurement.Name));
                            _traces.Add(availableTraceNumber, new TraceClass(_parentWindow, availableTraceNumber, AttachedMeasurement));

                            return availableTraceNumber;
                        }
                        /// <summary>
                        /// Deletes the specified trace from the collection and destroys the trace on the PNAX.
                        /// </summary>
                        /// <returns>True if the trace is deleted. False if the specified trace number doesn't exist.</returns>
                        public bool Delete(int TraceNumber)
                        {
                            if (_traces.ContainsKey(TraceNumber)) {
                                _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:DELete", _parentWindow.Number, TraceNumber));
                                _traces.Remove(TraceNumber);
                                return true;
                            } else if (OpenTraceNumbers.Contains(TraceNumber)) {
                                _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:DELete", _parentWindow.Number, TraceNumber));
                                return true;
                            } else {
                                return false;
                            }
                        }
                        /// <summary>
                        /// Iterates through all open trace numbers and deletes them.
                        /// </summary>
                        public void DeleteAll()
                        {
                            List<int> allNumbers = new List<int>(OpenTraceNumbers);
                            foreach (int number in allNumbers) {
                                _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:DELete", _parentWindow.Number, number));
                            }
                            _traces.Clear();
                        }

                    }

                    // Variables
                    private PNAX _parentPNAX;
                    private bool _scaleCoupling;
                    private string _title;
                    private bool _titleState;

                    // Properties
                    /// <summary>
                    /// Number of this window.
                    /// </summary>
                    public int Number
                    { get; private set; }
                    /// <summary>
                    /// Enable or disable scale coupling for this window.
                    /// </summary>
                    public bool ScaleCoupling
                    {
                        get { return this.GetScaleCoupling(); }
                        set { this.SetScaleCoupling(value); }
                    }
                    /// <summary>
                    /// Title to be displayed in the window title area.
                    /// </summary>
                    public string Title
                    {
                        get { return this.GetTitle(); }
                        set { this.SetTitle(value); }
                    }
                    /// <summary>
                    /// Enable or disable displaying of the title.
                    /// </summary>
                    public bool TitleState
                    {
                        get { return this.GetTitleState(); }
                        set { this.SetTitleState(value); }
                    }
                    /// <summary>
                    /// Control Traces in this window.
                    /// </summary>
                    public TraceCollectionClass Traces
                    { get; private set; }

                    // Constructor
                    internal WindowClass(int Number, PNAX ParentPNAX)
                    {
                        this.Number = Number;
                        _parentPNAX = ParentPNAX;

                        Traces = new TraceCollectionClass(this);
                    }

                    // Private Methods
                    private bool GetScaleCoupling()
                    {
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe:Y:SCALe:COUPle:STATe?", Number));
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        _scaleCoupling = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _scaleCoupling;
                    }
                    private void SetScaleCoupling(bool ScaleCoupling)
                    {
                        _scaleCoupling = ScaleCoupling;
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe:Y:SCALe:COUPle:STATe {1}", Number, _scaleCoupling ? "ON" : "OFF"));
                    }
                    private string GetTitle()
                    {
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:DATA?", Number));
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        _title = _parentPNAX.ReadString();
                        return _title;
                    }
                    private void SetTitle(string Title)
                    {
                        if (Title.Length > 50)
                            Title = Title.Substring(0, 50);

                        _title = Title;
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:DATA {1}", Number, _title));
                    }
                    private bool GetTitleState()
                    {
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:STATe?", Number));
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        _titleState = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _titleState;
                    }
                    private void SetTitleState(bool TitleState)
                    {
                        _titleState = TitleState;
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:STATe {1}", Number, _titleState ? "ON" : "OFF"));
                    }

                }
                public class WindowCollectionClass : IEnumerable<WindowClass> {

                    // Variables
                    private PNAX _parentPNAX;
                    private Dictionary<int, WindowClass> _windows;
                    private ScaleCouplingMethodEnum _scaleCouplingMethod;

                    // Properties
                    /// <summary>
                    /// Number of windows managed by this driver.
                    /// </summary>
                    public int Count
                    {
                        get { return this._windows.Count; }
                    }
                    /// <summary>
                    /// Window Numbers managed by this driver.
                    /// </summary>
                    public int[] ManagedWindowNumbers
                    {
                        get { return this._windows.Keys.ToArray(); }
                    }
                    /// <summary>
                    /// All open window numbers on the PNAX. Not all windows are necessarily managed by this driver.
                    /// i.e. If a window was created locally using the front panel, this driver does not manage its use.
                    /// </summary>
                    public int[] OpenWindowNumbers
                    {
                        get { return this.GetOpenWindowNumbers(); }
                    }
                    /// <summary>
                    /// Coupling method for trace scaling.
                    /// Off = No coupling.
                    /// Window = Scale settings are coupled for traces in each window.
                    /// All = Scale settings are coupled for traces in all selected windows. Setting controlled by coupling state of each window.
                    /// </summary>
                    public ScaleCouplingMethodEnum ScaleCouplingMethod
                    {
                        get { return this.GetScaleCouplingMethod(); }
                        set { this.SetScaleCouplingMethod(value); }
                    }

                    // Constructor
                    internal WindowCollectionClass(PNAX ParentPNAX)
                    {
                        _parentPNAX = ParentPNAX;
                    }

                    // Indexer
                    public WindowClass this[int Number]
                    {
                        get
                        {
                            if (!_windows.ContainsKey(Number))
                                throw new System.IndexOutOfRangeException("Window Number does not exist or is not managed by this driver.");

                            return _windows[Number];
                        }
                    }

                    // Private Methods
                    private int[] GetOpenWindowNumbers()
                    {
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString("SYSTem:WINDows:CATalog?");
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        return (int[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_I4, ",;");
                    }
                    private ScaleCouplingMethodEnum GetScaleCouplingMethod()
                    {
                        string retVal;
                        _parentPNAX.ClearEventRegisters();
                        _parentPNAX.WriteString("DISPlay:WINDow:TRACe:Y:SCALe:COUPle:METHod?");
                        _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                        retVal = _parentPNAX.ReadString();
                        if (retVal.Contains("OFF")) {
                            _scaleCouplingMethod = ScaleCouplingMethodEnum.Off;
                        } else if (retVal.Contains("WIND")) {
                            _scaleCouplingMethod = ScaleCouplingMethodEnum.Window;
                        } else {
                            _scaleCouplingMethod = ScaleCouplingMethodEnum.All;
                        }
                        return _scaleCouplingMethod;
                    }
                    private void SetScaleCouplingMethod(ScaleCouplingMethodEnum ScaleCouplingMethod)
                    {
                        _scaleCouplingMethod = ScaleCouplingMethod;
                        switch (_scaleCouplingMethod) {
                            case ScaleCouplingMethodEnum.Off:
                                _parentPNAX.WriteString("DISPlay:WINDow:TRACe:Y:SCALe:COUPle:METHod OFF");
                                break;
                            case ScaleCouplingMethodEnum.Window:
                                _parentPNAX.WriteString("DISPlay:WINDow:TRACe:Y:SCALe:COUPle:METHod WINDow");
                                break;
                            case ScaleCouplingMethodEnum.All:
                                _parentPNAX.WriteString("DISPlay:WINDow:TRACe:Y:SCALe:COUPle:METHod ALL");
                                break;
                        }
                    }

                    // Public Methods
                    /// <summary>
                    /// Adds a new window with the next available window number.
                    /// </summary>
                    /// <returns>
                    /// The window number of the newly created channel.
                    /// If the maximum number of windows is reached, returns -1.
                    /// </returns>
                    public int Add()
                    {
                        int availableWindowNumber = 1;
                        int[] openWindowNumbers = OpenWindowNumbers;

                        if (openWindowNumbers.Length == _parentPNAX.Capabilities.MaximumWindows)
                            return -1;

                        for (int i = 0; i < openWindowNumbers.Length; i++) {
                            if (!openWindowNumbers.Contains(i + 1)) {
                                availableWindowNumber = i + 1;
                                break;
                            }
                        }

                        // TO DO: Add a new window to the dictionary

                        return availableWindowNumber;
                    }
                    /// <summary>
                    /// Deletes the specified window number from the collection and destroys the window on the PNAX.
                    /// </summary>
                    /// <returns>True if the window is deleted. False if the specified window number doesn't exist.</returns>
                    public bool Delete(int WindowNumber)
                    {
                        if (_windows.ContainsKey(WindowNumber)) {
                            _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:STATe OFF", WindowNumber));
                            _windows.Remove(WindowNumber);
                            return true;
                        } else if (OpenWindowNumbers.Contains(WindowNumber)) {
                            _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:STATe OFF", WindowNumber));
                            return true;
                        } else {
                            return false;
                        }
                    }
                    /// <summary>
                    /// Iterates through all open window numbers and deletes them.
                    /// </summary>
                    public void DeleteAll()
                    {
                        List<int> allNumbers = new List<int>(OpenWindowNumbers);
                        foreach (int number in allNumbers) {
                            _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:STATe OFF", number));
                        }
                        _windows.Clear();
                    }
                    public IEnumerator<WindowClass> GetEnumerator()
                    {
                        return _windows.Values.GetEnumerator();
                    }

                    // Unused Interface Methods
                    IEnumerator IEnumerable.GetEnumerator()
                    {
                        throw new NotImplementedException();
                    }
                }

                // Variables
                private PNAX _parentPNAX;
                private bool _enable;
                private bool _visible;
                private bool _touchscreenState;
                private GridLineTypeEnum _gridLineType;

                // Properties
                /// <summary>
                /// Enable/disable all analyzer display information in all windows in the analyzer application.
                /// Marker data is not updated. More CPU time is spent making measurements instead of updating the display.
                /// </summary>
                public bool Enable
                {
                    get { return this.GetEnable(); }
                    set { this.SetEnable(value); }
                }
                /// <summary>
                /// Makes the PNA Application visible or not visible.
                /// </summary>
                public bool Visible
                {
                    get { return this.GetVisible(); }
                    set { this.SetVisible(value); }
                }
                /// <summary>
                /// Enables (true) or disables (false) the PNA-X touchscreen.
                /// </summary>
                public bool TouchscreenState
                {
                    get { return this.GetTouchscreenState(); }
                    set { this.SetTouchscreenState(value); }
                }
                /// <summary>
                /// The type of grid lines on the display.
                /// </summary>
                public GridLineTypeEnum GridLineType
                {
                    get { return this.GetGridLineType(); }
                    set { this.SetGridLineType(value); }
                }
                /// <summary>
                /// Control the windows of the PNAX.
                /// </summary>
                public WindowCollectionClass Windows
                { get; private set; }
                
                // Constructor
                internal DisplayClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                    Windows = new WindowCollectionClass(_parentPNAX);
                }

                // Private Methods
                private bool GetEnable()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("DISPlay:ENABle?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _enable = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _enable;
                }
                private void SetEnable(bool Enable)
                {
                    _enable = Enable;
                    _parentPNAX.WriteString(String.Format("DISPlay:ENABle {0}", _enable ? "ON" : "OFF"));
                }
                private bool GetVisible()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("DISPlay:VISible?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _visible = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _visible;
                }
                private void SetVisible(bool Visible)
                {
                    _visible = Visible;
                    _parentPNAX.WriteString(String.Format("DISPlay:VISible {0}", _visible ? "ON" : "OFF"));
                }
                private bool GetTouchscreenState()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:TOUChscreen:STATe?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _touchscreenState = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _touchscreenState;
                }
                private void SetTouchscreenState(bool TouchscreenState)
                {
                    _touchscreenState = TouchscreenState;
                    _parentPNAX.WriteString(String.Format("SYSTem:TOUChscreen:STATe {0}", _touchscreenState ? "ON" : "OFF"));
                }
                private GridLineTypeEnum GetGridLineType()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("DISPlay:WINDow:TRACe:GRATicule:GRID:LTYPE?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("SOL")) {
                        _gridLineType = GridLineTypeEnum.Solid;
                    } else {
                        _gridLineType = GridLineTypeEnum.Dotted;
                    }
                    return _gridLineType;
                }
                private void SetGridLineType(GridLineTypeEnum GridLineType)
                {
                    _gridLineType = GridLineType;
                    switch (_gridLineType) {
                        case GridLineTypeEnum.Solid:
                            _parentPNAX.WriteString("DISPlay:WINDow:TRACe:GRATicule:GRID:LTYPE SOLid");
                            break;
                        case GridLineTypeEnum.Dotted:
                            _parentPNAX.WriteString("DISPlay:WINDow:TRACe:GRATicule:GRID:LTYPE DOTTed");
                            break;
                    }
                }

                // Public Methods
                /// <summary>
                /// Grabs a screenshot of the display. It is recommended that the user increase
                /// the timeout of the driver to approximately 60 seconds before calling this function.
                /// The user can then reset the timeout after this function completes.
                /// </summary>
                public Bitmap GetScreenshot()
                {
                    _parentPNAX.WriteString("HCOPy:SDUMp:DATA:FORMat BMP");
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("HCOPy:SDUMp:DATA?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    return new Bitmap((Image)((new ImageConverter()).ConvertFrom((byte[])_parentPNAX.ReadIEEEBlock(IEEEBinaryType.BinaryType_UI1, true, true))));
                }

            }
            public class TriggerClass {

                // Variables
                private PNAX _parentPNAX;
                private TriggerSourceEnum _source;
                private TriggerScopeEnum _scope;
                private ExternalTriggerSlopeEnum _externalSlope;
                private ExternalTriggerTypeEnum _externalType;
                private ExternalTriggerRouteEnum _externalRoute;
                private double _globalDelay;

                // Properties
                public TriggerSourceEnum Source
                {
                    get { return this.GetSource(); }
                    set { this.SetSource(value); }
                }
                public TriggerScopeEnum Scope
                {
                    get { return this.GetScope(); }
                    set { this.SetScope(value); }
                }
                public ExternalTriggerSlopeEnum ExternalSlope
                {
                    get { return this.GetExternalSlope(); }
                    set { this.SetExternalSlope(value); }
                }
                public ExternalTriggerTypeEnum ExternalType
                {
                    get { return this.GetExternalType(); }
                    set { this.SetExternalType(value); }
                }
                public ExternalTriggerRouteEnum ExternalRoute
                {
                    get { return this.GetExternalRoute(); }
                    set { this.SetExternalRoute(value); }
                }                
                /// <summary>
                /// Global Trigger Delay in seconds.  Must be between 0 and 3 seconds.
                /// </summary>
                public double GlobalDelay
                {
                    get { return this.GetGlobalDelay(); }
                    set { this.SetGlobalDelay(value); }
                }

                // Constructor
                internal TriggerClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                }

                // Private Methods
                private TriggerSourceEnum GetSource()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:SEQuence:SOURce?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("IMM")) {
                        _source = TriggerSourceEnum.Immediate;
                    } else if (retVal.Contains("EXT")) {
                        _source = TriggerSourceEnum.External;
                    } else {
                        _source = TriggerSourceEnum.Manual;
                    }
                    return _source;
                }
                private void SetSource(TriggerSourceEnum Source)
                {
                    _source = Source;
                    switch (_source) {
                        case TriggerSourceEnum.External:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SOURce EXTernal");
                            break;
                        case TriggerSourceEnum.Immediate:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SOURce IMMediate");
                            break;
                        case TriggerSourceEnum.Manual:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SOURce MANual");
                            break;
                    }
                }
                private TriggerScopeEnum GetScope()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:SEQuence:SCOPe?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("ALL")) {
                        _scope = TriggerScopeEnum.All;
                    } else {
                        _scope = TriggerScopeEnum.Current;
                    }
                    return _scope;
                }
                private void SetScope(TriggerScopeEnum Scope)
                {
                    _scope = Scope;
                    switch (_scope) {
                        case TriggerScopeEnum.All:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SCOPe ALL");
                            break;
                        case TriggerScopeEnum.Current:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SCOPe CURRent");
                            break;
                    }
                }
                private ExternalTriggerSlopeEnum GetExternalSlope()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:SEQuence:SLOPe?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("POS")) {
                        _externalSlope = ExternalTriggerSlopeEnum.Positive;
                    } else {
                        _externalSlope = ExternalTriggerSlopeEnum.Negative;
                    }
                    return _externalSlope;
                }
                private void SetExternalSlope(ExternalTriggerSlopeEnum ExternalSlope)
                {
                    _externalSlope = ExternalSlope;
                    switch (_externalSlope) {
                        case ExternalTriggerSlopeEnum.Positive:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SLOPe POSitive");
                            break;
                        case ExternalTriggerSlopeEnum.Negative:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SLOPe NEGative");
                            break;
                    }
                }
                private ExternalTriggerTypeEnum GetExternalType()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:SEQuence:TYPE?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("EDGE")) {
                        _externalType = ExternalTriggerTypeEnum.Edge;
                    } else {
                        _externalType = ExternalTriggerTypeEnum.Level;
                    }
                    return _externalType;
                }
                private void SetExternalType(ExternalTriggerTypeEnum ExternalType)
                {
                    _externalType = ExternalType;
                    switch (_externalType) {
                        case ExternalTriggerTypeEnum.Edge:
                            _parentPNAX.WriteString("TRIGger:SEQuence:TYPE EDGE");
                            break;
                        case ExternalTriggerTypeEnum.Level:
                            _parentPNAX.WriteString("TRIGger:SEQuence:TYPE LEVel");
                            break;
                    }
                }
                private double GetGlobalDelay()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:DELay?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _globalDelay = (double)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _globalDelay;
                }
                private void SetGlobalDelay(double GlobalDelay)
                {
                    if (GlobalDelay < 0 || 3 < GlobalDelay)
                        throw new System.ArgumentException("Global Delay must be between 0 and 3 seconds.", "GlobalDelay");

                    _globalDelay = GlobalDelay;
                    _parentPNAX.WriteString(String.Format("TRIGger:DELay {0}", _globalDelay));
                }
                private ExternalTriggerRouteEnum GetExternalRoute()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:SEQuence:ROUTE:INPut?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("MAIN")) {
                        _externalRoute = ExternalTriggerRouteEnum.Main;
                    } else if (retVal.Contains("MATH")) {
                        _externalRoute = ExternalTriggerRouteEnum.Math;
                    } else {
                        _externalRoute = ExternalTriggerRouteEnum.Pulse3;
                    }
                    return _externalRoute;
                }
                private void SetExternalRoute(ExternalTriggerRouteEnum ExternalRoute)
                {
                    _externalRoute = ExternalRoute;
                    switch (_externalRoute) {
                        case ExternalTriggerRouteEnum.Main:
                            _parentPNAX.WriteString("TRIGger:SEQuence:ROUTE:INPut MAIN");
                            break;
                        case ExternalTriggerRouteEnum.Math:
                            _parentPNAX.WriteString("TRIGger:SEQuence:ROUTE:INPut MATH");
                            break;
                        case ExternalTriggerRouteEnum.Pulse3:
                            _parentPNAX.WriteString("TRIGger:SEQuence:ROUTE:INPut PULSE3");
                            break;
                    }
                }

                // Public Methods
                /// <summary>
                /// Stops all sweeps - then resumes per current trigger settings.
                /// </summary>
                public void Abort()
                {
                    _parentPNAX.WriteString("ABORt");
                }
                /// <summary>
                /// Places all channels in hold mode.
                /// </summary>
                public void HoldAllChannels()
                {
                    _parentPNAX.WriteString("SYSTem:CHANnels:HOLD");
                }
                /// <summary>
                /// Resumes the trigger mode of all channels that was in effect before sending HoldAllChannels()
                /// </summary>
                public void ResumeAllChannels()
                {
                    _parentPNAX.WriteString("SYSTem:CHANnels:RESume");
                }
            }
            public class OutputClass {

                // Variables
                private PNAX _parentPNAX;
                private bool _rfSourceState;
                private bool _noiseSourceState;

                // Properties
                /// <summary>
                /// Turns RF power from the source on or off.
                /// </summary>
                public bool RFSourceState
                {
                    get { return this.GetRFSourceState(); }
                    set { this.SetRFSourceState(value); }
                }
                /// <summary>
                /// Sets and reads the noise source (28V) on or off.
                /// </summary>
                public bool NoiseSourceState
                {
                    get { return this.GetNoiseSourceState(); }
                    set { this.SetNoiseSourceState(value); }
                }

                // Constructor
                internal OutputClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                }

                // Private Methods
                private bool GetRFSourceState()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("OUTPut:STATe?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _rfSourceState = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _rfSourceState;
                }
                private void SetRFSourceState(bool RFSourceState)
                {
                    _rfSourceState = RFSourceState;
                    _parentPNAX.WriteString(String.Format("OUTPut:STATe {0}", _rfSourceState));
                }
                private bool GetNoiseSourceState()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("OUTPut:MANual:NOISe:STATe?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _noiseSourceState = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _noiseSourceState;
                }
                private void SetNoiseSourceState(bool NoiseSourceState)
                {
                    _noiseSourceState = NoiseSourceState;
                    _parentPNAX.WriteString(String.Format("OUTPut:MANual:NOISe:STATe {0}", _noiseSourceState));
                }

            }
            public class ChannelClass {

                // Nested Classes
                public class PathClass {

                    // Nested Classes
                    public class ElementClass {

                        // Variables
                        private ChannelClass _parentChannel;
                        private string _setting;

                        // Properties
                        /// <summary>
                        /// The Name of this element.
                        /// </summary>
                        public string Name
                        { get; private set; }
                        /// <summary>
                        /// The current setting of this element.
                        /// </summary>
                        public string Setting
                        {
                            get { return this.GetSetting(); }
                            set { this.SetSetting(value); }
                        }
                        /// <summary>
                        /// The valid settings that can be used with this element.
                        /// </summary>
                        public string[] ValidSettings
                        {
                            get { return this.GetValidSettings(); }
                        }

                        // Constructor
                        internal ElementClass(string Name, ChannelClass ParentChannel)
                        {
                            this.Name = Name;
                            _parentChannel = ParentChannel;
                        }

                        // Private Methods
                        public string[] GetValidSettings()
                        {
                            object[] retVals;
                            List<string> settings = new List<string>();
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:ELEMent:VALue:CATalog? '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVals = (object[])_parentChannel._parentPNAX.ReadList(IEEEASCIIType.ASCIIType_Any, ",;");
                            foreach (object retVal in retVals) {
                                settings.Add(Convert.ToString(retVal));
                            }
                            return settings.ToArray();
                        }
                        public string GetSetting()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:ELEMent:STATe? '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _setting = _parentChannel._parentPNAX.ReadString();
                            return _setting;
                        }
                        public void SetSetting(string Setting)
                        {
                            _setting = Setting;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:ELEMent:STATe '{1}', '{2}'", _parentChannel.Number, Name, _setting));
                        }

                    }
                    public class ElementCollectionClass : IEnumerable<ElementClass> {

                        // Variables
                        private ChannelClass _parentChannel;
                        private Dictionary<string, ElementClass> _elements;

                        // Properties
                        /// <summary>
                        /// Number of configurable elements.
                        /// </summary>
                        public int Count
                        {
                            get { return this._elements.Count(); }
                        }
                        /// <summary>
                        /// The names of the elemenets available to configure.
                        /// </summary>
                        public string[] Names
                        {
                            get { return this._elements.Keys.ToArray(); }
                        }

                        // Constructor
                        internal ElementCollectionClass(ChannelClass ParentChannel);

                        // Indexer
                        public ElementClass this[string Name]
                        {
                            get
                            {
                                if (!_elements.ContainsKey(Name))
                                    throw new IndexOutOfRangeException("Element does not exist.");
                                return _elements[Name];
                            }
                        }
                        
                        // Internal Methods
                        internal void FindElements()
                        {
                            object[] retVals;
                            string elementName;
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:ELEMent:CATalog?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVals = (object[])_parentChannel._parentPNAX.ReadList(IEEEASCIIType.ASCIIType_Any, ",;");
                            _elements.Clear();
                            foreach (object retVal in retVals) {
                                elementName = Convert.ToString(retVal);
                                _elements.Add(elementName, new ElementClass(elementName, _parentChannel));
                            }
                        }

                        // Public Methods
                        public IEnumerator<ElementClass> GetEnumerator()
                        {
                            return _elements.Values.GetEnumerator();
                        }

                        // Unused Methods
                        IEnumerator IEnumerable.GetEnumerator()
                        {
                            throw new NotImplementedException();
                        }
                    }

                    // Variables
                    private ChannelClass _parentChannel;

                    // Properties
                    /// <summary>
                    /// Returns the current configuration name only if NO element settings had been changed since last selecting or storing a configuration.
                    /// </summary>
                    public string ConfigurationName
                    {
                        get { return this.GetConfigurationName(); }
                    }
                    /// <summary>
                    /// Configure elements on this channel.
                    /// </summary>
                    public ElementCollectionClass Elements
                    { get; private set; }

                    // Constructor
                    internal PathClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                        Elements = new ElementCollectionClass(_parentChannel);
                    }

                    // Private Methods
                    private string GetConfigurationName()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:NAME", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        return _parentChannel._parentPNAX.ReadString();
                    }

                    // Public Methods
                    /// <summary>
                    /// Copies the mechanical switch and attenuator settings from the specified channel to this channel.
                    /// </summary>
                    /// <param name="CopyFromChannelNumber"></param>
                    public void Copy(int CopyFromChannelNumber)
                    {
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:COPY {1}", _parentChannel.Number, CopyFromChannelNumber));
                        _parentChannel.Path.Elements.FindElements();
                    }
                    /// <summary>
                    /// Loads the named configuration onto this channel.
                    /// Loading a stored configuration will over-write MANY RF and IF path configuration settings.
                    /// Make your measurement settings AFTER recalling a stored configuration, NOT before.
                    /// </summary>
                    /// <param name="ConfigurationName"></param>
                    public void LoadConfiguration(string ConfigurationName)
                    {
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:SELect '{1}'", _parentChannel.Number, ConfigurationName));
                        _parentChannel.Path.Elements.FindElements();
                    }
                    /// <summary>
                    /// Saves the path configuration currently associated with the channel to the specified ConfigurationName.
                    /// </summary>
                    public void StoreConfiguration(string ConfigurationName)
                    {
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:PATH:CONFig:STORe '{1}'", _parentChannel.Number, ConfigurationName));
                    }

                }
                public class AveragingClass {

                    // Variables
                    private ChannelClass _parentChannel;
                    private bool _enabled;
                    private int _count;
                    private AveragingModeEnum _mode;

                    // Properties
                    /// <summary>
                    /// Enable or disable trace averaging on this channel.
                    /// </summary>
                    public bool Enabled
                    {
                        get { return this.GetEnabled(); }
                        set { this.SetEnabled(value); }
                    }
                    /// <summary>
                    /// Number of measurement to combine for an average.
                    /// Must be between 1 and 65536.
                    /// </summary>
                    public int Count
                    {
                        get { return this.GetCount(); }
                        set { this.SetCount(value); }
                    }
                    /// <summary>
                    /// Averaging mode, point or sweep.
                    /// </summary>
                    public AveragingModeEnum Mode
                    {
                        get { return this.GetMode(); }
                        set { this.SetMode(value); }
                    }

                    // Constructor
                    internal AveragingClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                    }

                    // Private Methods
                    private bool GetEnabled()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:STATe?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _enabled = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _enabled;
                    }
                    private void SetEnabled(bool Enabled)
                    {
                        _enabled = Enabled;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:STATe {1}", _parentChannel.Number, _enabled ? "ON" : "OFF"));
                    }
                    private int GetCount()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:COUNt?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _count = (int)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _count;
                    }
                    private void SetCount(int Count)
                    {
                        if (Count < 1 || 65536 < Count)
                            throw new System.ArgumentOutOfRangeException("Count must be between 1 and 65536.", "Count");

                        _count = Count;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:COUNt {1}", _parentChannel.Number, _count));
                    }
                    private AveragingModeEnum GetMode()
                    {
                        string retVal;
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:MODE?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = _parentChannel._parentPNAX.ReadString();
                        if (retVal.Contains("POIN")) {
                            _mode = AveragingModeEnum.Point;
                        } else {
                            _mode = AveragingModeEnum.Sweep;
                        }
                        return _mode;
                    }
                    private void SetMode(AveragingModeEnum Mode)
                    {
                        _mode = Mode;
                        switch (_mode) {
                            case AveragingModeEnum.Point:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:MODE POINt", _parentChannel.Number));
                                break;
                            case AveragingModeEnum.Sweep:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:MODE SWEEP", _parentChannel.Number));
                                break;
                        }
                    }

                    // Public Methods
                    /// <summary>
                    /// Clears and restarts averaging of the measurement data.
                    /// Does NOT apply to point averaging.
                    /// </summary>
                    public void Clear()
                    {
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:CLEar", _parentChannel.Number));
                    }

                }
                public class FrequencyClass {
                    
                    // Variables
                    private ChannelClass _parentChannel;
                    private double _center;
                    private double _span;
                    private double _start;
                    private double _stop;
                    private double _cw;
                    private double _minimumCenter;
                    private double _maximumCenter;
                    private double _minimumSpan;
                    private double _maximumSpan;
                    private double _minimumStart;
                    private double _maximumStart;
                    private double _minimumStop;
                    private double _maximumStop;
                    private double _minimumCW;
                    private double _maximumCW;

                    // Properties
                    /// <summary>
                    /// Center frequency of the channel.
                    /// </summary>
                    public double Center
                    {
                        get { return this.GetCenter(); }
                        set { this.SetCenter(value); }
                    }
                    /// <summary>
                    /// Frequency span of the channel.
                    /// </summary>
                    public double Span
                    {
                        get { return this.GetSpan(); }
                        set { this.SetSpan(value); }
                    }
                    /// <summary>
                    /// Start frequency of the channel
                    /// </summary>
                    public double Start
                    {
                        get { return this.GetStart(); }
                        set { this.SetStart(value); }
                    }
                    /// <summary>
                    /// Stop frequency of the channel.
                    /// </summary>
                    public double Stop
                    {
                        get { return this.GetStop(); }
                        set { this.SetStop(value); }
                    }
                    /// <summary>
                    /// CW frequency of the channel. Valid when the channel is put into CW Sweep mode.
                    /// </summary>
                    public double CW
                    {
                        get { return this.GetCW(); }
                        set { this.SetCW(value); }
                    }
                    /// <summary>
                    /// Minimum allowed center frequency.
                    /// </summary>
                    public double MinimumCenter
                    {
                        get { return this.GetMinimumCenter(); }
                    }
                    /// <summary>
                    /// Maximum allowed center frequency.
                    /// </summary>
                    public double MaximumCenter
                    {
                        get { return this.GetMaximumCenter(); }
                    }
                    /// <summary>
                    /// Minimum allowed span.
                    /// </summary>
                    public double MinimumSpan
                    {
                        get { return this.GetMinimumSpan(); }
                    }
                    /// <summary>
                    /// Maximum allowed span.
                    /// </summary>
                    public double MaximumSpan
                    {
                        get { return this.GetMaximumSpan(); }
                    }
                    /// <summary>
                    /// Minimum allowed start frequency.
                    /// </summary>
                    public double MinimumStart
                    {
                        get { return this.GetMinimumStart(); }
                    }
                    /// <summary>
                    /// Maximum allowed start frequency.
                    /// </summary>
                    public double MaximumStart
                    {
                        get { return this.GetMaximumStart(); }
                    }
                    /// <summary>
                    /// Minimum allowed stop frequency.
                    /// </summary>
                    public double MinimumStop
                    {
                        get { return this.GetMinimumStop(); }
                    }
                    /// <summary>
                    /// Maximum allowed stop frequency.
                    /// </summary>
                    public double MaximumStop
                    {
                        get { return this.GetMaximumStop(); }
                    }
                    /// <summary>
                    /// Minimum allowed cw frequency.
                    /// </summary>
                    public double MinimumCW
                    {
                        get { return this.GetMinimumCW(); }
                    }
                    /// <summary>
                    /// Maximum allowed cw frequency.
                    /// </summary>
                    public double MaximumCW
                    {
                        get { return this.GetMaximumCW(); }
                    }

                    // Constructor
                    internal FrequencyClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                    }

                    // Private Methods
                    private double GetCenter()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _center = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _center;
                    }
                    private void SetCenter(double Center)
                    {
                        _center = Center;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer {1}", _parentChannel.Number, _center));
                    }
                    private double GetSpan()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _span = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _span;
                    }
                    private void SetSpan(double Span)
                    {
                        _span = Span;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN {1}", _parentChannel.Number, _span));
                    }
                    private double GetStart()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _start = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _start;
                    }
                    private void SetStart(double Start)
                    {
                        _start = Start;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt {1}", _parentChannel.Number, _start));
                    }
                    private double GetStop()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _stop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _stop;
                    }
                    private void SetStop(double Stop)
                    {
                        _stop = Stop;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP {1}", _parentChannel.Number, _stop));
                    }
                    private double GetCW()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CW?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _cw = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _cw;
                    }
                    private void SetCW(double CW)
                    {
                        _cw = CW;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CW {1}", _parentChannel.Number, _cw));
                    }
                    private double GetMinimumCenter()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumCenter = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumCenter;
                    }
                    private double GetMaximumCenter()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumCenter = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumCenter;
                    }
                    private double GetMinimumSpan()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumSpan = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumSpan;
                    }
                    private double GetMaximumSpan()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumSpan = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumSpan;
                    }
                    private double GetMinimumStart()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumStart;
                    }
                    private double GetMaximumStart()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumStart;
                    }
                    private double GetMinimumStop()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumStop;
                    }
                    private double GetMaximumStop()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumStop;
                    }
                    private double GetMinimumCW()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CW? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumCW = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumCW;
                    }
                    private double GetMaximumCW()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CW? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumCW = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumCW;
                    }

                }
                public class IFBandwidthClass {

                    // Variables
                    private ChannelClass _parentChannel;
                    private double _resolution;
                    private bool _track;

                    // Properties
                    /// <summary>
                    /// IF Bandwidth of the digital filter.
                    /// </summary>
                    public double Resolution
                    {
                        get { return this.GetResolution(); }
                        set { this.SetResolution(value); }
                    }
                    /// <summary>
                    /// Whether or not to reduce IF Bandwidth at low frequencies.
                    /// </summary>
                    public bool Track
                    {
                        get { return this.GetTrack(); }
                        set { this.SetTrack(value); }
                    }

                    // Constructor
                    internal IFBandwidthClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                    }

                    // Private Methods
                    private double GetResolution()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:BANDwidth:RESolution?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _resolution = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _resolution;
                    }
                    private void SetResolution(double Resolution)
                    {
                        _resolution = Resolution;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:BANDwidth:RESolution {1}", _parentChannel.Number, _resolution));
                    }
                    private bool GetTrack()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:BANDwidth:TRACk?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _track = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _track;
                    }
                    private void SetTrack(bool Track)
                    {
                        _track = Track;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:BANDwidth:TRACk {1}", _parentChannel.Number, _track ? "ON" : "OFF"));
                    }

                }
                public class SourceClass {

                    // Nested Classes
                    public class PortClass {

                        // Nested Classes
                        public class PowerClass {

                            // Variables
                            private ChannelClass _parentChannel;
                            private string _portName;
                            private double _amplitude;
                            private double _start;
                            private double _stop;
                            private double _minimumAmplitude;
                            private double _maximumAmplitude;
                            private double _minimumStart;
                            private double _maximumStart;
                            private double _minimumStop;
                            private double _maximumStop;

                            // Properties
                            /// <summary>
                            /// RF Power output level
                            /// </summary>
                            public double Amplitude
                            {
                                get { return this.GetAmplitude(); }
                                set { this.SetAmplitude(value); }
                            }
                            /// <summary>
                            /// Power sweep start power in dBm for ALL ports being used by the channel.
                            /// Can set individual port powers by setting Channel.Power.Coupling = false
                            /// and Channel.Ports["PortName"].Power.Start = value.
                            /// </summary>
                            public double Start
                            {
                                get { return this.GetStart(); }
                                set { this.SetStart(value); }
                            }
                            /// <summary>
                            /// Power sweep stop power in dBm for ALL ports being used by the channel.
                            /// Can set individual port powers by setting Channel.Power.Coupling = false
                            /// and Channel.Ports["PortName"].Power.Stop = value.
                            /// </summary>
                            public double Stop
                            {
                                get { return this.GetStop(); }
                                set { this.SetStop(value); }
                            }
                            /// <summary>
                            /// Minimum RF Power output level
                            /// </summary>
                            public double MinimumAmplitude
                            {
                                get { return this.GetMinimumAmplitude(); }
                            }
                            /// <summary>
                            /// Maximum RF Power output level
                            /// </summary>
                            public double MaximumAmplitude
                            {
                                get { return this.GetMaximumAmplitude(); }
                            }
                            /// <summary>
                            /// Minimum start power for power sweep.
                            /// </summary>
                            public double MinimumStart
                            {
                                get { return this.GetMinimumStart(); }
                            }
                            /// <summary>
                            /// Maximum start power for power sweep.
                            /// </summary>
                            public double MaximumStart
                            {
                                get { return this.GetMaximumStart(); }
                            }
                            /// <summary>
                            /// Minimum stop power for power sweep.
                            /// </summary>
                            public double MinimumStop
                            {
                                get { return this.GetMinimumStop(); }
                            }
                            /// <summary>
                            /// Maximum stop power for power sweep.
                            /// </summary>
                            public double MaximumStop
                            {
                                get { return this.GetMaximumStop(); }
                            }

                            // Constructor
                            internal PowerClass(string PortName, ChannelClass ParentChannel)
                            {
                                _parentChannel = ParentChannel;
                                _portName = PortName;
                            }

                            // Private Methods
                            private double GetAmplitude()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:IMMediate:AMPLitude? '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _amplitude = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _amplitude;
                            }
                            private void SetAmplitude(double Amplitude)
                            {
                                _amplitude = Amplitude;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:IMMediate:AMPLitude {1}, '{2}'", _parentChannel.Number, _amplitude, _portName));
                            }
                            private double GetStart()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STARt? '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _start = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _start;
                            }
                            private void SetStart(double Start)
                            {
                                _start = Start;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STARt {1}, '{2}'", _parentChannel.Number, _start, _portName));
                            }
                            private double GetStop()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STOP? '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _stop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _stop;
                            }
                            private void SetStop(double Stop)
                            {
                                _stop = Stop;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STOP {1}, '{2}'", _parentChannel.Number, _stop, _portName));
                            }
                            private double GetMinimumAmplitude()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:IMMediate:AMPLitude? MINimum, '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _minimumAmplitude = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _minimumAmplitude;
                            }
                            private double GetMaximumAmplitude()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:IMMediate:AMPLitude? MAXimum, '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _maximumAmplitude = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _maximumAmplitude;
                            }
                            private double GetMinimumStart()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STARt? MINimum, '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _minimumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _minimumStart;
                            }
                            private double GetMaximumStart()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STARt? MAXimum, '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _maximumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _maximumStart;
                            }
                            private double GetMinimumStop()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STOP? MINimum, '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _minimumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _minimumStop;
                            }
                            private double GetMaximumStop()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:PORT:STOP? MAXimum, '{1}'", _parentChannel.Number, _portName));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _maximumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _maximumStop;
                            }

                        }

                        // Variables
                        private ChannelClass _parentChannel;
                        private PortModeEnum _mode;
                        private PortALCModeEnum _alcMode;
                        private bool _autoAttenuation;
                        private double _attenuation;
                        private double _minimumAttenuation;
                        private double _maximumAttenuation;

                        // Properties
                        /// <summary>
                        /// The name associated with this port.
                        /// </summary>
                        public string Name
                        { get; private set; }
                        /// <summary>
                        /// The number associated with this port (if the number exists).
                        /// If not, then returns -1.
                        /// </summary>
                        public int Number
                        { get; internal set; }
                        /// <summary>
                        /// State of the source for this port.
                        /// Auto = Source power is turned on when required for a measurement (default).
                        /// On = Source power is on regardless of the measurement.
                        /// Off = Source power is always off regardless of the measurement.
                        /// NoControl = Do not send OFF commands to the external sources. If an external source is in the OFF state,
                        /// this option is used to stop sending OFF commands to the external source to increase sweep speed.
                        /// </summary>
                        public PortModeEnum Mode
                        {
                            get { return this.GetMode(); }
                            set { this.SetMode(value); }
                        }
                        /// <summary>
                        /// Controls the ALC Mode for this port on this channel.
                        /// </summary>
                        public PortALCModeEnum ALCMode
                        {
                            get { return this.GetALCMode(); }
                            set { this.SetALCMode(value); }
                        }
                        /// <summary>
                        /// Automatic attenuation control on or off.
                        /// </summary>
                        public bool AutoAttenuation
                        {
                            get { return this.GetAutoAttenuation(); }
                            set { this.SetAutoAttenuation(value); }
                        }
                        /// <summary>
                        /// Attenuation Level in dB.
                        /// If ports power is coupled, this will set all other ports as well.
                        /// PNA-X sets attenuation in quantized step sizes. If an invalid attenuation number
                        /// is entered, the PNA will select the next lower valid value.
                        /// </summary>
                        public double Attenuation
                        {
                            get { return this.GetAttenuation(); }
                            set { this.SetAttenuation(value); }
                        }
                        /// <summary>
                        /// Minimum attenuation Level in dB.
                        /// </summary>
                        public double MinimumAttenuation
                        {
                            get { return this.GetMinimumAttenuation(); }
                        }
                        /// <summary>
                        /// Maximum attenuation Level in dB.
                        /// </summary>
                        public double MaximumAttenuation
                        {
                            get { return this.GetMaximumAttenuation(); }
                        }
                        /// <summary>
                        /// Control port specific power settings.
                        /// </summary>
                        public PowerClass Power
                        { get; private set; }
                        /// <summary>
                        /// Control source power correction settings.
                        /// Use this to perform source power calibrations.
                        /// </summary>
                        public CorrectionClass Correction
                        { get; private set; }

                        // Constructor
                        internal PortClass(string Name, int Number, ChannelClass ParentChannel)
                        {
                            _parentChannel = ParentChannel;
                            this.Name = Name;
                            this.Number = Number;
                            Power = new PowerClass(this.Name, _parentChannel);
                        }

                        // Private Methods
                        private PortModeEnum GetMode()
                        {
                            string retVal;
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:MODE? '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVal = _parentChannel._parentPNAX.ReadString();
                            if (retVal.Contains("AUTO")) {
                                _mode = PortModeEnum.Auto;
                            } else if (retVal.Contains("ON")) {
                                _mode = PortModeEnum.On;
                            } else if (retVal.Contains("OFF")) {
                                _mode = PortModeEnum.Off;
                            } else {
                                _mode = PortModeEnum.NoControl;
                            }
                            return _mode;
                        }
                        private void SetMode(PortModeEnum Mode)
                        {
                            _mode = Mode;
                            switch (_mode) {
                                case PortModeEnum.Auto:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:MODE AUTO, '{1}'", _parentChannel.Number, Name));
                                    break;
                                case PortModeEnum.On:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:MODE ON, '{1}'", _parentChannel.Number, Name));
                                    break;
                                case PortModeEnum.Off:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:MODE OFF, '{1}'", _parentChannel.Number, Name));
                                    break;
                                case PortModeEnum.NoControl:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:MODE NOCTL, '{1}'", _parentChannel.Number, Name));
                                    break;
                            }
                        }
                        private PortALCModeEnum GetALCMode()
                        {
                            string retVal;
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ALC:MODE? '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVal = _parentChannel._parentPNAX.ReadString();
                            if (retVal.Contains("INT")) {
                                _alcMode = PortALCModeEnum.Internal;
                            } else {
                                _alcMode = PortALCModeEnum.OpenLoop;
                            }
                            return _alcMode;
                        }
                        private void SetALCMode(PortALCModeEnum ALCMode)
                        {
                            _alcMode = ALCMode;
                            switch (_alcMode) {
                                case PortALCModeEnum.Internal:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ALC:MODE INTernal, '{1}'", _parentChannel.Number, Name));
                                    break;
                                case PortALCModeEnum.OpenLoop:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ALC:MODE OPENloop, '{1}'", _parentChannel.Number, Name));
                                    break;
                            }
                        }
                        private bool GetAutoAttenuation()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ATTenuation:AUTO? '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _autoAttenuation = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _autoAttenuation;
                        }
                        private void SetAutoAttenuation(bool AutoAttenuation)
                        {
                            _autoAttenuation = AutoAttenuation;
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ATTenuation:AUTO {1}, '{2}'", _parentChannel.Number, _autoAttenuation ? "ON" : "OFF", Name));
                        }
                        private double GetAttenuation()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ATTenuation? '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _attenuation = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _attenuation;
                        }
                        private void SetAttenuation(double Attenuation)
                        {
                            _attenuation = Attenuation;
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ATTenuation {1}, '{2}'", _parentChannel.Number, _attenuation, Name));
                        }
                        private double GetMinimumAttenuation()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ATTenuation? MINimum, '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _minimumAttenuation = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _minimumAttenuation;
                        }
                        private double GetMaximumAttenuation()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:ATTenuation? MAXimum, '{1}'", _parentChannel.Number, Name));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _maximumAttenuation = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _maximumAttenuation;
                        }

                    }
                    public class PortCollectionClass : IEnumerable<PortClass> {

                        // Nested Classes
                        public class PowerClass {

                            // Variables
                            private ChannelClass _parentChannel;
                            private bool _coupling;
                            private double _center;
                            private double _span;
                            private double _start;
                            private double _stop;
                            private double _slope;
                            private bool _slopeState;
                            private double _minimumStart;
                            private double _maximumStart;
                            private double _minimumStop;
                            private double _maximumStop;

                            // Properties
                            /// <summary>
                            /// Port Power Coupling on or off.
                            /// </summary>
                            public bool Coupling
                            {
                                get { return this.GetCoupling(); }
                                set { this.SetCoupling(value); }
                            }
                            /// <summary>
                            /// Power sweep center power in dBm. Must also set Channel.Sweep.Type = Power.
                            /// </summary>
                            public double Center
                            {
                                get { return this.GetCenter(); }
                                set { this.SetCenter(value); }
                            }
                            /// <summary>
                            /// Power sweep span power in dBm. Must also set Channel.Sweep.Type = Power
                            /// </summary>
                            public double Span
                            {
                                get { return this.GetSpan(); }
                                set { this.SetSpan(value); }
                            }
                            /// <summary>
                            /// Power sweep start power in dBm for ALL ports being used by the channel.
                            /// Can set individual port powers by setting Channel.Power.Coupling = false
                            /// and Channel.Ports["PortName"].Power.Start = value.
                            /// </summary>
                            public double Start
                            {
                                get { return this.GetStart(); }
                                set { this.SetStart(value); }
                            }
                            /// <summary>
                            /// Power sweep stop power in dBm for ALL ports being used by the channel.
                            /// Can set individual port powers by setting Channel.Power.Coupling = false
                            /// and Channel.Ports["PortName"].Power.Stop = value.
                            /// </summary>
                            public double Stop
                            {
                                get { return this.GetStop(); }
                                set { this.SetStop(value); }
                            }
                            /// <summary>
                            /// RF Power slope in dB/GHz.
                            /// Must be between -2 and 2. (0 = no slope).
                            /// </summary>
                            public double Slope
                            {
                                get { return this.GetSlope(); }
                                set { this.SetSlope(value); }
                            }
                            /// <summary>
                            /// Turn on or off power slope.
                            /// </summary>
                            public bool SlopeState
                            {
                                get { return this.GetSlopeState(); }
                                set { this.SetSlopeState(value); }
                            }
                            /// <summary>
                            /// Minimum start power for power sweep.
                            /// </summary>
                            public double MinimumStart
                            {
                                get { return this.GetMinimumStart(); }
                            }
                            /// <summary>
                            /// Maximum start power for power sweep.
                            /// </summary>
                            public double MaximumStart
                            {
                                get { return this.GetMaximumStart(); }
                            }
                            /// <summary>
                            /// Minimum stop power for power sweep.
                            /// </summary>
                            public double MinimumStop
                            {
                                get { return this.GetMinimumStop(); }
                            }
                            /// <summary>
                            /// Maximum stop power for power sweep.
                            /// </summary>
                            public double MaximumStop
                            {
                                get { return this.GetMaximumStop(); }
                            }

                            // Constructor
                            internal PowerClass(ChannelClass ParentChannel)
                            {
                                _parentChannel = ParentChannel;
                            }

                            // Private Methods
                            private bool GetCoupling()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:COUPle?", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _coupling = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                                return _coupling;
                            }
                            private void SetCoupling(bool Coupling)
                            {
                                _coupling = Coupling;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:COUPle {1}", _parentChannel.Number, _coupling ? "ON" : "OFF"));
                            }
                            private double GetCenter()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:CENTer?", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _center = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _center;
                            }
                            private void SetCenter(double Center)
                            {
                                _center = Center;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:CENTer {1}", _parentChannel.Number, _center));
                            }
                            private double GetSpan()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:SPAN?", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _span = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _span;
                            }
                            private void SetSpan(double Span)
                            {
                                _span = Span;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:SPAN {1}", _parentChannel.Number, _span));
                            }
                            private double GetStart()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STARt?", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _start = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _start;
                            }
                            private void SetStart(double Start)
                            {
                                _start = Start;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STARt {1}", _parentChannel.Number, _start));
                            }
                            private double GetStop()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STOP?", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _stop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _stop;
                            }
                            private void SetStop(double Stop)
                            {
                                _stop = Stop;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STOP {1}", _parentChannel.Number, _stop));
                            }
                            private double GetSlope()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:SLOPe?", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _slope = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _slope;
                            }
                            private void SetSlope(double Slope)
                            {
                                _slope = Slope;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:SLOPe {1}", _parentChannel.Number, _slope));
                            }
                            private bool GetSlopeState()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:SLOPe:STATe?", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _slopeState = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                                return _slopeState;
                            }
                            private void SetSlopeState(bool SlopeState)
                            {
                                _slopeState = SlopeState;
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:LEVel:SLOPe:STATe {1}", _parentChannel.Number, _slopeState ? "ON" : "OFF"));
                            }
                            private double GetMinimumStart()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STARt? MINimum", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _minimumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _minimumStart;
                            }
                            private double GetMaximumStart()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STARt? MAXimum", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _maximumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _maximumStart;
                            }
                            private double GetMinimumStop()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STOP? MINimum", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _minimumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _minimumStop;
                            }
                            private double GetMaximumStop()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:POWer:STOP? MAXimum", _parentChannel.Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _maximumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                                return _maximumStop;
                            }

                        }

                        // Variables
                        private ChannelClass _parentChannel;
                        private Dictionary<string, PortClass> _portsByName;
                        private Dictionary<int, PortClass> _portsByNumber;

                        // Properties
                        /// <summary>
                        /// Number of ports that can be controlled by this channel.
                        /// </summary>
                        public int Count
                        {
                            get { return this._portsByName.Count; }
                        }
                        /// <summary>
                        /// Names of the ports that can be controlled.
                        /// </summary>
                        public string[] Names
                        {
                            get { return this._portsByName.Keys.ToArray(); }
                        }
                        /// <summary>
                        /// Numbers of the ports that can be controlled.
                        /// Not all ports have numbers (i.e. an external source port only has a name). So this
                        /// array will be less than or equal to in length to the Names array.
                        /// </summary>
                        public int[] Numbers
                        {
                            get { return this._portsByNumber.Keys.ToArray(); }
                        }
                        /// <summary>
                        /// Control power settings of all ports on this channel.
                        /// </summary>
                        public PowerClass Power
                        { get; private set; }

                        // Constructor
                        internal PortCollectionClass(ChannelClass ParentChannel)
                        {
                            _parentChannel = ParentChannel;
                            _portsByName = new Dictionary<string, PortClass>();
                            Power = new PowerClass(_parentChannel);
                        }

                        // Indexer
                        /// <summary>
                        /// Get the port by name.
                        /// </summary>
                        public PortClass this[string Name]
                        {
                            get
                            {
                                if (!_portsByName.ContainsKey(Name))
                                    throw new IndexOutOfRangeException("Port name does not exist for this channel.");

                                return _portsByName[Name];
                            }
                        }
                        /// <summary>
                        /// Get the port by number.
                        /// </summary>
                        public PortClass this[int Number]
                        {
                            get
                            {
                                if (!_portsByNumber.ContainsKey(Number))
                                    throw new IndexOutOfRangeException("Port number does not exist for this channel.");

                                return _portsByNumber[Number];
                            }
                        }

                        // Internal Methods
                        internal void FindPorts()
                        {
                            object[] retVals;
                            string portName;
                            int portNumber;
                            string[] internalPortNames = _parentChannel._parentPNAX.Capabilities.FindInternalPortNames();
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SOURce{0}:CATalog?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVals = (object[])_parentChannel._parentPNAX.ReadList(IEEEASCIIType.ASCIIType_Any, ",;");
                            _portsByName.Clear();
                            _portsByNumber.Clear();
                            foreach (object retVal in retVals) {
                                portName = Convert.ToString(retVal);
                                _portsByName.Add(portName, new PortClass(portName, -1, _parentChannel));
                                if (internalPortNames.Contains(portName)) {
                                    portNumber = _parentChannel._parentPNAX.Capabilities.ConvertPortNameToNumber(portName);
                                    _portsByNumber.Add(portNumber, _portsByName[portName]);
                                    _portsByNumber[portNumber].Number = portNumber;
                                }
                            }
                        }

                        // Public Methods
                        public IEnumerator<PortClass> GetEnumerator()
                        {
                            return _portsByName.Values.GetEnumerator();
                        }

                        // Unused Interface Methods
                        IEnumerator IEnumerable.GetEnumerator()
                        {
                            throw new NotImplementedException();
                        }
                    }

                    // Variables
                    private ChannelClass _parentChannel;

                    // Properties
                    /// <summary>
                    /// Ports attached to this channel.
                    /// </summary>
                    public PortCollectionClass Ports
                    { get; private set; }

                    // Constructor
                    internal SourceClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                        Ports = new PortCollectionClass(_parentChannel);
                    }

                }
                public class SweepClass {

                    // Nested Classes
                    public class DwellClass {
                        
                        // Variables
                        private ChannelClass _parentChannel;
                        private double _time;
                        private bool _auto;
                        private double _sweepDelay;
                        private double _minimumTime;
                        private double _maximumTime;
                        private double _minimumSweepDelay;
                        private double _maximumSweepDelay;

                        // Properties
                        /// <summary>
                        /// Dwell time between each sweep point.
                        /// Only available when SweepClass.Generation is set to Stepped.
                        /// </summary>
                        public double Time
                        {
                            get { return this.GetTime(); }
                            set { this.SetTime(value); }
                        }
                        /// <summary>
                        /// Whether or not to automatically calculate and set the minimum possible dwell time.
                        /// Setting Auto = true has same effect as Time = 0
                        /// </summary>
                        public bool Auto
                        {
                            get { return this.GetAuto(); }
                            set { this.SetAuto(value); }
                        }
                        /// <summary>
                        /// Time to wait just before acquistion begins for each sweep.
                        /// </summary>
                        public double SweepDelay
                        {
                            get { return this.GetSweepDelay(); }
                            set { this.SetSweepDelay(value); }
                        }
                        /// <summary>
                        /// Minimum allowed dwell time.
                        /// </summary>
                        public double MinimumTime
                        {
                            get { return this.GetMinimumTime(); }
                        }
                        /// <summary>
                        /// Maximum allowed dwell time.
                        /// </summary>
                        public double MaximumTime
                        {
                            get { return this.GetMaximumTime(); }
                        }
                        /// <summary>
                        /// Minimum allowed sweep delay.
                        /// </summary>
                        public double MinimumSweepDelay
                        {
                            get { return this.GetMinimumSweepDelay(); }
                        }
                        /// <summary>
                        /// Maximum allowed sweep delay.
                        /// </summary>
                        public double MaximumSweepDelay
                        {
                            get { return this.GetMaximumSweepDelay(); }
                        }

                        // Constructor
                        internal DwellClass(ChannelClass ParentChannel)
                        {
                            _parentChannel = ParentChannel;
                        }

                        // Private Methods
                        private double GetTime()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _time = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _time;
                        }
                        private void SetTime(double Time)
                        {
                            _time = Time;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl {1}", _parentChannel.Number, _time));
                        }
                        private bool GetAuto()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl:AUTO?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _auto = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _auto;
                        }
                        private void SetAuto(bool Auto)
                        {
                            _auto = Auto;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl:AUTO {1}", _parentChannel.Number, _auto ? "ON" : "OFF"));
                        }
                        private double GetSweepDelay()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl:SDELay?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _sweepDelay = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _sweepDelay;
                        }
                        private void SetSweepDelay(double SweepDelay)
                        {
                            _sweepDelay = SweepDelay;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl:SDELay {1}", _parentChannel.Number, _sweepDelay));
                        }
                        private double GetMinimumTime()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl? MINimum", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _minimumTime = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _minimumTime;
                        }
                        private double GetMaximumTime()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl? MAXimum", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _maximumTime = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _maximumTime;
                        }
                        private double GetMinimumSweepDelay()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl:SDELay? MINimum", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _minimumSweepDelay = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _minimumSweepDelay;
                        }
                        private double GetMaximumSweepDelay()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:DWELl:SDELay? MAXimum", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _maximumSweepDelay = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _maximumSweepDelay;
                        }

                    }
                    public class TriggerClass {

                        // Variables
                        private ChannelClass _parentChannel;
                        private int _groupCount;
                        private double _delay;
                        private SweepTriggerModeEnum _mode;

                        // Properties
                        /// <summary>
                        /// Number of triggers this channel will respond to in Sweep.Mode = Group
                        /// </summary>
                        public int GroupCount
                        {
                            get { return this.GetGroupCount(); }
                            set { this.SetGroupCount(value); }
                        }
                        /// <summary>
                        /// Trigger delay in seconds of the specified channel. Only applies when PNAX.Trigger.Source = External and PNAX.Trigger.Scope = Current.
                        /// Must be between 0 and 3 sseconds.
                        /// </summary>
                        public double Delay
                        {
                            get { return this.GetDelay(); }
                            set { this.SetDelay(value); }
                        }
                        /// <summary>
                        /// Trigger Mode for the specified channel.
                        /// Channel = Each trigger signal causes all traces in the channel to be swept (default)
                        /// Sweep = Each manual or external trigger signal causes all traces that share a source port to be swept.
                        /// Point = Each manual or external trigger signal causes one data point to be measured.
                        /// Trace = Allowed only when Sweep.PointSweep is true. Each trigger signal causes two identical measurements to be triggered separately - one trigger signal is required for each measurement. Other trigger mode settings cause two identical parameters to be measured simultaneously.
                        /// </summary>
                        public SweepTriggerModeEnum Mode
                        {
                            get { return this.GetMode(); }
                            set { this.SetMode(value); }
                        }

                        // Constructor
                        internal TriggerClass(ChannelClass ParentChannel)
                        {
                            _parentChannel = ParentChannel;
                        }

                        // Private Methods
                        private int GetGroupCount()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:GROups:COUNt?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _groupCount = (int)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                            return _groupCount;
                        }
                        private void SetGroupCount(int GroupCount)
                        {
                            _groupCount = GroupCount;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:GROups:COUNt {1}", _parentChannel.Number, _groupCount));
                        }
                        private double GetDelay()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TRIGger:DELay?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _delay = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _delay;
                        }
                        private void SetDelay(double Delay)
                        {
                            _delay = Delay;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TRIGger:DELay {1}", _parentChannel.Number, _delay));
                        }
                        private SweepTriggerModeEnum GetMode()
                        {
                            string retVal;
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TRIGger:MODE?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVal = _parentChannel._parentPNAX.ReadString();
                            if (retVal.Contains("CHAN")) {
                                _mode = SweepTriggerModeEnum.Channel;
                            } else if (retVal.Contains("SWE")) {
                                _mode = SweepTriggerModeEnum.Sweep;
                            } else if (retVal.Contains("POIN")) {
                                _mode = SweepTriggerModeEnum.Point;
                            } else {
                                _mode = SweepTriggerModeEnum.Trace;
                            }
                            return _mode;
                        }
                        private void SetMode(SweepTriggerModeEnum Mode)
                        {
                            _mode = Mode;
                            switch (_mode) {
                                case SweepTriggerModeEnum.Channel:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TRIGger:MODE CHANnel", _parentChannel.Number));
                                    break;
                                case SweepTriggerModeEnum.Sweep:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TRIGger:MODE SWEep", _parentChannel.Number));
                                    break;
                                case SweepTriggerModeEnum.Point:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TRIGger:MODE POINt", _parentChannel.Number));
                                    break;
                                case SweepTriggerModeEnum.Trace:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TRIGger:MODE TRACe", _parentChannel.Number));
                                    break;
                            }
                        }

                    }
                    public class PulseClass {

                        // Variables
                        private ChannelClass _parentChannel;
                        private bool _autoCWTime;
                        private bool _autoDetectMode;
                        private bool _autoDrive;
                        private bool _autoIFGain;
                        private bool _autoPRF;
                        private bool _autoTiming;
                        private double _frequency;
                        private double _period;
                        private double _width;
                        private PulseModeEnum _mode;
                        private double _profileStartTime;
                        private double _profileStopTime;
                        private bool _softwareGating;
                        private PulseDetectionMode _detectionMode;

                        // Properties
                        /// <summary>
                        /// The state of automatic CW sweep time (in Pulse Profile mode).
                        /// </summary>
                        public bool AutoCWTime
                        {
                            get { return this.GetAutoCWTime(); }
                            set { this.SetAutoCWTime(value); }
                        }
                        /// <summary>
                        /// Automatically (true) or manually (false) set pulse mode (Narrow or Wideband) for the channel.
                        /// </summary>
                        public bool AutoDetectMode
                        {
                            get { return this.GetAutoDetectMode(); }
                            set { this.SetAutoDetectMode(value); }
                        }
                        /// <summary>
                        /// Automatically (true) or manually (false) set the drive for the source modulation in Narrowband mode.
                        /// </summary>
                        public bool AutoDrive
                        {
                            get { return this.GetAutoDrive(); }
                            set { this.SetAutoDrive(value); }
                        }
                        /// <summary>
                        /// Automatically (true) or manually (false) set the IF Gain in Narrowband pulse mode.
                        /// </summary>
                        public bool AutoIFGain
                        {
                            get { return this.GetAutoIFGain(); }
                            set { this.SetAutoIFGain(value); }
                        }
                        /// <summary>
                        /// Automatically (true) or manually (false) set the Pulse Repition Frequency.
                        /// For manual, the PRF is controlled using Frequency or Period.
                        /// </summary>
                        public bool AutoPRF
                        {
                            get { return this.GetAutoPRF(); }
                            set { this.SetAutoPRF(value); }
                        }
                        /// <summary>
                        /// Automatically (true) or manually (false) set the Width and Delay.
                        /// </summary>
                        public bool AutoTiming
                        {
                            get { return this.GetAutoTiming(); }
                            set { this.SetAutoTiming(value); }
                        }
                        /// <summary>
                        /// The master pulse measurement frequency in Hz.
                        /// Frequency = 1/Period.
                        /// </summary>
                        public double Frequency
                        {
                            get { return this.GetFrequency(); }
                            set { this.SetFrequency(value); }
                        }
                        /// <summary>
                        /// The master pulse measurement period in seconds.
                        /// Period = 1/Frequency.
                        /// </summary>
                        public double Period
                        {
                            get { return this.GetPeriod(); }
                            set { this.SetPeriod(value); }
                        }
                        /// <summary>
                        /// The master pulse measurement width in seconds.
                        /// </summary>
                        public double Width
                        {
                            get { return this.GetWidth(); }
                            set { this.SetWidth(value); }
                        }
                        /// <summary>
                        /// Pulse measurement state for this channel.
                        /// Off = Turn off pulse measurements (default).
                        /// Standard = Standard pulse measurements.
                        /// Profile = Pulse profile measurements.
                        /// </summary>
                        public PulseModeEnum Mode
                        {
                            get { return this.GetMode(); }
                            set { this.SetMode(value); }
                        }
                        /// <summary>
                        /// The start time in seconds of the pulse in pulse profile mode.
                        /// </summary>
                        public double ProfileStartTime
                        {
                            get { return this.GetProfileStartTime(); }
                            set { this.SetProfileStartTime(value); }
                        }
                        /// <summary>
                        /// The stop time in seconds of the pulse in pulse profile mode.
                        /// </summary>
                        public double ProfileStopTime
                        {
                            get { return this.GetProfileStopTime(); }
                            set { this.SetProfileStopTime(value); }
                        }
                        /// <summary>
                        /// When set to off (false), the improved software gating sensitivity is turned off and
                        /// all data outside the measurement band is zeroed. This is used for troubleshooting purposes.
                        /// There is NO user-interface control on the PNA for this setting.
                        /// Defaults to ON (true).
                        /// </summary>
                        public bool SoftwareGating
                        {
                            get { return this.GetSoftwareGating(); }
                            set { this.SetSoftwareGating(value); }
                        }
                        /// <summary>
                        /// Narrowband or Wideband pulse detection.
                        /// </summary>
                        public PulseDetectionMode DetectionMode
                        {
                            get { return this.GetDetectionMode(); }
                            set { this.SetDetectionMode(value); }
                        }

                        // Constructor
                        internal PulseClass(ChannelClass ParentChannel)
                        {
                            _parentChannel = ParentChannel;
                        }

                        // Private Methods
                        private bool GetAutoCWTime()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:CWTime:AUTO?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _autoCWTime = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _autoCWTime;
                        }
                        private void SetAutoCWTime(bool AutoCWTime)
                        {
                            _autoCWTime = AutoCWTime;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:CWTime:AUTO {1}", _parentChannel.Number, _autoCWTime ? "ON" : "OFF"));
                        }
                        private bool GetAutoDetectMode()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:DETectmode:AUTO?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _autoDetectMode = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _autoDetectMode;
                        }
                        private void SetAutoDetectMode(bool AutoDetectMode)
                        {
                            _autoDetectMode = AutoDetectMode;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:DETectmode:AUTO {1}", _parentChannel.Number, _autoDetectMode ? "ON" : "OFF"));
                        }
                        private bool GetAutoDrive()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:DRIVe:AUTO?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _autoDrive = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _autoDrive;
                        }
                        private void SetAutoDrive(bool AutoDrive)
                        {
                            _autoDrive = AutoDrive;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:DRIVe:AUTO {1}", _parentChannel.Number, _autoDrive ? "ON" : "OFF"));
                        }
                        private bool GetAutoIFGain()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:IFGain:AUTO?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _autoIFGain = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _autoIFGain;
                        }
                        private void SetAutoIFGain(bool AutoIFGain)
                        {
                            _autoIFGain = AutoIFGain;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:IFGain:AUTO {1}", _parentChannel.Number, _autoIFGain ? "ON" : "OFF"));
                        }
                        private bool GetAutoPRF()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:PRF:AUTO?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _autoPRF = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _autoPRF;
                        }
                        private void SetAutoPRF(bool AutoPRF)
                        {
                            _autoPRF = AutoPRF;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:PRF:AUTO {1}", _parentChannel.Number, _autoPRF ? "ON" : "OFF"));
                        }
                        private bool GetAutoTiming()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:TIMing:AUTO?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _autoTiming = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _autoTiming;
                        }
                        private void SetAutoTiming(bool AutoTiming)
                        {
                            _autoTiming = AutoTiming;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:TIMing:AUTO {1}", _parentChannel.Number, _autoTiming ? "ON" : "OFF"));
                        }
                        private double GetFrequency()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MASTer:FREQuency?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _frequency = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _frequency;
                        }
                        private void SetFrequency(double Frequency)
                        {
                            _frequency = Frequency;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MASTer:FREQuency {1}", _parentChannel.Number, _frequency));
                        }
                        private double GetPeriod()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MASTer:PERiod?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _period = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _period;
                        }
                        private void SetPeriod(double Period)
                        {
                            _period = Period;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MASTer:PERiod {1}", _parentChannel.Number, _period));
                        }
                        private double GetWidth()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MASTer:WIDth?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _width = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _width;
                        }
                        private void SetWidth(double Width)
                        {
                            _width = Width;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MASTer:WIDth {1}", _parentChannel.Number, _width));
                        }
                        private PulseModeEnum GetMode()
                        {
                            string retVal;
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MODE?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVal = _parentChannel._parentPNAX.ReadString();
                            if (retVal.Contains("OFF")) {
                                _mode = PulseModeEnum.Off;
                            } else if (retVal.Contains("STD")) {
                                _mode = PulseModeEnum.Standard;
                            } else {
                                _mode = PulseModeEnum.Profile;
                            }
                            return _mode;
                        }
                        private void SetMode(PulseModeEnum Mode)
                        {
                            _mode = Mode;
                            switch (_mode) {
                                case PulseModeEnum.Off:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MODE OFF", _parentChannel.Number));
                                    break;
                                case PulseModeEnum.Standard:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MODE STD", _parentChannel.Number));
                                    break;
                                case PulseModeEnum.Profile:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:MODE PROFILE", _parentChannel.Number));
                                    break;
                            }
                        }
                        private double GetProfileStartTime()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:PROFile:STARt?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _profileStartTime = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _profileStartTime;
                        }
                        private void SetProfileStartTime(double ProfileStartTime)
                        {
                            _profileStartTime = ProfileStartTime;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:PROFile:STARt {1}", _parentChannel.Number, _profileStartTime));
                        }
                        private double GetProfileStopTime()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:PROFile:STOP?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _profileStopTime = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                            return _profileStopTime;
                        }
                        private void SetProfileStopTime(double ProfileStopTime)
                        {
                            _profileStopTime = ProfileStopTime;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:PROFile:STOP {1}", _parentChannel.Number, _profileStopTime));
                        }
                        private bool GetSoftwareGating()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:SWGate:STATe?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            _softwareGating = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            return _softwareGating;
                        }
                        private void SetSoftwareGating(bool SoftwareGating)
                        {
                            _softwareGating = SoftwareGating;
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:SWGate:STATe {1}", _parentChannel.Number, _softwareGating ? "ON" : "OFF"));
                        }
                        private PulseDetectionMode GetDetectionMode()
                        {
                            bool retVal;
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:WIDeband:STATe?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            retVal = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                            _detectionMode = retVal ? PulseDetectionMode.Wideband : PulseDetectionMode.Narrowband;
                            return _detectionMode;
                        }
                        private void SetDetectionMode(PulseDetectionMode DetectionMode)
                        {
                            _detectionMode = DetectionMode;
                            switch (_detectionMode) {
                                case PulseDetectionMode.Narrowband:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:WIDeband:STATe OFF", _parentChannel.Number));
                                    break;
                                case PulseDetectionMode.Wideband:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:PULSe:WIDeband:STATe ON", _parentChannel.Number));
                                    break;
                            }
                        }

                    }

                    // Variables
                    private ChannelClass _parentChannel;
                    private bool _blocked;
                    private SweepGenerationEnum _generation;
                    private bool _pointSweep;
                    private SweepModeEnum _mode;
                    private int _points;
                    private bool _fast;
                    private double _frequencyStepSize;
                    private double _time;
                    private bool _autoTime;
                    private SweepTypeEnum _type;
                    private int _minimumPoints;
                    private int _maximumPoints;
                    private double _minimumTime;
                    private double _maximumTime;

                    // Properties
                    /// <summary>
                    /// Reads whether the channel is currently blocked from sweeping.
                    /// </summary>
                    public bool Blocked
                    {
                        get { return this.GetBlocked(); }
                    }
                    /// <summary>
                    /// Stepped or Analog sweep generation.
                    /// Stepped = Source frequency is constant during measurement of each displayed point. More accurate than Analog. Dwell time can be set in this mode.
                    /// Analog = Source frequency is continuously ramping during measurement of each display point. Faster than Stepped. Sweep time (not Dwell time) can be set in this mode.
                    /// </summary>
                    public SweepGenerationEnum Generation
                    {
                        get { return this.GetGeneration(); }
                        set { this.SetGeneration(value); }
                    }
                    /// <summary>
                    /// Turns on or off point sweep mode.
                    /// When enabled, the PNA measures both forward and reverse parameters at each frequency point before stepping to the next frequency.
                    /// </summary>
                    public bool PointSweep
                    {
                        get { return this.GetPointSweep(); }
                        set { this.SetPointSweep(value); }
                    }
                    /// <summary>
                    /// Set the sweep mode that this channel will respond to triggers.
                    /// Hold = Channel will not trigger.
                    /// Continuous = Channel triggers indefinitely.
                    /// Groups = Channel accepts the number of triggers specified with the last Sweep.Trigger.GroupCount setting. THIS IS AN OVERLAPPED COMMAND!
                    /// Single = Channel accepts one trigger then goes to Hold. 
                    /// </summary>
                    public SweepModeEnum Mode
                    {
                        get { return this.GetMode(); }
                        set { this.SetMode(value); }
                    }
                    /// <summary>
                    /// The number of data points for the sweep stimulus.
                    /// </summary>
                    public int Points
                    {
                        get { return this.GetPoints(); }
                        set { this.SetPoints(value); }
                    }
                    /// <summary>
                    /// Enable or disable fast sweep mode
                    /// </summary>
                    public bool Fast
                    {
                        get { return this.GetFast(); }
                        set { this.SetFast(value); }
                    }
                    /// <summary>
                    /// The frequency step size in Hz across the selected frequency range of the channel.
                    /// This effectively sets the number of data points.
                    /// Only available when Sweep.Type = Linear
                    /// </summary>
                    public double FrequencyStepSize
                    {
                        get { return this.GetFrequencyStepSize(); }
                        set { this.SetFrequencyStepSize(value); }
                    }
                    /// <summary>
                    /// The time in seconds it takes to complete one sweep.
                    /// </summary>
                    public double Time
                    {
                        get { return this.GetTime(); }
                        set { this.SetTime(value); }
                    }
                    /// <summary>
                    /// Controls the automatic sweep time function, on or off.
                    /// </summary>
                    public bool AutoTime
                    {
                        get { return this.GetAutoTime(); }
                        set { this.SetAutoTime(value); }
                    }
                    /// <summary>
                    /// The type of analyzer sweep.
                    /// First set sweep type, then set parameters such as frequency or power settings for the channel.
                    /// </summary>
                    public SweepTypeEnum Type
                    {
                        get { return this.GetType(); }
                        set { this.SetType(value); }
                    }
                    /// <summary>
                    /// The minimum number of data points for the sweep stimulus.
                    /// </summary>
                    public int MinimumPoints
                    {
                        get { return this.GetMinimumPoints(); }
                    }
                    /// <summary>
                    /// The maximum number of data points for the sweep stimulus.
                    /// </summary>
                    public int MaximumPoints
                    {
                        get { return this.GetMaximumPoints(); }
                    }
                    /// <summary>
                    /// The minimum time in seconds it takes to complete one sweep.
                    /// </summary>
                    public double MinimumTime
                    {
                        get { return this.GetMinimumTime(); }
                    }
                    /// <summary>
                    /// The maximum time in seconds it takes to complete one sweep.
                    /// </summary>
                    public double MaximumTime
                    {
                        get { return this.GetMaximumTime(); }
                    }
                    /// <summary>
                    /// Control dwell properties of the sweep.
                    /// </summary>
                    public DwellClass Dwell
                    { get; private set; }
                    /// <summary>
                    /// Control the trigger properties of this channel sweep.
                    /// </summary>
                    public TriggerClass Trigger
                    { get; private set; }
                    /// <summary>
                    /// Control the pulse properties of this channel.
                    /// </summary>
                    public PulseClass Pulse
                    { get; private set; }
                    
                    // Constructor
                    internal SweepClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                        Dwell = new DwellClass(_parentChannel);
                        Trigger = new TriggerClass(_parentChannel);
                        Pulse = new PulseClass(_parentChannel);
                    }

                    // Private Methods
                    private bool GetBlocked()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:BLOCked?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _blocked = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _blocked;
                    }
                    private SweepGenerationEnum GetGeneration()
                    {
                        string retVal;
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:GENeration?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = _parentChannel._parentPNAX.ReadString();
                        if (retVal.Contains("STEP")) {
                            _generation = SweepGenerationEnum.Stepped;
                        } else {
                            _generation = SweepGenerationEnum.Analog;
                        }
                        return _generation;
                    }
                    private void SetGeneration(SweepGenerationEnum Generation)
                    {
                        _generation = Generation;
                        switch (_generation) {
                            case SweepGenerationEnum.Stepped:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:GENeration STEPped", _parentChannel.Number));
                                break;
                            case SweepGenerationEnum.Analog:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:GENeration ANALog", _parentChannel.Number));
                                break;
                        }
                    }
                    private bool GetPointSweep()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:GENeration:POINtsweep?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _pointSweep = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _pointSweep;
                    }
                    private void SetPointSweep(bool PointSweep)
                    {
                        _pointSweep = PointSweep;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:GENeration:POINtsweep {1}", _parentChannel.Number, _pointSweep ? "ON" : "OFF"));
                    }
                    private SweepModeEnum GetMode()
                    {
                        string retVal;
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:MODE?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = _parentChannel._parentPNAX.ReadString();
                        if (retVal.Contains("HOLD")) {
                            _mode = SweepModeEnum.Hold;
                        } else if (retVal.Contains("CONT")) {
                            _mode = SweepModeEnum.Continuous;
                        } else if (retVal.Contains("GRO")) {
                            _mode = SweepModeEnum.Groups;
                        } else {
                            _mode = SweepModeEnum.Single;
                        }
                        return _mode;
                    }
                    private void SetMode(SweepModeEnum Mode)
                    {
                        _mode = Mode;
                        switch (_mode) {
                            case SweepModeEnum.Hold:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:MODE HOLD", _parentChannel.Number));
                                break;
                            case SweepModeEnum.Continuous:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:MODE CONTinuous", _parentChannel.Number));
                                break;
                            case SweepModeEnum.Groups:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:MODE GROups", _parentChannel.Number));
                                break;
                            case SweepModeEnum.Single:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:MODE SINGle", _parentChannel.Number));
                                break;
                        }
                    }
                    private int GetPoints()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:POINts?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _points = (int)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _points;
                    }
                    private void SetPoints(int Points)
                    {
                        _points = Points;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:POINts {1}", _parentChannel.Number, _points));
                    }
                    private bool GetFast()
                    {
                        string retVal;
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:SPEed?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = _parentChannel._parentPNAX.ReadString();
                        if (retVal.Contains("FAST")) {
                            _fast = true;
                        } else {
                            _fast = false;
                        }
                        return _fast;
                    }
                    private void SetFast(bool Fast)
                    {
                        _fast = Fast;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:SPEed {1}", _parentChannel.Number, _fast ? "FAST" : "NORMal"));
                    }
                    private double GetFrequencyStepSize()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:STEP?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _frequencyStepSize = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _frequencyStepSize;
                    }
                    private void SetFrequencyStepSize(double FrequencyStepSize)
                    {
                        _frequencyStepSize = FrequencyStepSize;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:STEP {1}", _parentChannel.Number, _frequencyStepSize));
                    }
                    private double GetTime()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TIME?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _time = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _time;
                    }
                    private void SetTime(double Time)
                    {
                        _time = Time;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TIME {1}", _parentChannel.Number, _time));
                    }
                    private bool GetAutoTime()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TIME:AUTO?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _autoTime = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _autoTime;
                    }
                    private void SetAutoTime(bool AutoTime)
                    {
                        _autoTime = AutoTime;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TIME:AUTO {1}", _parentChannel.Number, _autoTime ? "ON" : "OFF"));
                    }
                    private SweepTypeEnum GetType()
                    {
                        string retVal;
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TYPE?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = _parentChannel._parentPNAX.ReadString();
                        if (retVal.Contains("LIN")) {
                            _type = SweepTypeEnum.Linear;
                        } else if (retVal.Contains("LOG")) {
                            _type = SweepTypeEnum.Logarithmic;
                        } else if (retVal.Contains("POW")) {
                            _type = SweepTypeEnum.Power;
                        } else if (retVal.Contains("CW")) {
                            _type = SweepTypeEnum.CW;
                        } else if (retVal.Contains("SEGM")) {
                            _type = SweepTypeEnum.Segment;
                        } else {
                            _type = SweepTypeEnum.Phase;
                        }
                        return _type;
                    }
                    private void SetType(SweepTypeEnum Type)
                    {
                        _type = Type;
                        switch (_type) {
                            case SweepTypeEnum.Linear:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TYPE LINear", _parentChannel.Number));
                                break;
                            case SweepTypeEnum.Logarithmic:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TYPE LOGarithmic", _parentChannel.Number));
                                break;
                            case SweepTypeEnum.Power:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TYPE POWer", _parentChannel.Number));
                                break;
                            case SweepTypeEnum.CW:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TYPE CW", _parentChannel.Number));
                                break;
                            case SweepTypeEnum.Segment:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TYPE SEGMent", _parentChannel.Number));
                                break;
                            case SweepTypeEnum.Phase:
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TYPE PHASe", _parentChannel.Number));
                                break;
                        }
                    }
                    private int GetMinimumPoints()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:POINts? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumPoints = (int)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _minimumPoints;
                    }
                    private int GetMaximumPoints()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:POINts? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumPoints = (int)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _maximumPoints;
                    }
                    private double GetMinimumTime()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TIME? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumTime = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumTime;
                    }
                    private double GetMaximumTime()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:SWEep:TIME? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumTime = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumTime;
                    }

                }
                public class CorrectionClass {
                    
                    // Nested Classes
                    public class GuidedClass {

                        // Nested Classes
                        public class PortClass {

                            // Variables
                            private ChannelClass _parentChannel;
                            private string _connector;
                            private string _calKit;

                            // Properties
                            /// <summary>
                            /// Number of this port.
                            /// </summary>
                            public int Number
                            { get; private set; }
                            /// <summary>
                            /// Connector type of this port. If not used for this calibration, set to "Not used"
                            /// </summary>
                            public string Connector
                            {
                                get { return this.GetConnector(); }
                                set { this.SetConnector(value); }
                            }
                            /// <summary>
                            /// The calibration kit (mechanical or ECal) for this port to be used during a Guided Calibration.
                            /// </summary>
                            public string CalKit
                            {
                                get { return this.GetCalKit(); }
                                set { this.SetCalKit(value); }
                            }

                            // Constructor
                            internal PortClass(int Number, ChannelClass ParentChannel)
                            {
                                this.Number = Number;
                                _parentChannel = ParentChannel;
                            }

                            // Private Methods
                            private string GetConnector()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:CONNector:PORT{1}:SELect?", _parentChannel.Number, Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _connector = _parentChannel._parentPNAX.ReadString();
                                return _connector;
                            }
                            private void SetConnector(string Connector)
                            {
                                _connector = Connector;
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:CONNector:PORT{1}:SELect '{2}'", _parentChannel.Number, Number, _connector));
                            }
                            private string GetCalKit()
                            {
                                _parentChannel._parentPNAX.ClearEventRegisters();
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:CKIT:PORT{1}:SELect?", _parentChannel.Number, Number));
                                _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                                _calKit = _parentChannel._parentPNAX.ReadString();
                                return _calKit;
                            }
                            private void SetCalKit(string CalKit)
                            {
                                _calKit = CalKit;
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:CKIT:PORT{1}:SELect '{2}'", _parentChannel.Number, Number, _calKit));
                            }

                        }
                        public class PortCollectionClass : IEnumerable<PortClass> {

                            // Variables
                            private ChannelClass _parentChannel;
                            private Dictionary<int, PortClass> _ports;

                            // Properties
                            /// <summary>
                            /// Number of ports that can be used by this Guided calibration.
                            /// </summary>
                            public int Count
                            {
                                get { return this._ports.Count; }
                            }
                            /// <summary>
                            /// The numbers of the ports.
                            /// </summary>
                            public int[] Numbers
                            {
                                get { return this._ports.Keys.ToArray(); }
                            }

                            // Constructor
                            internal PortCollectionClass(ChannelClass ParentChannel)
                            {
                                _parentChannel = ParentChannel;
                            }

                            // Indexer
                            public PortClass this[int Number]
                            {
                                get
                                {
                                    if (!_ports.ContainsKey(Number))
                                        throw new IndexOutOfRangeException("This number port doesn't exist");

                                    return _ports[Number];
                                }
                            }

                            // Internal Methods
                            internal void FindPorts()
                            {
                                List<int> portNumbers = new List<int>();
                                string[] internalPortNames = _parentChannel._parentPNAX.Capabilities.FindInternalPortNames();
                                foreach (string portName in internalPortNames) {
                                    portNumbers.Add(_parentChannel._parentPNAX.Capabilities.ConvertPortNameToNumber(portName));
                                }
                                portNumbers.Sort();
                                _ports.Clear();
                                foreach (int portNumber in portNumbers) {
                                    _ports.Add(portNumber, new PortClass(portNumber, _parentChannel));
                                }
                            }

                            // Public Methods
                            public IEnumerator<PortClass> GetEnumerator()
                            {
                                return this._ports.Values.GetEnumerator();
                            }

                            // Unused Interface Methods
                            IEnumerator IEnumerable.GetEnumerator()
                            {
                                throw new NotImplementedException();
                            }
                        }

                        // Variables
                        private ChannelClass _parentChannel;

                        // Properties
                        /// <summary>
                        /// Ports controlled in this guided calibration.
                        /// </summary>
                        public PortCollectionClass Ports
                        { get; private set; }
                        
                        // Constructor
                        internal GuidedClass(ChannelClass ParentChannel)
                        {
                            _parentChannel = ParentChannel;
                            Ports = new PortCollectionClass(_parentChannel);
                        }

                        // Private Methods

                        // Public Methods
                        /// <summary>
                        /// Aborts the acquiring of a guided calibration that has been Initialized but has not yet been concluded using the Save command.
                        /// If at least one Cal standard has already been measured, and the Calibration Window is being displayed, this command also closes
                        /// the Calibration Window and re-tiles the other measurement windows.
                        /// </summary>
                        public void Abort()
                        {
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:ABORt", _parentChannel.Number));
                        }
                        /// <summary>
                        /// Initiates the measurement of the specified calibration step. Executing this command with an unnecessary step has no effect.
                        /// </summary>
                        /// <param name="Step">
                        /// The number of the cal step connection steps for the calibration.
                        /// </param>
                        /// <param name="Sync">
                        /// Synchronous = Blocks SCPI commands during standard measurement (default).
                        /// Asynchronous = Does not block SCPI commands during standard measurement. (Generally for advanced users).
                        /// </param>
                        public void Acquire(int Step, GuidedCalibrationSyncEnum Sync = GuidedCalibrationSyncEnum.Synchronous)
                        {
                            switch (Sync) {
                                case GuidedCalibrationSyncEnum.Synchronous:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:ACQuire STAN{1}, 'SYNChronous'", _parentChannel.Number, Step));
                                    break;
                                case GuidedCalibrationSyncEnum.Asynchronous:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:ACQuire STAN{1}, 'ASYNchronous'", _parentChannel.Number, Step));
                                    break;
                            }
                        }
                        /// <summary>
                        /// Returns the connection description for the specified calibration step.
                        /// </summary>
                        public string GetStepDescription(int Step)
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:DESCription? {1}"));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            return _parentChannel._parentPNAX.ReadString();
                        }
                        /// <summary>
                        /// Initiates a guided calibration. Use this to start a guided calibration, then query the number of steps, issue Acquire(...) commands.
                        /// </summary>
                        /// <param name="ExistingCalSetName">
                        /// Existing Cal Set name or GUID. The Cal Set is either supplemented or overwritten depending on the method, connectors, and ports selected.
                        /// If no Cal Set name is specified, the behavior depends on the Save Preference setting.
                        /// </param>
                        /// <param name="CalSetNameIsGUID">
                        /// False = If ExistingCalSetName is a name.
                        /// True = If ExistingCalSetName is a GUID (less common).
                        /// </param>
                        /// <param name="ChangeStimulusSettings">
                        /// If ExistingCalSetName stimulus settings differ from the existing channel, do (true) or do not (false, default) change the stimulus settings.
                        /// </param>
                        /// <param name="Sync">
                        /// Synchronous = Blocks SCPI commands while processing this command (default).
                        /// Asynchronous = Does not block SCPI commands while processing this command. (Generally for advanced users).
                        /// </param>
                        public void Initiate(string ExistingCalSetName = null, bool CalSetNameIsGUID = false, bool ChangeStimulusSettings = false, GuidedCalibrationSyncEnum Sync = GuidedCalibrationSyncEnum.Synchronous)
                        {
                            if (String.IsNullOrEmpty(ExistingCalSetName)) {
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:INITiate:IMMediate", _parentChannel.Number));
                            } else {
                                ExistingCalSetName = CalSetNameIsGUID ? String.Format("{{{0}}}", ExistingCalSetName) : ExistingCalSetName;
                                switch (Sync) {
                                    case GuidedCalibrationSyncEnum.Synchronous:
                                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:INITiate:IMMediate '{1}', {2}, SYNChronous", _parentChannel.Number, ExistingCalSetName, ChangeStimulusSettings ? "ON" : "OFF"));
                                        break;
                                    case GuidedCalibrationSyncEnum.Asynchronous:
                                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:INITiate:IMMediate '{1}', {2}, ASYNchronous", _parentChannel.Number, ExistingCalSetName, ChangeStimulusSettings ? "ON" : "OFF"));
                                        break;
                                }
                            }
                        }
                        /// <summary>
                        /// Specify the paths (port pairs) to make isolation measurements on during a guided calibration.
                        /// </summary>
                        /// <param name="Isolation">
                        /// All = Measure isolation on all pairings of the ports that are to be calibrated.
                        /// None = Do not measure isolation on any pairing of the ports to be measured.
                        /// Add = Add one or more specific pairings of ports to the list of ports pairings for which isolation will be measured.
                        /// Remove = Remove one or more specific pairings of ports from the list of port pairings for which isolation will be measured. If many paths are to be measured,
                        /// it may be easier to first send All, then Remove and specify the paths to remove.
                        /// </param>
                        /// <param name="PortNumbers">
                        /// For use when Isolation = Add or Remove.
                        /// Specify port numbers in pairs (i.e. new int[] {1, 2, 1, 3} will specify isolation paths port 1 to port 2 and port 1 to port 3.
                        /// For 3-port calibrations, specify up to 3 pairs.
                        /// For 4-port calibrations, specify up to 6 pairs.
                        /// </param>
                        public void SpecifyIsolationPaths(GuidedCalibrationIsolationEnum Isolation = GuidedCalibrationIsolationEnum.All, int[] PortNumbers = null)
                        {
                            StringBuilder ports;
                            switch (Isolation) {
                                case GuidedCalibrationIsolationEnum.All:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:GUIDed:ISOLation:PATHs ALL", _parentChannel.Number));
                                    break;
                                case GuidedCalibrationIsolationEnum.None:
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:GUIDed:ISOLation:PATHs NONE", _parentChannel.Number));
                                    break;
                                case GuidedCalibrationIsolationEnum.Add:
                                    if (PortNumbers == null || PortNumbers.Length % 2 != 0)
                                        throw new System.ArgumentException("PortNumbers is either null or not divisible by two.", "PortNumbers");
                                    ports = new StringBuilder();
                                    for (int i = 0; i < PortNumbers.Length; i++) {
                                        ports.AppendFormat("{0},", PortNumbers[i]);
                                    }
                                    ports.Remove(ports.Length - 1, 1);
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:GUIDed:ISOLation:PATHs ADD,{1}", _parentChannel.Number, ports.ToString()));
                                    break;
                                case GuidedCalibrationIsolationEnum.Remove:
                                    if (PortNumbers == null || PortNumbers.Length % 2 != 0)
                                        throw new System.ArgumentException("PortNumbers is either null or not divisible by two.", "PortNumbers");
                                    ports = new StringBuilder();
                                    for (int i = 0; i < PortNumbers.Length; i++) {
                                        ports.AppendFormat("{0},", PortNumbers[i]);
                                    }
                                    ports.Remove(ports.Length - 1, 1);
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:GUIDed:ISOLation:PATHs ADD,{1}", _parentChannel.Number, ports.ToString()));
                                    break;
                            }
                        }
                        /// <summary>
                        /// Resets the specified guided cal connection step as unmeasured. This clears all previous measurements made for that step.
                        /// </summary>
                        public void ResetStep(int Step)
                        {
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:ITERations:RESet {1}", _parentChannel.Number, Step));
                        }
                        /// <summary>
                        /// Completes the guided cal by computing the error terms and turning correction ON.
                        /// <param name="CalSetNameIsGUID">
                        /// False = If ExistingCalSetName is a name.
                        /// True = If ExistingCalSetName is a GUID (less common).
                        /// </param>
                        /// <param name="ExistingCalSetName">
                        /// If ExistingCalSetName is specified, the cal set will be saved to this name.
                        /// If ExistingCalSetName is not specified And...
                        /// </param>
                        /// <param name="defaultToSavePreference">
                        /// If defaultToSavePreference = true, the behavior will default to the preference set at PNAX.Correction.SavePreference
                        /// If defaultToSavePreference = false, the behavior will be...
                        /// </param>
                        /// <param name="CalRegisterAndUserCalSet">
                        /// If CalRegisterAndUserCalSet = true, Cal data will be saved to a Cal Register and a User Cal Set. The filename is automatically generated.
                        /// If CalRegisterAndUserCalSet = false, Cal data will be save only to a Cal Register.
                        /// </param>
                        /// </summary>
                        public void Save(string ExistingCalSetName = null, bool CalSetNameIsGUID = false, bool defaultToSavePreference = true, bool CalRegisterAndUserCalSet = false)
                        {
                            if (String.IsNullOrEmpty(ExistingCalSetName)) {
                                if (defaultToSavePreference == true) {
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:SAVE:IMMediate", _parentChannel.Number));
                                } else {
                                    _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:SAVE:IMMediate {1}", _parentChannel.Number, CalRegisterAndUserCalSet ? "ON" : "OFF"));
                                }
                            } else {
                                ExistingCalSetName = CalSetNameIsGUID ? String.Format("{{{0}}}", ExistingCalSetName) : ExistingCalSetName;
                                _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:SAVE:CSET '{1}'", _parentChannel.Number, ExistingCalSetName));
                            }
                        }
                        /// <summary>
                        /// Gets the number of measurement steps required to complete the current guided calibration.
                        /// This command is sent after assigning port connectors, port cal kits, and Initiate(...).
                        /// </summary>
                        /// <returns></returns>
                        public int GetNumberOfSteps()
                        {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:CORRection:COLLect:GUIDed:STEPs?", _parentChannel.Number));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            return (int)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        }
                    }

                    // Variables
                    private ChannelClass _parentChannel;

                    // Properties
                    /// <summary>
                    /// Manage Guided Calibrations on this channel.
                    /// </summary>
                    public GuidedClass Guided
                    { get; private set; }

                    // Constructor
                    internal CorrectionClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                        Guided = new GuidedClass(_parentChannel);
                    }

                    // Private Methods


                    // Public Methods


                }
                public class MeasurementClass {

                    // Nested Classes
                    public class OffsetClass {
                        
                        // Variables
                        private MeasurementClass _parentMeasurement;
                        private double _magnitude;
                        private double _phase;
                        private double _slope;

                        // Properties
                        /// <summary>
                        /// Offset the data trace magnitude.
                        /// </summary>
                        public double Magnitude
                        {
                            get { return this.GetMagnitude(); }
                            set { this.SetMagnitude(value); }
                        }
                        /// <summary>
                        /// Offset the data trace phase in degrees.
                        /// Must be between -360 and 360.
                        /// </summary>
                        public double Phase
                        {
                            get { return this.GetPhase(); }
                            set { this.SetPhase(value); }
                        }
                        /// <summary>
                        /// Offset the data trace magnitude slope in dB/1GHz
                        /// </summary>
                        public double Slope
                        {
                            get { return this.GetSlope(); }
                            set { this.SetSlope(value); }
                        }

                        // Constructor
                        internal OffsetClass(MeasurementClass ParentMeasurement)
                        {
                            _parentMeasurement = ParentMeasurement;
                        }

                        // Private Methods
                        private double GetMagnitude()
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.ClearEventRegisters();
                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:OFFset:MAGNitude?", _parentMeasurement._parentChannel.Number));
                            _parentMeasurement._parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentMeasurement._parentChannel._parentPNAX.Timeout);
                            _magnitude = (double)_parentMeasurement._parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);


                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }

                            return _magnitude;
                        }
                        private void SetMagnitude(double Magnitude)
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _magnitude = Magnitude;

                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:OFFset:MAGNitude {1}", _parentMeasurement._parentChannel.Number, _magnitude));

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }
                        private double GetPhase()
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.ClearEventRegisters();
                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:OFFset:PHASe?", _parentMeasurement._parentChannel.Number));
                            _parentMeasurement._parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentMeasurement._parentChannel._parentPNAX.Timeout);
                            _phase = (double)_parentMeasurement._parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);


                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }

                            return _phase;
                        }
                        private void SetPhase(double Phase)
                        {

                            if (Phase < -360 || 360 < Phase)
                                throw new System.ArgumentException("Phase must be between -360 and 360.", "Phase");

                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _phase = Phase;

                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:OFFset:PHASe {1}", _parentMeasurement._parentChannel.Number, _phase));

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }
                        private double GetSlope()
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.ClearEventRegisters();
                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:OFFset:SLOPe?", _parentMeasurement._parentChannel.Number));
                            _parentMeasurement._parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentMeasurement._parentChannel._parentPNAX.Timeout);
                            _slope = (double)_parentMeasurement._parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);


                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }

                            return _slope;
                        }
                        private void SetSlope(double Slope)
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _slope = Slope;

                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:OFFset:SLOPe {1}", _parentMeasurement._parentChannel.Number, _slope));

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }

                    }
                    public class SmoothingClass {

                        // Variables
                        private MeasurementClass _parentMeasurement;
                        private double _aperture;
                        private int _points;
                        private bool _state;

                        // Properties
                        /// <summary>
                        /// Amount of smoothing as a percentage of the number of data points in the channel.
                        /// Must be between 1 and 25.
                        /// </summary>
                        public double Aperture
                        {
                            get { return this.GetAperture(); }
                            set { this.SetAperture(value); }
                        }
                        /// <summary>
                        /// Number of adjacent data points to average.
                        /// </summary>
                        public int Points
                        {
                            get { return this.GetPoints(); }
                            set { this.SetPoints(value); }
                        }
                        /// <summary>
                        /// Turns smoothing on and off.
                        /// </summary>
                        public bool State
                        {
                            get { return this.GetState(); }
                            set { this.SetState(value); }
                        }

                        // Constructor
                        internal SmoothingClass(MeasurementClass ParentMeasurement)
                        {
                            _parentMeasurement = ParentMeasurement;
                        }

                        // Private Methods
                        private double GetAperture()
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.ClearEventRegisters();
                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:SMOothing:APERture?", _parentMeasurement._parentChannel.Number));
                            _parentMeasurement._parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentMeasurement._parentChannel._parentPNAX.Timeout);
                            _aperture = (double)_parentMeasurement._parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);


                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }

                            return _aperture;
                        }
                        private void SetAperture(double Aperture)
                        {
                            if (Aperture < 1 || 25 < Aperture)
                                throw new System.ArgumentException("Aperture must be between 1 and 25 percent.", "Aperture");

                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _aperture = Aperture;

                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:SMOothing:APERture {1}", _parentMeasurement._parentChannel.Number, _aperture));

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }
                        private int GetPoints()
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.ClearEventRegisters();
                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:SMOothing:POINts?", _parentMeasurement._parentChannel.Number));
                            _parentMeasurement._parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentMeasurement._parentChannel._parentPNAX.Timeout);
                            _points = (int)_parentMeasurement._parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);


                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }

                            return _points;
                        }
                        private void SetPoints(int Points)
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                           _points = Points;

                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:SMOothing:POINts {1}", _parentMeasurement._parentChannel.Number, _points));

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }
                        private bool GetState()
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.ClearEventRegisters();
                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:SMOothing:STATe?", _parentMeasurement._parentChannel.Number));
                            _parentMeasurement._parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentMeasurement._parentChannel._parentPNAX.Timeout);
                            _state = (((byte)_parentMeasurement._parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);


                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }

                            return _state;
                        }
                        private void SetState(bool State)
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _state = State;

                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:SMOothing:STATe {1}", _parentMeasurement._parentChannel.Number, _state ? "ON" : "OFF"));

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }

                    }
                    public class TraceHoldClass {

                        // Variables
                        private MeasurementClass _parentMeasurement;
                        private TraceHoldTypeEnum _type;

                        // Properties
                        /// <summary>
                        /// The type of trace hold to perform.
                        /// </summary>
                        public TraceHoldTypeEnum Type
                        {
                            get { return this.GetType(); }
                            set { this.SetType(value); }
                        }

                        // Constructor
                        internal TraceHoldClass(MeasurementClass ParentMeasurement)
                        {
                            _parentMeasurement = ParentMeasurement;
                        }

                        // Private Methods
                        private TraceHoldTypeEnum GetType()
                        {
                            string retVal;
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.ClearEventRegisters();
                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:HOLD:TYPE?", _parentMeasurement._parentChannel.Number));
                            _parentMeasurement._parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentMeasurement._parentChannel._parentPNAX.Timeout);
                            retVal = _parentMeasurement._parentChannel._parentPNAX.ReadString();

                            if (retVal.Contains("OFF")) {
                                _type = TraceHoldTypeEnum.Off;
                            } else if (retVal.Contains("MINimum")) {
                                _type = TraceHoldTypeEnum.Minimum;
                            } else {
                                _type = TraceHoldTypeEnum.Maximum;
                            }


                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }

                            return _type;
                        }
                        private void SetType(TraceHoldTypeEnum Type)
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _type = Type;
                            switch (_type) {
                                case TraceHoldTypeEnum.Off:
                                    _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:HOLD:TYPE OFF", _parentMeasurement._parentChannel.Number));
                                    break;
                                case TraceHoldTypeEnum.Minimum:
                                    _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:HOLD:TYPE MINimum", _parentMeasurement._parentChannel.Number));
                                    break;
                                case TraceHoldTypeEnum.Maximum:
                                    _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:HOLD:TYPE MAXimum", _parentMeasurement._parentChannel.Number));
                                    break;
                            }

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }

                        // Public Methods
                        /// <summary>
                        /// Resets the currently stored data points to the live data trace and restarts the currently-selected Trace Hold Type.
                        /// </summary>
                        public void Clear()
                        {
                            string selectedName = _parentMeasurement._parentChannel.Measurements.SelectedMeasurementName;
                            _parentMeasurement.Select();

                            _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:HOLD:CLEar", _parentMeasurement._parentChannel.Number));

                            if (!String.IsNullOrWhiteSpace(selectedName)) {
                                _parentMeasurement._parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentMeasurement._parentChannel.Number, selectedName));
                            }
                        }
                    }

                    // Variables
                    private ChannelClass _parentChannel;
                    private string _type;
                    private MeasurementFormatEnum _format;

                    // Properties
                    /// <summary>
                    /// The measurement name. Cannot be changed after the measurement is created.
                    /// </summary>
                    public string Name
                    { get; private set; }
                    /// <summary>
                    /// The measurement type.
                    /// </summary>
                    public string Type
                    {
                        get { return this._type; }
                        set { this.SetType(value); }
                    }
                    /// <summary>
                    /// Get the stimulus values for this measurement.
                    /// </summary>
                    public double[] XAxisValues
                    {
                        get { return this.GetXAxisValues(); }
                    }
                    /// <summary>
                    /// Format of the specified measurement
                    /// </summary>
                    public MeasurementFormatEnum Format
                    {
                        get { return this.GetFormat(); }
                        set { this.SetFormat(value); }
                    }
                    /// <summary>
                    /// Control offset settings of this measurement.
                    /// </summary>
                    public OffsetClass Offset
                    { get; private set; }
                    /// <summary>
                    /// Control smoothing settings of this measurement
                    /// </summary>
                    public SmoothingClass Smoothing
                    { get; private set; }
                    /// <summary>
                    /// Control trace hold settings of this measurement.
                    /// </summary>
                    public TraceHoldClass Hold
                    { get; private set; }
                    /// <summary>
                    /// The trace that this measurement is attached to.
                    /// null if this measurement has not been attached to a trace.
                    /// </summary>
                    public PNAX.DisplayClass.WindowClass.TraceClass AttachedTrace
                    { get; internal set; }

                    // Constructor
                    internal MeasurementClass(string Name, string Type, ChannelClass ParentChannel)
                    {
                        this.Name = Name;
                        _type = Type;
                        _parentChannel = ParentChannel;

                        Offset = new OffsetClass(this);
                        Smoothing = new SmoothingClass(this);
                        Hold = new TraceHoldClass(this);
                        AttachedTrace = null;
                    }

                    // Private Methods
                    private void SetType(string Type)
                    {
                        // TO DO: Check that Type exists for the measurement application type.
                        _type = Type;

                        // TO DO: Allow modifying for different measurement application types.
                        string selectedName = _parentChannel.Measurements.SelectedMeasurementName;
                        Select();
                        _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:MODify:EXTended {1}", _parentChannel.Number, _type));
                        if (!String.IsNullOrWhiteSpace(selectedName)) {
                            _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentChannel.Number, selectedName));
                        }

                    }
                    private double[] GetXAxisValues()
                    {
                        double[] retVal;
                        string selectedName = _parentChannel.Measurements.SelectedMeasurementName;
                        Select();

                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:X:VALues?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = (double[])_parentChannel._parentPNAX.ReadList(IEEEASCIIType.ASCIIType_R8, ",;");

                        if (!String.IsNullOrWhiteSpace(selectedName)) {
                            _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentChannel.Number, selectedName));
                        }

                        return retVal;
                    }
                    private MeasurementFormatEnum GetFormat()
                    {
                        string retVal;
                        string selectedName = _parentChannel.Measurements.SelectedMeasurementName;
                        Select();

                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = _parentChannel._parentPNAX.ReadString();

                        if (retVal.Contains("MLIN")) { _format = MeasurementFormatEnum.Linear; }
                        else if (retVal.Contains("MLOG")) { _format = MeasurementFormatEnum.Logarithmic; }
                        else if (retVal.Contains("PHAS")) { _format = MeasurementFormatEnum.Phase; }
                        else if (retVal.Contains("UPH")) { _format = MeasurementFormatEnum.UnwrappedPhase; }
                        else if (retVal.Contains("IMAG")) { _format = MeasurementFormatEnum.Imaginary; }
                        else if (retVal.Contains("REAL")) { _format = MeasurementFormatEnum.Real; }
                        else if (retVal.Contains("POL")) { _format = MeasurementFormatEnum.Polar; }
                        else if (retVal.Contains("SMIT")) { _format = MeasurementFormatEnum.Smith; }
                        else if (retVal.Contains("SADM")) { _format = MeasurementFormatEnum.SmithAdmittance; }
                        else if (retVal.Contains("SWR")) { _format = MeasurementFormatEnum.SWR; }
                        else if (retVal.Contains("GDEL")) { _format = MeasurementFormatEnum.GroupDelay; }
                        else if (retVal.Contains("KELV")) { _format = MeasurementFormatEnum.Kelvin; }
                        else if (retVal.Contains("FAHR")) { _format = MeasurementFormatEnum.Fahrenheit; }
                        else { _format = MeasurementFormatEnum.Celsius; }

                        if (!String.IsNullOrWhiteSpace(selectedName)) {
                            _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentChannel.Number, selectedName));
                        }

                        return _format;
                    }
                    private void SetFormat(MeasurementFormatEnum Format)
                    {
                        _format = Format;

                        string selectedName = _parentChannel.Measurements.SelectedMeasurementName;
                        Select();

                        switch (_format) {
                            case MeasurementFormatEnum.Linear:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat MLINear", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Logarithmic:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat MLOGarithmic", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Phase:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat PHASe", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.UnwrappedPhase:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat UPHase", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Imaginary:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat IMAGinary", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Real:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat REAL", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Polar:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat POLar", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Smith:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat SMITh", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.SmithAdmittance:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat SADMittance", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.SWR:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat SWR", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.GroupDelay:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat GDELay", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Kelvin:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat KELVin", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Fahrenheit:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat FAHRenheit", _parentChannel.Number));
                                break;
                            case MeasurementFormatEnum.Celsius:
                                _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat CELSius", _parentChannel.Number));
                                break;
                        }

                        if (!String.IsNullOrWhiteSpace(selectedName)) {
                            _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentChannel.Number, selectedName));
                        }

                    }
                    
                    // Public Methods
                    public void Select()
                    {
                        _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", _parentChannel.Number, Name));
                    }

                }
                public class MeasurementCollectionClass : IEnumerable<MeasurementClass> {

                    // Variables
                    private ChannelClass _parentChannel;
                    private Dictionary<string, MeasurementClass> _measurements;

                    // Properties
                    /// <summary>
                    /// Number of measurements managed by this driver.
                    /// </summary>
                    public int Count
                    {
                        get { return this._measurements.Count(); }
                    }
                    /// <summary>
                    /// Measurement names of the managed measurements.
                    /// </summary>
                    public string[] ManagedMeasurementNames
                    {
                        get { return this._measurements.Keys.ToArray(); }
                    }
                    /// <summary>
                    /// Measurement names on the channel, not necessarily managed by this driver.
                    /// </summary>
                    public string[] OpenMeasurementNames
                    {
                        get { return this.GetOpenMeasurementNames(); }
                    }
                    /// <summary>
                    /// The name of the currently selected measurement on the channel.
                    /// </summary>
                    public string SelectedMeasurementName
                    {
                        get { return this.GetSelectedMeasurementName(); }
                    }

                    // Constructor
                    internal MeasurementCollectionClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                    }

                    // Indexer
                    public MeasurementClass this[string Name]
                    {
                        get
                        {
                            if (!_measurements.ContainsKey(Name))
                                throw new System.IndexOutOfRangeException("Measurement name does not exist.");

                            return _measurements[Name];
                        }
                    }

                    // Private Methods
                    private string[] GetOpenMeasurementNames()
                    {
                        int[] retVal;
                        List<string> measurementNames = new List<string>();
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SYSTem:MEASurement:CATalog? {0}", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        retVal = (int[])_parentChannel._parentPNAX.ReadList(IEEEASCIIType.ASCIIType_I4, ",;");
                        for (int i = 0; i < retVal.Length; i++) {
                            _parentChannel._parentPNAX.ClearEventRegisters();
                            _parentChannel._parentPNAX.WriteString(String.Format("SYSTem:MEASurement{0}:NAME?", retVal[i]));
                            _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                            measurementNames.Add(_parentChannel._parentPNAX.ReadString());
                        }
                        return measurementNames.ToArray();
                    }
                    private string GetSelectedMeasurementName()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        return _parentChannel._parentPNAX.ReadString();
                    }

                    // Public Methods
                    /// <summary>
                    /// Creates a measurement with the specified name and type.
                    /// </summary>
                    /// <returns>True if the measurement is created successfully. False if the measurement name already exists or the type isn't supported </returns>
                    public bool Add(string Name, string Type)
                    {
                        if (_measurements.ContainsKey(Name))
                            return false;

                        // TO DO: Support all measurement application types (i.e. NoiseFigure, GainCompression, etc.)
                        // TO DO: Check that Type is allowed by the measurement application type

                        // Create Basic S-Par measurement
                        _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:DEFine:EXTended {1}, {2}", _parentChannel.Number, Name, Type));
                        _measurements.Add(Name, new MeasurementClass(Name, Type, _parentChannel));

                        return true;
                    }
                    /// <summary>
                    /// Deletes the measurement with the specified name from the channel.
                    /// If the measurement is attached to a trace, that trace is also deleted.
                    /// </summary>
                    /// <returns>True if deleted successfully. False if the measurement doesn't exist.</returns>
                    public bool Delete(string Name)
                    {
                        if (_measurements.ContainsKey(Name)) {
                            _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:DELete:NAME {1}", _parentChannel.Number, Name));
                            if (_measurements[Name].AttachedTrace != null) {
                                _parentChannel._parentPNAX.Display.Windows[_measurements[Name].AttachedTrace._parentWindow.Number].Traces.Delete(_measurements[Name].AttachedTrace.Number);
                            }
                            _measurements.Remove(Name);
                            return true;
                        } else if (OpenMeasurementNames.Contains(Name)) {
                            _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:DELete:NAME {1}", _parentChannel.Number, Name));
                            return true;
                        } else {
                            return false;
                        }
                    }
                    /// <summary>
                    /// Iterates through all open measurement names and deletes all measurements on the channel.
                    /// If the measurements are attached to a trace, those traces are also deleted.
                    /// </summary>
                    public void DeleteAll()
                    {
                        List<string> allNames = new List<string>(OpenMeasurementNames);
                        foreach (string name in allNames) {
                            _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:DELete:NAME {1}", _parentChannel.Number, name));
                            if (_measurements.ContainsKey(name) && _measurements[name].AttachedTrace != null) {
                                _parentChannel._parentPNAX.Display.Windows[_measurements[name].AttachedTrace._parentWindow.Number].Traces.Delete(_measurements[name].AttachedTrace.Number);
                            }
                        }
                        _measurements.Clear();
                    }
                    public IEnumerator<MeasurementClass> GetEnumerator()
                    {
                        return _measurements.Values.GetEnumerator();
                    }

                    // Unused Interface Methods
                    IEnumerator IEnumerable.GetEnumerator()
                    {
                        throw new NotImplementedException();
                    }

                }

                // Variables
                private PNAX _parentPNAX;

                // Properties
                /// <summary>
                /// Channel number of this channel.
                /// </summary>
                public int Number
                { get; private set; }
                /// <summary>
                /// Control the path configuration for this channel.
                /// </summary>
                public PathClass Path
                { get; private set; }
                /// <summary>
                /// Control averaging settings on this channel.
                /// </summary>
                public AveragingClass Averaging
                { get; private set; }
                /// <summary>
                /// Control frequency settings on this channel.
                /// </summary>
                public FrequencyClass Frequency
                { get; private set; }
                /// <summary>
                /// Control IF Bandwidth settings on this channel.
                /// </summary>
                public IFBandwidthClass IFBandwidth
                { get; private set; }
                /// <summary>
                /// Control the properties of the sources in this channel
                /// </summary>
                public SourceClass Source
                { get; private set; }
                /// <summary>
                /// Control the sweep properties of this channel.
                /// </summary>
                public SweepClass Sweep
                { get; private set; }
                /// <summary>
                /// Control Calibrations for this channel.
                /// </summary>
                public CorrectionClass Correction
                { get; private set; }
                /// <summary>
                /// Control measurements on this channel.
                /// </summary>
                public MeasurementCollectionClass Measurements
                { get; private set; }

                // Constructor
                internal ChannelClass(int Number, PNAX ParentPNAX)
                {
                    this.Number = Number;
                    _parentPNAX = ParentPNAX;
                    Path = new PathClass(this);
                    Averaging = new AveragingClass(this);
                    Frequency = new FrequencyClass(this);
                    IFBandwidth = new IFBandwidthClass(this);
                    Source = new SourceClass(this);
                    Sweep = new SweepClass(this);
                    Correction = new CorrectionClass(this);
                    Measurements = new MeasurementCollectionClass(this);
                }

                // Private Methods


                // Public Methods

            }
            public class ChannelCollectionClass : IEnumerable<ChannelClass> {

                // Variables
                private PNAX _parentPNAX;
                private Dictionary<int, ChannelClass> _channels;

                // Properties
                /// <summary>
                /// Number of channels managed by this driver.
                /// </summary>
                public int Count
                {
                    get { return this._channels.Count; }
                }
                /// <summary>
                /// Channel Numbers managed by this driver.
                /// </summary>
                public int[] ManagedChannelNumbers
                {
                    get { return this._channels.Keys.ToArray(); }
                }
                /// <summary>
                /// All open channel numbers on the PNAX. Not all channels are necessarily managed by this driver.
                /// i.e. If a channel was created locally using the front panel, this driver does not manage its use.
                /// </summary>
                public int[] OpenChannelNumbers
                {
                    get { return this.GetOpenChannelNumbers(); }
                }
                /// <summary>
                /// Gets the channel number of the active channel.  If no channel is active, returns 0.
                /// </summary>
                public int ActiveChannelNumber
                {
                    get { return this.GetActiveChannelNumber(); }
                }
                /// <summary>
                /// The name of the active measurement. If no measurement is active, returns an empty string.
                /// </summary>
                public string ActiveMeasurementName
                {
                    get { return this.GetActiveMeasurementName(); }
                }

                // Constructor
                internal ChannelCollectionClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                }

                // Indexer
                public ChannelClass this[int Number]
                {
                    get
                    {
                        if (!_channels.ContainsKey(Number))
                            throw new System.IndexOutOfRangeException("Channel does not exist at this number.");

                        return _channels[Number];
                    }
                }

                // Private Methods
                private int[] GetOpenChannelNumbers()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CHANnels:CATalog?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    return (int[])_parentPNAX.ReadList(IEEEASCIIType.ASCIIType_I4, ",;");
                }
                private int GetActiveChannelNumber()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:ACTive:CHANnel?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    return (int)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                }
                private string GetActiveMeasurementName()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:ACTive:MEASurement?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    return _parentPNAX.ReadString();
                }

                // Public Methods
                /// <summary>
                /// Adds a new channel with the next available channel number.
                /// </summary>
                /// <returns>
                /// The channel number of the newly created channel.
                /// Returns -1, if the maximum number of channels is reached.
                /// </returns>
                public int Add()
                {
                    int availableChannelNumber = 1;
                    int[] openChannelNumbers = OpenChannelNumbers;

                    if (openChannelNumbers.Length == _parentPNAX.Capabilities.MaximumChannels)
                        return -1;

                    for (int i = 0; i < openChannelNumbers.Length; i++) {
                        if (!openChannelNumbers.Contains(i + 1)) {
                            availableChannelNumber = i + 1;
                            break;
                        }
                    }

                    // TO DO: Add a new channel to the dictionary
                    // TO DO: When a new channel is added, call to FindPorts in Channel.Source.Ports.FindPorts()
                    // TO DO: When a new channel is added, call to FindPorts in Channel.Correction.Guided.Ports.FindPorts()
                    // TO DO: When a new channel is added, call to FindElements in Channel.Path.Elements.FindElements()

                    return availableChannelNumber;
                }
                /// <summary>
                /// Deletes the specified channel number from the collection and destroys the channel on the PNAX.
                /// </summary>
                /// <returns>True if the channel is deleted. False if the specified channel number doesn't exist.</returns>
                public bool Delete(int ChannelNumber)
                {
                    if (_channels.ContainsKey(ChannelNumber)) {
                        _parentPNAX.WriteString(String.Format("SYSTem:CHANnels:DELete {0}", ChannelNumber));
                        _channels.Remove(ChannelNumber);
                        return true;
                    } else if (OpenChannelNumbers.Contains(ChannelNumber)) {
                        _parentPNAX.WriteString(String.Format("SYSTem:CHANnels:DELete {0}", ChannelNumber));
                        return true;
                    } else {
                        return false;
                    }
                }
                /// <summary>
                /// Iterates through all open channel numbers and deletes them.
                /// </summary>
                public void DeleteAll()
                {
                    List<int> allNumbers = new List<int>(OpenChannelNumbers);
                    foreach (int number in allNumbers) {
                        _parentPNAX.WriteString(String.Format("SYSTem:CHANnels:DELete {0}", number));
                    }
                    _channels.Clear();
                }
                public IEnumerator<ChannelClass> GetEnumerator()
                {
                    return _channels.Values.GetEnumerator();
                }

                // Unused Interface Methods
                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }

            }

            // Enumerations
                // Data Format
            public enum DataFormatEnum { Real32, Real64, ASCii0 };
                // Display
            public enum GridLineTypeEnum { Solid, Dotted };
                // Trigger
            public enum TriggerSourceEnum { External, Immediate, Manual };
            public enum TriggerScopeEnum { All, Current };
            public enum ExternalTriggerSlopeEnum { Positive, Negative };
            public enum ExternalTriggerTypeEnum { Edge, Level };
            public enum ExternalTriggerRouteEnum { Main, Math, Pulse3 };
                // Calibration
            public enum CalibrationWizardEnum { Main, CalKit };
            public enum GuidedCalibrationSyncEnum { Synchronous, Asynchronous };
            public enum GuidedCalibrationIsolationEnum { All, None, Add, Remove };
            public enum GuidedCalibrationSavePreferenceEnum { CalRegister, UserCalSet, Reuse };
                // Averaging
            public enum AveragingModeEnum { Point, Sweep };
                // Sweep
            public enum SweepGenerationEnum { Stepped, Analog };
            public enum SweepModeEnum { Hold, Continuous, Groups, Single };
            public enum SweepTypeEnum { Linear, Logarithmic, Power, CW, Segment, Phase };
            public enum SweepTriggerModeEnum { Channel, Sweep, Point, Trace };
                // Pulse
            public enum PulseModeEnum { Off, Standard, Profile };
            public enum PulseDetectionMode { Narrowband, Wideband };
                // Port
            public enum PortALCModeEnum { Internal, OpenLoop };
            public enum PortModeEnum { Auto, On, Off, NoControl };
                // Measurement
            public enum MeasurementFormatEnum { Linear, Logarithmic, Phase, UnwrappedPhase, Imaginary, Real, Polar, Smith, SmithAdmittance, SWR, GroupDelay, Kelvin, Fahrenheit, Celsius };
            public enum TraceHoldTypeEnum { Off, Minimum, Maximum };
                // Window
            public enum ScaleCouplingMethodEnum { Off, Window, All };

            // Variables
            private DataFormatEnum _dataFormat;

            // Properties
            /// <summary>
            /// Format of data transfers from PNA to computer.
            /// Use Real64 for frequency data and Real32 for measurement data.
            /// Transferring data in ASCii format is not recommended because it significantly increases transfer times.
            /// </summary>
            public DataFormatEnum DataFormat
            {
                get { return this.GetDataFormat(); }
                set { this.SetDataFormat(value); }
            }
            /// <summary>
            /// Properties showing capabilities of this PNAX
            /// </summary>
            public CapabilitiesClass Capabilities
            { get; private set; }
            /// <summary>
            /// Global correction/calibration settings of the PNAX.
            /// </summary>
            public CorrectionClass Correction
            { get; private set; }
            /// <summary>
            /// Control the display of the PNAX.
            /// </summary>
            public DisplayClass Display
            { get; private set; }
            /// <summary>
            /// Control the trigger properties.
            /// </summary>
            public TriggerClass Trigger
            { get; private set; }
            /// <summary>
            /// Control the output properties.
            /// </summary>
            public OutputClass Output
            { get; private set; }
            /// <summary>
            /// Control the channels of the PNAX.
            /// </summary>
            public ChannelCollectionClass Channels
            { get; private set; }

            // Constructor
            public PNAX()
                : base()
            {
                Capabilities = new CapabilitiesClass(this);
                Correction = new CorrectionClass(this);
                Display = new DisplayClass(this);
                Trigger = new TriggerClass(this);
                Output = new OutputClass(this);
                Channels = new ChannelCollectionClass(this);
            }
            
            // Private Methods
            private DataFormatEnum GetDataFormat()
            {
                string retVal;
                ClearEventRegisters();
                WriteString("FORMat:DATA?");
                WaitForMeasurementToComplete(Timeout);
                retVal = ReadString();
                if (retVal.Contains("REAL")) {
                    if (retVal.Contains("32")) {
                        _dataFormat = DataFormatEnum.Real32;
                    } else {
                        _dataFormat = DataFormatEnum.Real64;
                    }
                } else {
                    _dataFormat = DataFormatEnum.ASCii0;
                }
                return _dataFormat;
            }
            private void SetDataFormat(DataFormatEnum DataFormat)
            {
                _dataFormat = DataFormat;
                switch (_dataFormat) {
                    case DataFormatEnum.Real32:
                        WriteString("FORMat:DATA REAL,32");
                        break;
                    case DataFormatEnum.Real64:
                        WriteString("FORMat:DATA REAL,64");
                        break;
                    case DataFormatEnum.ASCii0:
                        WriteString("FORMat:DATA ASCii,0");
                        break;
                }
            }

            // Public Methods
            public override void Initialize(string GPIBAddress, int Timeout)
            {
                base.Initialize(GPIBAddress, Timeout);
                Capabilities.FindCapabilities();
            }
            /// <summary>
            /// Deletes all traces, measurements, and windows. Resets the analyzer to factory defined
            /// default settings. Creates an S11 measurement named "CH1_S11_1"; For this reason, it is
            /// recommended to reset the PNA using FullPreset() so that all Channels, Windows, etc. will
            /// be managed by this driver.
            /// </summary>
            public override void Reset()
            {
                base.Reset();
                // TO DO: Reset ALL configured channels, windows, etc.
            }
            /// <summary>
            /// Performs essentially the same function as Reset() except
            /// this function does NOT reset DataFormat to ASCii as does Reset()
            /// </summary>
            public void Preset()
            {
                WriteString("SYSTem:PRESet");
                // TO DO: Reset ALL configured channels, windows, etc.
            }
            /// <summary>
            /// A full preset performs a standard Preset(), then deletes the default trace, measurement and window.
            /// The PNA screen becomes blank.  This is the recommended way to reset the PNA when using this driver.
            /// </summary>
            public void FullPreset()
            {
                WriteString("SYSTem:FPReset");
                // TO DO: Reset ALL configured channels, windows, etc.
            }
            public void WaitForOperationsToComplete(int Timeout)
            {

            }
        }

    }

}
