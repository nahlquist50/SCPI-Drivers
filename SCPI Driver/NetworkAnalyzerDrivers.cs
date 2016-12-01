using System;
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

        public class PNAX : SCPIDriver {

            // Nested Classes
            public class TriggerClass {

                // Variables
                private PNAX _parentPNAX;
                private TriggerSourceEnum _source;
                private TriggerScopeEnum _scope;
                private ExternalTriggerTypeEnum _externalTriggerType;
                private ExternalTriggerSlopeEnum _externalTriggerSlope;
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
                public ExternalTriggerTypeEnum ExternalTriggerType
                {
                    get { return this.GetExternalTriggerType(); }
                    set { this.SetExternalTriggerType(value); }
                }
                public ExternalTriggerSlopeEnum ExternalTriggerSlope
                {
                    get { return this.GetExternalTriggerSlope(); }
                    set { this.SetExternalTriggerSlope(value); }
                }
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

                // Protected Methods
                protected virtual TriggerSourceEnum GetSource()
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
                protected virtual void SetSource(TriggerSourceEnum Source)
                {
                    _source = Source;
                    switch (_source) {
                        case TriggerSourceEnum.Immediate:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SOURce IMMediate");
                            break;
                        case TriggerSourceEnum.External:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SOURce EXTernal");
                            break;
                        case TriggerSourceEnum.Manual:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SOURce MANual");
                            break;
                    }
                }
                protected virtual TriggerScopeEnum GetScope()
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
                protected virtual void SetScope(TriggerScopeEnum Scope)
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
                protected virtual ExternalTriggerTypeEnum GetExternalTriggerType()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:SEQuence:TYPE?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("EDGE")) {
                        _externalTriggerType = ExternalTriggerTypeEnum.Edge;
                    } else {
                        _externalTriggerType = ExternalTriggerTypeEnum.Level;
                    }
                    return _externalTriggerType;
                }
                protected virtual void SetExternalTriggerType(ExternalTriggerTypeEnum ExternalTriggerType)
                {
                    _externalTriggerType = ExternalTriggerType;
                    switch (_externalTriggerType) {
                        case ExternalTriggerTypeEnum.Edge:
                            _parentPNAX.WriteString("TRIGger:SEQuence:TYPE EDGE");
                            break;
                        case ExternalTriggerTypeEnum.Level:
                            _parentPNAX.WriteString("TRIGger:SEQuence:TYPE LEVel");
                            break;
                    }
                }
                protected virtual ExternalTriggerSlopeEnum GetExternalTriggerSlope()
                {
                    string retVal;
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:SEQuence:SLOPe?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVal = _parentPNAX.ReadString();
                    if (retVal.Contains("POS")) {
                        _externalTriggerSlope = ExternalTriggerSlopeEnum.Positive;
                    } else {
                        _externalTriggerSlope = ExternalTriggerSlopeEnum.Negative;
                    }
                    return _externalTriggerSlope;
                }
                protected virtual void SetExternalTriggerSlope(ExternalTriggerSlopeEnum ExternalTriggerSlope)
                {
                    _externalTriggerSlope = ExternalTriggerSlope;
                    switch (_externalTriggerSlope) {
                        case ExternalTriggerSlopeEnum.Positive:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SLOPe POSitive");
                            break;
                        case ExternalTriggerSlopeEnum.Negative:
                            _parentPNAX.WriteString("TRIGger:SEQuence:SLOPe NEGative");
                            break;
                    }
                }
                protected virtual double GetGlobalDelay()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("TRIGger:DELay?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _globalDelay = (double)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _globalDelay;
                }
                protected virtual void SetGlobalDelay(double GlobalDelay)
                {
                    _globalDelay = GlobalDelay;
                    _parentPNAX.WriteString(String.Format("TRIGger:DELay {0}", _globalDelay));
                }

            }
            public abstract class ChannelClass {

                // Nested Classes
                public class AveragingClass {

                    // Enumerations
                    public enum AveragingModeEnum { Point, Sweep };

                    // Variables
                    private ChannelClass _parentChannel;
                    private bool _enabled;
                    private ushort _count;
                    private AveragingModeEnum _mode;

                    // Properties
                    public bool Enabled
                    {
                        get { return this.GetEnabled(); }
                        set { this.SetEnabled(value); }
                    }
                    public ushort Count
                    {
                        get { return this.GetCount(); }
                        set { this.SetCount(value); }
                    }
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

                    // Protected Methods
                    protected virtual bool GetEnabled()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:STATe?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _enabled = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _enabled;
                    }
                    protected virtual void SetEnabled(bool Enabled)
                    {
                        _enabled = Enabled;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:STATe {1}", _parentChannel.Number, _enabled ? "ON" : "OFF"));
                    }
                    protected virtual ushort GetCount()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:COUNt?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _count = (ushort)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I2, true);
                        return _count;
                    }
                    protected virtual void SetCount(ushort Count)
                    {
                        if (Count == 0)
                            throw new System.ArgumentException("Averaging Count must be greater than 0 and less than 65536");

                        _count = Count;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:COUNt {1}", _parentChannel.Number, _count));
                    }
                    protected virtual AveragingModeEnum GetMode()
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
                    protected virtual void SetMode(AveragingModeEnum Mode)
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
                    public virtual void Clear()
                    {
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:AVERage:CLEar", _parentChannel.Number));
                    }
                }
                public class FrequencyClass {

                    // Variables
                    private ChannelClass _parentChannel;
                    private double _start;
                    private double _stop;
                    private double _span;
                    private double _center;
                    private double _minimumStart;
                    private double _maximumStart;
                    private double _minimumStop;
                    private double _maximumStop;
                    private double _minimumSpan;
                    private double _maximumSpan;
                    private double _minimumCenter;
                    private double _maximumCenter;

                    // Properties
                    public double Start
                    {
                        get { return this.GetStart(); }
                        set { this.SetStart(value); }
                    }
                    public double Stop
                    {
                        get { return this.GetStop(); }
                        set { this.SetStop(value); }
                    }
                    public double Span
                    {
                        get { return this.GetSpan(); }
                        set { this.SetSpan(value); }
                    }
                    public double Center
                    {
                        get { return this.GetCenter(); }
                        set { this.SetCenter(value); }
                    }
                    public double MinimumStart
                    {
                        get { return this.GetMinimumStart(); }
                    }
                    public double MaximumStart
                    {
                        get { return this.GetMaximumStart(); }
                    }
                    public double MinimumStop
                    {
                        get { return this.GetMinimumStop(); }
                    }
                    public double MaximumStop
                    {
                        get { return this.GetMaximumStop(); }
                    }
                    public double MinimumSpan
                    {
                        get { return this.GetMinimumSpan(); }
                    }
                    public double MaximumSpan
                    {
                        get { return this.GetMaximumSpan(); }
                    }
                    public double MinimumCenter
                    {
                        get { return this.GetMinimumCenter(); }
                    }
                    public double MaximumCenter
                    {
                        get { return this.GetMaximumCenter(); }
                    }
                    public double[] Values
                    {
                        get { return this.GetValues(); }
                    }

                    // Constructor
                    internal FrequencyClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                    }

                    // Protected Methods
                    protected virtual double GetStart()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _start = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _start;
                    }
                    protected virtual void SetStart(double Start)
                    {
                        _start = Start;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt {1}", _parentChannel.Number, _start));
                    }
                    protected virtual double GetStop()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _stop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _stop;
                    }
                    protected virtual void SetStop(double Stop)
                    {
                        _stop = Stop;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP {1}", _parentChannel.Number, _stop));
                    }
                    protected virtual double GetSpan()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _span = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _span;
                    }
                    protected virtual void SetSpan(double Span)
                    {
                        _span = Span;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN {1}", _parentChannel.Number, _span));
                    }
                    protected virtual double GetCenter()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _center = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _center;
                    }
                    protected virtual void SetCenter(double Center)
                    {
                        _center = Center;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer {1}", _parentChannel.Number, _center));
                    }
                    protected virtual double GetMinimumStart()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumStart;
                    }
                    protected virtual double GetMaximumStart()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STARt? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumStart = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumStart;
                    }
                    protected virtual double GetMinimumStop()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumStop;
                    }
                    protected virtual double GetMaximumStop()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:STOP? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumStop = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumStop;
                    }
                    protected virtual double GetMinimumSpan()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumSpan = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumSpan;
                    }
                    protected virtual double GetMaximumSpan()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:SPAN? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumSpan = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumSpan;
                    }
                    protected virtual double GetMinimumCenter()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer? MINimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _minimumCenter = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumCenter;
                    }
                    protected virtual double GetMaximumCenter()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:FREQuency:CENTer? MAXimum", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _maximumCenter = (double)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumCenter;
                    }
                    protected virtual double[] GetValues()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("CALCulate{0}:X:VALues?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        return (double[])_parentChannel._parentPNAX.ReadList(IEEEASCIIType.ASCIIType_R8, ",");
                    }

                }
                public class NoiseAveragingClass {

                    // Variables
                    private ChannelClass _parentChannel;
                    private bool _enabled;
                    private int _count;

                    // Properties
                    public bool Enabled
                    {
                        get { return this.GetEnabled(); }
                        set { this.SetEnabled(value); }
                    }
                    public int Count
                    {
                        get { return this.GetCount(); }
                        set { this.SetCount(value); }
                    }

                    // Constructor
                    internal NoiseAveragingClass(ChannelClass ParentChannel)
                    {
                        _parentChannel = ParentChannel;
                    }

                    // Protected Methods
                    protected virtual bool GetEnabled()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:NOISe:AVERage:STATe?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _enabled = (((byte)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _enabled;
                    }
                    protected virtual void SetEnabled(bool Enabled)
                    {
                        _enabled = Enabled;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:NOISe:AVERage:STATe {1}", _parentChannel.Number, _enabled ? "ON" : "OFF"));
                    }
                    protected virtual int GetCount()
                    {
                        _parentChannel._parentPNAX.ClearEventRegisters();
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:NOISe:AVERage:COUNt?", _parentChannel.Number));
                        _parentChannel._parentPNAX.WaitForMeasurementToComplete(_parentChannel._parentPNAX.Timeout);
                        _count = (int)_parentChannel._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _count;
                    }
                    protected virtual void SetCount(int Count)
                    {
                        if (!(0 < Count && Count < 100))
                            throw new System.ArgumentException("Noise Averaging Count must be greater than 0 and less than 100");

                        _count = Count;
                        _parentChannel._parentPNAX.WriteString(String.Format("SENSe{0}:NOISe:AVERage:COUNt {1}", _parentChannel.Number, _count));
                    }

                }

                // Variables
                protected PNAX _parentPNAX;
                protected MeasurementClassEnum _measurementClass;
                protected Dictionary<string, string> _measurements;
                protected double _ifBandwidth;
                internal bool _mappedToWindow;
                internal int _mappedWindowNumber;

                // Properties
                public int Number
                { get; private set; }
                public MeasurementClassEnum MeasurementClass
                {
                    get { return this.GetMeasurementClass(); }
                    set { this.SetMeasurementClass(value); }
                }
                public string[] MeasurementNames
                {
                    get { return _measurements.Keys.ToArray(); }
                }
                public double IFBandwidth
                {
                    get { return this.GetIFBandwidth(); }
                    set { this.SetIFBandwidth(value); }
                }
                public bool MappedToWindow
                {
                    get { return this._mappedToWindow; }
                }
                public int MappedWindowNumber
                {
                    get { return this._mappedWindowNumber; }
                }

                // Constructor
                internal ChannelClass(PNAX ParentPNAX, int Number)
                {
                    _parentPNAX = ParentPNAX;
                    this.Number = Number;
                    _measurements = new Dictionary<string, string>();
                    _mappedToWindow = false;
                    _mappedWindowNumber = -1;
                }

                // Protected Methods
                protected virtual MeasurementClassEnum GetMeasurementClass()
                {
                    return _measurementClass;
                }
                protected virtual bool SetMeasurementClass(MeasurementClassEnum MeasurementClass)
                {
                    if (_measurements.Count != 0) {
                        return false;
                    }
                    _measurementClass = MeasurementClass;
                    return true;
                }
                protected virtual double GetIFBandwidth()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString(String.Format("SENSe{0}:BANDwidth:RESolution?", Number));
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _ifBandwidth = (double)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _ifBandwidth;
                }
                protected virtual void SetIFBandwidth(double IFBandwidth)
                {
                    _ifBandwidth = IFBandwidth;
                    _parentPNAX.WriteString(String.Format("SENSe{0}:BANDwidth:RESolution {1}", Number, _ifBandwidth));
                }

                // Public Methods
                public abstract bool AddMeasurement(string MeasurementName, string MeasurementType, MeasurementFormatEnum MeasurementFormat = MeasurementFormatEnum.Logarithmic);
                public virtual bool SetMeasurementFormat(string MeasurementName, MeasurementFormatEnum MeasurementFormat)
                {
                    if (!_measurements.ContainsKey(MeasurementName))
                        return false;

                    _parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", Number, MeasurementName));

                    switch (MeasurementFormat) {
                        case MeasurementFormatEnum.Linear:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat MLINear", Number));
                            break;
                        case MeasurementFormatEnum.Logarithmic:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat MLOGarithmic", Number));
                            break;
                        case MeasurementFormatEnum.Phase:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat PHASe", Number));
                            break;
                        case MeasurementFormatEnum.UnwrappedPhase:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat UPHase", Number));
                            break;
                        case MeasurementFormatEnum.Imaginary:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat IMAGinary", Number));
                            break;
                        case MeasurementFormatEnum.Real:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat REAL", Number));
                            break;
                        case MeasurementFormatEnum.Polar:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat POLar", Number));
                            break;
                        case MeasurementFormatEnum.Smith:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat SMITh", Number));
                            break;
                        case MeasurementFormatEnum.SmithAdmittance:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat SADMittance", Number));
                            break;
                        case MeasurementFormatEnum.SWR:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:FORMat SWR", Number));
                            break;
                    }

                    return true;
                }
                public virtual bool DeleteMeasurement(string MeasurementName)
                {
                    if (!_measurements.ContainsKey(MeasurementName)) {
                        return false;
                    }

                    _parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:DELete:NAME {1}", Number, MeasurementName));

                    _measurements.Remove(MeasurementName);
                    return true;
                }
                public virtual bool GetMeasurementData(string MeasurementName, out double[] Data, DataAccessMapEnum DataAccessMap = DataAccessMapEnum.FDATA)
                {

                    if (!_measurements.Keys.Contains(MeasurementName)) {
                        Data = new double[] { };
                        return false;
                    }

                    _parentPNAX.WriteString(String.Format("CALCulate{0}:PARameter:SELect {1}", Number, MeasurementName));
                    _parentPNAX.ClearEventRegisters();
                    switch (DataAccessMap) {
                        case DataAccessMapEnum.FDATA:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:DATA? FDATA", Number));
                            break;
                        case DataAccessMapEnum.RDATA:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:DATA? RDATA", Number));
                            break;
                        case DataAccessMapEnum.SDATA:
                            _parentPNAX.WriteString(String.Format("CALCulate{0}:DATA? SDATA", Number));
                            break;
                        default:
                            break;
                    }
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);

                    Data = (double[])_parentPNAX.ReadIEEEBlock(IEEEBinaryType.BinaryType_R8, false, true);

                    return true;
                }

            }
            public class SParameterChannelClass : ChannelClass {

                // Properties
                public AveragingClass Averaging
                { get; private set; }
                public FrequencyClass Frequency
                { get; private set; }
                public string[] AllowedMeasurementTypes
                {
                    get { return this.GetAllowedMeasurementTypes(); }
                }

                // Constructor
                internal SParameterChannelClass(PNAX ParentPNAX, int Number)
                    : base(ParentPNAX, Number)
                {
                    _measurementClass = MeasurementClassEnum.SParameters;
                }

                // Protected Methods
                protected virtual string[] GetAllowedMeasurementTypes()
                {
                    List<string> allowedMeasurementTypes = new List<string>();
                    List<int> portNums = new List<int>();
                    string[] retStrings;

                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SOURce:CATalog?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retStrings = _parentPNAX.ReadString().Split(new string[] { ",", "\"", "\n", "Port" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string port in retStrings) {
                        if (!port.Contains("Src")) {
                            portNums.Add(int.Parse(port));
                        }
                    }

                    portNums = portNums.Distinct().ToList();

                    for (int i = 0; i < portNums.Count; i++) {
                        for (int j = 0; j < portNums.Count; j++) {
                            allowedMeasurementTypes.Add(String.Format("S{0}_{1}", portNums[i], portNums[j]));
                        }
                    }

                    return allowedMeasurementTypes.ToArray();
                }

                // Public Methods
                public override bool AddMeasurement(string MeasurementName, string MeasurementType, MeasurementFormatEnum MeasurementFormat = MeasurementFormatEnum.Logarithmic)
                {
                    if (_measurements.ContainsKey(MeasurementName))
                        return false;

                    if (!AllowedMeasurementTypes.Contains(MeasurementType))
                        return false;

                    _measurements.Add(MeasurementName, MeasurementType);
                    _parentPNAX.WriteString(String.Format("CALCulate{0}:PARamater:DEFine:EXTended {1}, {2}", Number, MeasurementName, MeasurementType));
                    SetMeasurementFormat(MeasurementName, MeasurementFormat);

                    return true;
                }
            }
            public class ChannelCollectionClass {

                // Variables
                private PNAX _parentPNAX;
                private Dictionary<int, ChannelClass> _channels;

                // Properties
                /// <summary>
                /// The channel numbers used by the PNAX, not necessarily created using this driver
                /// </summary>
                public int[] OpenChannelNumbers
                {
                    get { return this.GetOpenChannelNumbers(); }
                }
                /// <summary>
                /// The channel numbers managed by this driver instance. There may or may not be more channels on the PNAX created in local mode that are not managed here.
                /// </summary>
                public int[] ManagedChannelNumbers
                {
                    get { return this._channels.Keys.ToArray(); }
                }

                // Constructor
                internal ChannelCollectionClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                    _channels = new Dictionary<int, ChannelClass>();
                }

                // Indexer
                /// <summary>
                /// Access the channel by the Channel Number. Using the Measurement class, cast the returned channel to the appropriate type.
                /// </summary>
                /// <param name="ChannelNumber">Channel number trying to access (lowest is 1)</param>
                /// <returns>The channel given by the channel number.</returns>
                public ChannelClass this[int ChannelNumber]
                {
                    get
                    {
                        if (!_channels.ContainsKey(ChannelNumber))
                            throw new System.ArgumentException("Channel does not exist", "ChannelNumber");

                        return _channels[ChannelNumber];
                    }
                }

                // Protected Methods
                protected virtual int[] GetOpenChannelNumbers()
                {
                    List<int> channels = new List<int>();
                    string[] retVals;

                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:CHANnels:CATalog?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVals = _parentPNAX.ReadString().Split(new char[] { '"', ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string channelNumber in retVals) {
                        channels.Add(int.Parse(channelNumber));
                    }

                    channels.Sort();

                    return channels.ToArray();
                }

                // Public Methods
                public int Add(MeasurementClassEnum MeasurementClass)
                {
                    int firstOpenChannelNumber = 1;
                    List<int> currentChannelNumbers = new List<int>(OpenChannelNumbers);
                    currentChannelNumbers.AddRange(_channels.Keys);

                    currentChannelNumbers = currentChannelNumbers.Distinct().ToList();
                    currentChannelNumbers.Sort();

                    for (int i = 0; i < currentChannelNumbers.Count; i++) {
                        if ((i + 1) != currentChannelNumbers[i]) {
                            firstOpenChannelNumber = i + 1;
                            break;
                        }
                    }

                    switch (MeasurementClass) {
                        case MeasurementClassEnum.SParameters:
                            _channels.Add(firstOpenChannelNumber, new SParameterChannelClass(_parentPNAX, firstOpenChannelNumber));
                            break;
                    }

                    return firstOpenChannelNumber;
                }
                /// <summary>
                /// Deletes the channel from the PNA and unmaps from the window (if mapped).
                /// </summary>
                /// <param name="ChannelNumber">Channel number to delete.</param>
                public void Delete(int ChannelNumber)
                {
                    if (_channels.ContainsKey(ChannelNumber)) {
                        if (this[ChannelNumber]._mappedToWindow) {
                            _parentPNAX.Windows[this[ChannelNumber]._mappedWindowNumber].UnMapChannel();
                            this[ChannelNumber]._mappedToWindow = false;
                            this[ChannelNumber]._mappedWindowNumber = -1;
                        }
                        _parentPNAX.WriteString(String.Format("SYSTem:CHANnels:DELete {0}", ChannelNumber));
                        _channels.Remove(ChannelNumber);
                    } else if (OpenChannelNumbers.Contains(ChannelNumber)) {
                        _parentPNAX.WriteString(String.Format("SYSTem:CHANnels:DELete {0}", ChannelNumber));
                    }
                }

            }
            public class WindowClass {

                // Nested Classes
                public class TraceClass {

                    // Variables
                    private WindowClass _parentWindow;
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
                    public int Number
                    { get; private set; }
                    public string MeasurementName
                    { get; private set; }
                    public bool Memory
                    {
                        get { return this.GetMemory(); }
                        set { this.SetMemory(value); }
                    }
                    public bool Display
                    {
                        get { return this.GetDisplay(); }
                        set { this.SetDisplay(value); }
                    }
                    public string Title
                    {
                        get { return this.GetTitle(); }
                        set { this.SetTitle(value); }
                    }
                    public bool TitleState
                    {
                        get { return this.GetTitleState(); }
                        set { this.SetTitleState(value); }
                    }
                    public double PerDivision
                    {
                        get { return this.GetPerDivision(); }
                        set { this.SetPerDivision(value); }
                    }
                    public double MinimumPerDivision
                    {
                        get { return this.GetMinimumPerDivision(); }
                    }
                    public double MaximumPerDivision
                    {
                        get { return this.GetMaximumPerDivision(); }
                    }
                    public double ReferenceLevel
                    {
                        get { return this.GetReferenceLevel(); }
                        set { this.SetReferenceLevel(value); }
                    }
                    public double MinimumReferenceLevel
                    {
                        get { return this.GetMinimumReferenceLevel(); }
                    }
                    public double MaximumReferenceLevel
                    {
                        get { return this.GetMaximumReferenceLevel(); }
                    }
                    public int ReferencePosition
                    {
                        get { return this.GetReferencePosition(); }
                        set { this.SetReferencePosition(value); }
                    }
                    public int MinimumReferencePosition
                    {
                        get { return this.GetMinimumReferencePosition(); }
                    }
                    public int MaximumReferencePosition
                    {
                        get { return this.GetMaximumReferencePosition(); }
                    }

                    // Constructor
                    internal TraceClass(WindowClass ParentWindow, int Number, string MeasurementName)
                    {
                        _parentWindow = ParentWindow;
                        this.Number = Number;
                        this.MeasurementName = MeasurementName;
                    }

                    // Protected Methods
                    protected virtual bool GetMemory()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:MEMory:STATe?", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _memory = (((byte)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _memory;
                    }
                    protected virtual void SetMemory(bool Memory)
                    {
                        _memory = Memory;
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:MEMory:STATe {2}", _parentWindow.Number, Number, _memory ? "ON" : "OFF"));
                    }
                    protected virtual bool GetDisplay()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:STATe?", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _display = (((byte)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _display;
                    }
                    protected virtual void SetDisplay(bool Display)
                    {
                        _display = Display;
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:STATe {2}", _parentWindow.Number, Number, _display ? "ON" : "OFF"));
                    }
                    protected virtual string GetTitle()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:DATA?", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _title = _parentWindow._parentPNAX.ReadString();
                        return _title;
                    }
                    protected virtual void SetTitle(string Title)
                    {
                        _title = Title;
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:DATA {2}", _parentWindow.Number, Number, _title));
                    }
                    protected virtual bool GetTitleState()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:STATe?", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _titleState = (((byte)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _titleState;
                    }
                    protected virtual void SetTitleState(bool TitleState)
                    {
                        _titleState = TitleState;
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:TITLe:STATe {2}", _parentWindow.Number, Number, _titleState ? "ON" : "OFF"));
                    }
                    protected virtual double GetPerDivision()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision?", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _perDivision = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _perDivision;
                    }
                    protected virtual void SetPerDivision(double PerDivision)
                    {
                        _perDivision = PerDivision;
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision {2}", _parentWindow.Number, Number, _perDivision));
                    }
                    protected virtual double GetMinimumPerDivision()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision? MINimum", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _minimumPerDivision = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumPerDivision;
                    }
                    protected virtual double GetMaximumPerDivision()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:PDIVision? MAXimum", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _maximumPerDivision = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumPerDivision;
                    }
                    protected virtual double GetReferenceLevel()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel?", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _referenceLevel = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _referenceLevel;
                    }
                    protected virtual void SetReferenceLevel(double ReferenceLevel)
                    {
                        _referenceLevel = ReferenceLevel;
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel {2}", _parentWindow.Number, Number, _referenceLevel));
                    }
                    protected virtual double GetMinimumReferenceLevel()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel? MINimum", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _minimumReferenceLevel = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumReferenceLevel;
                    }
                    protected virtual double GetMaximumReferenceLevel()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RLEVel? MAXimum", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _maximumReferenceLevel = (double)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumReferenceLevel;
                    }
                    protected virtual int GetReferencePosition()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition?", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _referencePosition = (int)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _referencePosition;
                    }
                    protected virtual void SetReferencePosition(int ReferencePosition)
                    {
                        _referencePosition = ReferencePosition;
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition {2}", _parentWindow.Number, Number, _referencePosition));
                    }
                    protected virtual int GetMinimumReferencePosition()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition? MINimum", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _minimumReferencePosition = (int)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _minimumReferencePosition;
                    }
                    protected virtual int GetMaximumReferencePosition()
                    {
                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:Y:SCALe:RPOSition? MAXimum", _parentWindow.Number, Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        _maximumReferencePosition = (int)_parentWindow._parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                        return _maximumReferencePosition;
                    }

                    // Public Methods
                    public void Select()
                    {
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:SELect", _parentWindow.Number, Number));
                    }
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
                    public int[] OpenTraceNumbers
                    {
                        get { return this.GetOpenTraceNumbers(); }
                    }
                    public int[] ManagedTraceNumbers
                    {
                        get { return this._traces.Keys.ToArray(); }
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
                        List<int> traces = new List<int>();
                        string[] retVals;

                        _parentWindow._parentPNAX.ClearEventRegisters();
                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:CATalog?", _parentWindow.Number));
                        _parentWindow._parentPNAX.WaitForMeasurementToComplete(_parentWindow._parentPNAX.Timeout);
                        retVals = _parentWindow._parentPNAX.ReadString().Split(new char[] { '"', ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string traceNumber in retVals) {
                            traces.Add(int.Parse(traceNumber));
                        }

                        traces.Sort();

                        return traces.ToArray();
                    }

                    // Public Methods
                    public int Add(string MeasurementName)
                    {
                        int firstOpenTraceNumber = 1;
                        List<int> currentTraceNumbers = new List<int>(OpenTraceNumbers);
                        currentTraceNumbers.AddRange(_traces.Keys);

                        currentTraceNumbers = currentTraceNumbers.Distinct().ToList();
                        currentTraceNumbers.Sort();

                        for (int i = 0; i < currentTraceNumbers.Count; i++) {
                            if ((i + 1) != currentTraceNumbers[i]) {
                                firstOpenTraceNumber = i + 1;
                                break;
                            }
                        }

                        _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:FEED {2}", _parentWindow.Number, firstOpenTraceNumber, MeasurementName));
                        _traces.Add(firstOpenTraceNumber, new TraceClass(_parentWindow, firstOpenTraceNumber, MeasurementName));

                        return firstOpenTraceNumber;
                    }
                    public void Delete(int TraceNumber)
                    {
                        if (_traces.ContainsKey(TraceNumber) || OpenTraceNumbers.Contains(TraceNumber)) {
                            _parentWindow._parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe{1}:DELete", _parentWindow.Number, TraceNumber));
                            if (_traces.ContainsKey(TraceNumber)) {
                                _traces.Remove(TraceNumber);
                            }
                        }
                    }

                }

                // Variables
                private PNAX _parentPNAX;
                private ChannelClass _mappedChannel;
                private bool _enabled;
                private bool _scaleCoupling;
                private string _title;
                private bool _titleState;

                // Properties
                /// <summary>
                /// Gets the number of this window.
                /// </summary>
                public int Number
                { get; private set; }
                /// <summary>
                /// Get the channel number of the mapped channel. If no channel is mapped, returns -1.
                /// </summary>
                public int MappedChannelNumber
                {
                    get
                    {
                        if (_mappedChannel != null)
                            return _mappedChannel.Number;
                        else
                            return -1;
                    }
                }
                public bool Enabled
                {
                    get { return this.GetEnabled(); }
                    set { this.SetEnabled(value); }
                }
                public bool ScaleCoupling
                {
                    get { return this.GetScaleCoupling(); }
                    set { this.SetScaleCoupling(value); }
                }
                public string Title
                {
                    get { return this.GetTitle(); }
                    set { this.SetTitle(value); }
                }
                public bool TitleState
                {
                    get { return this.GetTitleState(); }
                    set { this.SetTitleState(value); }
                }
                public TraceCollectionClass Traces
                { get; private set; }

                // Constructor
                public WindowClass(PNAX ParentPNAX, int Number)
                {
                    _parentPNAX = ParentPNAX;
                    this.Number = Number;
                    Traces = new TraceCollectionClass(this);
                    _mappedChannel = null;
                }

                // Protected Methods
                protected virtual bool GetEnabled()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:ENABle?", Number));
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _enabled = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _enabled;
                }
                protected virtual void SetEnabled(bool Enabled)
                {
                    _enabled = Enabled;
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:ENABle {1}", Number, _enabled ? "ON" : "OFF"));
                }
                protected virtual bool GetScaleCoupling()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe:Y:SCALe:COUPle:STATe?", Number));
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _scaleCoupling = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _scaleCoupling;
                }
                protected virtual void SetScaleCoupling(bool ScaleCoupling)
                {
                    _scaleCoupling = ScaleCoupling;
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TRACe:Y:SCALe:COUPle:STATe {1}", Number, _scaleCoupling ? "ON" : "OFF"));
                }
                protected virtual string GetTitle()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:DATA?", Number));
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _title = _parentPNAX.ReadString();
                    return _title;
                }
                protected virtual void SetTitle(string Title)
                {
                    _title = Title;
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:DATA {1}", Number, _title));
                }
                protected virtual bool GetTitleState()
                {
                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:STATe?", Number));
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    _titleState = (((byte)_parentPNAX.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _titleState;
                }
                protected virtual void SetTitleState(bool TitleState)
                {
                    _titleState = TitleState;
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:TITLe:STATe {1}", Number, _titleState ? "ON" : "OFF"));
                }

                // Public Methods
                /// <summary>
                /// Maps the channels measurements to traces in this window.
                /// The maximum traces to display is 24.
                /// </summary>
                /// <param name="Channel">Channel to map to this window.</param>
                /// <returns>True if a channel is successfully mapped.
                /// False if the channel has >24 measurements or if there is already a mapped channel.</returns>
                public virtual bool MapChannel(ChannelClass Channel)
                {
                    if (_mappedChannel != null)
                        return false;

                    // Check that the traces don't exceed maximum number of traces (24) per window or there are no measurements
                    if (Channel.MeasurementNames.Length > 24 || Channel.MeasurementNames.Length == 0)
                        return false;

                    _mappedChannel = Channel;

                    for (int i = 0; i < _mappedChannel.MeasurementNames.Length; i++) {
                        Traces.Add(_mappedChannel.MeasurementNames[i]);
                    }

                    _mappedChannel._mappedToWindow = true;
                    _mappedChannel._mappedWindowNumber = Number;

                    return true;
                }
                /// <summary>
                /// Deletes the traces associated with this window and unmaps the channel from this window.
                /// Note that this does not delete the measurements associated with the channel.
                /// </summary>
                /// <returns>True if successfully unmaps. False if there is nothing to unmap.</returns>
                public virtual bool UnMapChannel()
                {
                    if (_mappedChannel == null)
                        return false;

                    List<int> openTraces = new List<int>(Traces.OpenTraceNumbers);
                    foreach (int openTrace in openTraces) {
                        Traces.Delete(openTrace);
                    }

                    _mappedChannel._mappedToWindow = false;
                    _mappedChannel._mappedWindowNumber = -1;

                    _mappedChannel = null;

                    return true;
                }
                /// <summary>
                /// Autoscales all traces in this window.
                /// </summary>
                public virtual void AutoscaleAllTraces()
                {
                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:Y:AUTO", Number));
                }

            }
            public class WindowCollectionClass {

                // Variables
                private PNAX _parentPNAX;
                private Dictionary<int, WindowClass> _windows;
                private ScaleCouplingMethodEnum _scaleCouplingMethod;

                // Properties
                /// <summary>
                /// The window numbers used by the PNAX, not necessarily created using this driver
                /// </summary>
                public int[] OpenWindowNumbers
                {
                    get { return this.GetOpenWindowNumbers(); }
                }
                /// <summary>
                /// The window numbers managed by this driver instance. There may or may not be more windows on the PNAX created in local mode that are not managed here.
                /// </summary>
                public int[] ManagedWindowNumbers
                {
                    get { return this._windows.Keys.ToArray(); }
                }
                public ScaleCouplingMethodEnum ScaleCouplingMethod
                {
                    get { return this.GetScaleCouplingMethod(); }
                    set { this.SetScaleCouplingMethod(value); }
                }

                // Constructor
                public WindowCollectionClass(PNAX ParentPNAX)
                {
                    _parentPNAX = ParentPNAX;
                    _windows = new Dictionary<int, WindowClass>();
                }

                // Indexer
                /// <summary>
                /// Access the window by the Window Number.
                /// </summary>
                /// <param name="WindowNumber">Window number trying to access (lowest is 1)</param>
                /// <returns>The window given by the window number.</returns>
                public WindowClass this[int WindowNumber]
                {
                    get
                    {
                        if (!_windows.ContainsKey(WindowNumber))
                            throw new System.ArgumentException("Window does not exist", "WindowNumber");

                        return _windows[WindowNumber];
                    }
                }

                // Protected Methods
                protected virtual int[] GetOpenWindowNumbers()
                {
                    List<int> windows = new List<int>();
                    string[] retVals;

                    _parentPNAX.ClearEventRegisters();
                    _parentPNAX.WriteString("SYSTem:WINDows:CATalog?");
                    _parentPNAX.WaitForMeasurementToComplete(_parentPNAX.Timeout);
                    retVals = _parentPNAX.ReadString().Split(new char[] { '"', ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string windowNumber in retVals) {
                        windows.Add(int.Parse(windowNumber));
                    }

                    windows.Sort();

                    return windows.ToArray();
                }
                protected virtual ScaleCouplingMethodEnum GetScaleCouplingMethod()
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
                protected virtual void SetScaleCouplingMethod(ScaleCouplingMethodEnum ScaleCouplingMethod)
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
                public int Add()
                {
                    int firstOpenWindowNumber = 1;
                    List<int> currentWindowNumbers = new List<int>(OpenWindowNumbers);
                    currentWindowNumbers.AddRange(_windows.Keys);

                    currentWindowNumbers = currentWindowNumbers.Distinct().ToList();
                    currentWindowNumbers.Sort();

                    for (int i = 0; i < currentWindowNumbers.Count; i++) {
                        if ((i + 1) != currentWindowNumbers[i]) {
                            firstOpenWindowNumber = i + 1;
                            break;
                        }
                    }

                    _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:STATe ON", firstOpenWindowNumber));
                    _windows.Add(firstOpenWindowNumber, new WindowClass(_parentPNAX, firstOpenWindowNumber));

                    return firstOpenWindowNumber;
                }
                public void Delete(int WindowNumber)
                {
                    if (_windows.ContainsKey(WindowNumber)) {
                        this[WindowNumber].UnMapChannel();
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:STATe OFF", WindowNumber));
                        _windows.Remove(WindowNumber);
                    } else if (OpenWindowNumbers.Contains(WindowNumber)) {
                        _parentPNAX.WriteString(String.Format("DISPlay:WINDow{0}:STATe OFF", WindowNumber));
                    }
                }
            }

            // Enumerations
            // Trigger
            public enum TriggerSourceEnum { Immediate, External, Manual };
            public enum TriggerScopeEnum { All, Current };
            public enum ExternalTriggerTypeEnum { Edge, Level };
            public enum ExternalTriggerSlopeEnum { Positive, Negative };
            // Measurement
            public enum MeasurementClassEnum { SParameters };
            public enum MeasurementFormatEnum { Linear, Logarithmic, Phase, UnwrappedPhase, Imaginary, Real, Polar, Smith, SmithAdmittance, SWR };
            // Trace Scaling
            public enum ScaleCouplingMethodEnum { Off, Window, All };
            // Noise
            public enum NoiseReceiverGainEnum { Low, Medium, High };
            // Data
            public enum DataFormatEnum { ASCii, Real32, Real64 };
            public enum DataAccessMapEnum { FDATA, RDATA, SDATA };

            // Variables
            private bool _displayState;
            private bool _rfOutputState;
            private DataFormatEnum _dataFormat;

            // Properties
            public TriggerClass Trigger
            { get; private set; }
            public ChannelCollectionClass Channels
            { get; private set; }
            public WindowCollectionClass Windows
            { get; private set; }
            public bool DisplayState
            {
                get { return this.GetDisplayState(); }
                set { this.SetDisplayState(value); }
            }
            public bool RFOutputState
            {
                get { return this.GetRFOutputState(); }
                set { this.SetRFOutputState(value); }
            }
            public DataFormatEnum DataFormat
            {
                get { return this.GetDataFormat(); }
                set { this.SetDataFormat(value); }
            }

            // Constructor
            public PNAX()
                : base()
            {
                Trigger = new TriggerClass(this);
                Channels = new ChannelCollectionClass(this);
                Windows = new WindowCollectionClass(this);
            }


            // Protected Methods
            protected virtual bool GetDisplayState()
            {
                ClearEventRegisters();
                WriteString("DISPlay:VISible?");
                WaitForMeasurementToComplete(Timeout);
                _displayState = (((byte)ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                return _displayState;
            }
            protected virtual void SetDisplayState(bool DisplayState)
            {
                _displayState = DisplayState;
                WriteString(String.Format("DISPlay:VISible {0}", _displayState ? "ON" : "OFF"));
            }
            protected virtual bool GetRFOutputState()
            {
                ClearEventRegisters();
                WriteString("OUTput:STATe?");
                WaitForMeasurementToComplete(Timeout);
                _rfOutputState = (((byte)ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                return _rfOutputState;
            }
            protected virtual void SetRFOutputState(bool RFOutputState)
            {
                _rfOutputState = RFOutputState;
                WriteString(String.Format("OUTPut:STATe {0}", _rfOutputState ? "ON" : "OFF"));
            }
            protected virtual DataFormatEnum GetDataFormat()
            {
                string retVal;
                ClearEventRegisters();
                WriteString("FORMat:DATA?");
                WaitForMeasurementToComplete(Timeout);
                retVal = ReadString();
                if (retVal.Contains("REAL")) {
                    if (retVal.Contains("64")) {
                        _dataFormat = DataFormatEnum.Real64;
                    } else {
                        _dataFormat = DataFormatEnum.Real32;
                    }
                } else {
                    _dataFormat = DataFormatEnum.ASCii;
                }
                return _dataFormat;
            }
            protected virtual void SetDataFormat(DataFormatEnum DataFormat)
            {
                _dataFormat = DataFormat;
                switch (_dataFormat) {
                    case DataFormatEnum.ASCii:
                        WriteString("FORMat:DATA ASCii,0");
                        break;
                    case DataFormatEnum.Real32:
                        WriteString("FORMat:DATA REAL,32");
                        break;
                    case DataFormatEnum.Real64:
                        WriteString("FORMat:DATA REAL,64");
                        break;
                }
            }

            // Public Methods
            public Bitmap GetScreenCapture()
            {
                // Print all windows in bitmap format
                WriteString("HCOPy:ITEM:AWINdow:STATe OFF");
                WriteString("HCOPy:SDUMp:DATA:FORMat BMP");
                ClearEventRegisters();
                WriteString("HCOPy:SDUMp:DATA?");
                WaitForMeasurementToComplete(Timeout);
                return new Bitmap((Image)new ImageConverter().ConvertFrom((Byte[])ReadIEEEBlock(IEEEBinaryType.BinaryType_UI1, true, true)));
            }
            public override void Reset()
            {
                WriteString("SYSTem:FPReset");
            }
        }

    }

}
