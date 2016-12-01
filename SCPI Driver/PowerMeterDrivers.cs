using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ivi.Visa.Interop;

namespace SCPI {

    namespace PowerMeterDrivers {

        public class Gigatronics854x : SCPIDriver {

            // Nested Classes
            public class DisplayClass {

                // Variables
                private Gigatronics854x _parentGigatronics854x;
                private bool _state;
                
                // Properties
                public bool State
                {
                    get { return this._state; }
                    set { this.SetState(value); }
                }

                // Constructor
                internal DisplayClass(Gigatronics854x ParentGigatronics854x)
                {
                    _parentGigatronics854x = ParentGigatronics854x;
                }

                // Internal Methods
                internal void Reset()
                {
                    _state = true;
                }

                // Private Methods
                private void SetState(bool State)
                {
                    _state = State;
                    _parentGigatronics854x.WriteString(String.Format("D{0}", _state ? 'E' : 'D'));
                }

                // Public Methods
                public void ShowMessage(string Message)
                {
                    if(Message.Length > 16)
                        Message = Message.Substring(0, 16);

                    _parentGigatronics854x.WriteString(String.Format("DU {0}", Message));
                }
                public void Test()
                {
                    _parentGigatronics854x.WriteString("DA");
                }
            }
            public class TriggerClass {

                // Variables
                private Gigatronics854x _parentGigatronics854x;

                // Constructor
                internal TriggerClass(Gigatronics854x ParentGigatronics854x)
                {
                    _parentGigatronics854x = ParentGigatronics854x;
                }

                // Public Methods
                public void Send(TriggerTypeEnum TriggerType) {
                    switch (TriggerType) {
                        case TriggerTypeEnum.Hold:
                            _parentGigatronics854x.WriteString("TR0");
                            break;
                        case TriggerTypeEnum.ImmediateSingle:
                            _parentGigatronics854x.WriteString("TR1");
                            break;
                        case TriggerTypeEnum.ImmediateAveraging:
                            _parentGigatronics854x.WriteString("TR2");
                            break;
                        case TriggerTypeEnum.FreeRun:
                            _parentGigatronics854x.WriteString("TR3");
                            break;
                    }
                }

            }
            public class SensorClass {
                
                // Nested Classes
                public class AveragingClass {

                    // Variables
                    private SensorClass _parentSensor;
                    private bool _auto;
                    private SensorAveragingCountEnum _count;

                    // Properties
                    public bool Auto
                    {
                        get { return this._auto; }
                        set { this.SetAuto(value); }
                    }
                    public SensorAveragingCountEnum Count
                    {
                        get { return this._count; }
                        set { this.SetCount(value); }
                    }

                    // Constructor
                    internal AveragingClass(SensorClass ParentSensor)
                    {
                        _parentSensor = ParentSensor;
                    }

                    // Internal Methods
                    internal void Reset()
                    {
                        _auto = true;
                        _count = SensorAveragingCountEnum.AutoIsSet;
                    }

                    // Private Methods
                    private void SetAuto(bool Auto)
                    {
                        _auto = Auto;
                        _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E F{1}", _parentSensor._charDesignator, _auto ? 'A' : 'H'));
                        if (_auto == true)
                            _count = SensorAveragingCountEnum.AutoIsSet;
                    }
                    private void SetCount(SensorAveragingCountEnum Count)
                    {
                        _count = Count;
                        _auto = false;
                        switch (_count) {
                            case SensorAveragingCountEnum._1:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 0 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._2:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 1 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._4:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 2 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._8:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 3 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._16:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 4 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._32:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 5 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._64:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 6 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._128:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 7 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._256:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 8 EN", _parentSensor._charDesignator));
                                break;
                            case SensorAveragingCountEnum._512:
                                _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E FM 9 EN", _parentSensor._charDesignator));
                                break;
                        }
                    }
                }
                public class OffsetClass {

                    // Variables
                    private SensorClass _parentSensor;
                    private bool _enable;
                    private double _value;

