using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ivi.Visa.Interop;

namespace SCPI {

    namespace PulseGeneratorDrivers {

        public class PulseGenerator : SCPIDriver {

            // Nested Classes
            public class TriggerClass {
                
                // Variables
                private PulseGenerator _parentPulseGenerator;
                private TriggerSourceEnum _source;
                private int _count;
                private ExternalTriggerSenseEnum _externalTriggerSense;
                private ExternalTriggerSlopeEnum _externalTriggerSlope;
                private ExternalTriggerInputImpedanceEnum _externalTriggerInputImpedance;

                // Properties
                public TriggerSourceEnum Source
                {
                    get { return this.GetSource(); }
                    set { this.SetSource(value); }
                }
                public int Count
                {
                    get { return this.GetCount(); }
                    set { this.SetCount(value); }
                }
                public ExternalTriggerSenseEnum ExternalTriggerSense
                {
                    get { return this.GetExternalTriggerSense(); }
                    set { this.SetExternalTriggerSense(value); }
                }
                public ExternalTriggerSlopeEnum ExternalTriggerSlope
                {
                    get { return this.GetExternalTriggerSlope(); }
                    set { this.SetExternalTriggerSlope(value); }
                }
                public ExternalTriggerInputImpedanceEnum ExternalTriggerInputImpedance
                {
                    get { return this.GetExternalTriggerInputImpedance(); }
                    set { this.SetExternalTriggerInputImpedance(value); }
                }

                // Constructor
                internal TriggerClass(PulseGenerator ParentPulseGenerator)
                {
                    _parentPulseGenerator = ParentPulseGenerator;
                }

                // Protected Methods
                protected virtual TriggerSourceEnum GetSource()
                {
                    string retVal;
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SOURce?");
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    retVal = _parentPulseGenerator.ReadString();
                    if (retVal.Contains("IMM")) {
                        _source = TriggerSourceEnum.Immediate;
                    } else if (retVal.Contains("MAN")) {
                        _source = TriggerSourceEnum.Manual;
                    } else if (retVal.Contains("EXT")) {
                        _source = TriggerSourceEnum.External;
                    } else {
                        _source = TriggerSourceEnum.Internal;
                    }
                    return _source;
                }
                protected virtual void SetSource(TriggerSourceEnum Source)
                {
                    _source = Source;
                    switch (_source) {
                        case TriggerSourceEnum.Immediate:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SOURce IMMediate");
                            break;
                        case TriggerSourceEnum.Internal:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SOURce INTernal2");
                            break;
                        case TriggerSourceEnum.External:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SOURce EXTernal");
                            break;
                        case TriggerSourceEnum.Manual:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SOURce MANual");
                            break;
                    }
                }
                protected virtual int GetCount()
                {
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString("TRIGger:SEQuence:COUNt?");
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    _count = (int)_parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
                    return _count;
                }
                protected virtual void SetCount(int Count)
                {
                    if (Count < 1)
                        throw new System.ArgumentException("Trigger count must be greather than 0.", "Count");

                    _count = Count;
                    _parentPulseGenerator.WriteString(String.Format("TRIGger:SEQuence:COUNt {0}", _count));
                }
                protected virtual ExternalTriggerSenseEnum GetExternalTriggerSense()
                {
                    string retVal;
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SENSe?");
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    retVal = _parentPulseGenerator.ReadString();
                    if (retVal.Contains("EDGE")) {
                        _externalTriggerSense = ExternalTriggerSenseEnum.Edge;
                    } else {
                        _externalTriggerSense = ExternalTriggerSenseEnum.Level;
                    }
                    return _externalTriggerSense;
                }
                protected virtual void SetExternalTriggerSense(ExternalTriggerSenseEnum ExternalTriggerSense)
                {
                    _externalTriggerSense = ExternalTriggerSense;
                    switch (_externalTriggerSense) {
                        case ExternalTriggerSenseEnum.Edge:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SENSe EDGE");
                            break;
                        case ExternalTriggerSenseEnum.Level:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SENSe LEVel");
                            break;
                    }
                }
                protected virtual ExternalTriggerSlopeEnum GetExternalTriggerSlope()
                {
                    string retVal;
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SLOPe?");
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    retVal = _parentPulseGenerator.ReadString();
                    if (retVal.Contains("POS")) {
                        _externalTriggerSlope = ExternalTriggerSlopeEnum.Positive;
                    } else if (retVal.Contains("NEG")) {
                        _externalTriggerSlope = ExternalTriggerSlopeEnum.Negative;
                    } else {
                        _externalTriggerSlope = ExternalTriggerSlopeEnum.Either;
                    }
                    return _externalTriggerSlope;
                }
                protected virtual void SetExternalTriggerSlope(ExternalTriggerSlopeEnum ExternalTriggerSlope)
                {
                    _externalTriggerSlope = ExternalTriggerSlope;
                    switch (_externalTriggerSlope) {
                        case ExternalTriggerSlopeEnum.Positive:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SLOPe POSitive");
                            break;
                        case ExternalTriggerSlopeEnum.Negative:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SLOPe NEGative");
                            break;
                        case ExternalTriggerSlopeEnum.Either:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:SLOPe EITHer");
                            break;
                    }
                }
                protected virtual ExternalTriggerInputImpedanceEnum GetExternalTriggerInputImpedance()
                {
                    double retVal;
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:IMPedance?");
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    retVal = (double)_parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    if (49 < retVal && retVal < 51) {
                        _externalTriggerInputImpedance = ExternalTriggerInputImpedanceEnum._50OHM;
                    } else {
                        _externalTriggerInputImpedance = ExternalTriggerInputImpedanceEnum._10KOHM;
                    }
                    return _externalTriggerInputImpedance;
                }
                protected virtual void SetExternalTriggerInputImpedance(ExternalTriggerInputImpedanceEnum ExternalTriggerInputImpedance)
                {
                    _externalTriggerInputImpedance = ExternalTriggerInputImpedance;
                    switch (_externalTriggerInputImpedance) {
                        case ExternalTriggerInputImpedanceEnum._50OHM:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:IMPedance 50OHM");
                            break;
                        case ExternalTriggerInputImpedanceEnum._10KOHM:
                            _parentPulseGenerator.WriteString("ARM:SEQuence:LAYer:IMPedance 10KOHM");
                            break;
                    }
                }