                    // Properties
                    public bool Enable
                    {
                        get { return this._enable; }
                        set { this.SetEnable(value); }
                    }
                    public double Value
                    {
                        get { return this._value; }
                        set { this.SetValue(value); }
                    }

                    // Constructor
                    internal OffsetClass(SensorClass ParentSensor)
                    {
                        _parentSensor = ParentSensor;
                    }
                    
                    // Internal Methods
                    internal void Reset()
                    {
                        _enable = false;
                        _value = 0.00;
                    }

                    // Private Methods
                    private void SetEnable(bool Enable)
                    {
                        _enable = Enable;
                        _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E OF{1}", _parentSensor._charDesignator, _enable ? '1' : '0'));
                    }
                    private void SetValue(double Value)
                    {
                        _value = Value;
                        _parentSensor._parentGigatronics854x.WriteString(String.Format("{0}E OS {1} EN", _parentSensor._charDesignator, _value));
                    }
                }

                // Variables
                protected Gigatronics854x _parentGigatronics854x;
                protected readonly char _charDesignator;
                private double _frequency;

                // Properties
                public SensorDesignatorEnum Designator
                { get; private set; }
                public SensorTypeEnum Type
                { get; private set; }
                public AveragingClass Averaging
                { get; private set; }
                public OffsetClass Offset
                { get; private set; }
                public double Frequency
                {
                    get { return this._frequency; }
                    set { this.SetFrequency(value); }
                }

                // Constructor
                internal SensorClass(Gigatronics854x ParentGigatronics854x, SensorDesignatorEnum Designator, SensorTypeEnum Type)
                {
                    _parentGigatronics854x = ParentGigatronics854x;
                    this.Designator = Designator;
                    this.Type = Type;
                    Averaging = new AveragingClass(this);
                    Offset = new OffsetClass(this);

                    switch (this.Designator) {
                        case SensorDesignatorEnum.A:
                            _charDesignator = 'A';
                            break;
                        case SensorDesignatorEnum.B:
                            _charDesignator = 'B';
                            break;
                        default:
                            _charDesignator = 'A';
                            break;
                    }
                }

                // Internal Methods
                internal virtual void Reset()
                {
                    Averaging.Reset();
                    Offset.Reset();
                    _frequency = 50e6;
                }

                // Private Methods
                private void SetFrequency(double Frequency)
                {
                    _frequency = Frequency;
                    _parentGigatronics854x.WriteString(String.Format("{0}E FR {1} HZ", _charDesignator, _frequency));
                }

                // Internal Methods
                /// <summary>
                /// Select the sensor for manual measurement taking.
                /// </summary>
                internal void Select()
                {
                    _parentGigatronics854x.WriteString(String.Format("{0}P", _charDesignator));
                }

                // Public Methods
                /// <summary>
                /// Calibrate the sensor.
                /// </summary>
                /// <param name="MillisecondsTimeout">Millisecond timeout value. How long to wait for the calibration to finish.</param>
                /// <returns>True if calibration completes before timing out. False if an error occurred or the function timed out.</returns>
                public bool Calibrate(int MillisecondsTimeout)
                {
                    bool success = false;

                    _parentGigatronics854x.ClearEventRegisters();
                    _parentGigatronics854x.WriteString(String.Format("{0}E CL 100 EN", _charDesignator));
                    success = _parentGigatronics854x.WaitForCalibrationOrZeroToComplete(MillisecondsTimeout);

                    return success;
                }
                /// <summary>
                /// Zero the sensor.
                /// </summary>
                /// <param name="MillisecondsTimeout">Millisecond timeout value. How long to wait for the zeroing to finish.</param>
                /// <returns>True if the zeroing completes before timing out. False if an error occurred or the function timed out.</returns>
                public bool Zero(int MillisecondsTimeout)
                {
                    bool success = false;

                    _parentGigatronics854x.ClearEventRegisters();
                    _parentGigatronics854x.WriteString(String.Format("{0}E ZE", _charDesignator));
                    success = _parentGigatronics854x.WaitForCalibrationOrZeroToComplete(MillisecondsTimeout);

                    return success;
                }

            }
            public class PeakSensor80350AClass : SensorClass {