                // Public Methods
                public virtual void SendManualTrigger()
                {
                    _parentPulseGenerator.WriteString("*TRG");
                }
            }
            public class OutputClass {

                // Nested Classes
                public class VoltageClass {

                    // Variables
                    private OutputClass _parentOutput;
                    private double _amplitude;
                    private double _offset;
                    private double _highLevel;
                    private double _lowLevel;
                    private double _highLevelLimit;
                    private double _lowLevelLimit;
                    private bool _levelLimiting;
                    private double _maximumAmplitude;
                    private double _minimumAmplitude;
                    private double _maximumOffset;
                    private double _minimumOffset;
                    private double _maximumHighLevel;
                    private double _minimumHighLevel;
                    private double _maximumLowLevel;
                    private double _minimumLowLevel;

                    // Properties
                    public double Amplitude
                    {
                        get { return this.GetAmplitude(); }
                        set { this.SetAmplitude(value); }
                    }
                    public double Offset
                    {
                        get { return this.GetOffset(); }
                        set { this.SetOffset(value); }
                    }
                    public double HighLevel
                    {
                        get { return this.GetHighLevel(); }
                        set { this.SetHighLevel(value); }
                    }
                    public double LowLevel
                    {
                        get { return this.GetLowLevel(); }
                        set { this.SetLowLevel(value); }
                    }
                    public double HighLevelLimit
                    {
                        get { return this.GetHighLevelLimit(); }
                        set { this.SetHighLevelLimit(value); }
                    }
                    public double LowLevelLimit
                    {
                        get { return this.GetLowLevelLimit(); }
                        set { this.SetLowLevelLimit(value); }
                    }
                    public bool LevelLimiting
                    {
                        get { return this.GetLevelLimiting(); }
                        set { this.SetLevelLimiting(value); }
                    }
                    public double MaximumAmplitude
                    {
                        get { return this.GetMaximumAmplitude(); }
                    }
                    public double MinimumAmplitude
                    {
                        get { return this.GetMinimumAmplitude(); }
                    }
                    public double MaximumOffset
                    {
                        get { return this.GetMaximumOffset(); }
                    }
                    public double MinimumOffset
                    {
                        get { return this.GetMinimumOffset(); }
                    }
                    public double MaximumHighLevel
                    {
                        get { return this.GetMaximumHighLevel(); }
                    }
                    public double MinimumHighLevel
                    {
                        get { return this.GetMinimumHighLevel(); }
                    }
                    public double MaximumLowLevel
                    {
                        get { return this.GetMaximumLowLevel(); }
                    }
                    public double MinimumLowLevel
                    {
                        get { return this.GetMinimumLowLevel(); }
                    }