                // Variables
                private PeakSensorTriggerModeEnum _triggerMode;
                private double _triggerLevel;
                private double _delay;
                private double _delayOffset;

                // Properties
                public PeakSensorTriggerModeEnum TriggerMode
                {
                    get { return this.GetTriggerMode(); }
                }
                public double TriggerLevel
                {
                    get { return _triggerLevel; }
                }
                public double Delay
                {
                    get { return this.GetDelay(); }
                    set { this.SetDelay(value); }
                }
                public double DelayOffset
                {
                    get { return this.GetDelayOffset(); }
                    set { this.SetDelayOffset(value); }
                }

                // Constructor
                internal PeakSensor80350AClass(Gigatronics854x ParentGigatronics854x, SensorDesignatorEnum Designator, SensorTypeEnum Type)
                    : base(ParentGigatronics854x, Designator, Type)
                {

                }

                // Internal Methods
                internal override void Reset()
                {
                    base.Reset();
                    _triggerMode = PeakSensorTriggerModeEnum.Internal;
                    _triggerLevel = 0.00;
                }

                // Private Methods
                private PeakSensorTriggerModeEnum GetTriggerMode()
                {
                    string retVal;
                    _parentGigatronics854x.ClearEventRegisters();
                    _parentGigatronics854x.WriteString(String.Format("PEAK {0}?", _charDesignator));
                    _parentGigatronics854x.WaitForMeasurementToComplete(_parentGigatronics854x.Timeout);
                    retVal = _parentGigatronics854x.ReadString();
                    if (retVal.Contains("EXT")) {
                        _triggerMode = PeakSensorTriggerModeEnum.External;
                    } else if (retVal.Contains("INT")) {
                        _triggerMode = PeakSensorTriggerModeEnum.Internal;
                    } else {
                        _triggerMode = PeakSensorTriggerModeEnum.CW;
                    }
                    return _triggerMode;
                }
                private double GetDelay()
                {
                    _parentGigatronics854x.ClearEventRegisters();
                    _parentGigatronics854x.WriteString(String.Format("PEAK {0} DELAY?", _charDesignator));
                    _parentGigatronics854x.WaitForMeasurementToComplete(_parentGigatronics854x.Timeout);
                    _delay = (double)_parentGigatronics854x.ReadNumber(Ivi.Visa.Interop.IEEEASCIIType.ASCIIType_R8, true);
                    return _delay;
                }
                private void SetDelay(double Delay)
                {
                    if (Delay < -20e-9 || 104e-3 < Delay)
                        throw new System.ArgumentException("Delay must be between -20ns and 104ms", "Delay");

                    _delay = Delay;
                    _parentGigatronics854x.WriteString(String.Format("PEAK {0} DELAY {1}", _charDesignator, _delay));
                }
                private double GetDelayOffset()
                {
                    _parentGigatronics854x.ClearEventRegisters();
                    _parentGigatronics854x.WriteString(String.Format("PEAK {0} OFFSET?", _charDesignator));
                    _parentGigatronics854x.WaitForMeasurementToComplete(_parentGigatronics854x.Timeout);
                    _delayOffset = (double)_parentGigatronics854x.ReadNumber(Ivi.Visa.Interop.IEEEASCIIType.ASCIIType_R8, true);
                    return _delayOffset;
                }
                private void SetDelayOffset(double DelayOffset)
                {
                    if (DelayOffset < -20e-9 || 104e-3 < DelayOffset)
                        throw new System.ArgumentException("DelayOffset must be between -20ns and 104ms", "DelayOffset");

                    _delayOffset = DelayOffset;
                    _parentGigatronics854x.WriteString(String.Format("PEAK {0} OFFSET {1}", _charDesignator, _delayOffset));
                }

                // Public Methods
                public void SetTriggerModeAndLevel(PeakSensorTriggerModeEnum TriggerMode, double TriggerLevel)
                {
                    _triggerMode = TriggerMode;
                    _triggerLevel = TriggerLevel;

                    switch (TriggerMode) {
                        case PeakSensorTriggerModeEnum.Internal:
                            _parentGigatronics854x.WriteString(String.Format("PEAK {0} INT TRIG {1}", _charDesignator, TriggerLevel));
                            break;
                        case PeakSensorTriggerModeEnum.External:
                            _parentGigatronics854x.WriteString(String.Format("PEAK {0} EXT TRIG {1}", _charDesignator, TriggerLevel));
                            break;
                        case PeakSensorTriggerModeEnum.CW:
                            _parentGigatronics854x.WriteString(String.Format("PEAK {0} CW", _charDesignator));
                            break;
                    }
                }

            }
            public class SensorCollectionClass : IEnumerable<SensorClass> {

                // Variables
                private Gigatronics854x _parentGigatronics854x;
                private Dictionary<SensorDesignatorEnum, SensorClass> _sensors;
                
                // Properties
                public int Count
                {
                    get { return this._sensors.Count; }
                }
                public SensorDesignatorEnum[] Designators
                {
                    get { return this._sensors.Keys.ToArray(); }
                }

                // Constructors
                internal SensorCollectionClass(Gigatronics854x ParentGigatronics854x)
                {
                    _parentGigatronics854x = ParentGigatronics854x;
                    _sensors = new Dictionary<SensorDesignatorEnum, SensorClass>();
                }

                // Indexer
                public SensorClass this[SensorDesignatorEnum Designator]
                {
                    get
                    {
                        if (!_sensors.ContainsKey(Designator))
                            throw new System.ArgumentException("Sensor doesn't exist", "Designator");

                        return _sensors[Designator];
                    }
                }
                                
                // Public Methods
                public bool Add(SensorDesignatorEnum Designator, SensorTypeEnum SensorType)
                {
                    if (_sensors.ContainsKey(Designator))
                        return false;

                    switch (SensorType) {
                        case SensorTypeEnum.Peak_80350A:
                            _sensors.Add(Designator, new PeakSensor80350AClass(_parentGigatronics854x, Designator, SensorTypeEnum.Peak_80350A));
                            break;
                        case SensorTypeEnum.Regular:
                            _sensors.Add(Designator, new SensorClass(_parentGigatronics854x, Designator, SensorTypeEnum.Regular));
                            break;
                    }

                    return true;
                }
                public bool Delete(SensorDesignatorEnum Designator)
                {
                    if (!_sensors.ContainsKey(Designator))
                        return false;

                    _sensors.Remove(Designator);
                    return true;
                }
                public bool GetReading(TriggerTypeEnum TriggerType, SensorClass Sensor, out double Reading)
                {
                    Reading = 999999;
                    switch (TriggerType) {
                        case TriggerTypeEnum.Hold:
                            return false;
                        case TriggerTypeEnum.ImmediateSingle:
                            Sensor.Select();
                            _parentGigatronics854x.Trigger.Send(TriggerType);
                            _parentGigatronics854x.WaitForMeasurementToComplete(_parentGigatronics854x.Timeout);
                            Reading = (double)_parentGigatronics854x.ReadNumber(Ivi.Visa.Interop.IEEEASCIIType.ASCIIType_R8, true);
                            return true;
                        case TriggerTypeEnum.ImmediateAveraging:
                            Sensor.Select();
                            _parentGigatronics854x.Trigger.Send(TriggerType);
                            _parentGigatronics854x.WaitForMeasurementToComplete(_parentGigatronics854x.Timeout);
                            Reading = (double)_parentGigatronics854x.ReadNumber(Ivi.Visa.Interop.IEEEASCIIType.ASCIIType_R8, true);
                            return true;
                        case TriggerTypeEnum.FreeRun:
                            Sensor.Select();
                            _parentGigatronics854x.Trigger.Send(TriggerType);
                            Reading = (double)_parentGigatronics854x.ReadNumber(Ivi.Visa.Interop.IEEEASCIIType.ASCIIType_R8, true);
                            return true;
                    }
                    return false;
                }
                public bool GetReading(TriggerTypeEnum TriggerType, SensorDesignatorEnum Designator, out double Reading)
                {
                    Reading = 999999;

                    if (!_sensors.ContainsKey(Designator))
                        return false;

                    return GetReading(TriggerType, this[Designator], out Reading);
                }
                public IEnumerator<SensorClass> GetEnumerator()
                {
                    return _sensors.Values.GetEnumerator();
                }