                    // Constructor
                    internal VoltageClass(OutputClass ParentOutput)
                    {
                        _parentOutput = ParentOutput;
                    }

                    // Protected Methods
                    protected virtual double GetAmplitude()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:AMPlitude?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _amplitude = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _amplitude;
                    }
                    protected virtual void SetAmplitude(double Amplitude)
                    {
                        _amplitude = Amplitude;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:AMPlitude {1}", _parentOutput._number, _amplitude));
                    }
                    protected virtual double GetOffset()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:OFFset?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _offset = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _offset;
                    }
                    protected virtual void SetOffset(double Offset)
                    {
                        _offset = Offset;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:OFFset {1}", _parentOutput._number, _offset));
                    }
                    protected virtual double GetHighLevel()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:HIGH?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _highLevel = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _highLevel;
                    }
                    protected virtual void SetHighLevel(double HighLevel)
                    {

                        _highLevel = HighLevelLimit;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:HIGH {1}", _parentOutput._number, _highLevel));
                    }
                    protected virtual double GetLowLevel()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:LOW?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _lowLevel = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _lowLevel;
                    }
                    protected virtual void SetLowLevel(double LowLevel)
                    {

                        _lowLevel = LowLevel;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:LOW {1}", _parentOutput._number, _lowLevel));
                    }
                    protected virtual double GetHighLevelLimit()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LIMit:HIGH?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _highLevelLimit = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _highLevelLimit;
                    }
                    protected virtual void SetHighLevelLimit(double HighLevelLimit)
                    {

                        _highLevelLimit = HighLevelLimit;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LIMit:HIGH {1}", _parentOutput._number, _highLevelLimit));
                    }
                    protected virtual double GetLowLevelLimit()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LIMit:LOW?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _lowLevelLimit = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _lowLevelLimit;
                    }
                    protected virtual void SetLowLevelLimit(double LowLevelLimit)
                    {

                        _lowLevelLimit = LowLevelLimit;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LIMit:LOW {1}", _parentOutput._number, _lowLevelLimit));
                    }
                    protected virtual bool GetLevelLimiting()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LIMit:STATe?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _levelLimiting = (((byte)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _levelLimiting;
                    }
                    protected virtual void SetLevelLimiting(bool LevelLimiting)
                    {
                        _levelLimiting = LevelLimiting;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LIMit:STATe {1}", _parentOutput._number, _levelLimiting ? "ON" : "OFF"));
                    }
                    protected virtual double GetMaximumAmplitude()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:AMPlitude? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumAmplitude = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumAmplitude;
                    }
                    protected virtual double GetMinimumAmplitude()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:AMPlitude? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumAmplitude = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumAmplitude;
                    }
                    protected virtual double GetMaximumOffset()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:OFFset? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumOffset = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumOffset;
                    }
                    protected virtual double GetMinimumOffset()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:OFFset? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumOffset = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumOffset;
                    }
                    protected virtual double GetMaximumHighLevel()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:HIGH? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumHighLevel = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumHighLevel;
                    }
                    protected virtual double GetMinimumHighLevel()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:HIGH? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumHighLevel = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumHighLevel;
                    }
                    protected virtual double GetMaximumLowLevel()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:LOW? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumLowLevel = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumLowLevel;
                    }
                    protected virtual double GetMinimumLowLevel()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:VOLTage{0}:LEVel:IMMediate:LOW? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumLowLevel = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumLowLevel;
                    }
                }
                public class PulseClass {
                    
                    // Variables
                    private OutputClass _parentOutput;
                    private PulseHoldEnum _pulseHold;
                    private double _dutyCycle;
                    private double _width;
                    private double _period;
                    private double _delay;
                    private double _leadingEdge;
                    private double _trailingEdge;
                    private bool _coupledEdges;
                    private double _maximumDutyCycle;
                    private double _minimumDutyCycle;
                    private double _maximumWidth;
                    private double _minimumWidth;
                    private double _maximumPeriod;
                    private double _minimumPeriod;
                    private double _maximumDelay;
                    private double _minimumDelay;
                    private double _maximumLeadingEdge;
                    private double _minimumLeadingEdge;
                    private double _maximumTrailingEdge;
                    private double _minimumTrailingEdge;

                    // Properties
                    public PulseHoldEnum Hold
                    {
                        get { return this.GetHold(); }
                        set { this.SetHold(value); }
                    }
                    public double DutyCycle
                    {
                        get { return this.GetDutyCycle(); }
                        set { this.SetDutyCycle(value); }
                    }
                    public double Width
                    {
                        get { return this.GetWidth(); }
                        set { this.SetWidth(value); }
                    }
                    public double Period
                    {
                        get { return this.GetPeriod(); }
                        set { this.SetPeriod(value); }
                    }
                    public double Delay
                    {
                        get { return this.GetDelay(); }
                        set { this.SetDelay(value); }
                    }
                    public double LeadingEdge
                    {
                        get { return this.GetLeadingEdge(); }
                        set { this.SetLeadingEdge(value); }
                    }
                    public double TrailingEdge
                    {
                        get { return this.GetTrailingEdge(); }
                        set { this.SetTrailingEdge(value); }
                    }
                    public bool CoupledEdges
                    {
                        get { return this.GetCoupledEdges(); }
                        set { this.SetCoupledEdges(value); }
                    }
                    public double MaximumDutyCycle
                    {
                        get { return this.GetMaximumDutyCycle(); }
                    }
                    public double MinimumDutyCycle
                    {
                        get { return this.GetMinimumDutyCycle(); }
                    }
                    public double MaximumWidth
                    {
                        get { return this.GetMaximumWidth(); }
                    }
                    public double MinimumWidth
                    {
                        get { return this.GetMinimumWidth(); }
                    }
                    public double MaximumPeriod
                    {
                        get { return this.GetMaximumPeriod(); }
                    }
                    public double MinimumPeriod
                    {
                        get { return this.GetMinimumPeriod(); }
                    }
                    public double MaximumDelay
                    {
                        get { return this.GetMaximumDelay(); }
                    }
                    public double MinimumDelay
                    {
                        get { return this.GetMinimumDelay(); }
                    }
                    public double MaximumLeadingEdge
                    {
                        get { return this.GetMaximumLeadingEdge(); }
                    }
                    public double MinimumLeadingEdge
                    {
                        get { return this.GetMinimumLeadingEdge(); }
                    }
                    public double MaximumTrailingEdge
                    {
                        get { return this.GetMaximumTrailingEdge(); }
                    }
                    public double MinimumTrailingEdge
                    {
                        get { return this.GetMinimumTrailingEdge(); }
                    }

                    // Constructor
                    internal PulseClass(OutputClass ParentOutput)
                    {
                        _parentOutput = ParentOutput;
                    }

                    // Protected Methods
                    protected virtual PulseHoldEnum GetHold()
                    {
                        string retVal;
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:HOLD{0}?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        retVal = _parentOutput._parentPulseGenerator.ReadString();
                        if (retVal.Contains("WIDT")) {
                            _pulseHold = PulseHoldEnum.Width;
                        } else if (retVal.Contains("DCYC")) {
                            _pulseHold = PulseHoldEnum.DutyCycle;
                        } else {
                            _pulseHold = PulseHoldEnum.TrailingEdgeDelay;
                        }
                        return _pulseHold;
                    }
                    protected virtual void SetHold(PulseHoldEnum PulseHold)
                    {
                        _pulseHold = PulseHold;
                        switch (_pulseHold) {
                            case PulseHoldEnum.Width:
                                _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:HOLD{0} WIDTh", _parentOutput._number));
                                break;
                            case PulseHoldEnum.DutyCycle:
                                _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:HOLD{0} DCYCle", _parentOutput._number));
                                break;
                            case PulseHoldEnum.TrailingEdgeDelay:
                                _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:HOLD{0} TDELay", _parentOutput._number));
                                break;
                        }
                    }
                    protected virtual double GetDutyCycle()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DCYCLe{0}?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _dutyCycle = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _dutyCycle;
                    }
                    protected virtual void SetDutyCycle(double DutyCycle)
                    {
                        _dutyCycle = DutyCycle;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DCYCle{0} {1}", _parentOutput._number, _dutyCycle));
                    }
                    protected virtual double GetWidth()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:WIDTh{0}?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _width = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _width;
                    }
                    protected virtual void SetWidth(double Width)
                    {
                        _width = Width;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:WIDTh{0} {1}", _parentOutput._number, _width));
                    }
                    protected virtual double GetPeriod()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:PERiod{0}?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _period = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _period;
                    }
                    protected virtual void SetPeriod(double Period)
                    {
                        _period = Period;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:PERiod{0} {1}", _parentOutput._number, _period));
                    }
                    protected virtual double GetDelay()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DELay{0}?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _delay = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _delay;
                    }
                    protected virtual void SetDelay(double Delay)
                    {
                        _delay = Delay;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DELay{0} {1}", _parentOutput._number, _delay));
                    }
                    protected virtual double GetLeadingEdge()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:LEADing?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _leadingEdge = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _leadingEdge;
                    }
                    protected virtual void SetLeadingEdge(double LeadingEdge)
                    {
                        _leadingEdge = LeadingEdge;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:LEADing {1}", _parentOutput._number, _leadingEdge));
                    }
                    protected virtual double GetTrailingEdge()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:TRAiling?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _trailingEdge = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _trailingEdge;
                    }
                    protected virtual void SetTrailingEdge(double TrailingEdge)
                    {
                        _trailingEdge = TrailingEdge;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:TRAiling {1}", _parentOutput._number, _trailingEdge));
                    }
                    protected virtual bool GetCoupledEdges()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:TRAiling:AUTO?", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _coupledEdges = (((byte)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                        return _coupledEdges;
                    }
                    protected virtual void SetCoupledEdges(bool CoupledEdges)
                    {
                        _coupledEdges = CoupledEdges;
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:TRAiling:AUTO {1}", _parentOutput._number, _coupledEdges ? "ON" : "OFF"));
                    }
                    protected virtual double GetMaximumDutyCycle()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DCYCLe{0}? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumDutyCycle = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumDutyCycle;
                    }
                    protected virtual double GetMinimumDutyCycle()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DCYCLe{0}? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumDutyCycle = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumDutyCycle;
                    }
                    protected virtual double GetMaximumWidth()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:WIDTh{0}? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumWidth = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumWidth;
                    }
                    protected virtual double GetMinimumWidth()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:WIDTh{0}? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumWidth = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumWidth;
                    }
                    protected virtual double GetMaximumPeriod()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:PERiod{0}? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumPeriod = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumPeriod;
                    }
                    protected virtual double GetMinimumPeriod()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:PERiod{0}? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumPeriod = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumPeriod;
                    }
                    protected virtual double GetMaximumDelay()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DELay{0}? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumDelay = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumDelay;
                    }
                    protected virtual double GetMinimumDelay()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:DELay{0}? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumDelay = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumDelay;
                    }
                    protected virtual double GetMaximumLeadingEdge()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:LEADing? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumLeadingEdge = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumLeadingEdge;
                    }
                    protected virtual double GetMinimumLeadingEdge()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:LEADing? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumLeadingEdge = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumLeadingEdge;
                    }
                    protected virtual double GetMaximumTrailingEdge()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:TRAiling? MAXimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _maximumTrailingEdge = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _maximumTrailingEdge;
                    }
                    protected virtual double GetMinimumTrailingEdge()
                    {
                        _parentOutput._parentPulseGenerator.ClearEventRegisters();
                        _parentOutput._parentPulseGenerator.WriteString(String.Format("SOURce:PULSe:TRANsition{0}:TRAiling? MINimum", _parentOutput._number));
                        _parentOutput._parentPulseGenerator.WaitForMeasurementToComplete(_parentOutput._parentPulseGenerator.Timeout);
                        _minimumTrailingEdge = (double)_parentOutput._parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                        return _minimumTrailingEdge;
                    }

                }

                // Variables
                private PulseGenerator _parentPulseGenerator;
                private int _number;
                private bool _state;
                private OutputImpedanceEnum _impedance;
                private double _expectedImpedance;
                private OutputPolarityEnum _polarity;

                // Properties
                public int Number
                {
                    get { return _number; }
                }
                public bool State
                {
                    get { return this.GetState(); }
                    set { this.SetState(value); }
                }
                public OutputImpedanceEnum OutputImpedance
                {
                    get { return this.GetImpedance(); }
                    set { this.SetImpedance(value); }
                }
                public double ExpectedImpedance
                {
                    get { return this.GetExpectedImpedance(); }
                    set { this.SetExpectedImpedance(value); }
                }
                public OutputPolarityEnum Polarity
                {
                    get { return this.GetPolarity(); }
                    set { this.SetPolarity(value); }
                }
                public VoltageClass Voltage
                { get; private set; }

                // Constructor
                internal OutputClass(int Number, PulseGenerator ParentPulseGenerator)
                {
                    if (Number < 0)
                        throw new System.ArgumentException("The output number must be greater than 0.", "Number");
                    _number = Number;
                    _parentPulseGenerator = ParentPulseGenerator;
                    Voltage = new VoltageClass(this);
                }

                // Protected Methods
                protected virtual bool GetState()
                {
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:NORMal:STATe?", _number));
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    _state = (((byte)_parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                    return _state;
                }
                protected virtual void SetState(bool State)
                {
                    _state = State;
                    _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:NORMal:STATe {1}", _number, _state ? "ON" : "OFF"));
                }
                protected virtual OutputImpedanceEnum GetImpedance()
                {
                    double retVal;
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:IMPedance:INTernal?", _number));
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    retVal = (double)_parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    if (49 < retVal && retVal < 51) {
                        _impedance = OutputImpedanceEnum._50OHM;
                    } else {
                        _impedance = OutputImpedanceEnum._1KOHM;
                    }
                    return _impedance;
                }
                protected virtual void SetImpedance(OutputImpedanceEnum Impedance)
                {
                    _impedance = Impedance;
                    switch (_impedance) {
                        case OutputImpedanceEnum._50OHM:
                            _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:IMPedance:INTernal 50OHM", _number));
                            break;
                        case OutputImpedanceEnum._1KOHM:
                            _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:IMPedance:INTernal 1KOHM", _number));
                            break;
                    }
                }
                protected virtual double GetExpectedImpedance()
                {
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:IMPedance:EXTernal?", _number));
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    _expectedImpedance = (double)_parentPulseGenerator.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _expectedImpedance;
                }
                protected virtual void SetExpectedImpedance(double ExpectedImpedance)
                {
                    _expectedImpedance = ExpectedImpedance;
                    _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:IMPedance:EXTernal {1}", _number, _expectedImpedance));
                }
                protected virtual OutputPolarityEnum GetPolarity()
                {
                    string retVal;
                    _parentPulseGenerator.ClearEventRegisters();
                    _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:POLarity?", _number));
                    _parentPulseGenerator.WaitForMeasurementToComplete(_parentPulseGenerator.Timeout);
                    retVal = _parentPulseGenerator.ReadString();
                    if (retVal.Contains("NORM")) {
                        _polarity = OutputPolarityEnum.Normal;
                    } else {
                        _polarity = OutputPolarityEnum.Inverted;
                    }
                    return _polarity;
                }
                protected virtual void SetPolarity(OutputPolarityEnum Polarity)
                {
                    _polarity = Polarity;
                    switch (_polarity) {
                        case OutputPolarityEnum.Normal:
                            _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:POLarity NORMal", _number));
                            break;
                        case OutputPolarityEnum.Inverted:
                            _parentPulseGenerator.WriteString(String.Format("OUTPut{0}:POLarity INVerted", _number));
                            break;
                    }
                }
            }
            public class OutputCollectionClass : IEnumerable<OutputClass> {

                // Variables
                private PulseGenerator _parentPulseGenerator;
                private Dictionary<int, OutputClass> _outputs;

                // Properties
                public int Count
                {
                    get { return this._outputs.Count; }
                }
                public int[] Numbers
                {
                    get { return this._outputs.Keys.ToArray(); }
                }

                // Constructor
                internal OutputCollectionClass(PulseGenerator ParentPulseGenerator)
                {
                    _parentPulseGenerator = ParentPulseGenerator;
                    _outputs = new Dictionary<int, OutputClass>();
                }

                // Indexer
                public OutputClass this[int Number]
                {
                    get
                    {
                        if (!_outputs.ContainsKey(Number))
                            throw new System.ArgumentException("Output doesn't exist at this number.", "Number");

                        return _outputs[Number];
                    }
                }

                // Public Methods
                public bool Add(int Number)
                {
                    if (_outputs.ContainsKey(Number))
                        return false;

                    _outputs.Add(Number, new OutputClass(Number, _parentPulseGenerator));
                    return true;
                }
                public bool Delete(int Number)
                {
                    if (!_outputs.ContainsKey(Number))
                        return false;

                    _outputs.Remove(Number);
                    return true;
                }
                public IEnumerator<OutputClass> GetEnumerator()
                {
                    return _outputs.Values.GetEnumerator();
                }

                // Unused Interface Methods
                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            // Enumerations
                // Trigger
            public enum TriggerSourceEnum { Immediate, Internal, External, Manual };
            public enum ExternalTriggerSenseEnum { Edge, Level };
            public enum ExternalTriggerSlopeEnum { Positive, Negative, Either };
            public enum ExternalTriggerInputImpedanceEnum { _50OHM, _10KOHM };
                // Output
            public enum OutputImpedanceEnum { _50OHM, _1KOHM };
            public enum OutputPolarityEnum { Normal, Inverted };
                // Pulse
            public enum PulseHoldEnum { Width, DutyCycle, TrailingEdgeDelay };

            // Variables
            private bool _displayState;

            // Properties
            public OutputCollectionClass Outputs
            { get; private set; }
            public TriggerClass Trigger
            { get; private set; }
            public bool DisplayState
            {
                get { return this.GetDisplayState(); }
                set { this.SetDisplayState(value); }
            }

            // Constructors
            public PulseGenerator()
                : base()
            {
                Outputs = new OutputCollectionClass(this);
                Trigger = new TriggerClass(this);
            }
            public PulseGenerator(int[] OutputNumbers)
                : this()
            {
                foreach (int OutputNumber in OutputNumbers) {
                    if (Outputs.Add(OutputNumber) == false)
                        throw new System.ArgumentException("Output Number already exists! Failed to create output.", "OutputNumbers");
                }
            }

            // Protected Methods
            protected virtual bool GetDisplayState()
            {
                ClearEventRegisters();
                WriteString("DISPlay:WINDow:STATe?");
                WaitForMeasurementToComplete(Timeout);
                _displayState = (((byte)ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                return _displayState;
            }
            protected virtual void SetDisplayState(bool DisplayState)
            {
                _displayState = DisplayState;
                WriteString(String.Format("DISPlay:WINDow:STATe {0}", _displayState ? "ON" : "OFF"));
            }

        }

    }

}