                // Unused Interface Methods
                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }

            }

            // Enumerations
            public enum TriggerTypeEnum { Hold, ImmediateSingle, ImmediateAveraging, FreeRun };
            public enum SensorDesignatorEnum { A, B };
            public enum SensorTypeEnum { Peak_80350A, Regular };
            public enum PeakSensorTriggerModeEnum { Internal, External, CW };
            public enum SensorAveragingCountEnum { _1, _2, _4, _8, _16, _32, _64, _128, _256, _512, AutoIsSet };

            // Properties
            public DisplayClass Display
            { get; private set; }
            public TriggerClass Trigger
            { get; private set; }
            public SensorCollectionClass Sensors
            { get; private set; }

            // Constructors
            public Gigatronics854x()
                : base()
            {
                Display = new DisplayClass(this);
                Trigger = new TriggerClass(this);
                Sensors = new SensorCollectionClass(this);
            }
            public Gigatronics854x(SensorDesignatorEnum[] SensorDesignators, SensorTypeEnum[] SensorTypes)
                : this()
            {
                if (SensorDesignators.Length != SensorTypes.Length)
                    throw new System.ArgumentException("The lengths of SensorDesignators and SensorTypes must be equal!");

                for (int i = 0; i < SensorDesignators.Length; i++) {
                    if (Sensors.Add(SensorDesignators[i], SensorTypes[i]) == false)
                        throw new System.ArgumentException("Sensor already exists! Failed to create Sensor.");
                }
            }

            // Protected Methods
            protected override void WaitFunction_Measurement()
            {
                // poll the Data Ready Bit in the status byte
                short stb;
                _continueWaiting = true;
                do {
                    stb = ReadStatusByte();
                } while ((((int)stb & 0x01) != 0x01) && _continueWaiting);
                _continueWaiting = true;
            }
            protected bool WaitFunction_CalibrateOrZero()
            {
                // poll the Cal/Zero Complete bit in the status byte
                // poll the Measurement/Cal/Zero Error bit in the status byte
                bool success = false;
                short stb;
                bool done;
                bool error;
                _continueWaiting = true;
                do {
                    stb = ReadStatusByte();
                    done = (((int)stb & 0x01) == 0x01);
                    error = (((int)stb & 0x08) == 0x08);
                } while (!done && !error && _continueWaiting);
                success = (done && !error && _continueWaiting);
                _continueWaiting = true;

                return success;
            }
            protected bool WaitForCalibrationOrZeroToComplete(int Milliseconds)
            {
                bool success = false;
                Thread WaitThread = new Thread(delegate() { success = this.WaitFunction_CalibrateOrZero(); });
                WaitThread.Start();
                while (!WaitThread.IsAlive) { }
                if (Milliseconds > 0) {
                    if (!WaitThread.Join(Milliseconds)) {
                        StopWaiting();
                    }
                } else if (Milliseconds == 0) {
                    StopWaiting();
                } else {
                    WaitThread.Join();
                }
                return success;
            }

            // Public Methods
            public override void Reset()
            {
                base.Reset();
                Display.Reset();
                foreach (SensorClass Sensor in Sensors) {
                    Sensor.Reset();
                }
            }

        }

    }

}
