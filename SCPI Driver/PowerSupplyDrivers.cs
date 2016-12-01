using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ivi.Visa.Interop;

namespace SCPI {

    namespace PowerSupplyDrivers {

        public class PowerSupply : SCPIDriver {

            // Nested Class
            public class ChannelClass {

                // Variables
                private PowerSupply _parentPowerSupply;
                private double _voltageSetting;
                private double _voltageReading;
                private double _currentLimit;
                private double _current;

                // Properties
                public string Name { get; private set; }
                public double VoltageSetting
                {
                    get { return this.GetVoltageSetting(); }
                    set { this.SetVoltageSetting(value); }
                }
                public double VoltageReading
                {
                    get { return this.GetVoltageReading(); }
                }
                public double CurrentLimit
                {
                    get { return this.GetCurrentLimit(); }
                    set { this.SetCurrentLimit(value); }
                }
                public double Current
                {
                    get { return this.GetCurrent(); }
                }

                // Constructor
                internal ChannelClass(string Name, PowerSupply ParentPowerSupply)
                {
                    this.Name = Name;
                    this._parentPowerSupply = ParentPowerSupply;
                }

                // Protected Methods
                protected virtual double GetVoltageSetting()
                {
                    _parentPowerSupply.ClearEventRegisters();
                    if (!String.IsNullOrWhiteSpace(Name)) {
                        _parentPowerSupply.WriteString(String.Format("INSTrument:SELect {0}", Name));
                    }
                    _parentPowerSupply.WriteString("SOURce:VOLTage:LEVel:IMMediate:AMPLitude?");
                    _parentPowerSupply.WaitForMeasurementToComplete(_parentPowerSupply.Timeout);
                    _voltageSetting = (double)_parentPowerSupply.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _voltageSetting;
                }
                protected virtual void SetVoltageSetting(double Voltage)
                {
                    _voltageSetting = Voltage;
                    if (!String.IsNullOrWhiteSpace(Name)) {
                        _parentPowerSupply.WriteString(String.Format("INSTrument:SELect {0}", Name));
                    }
                    _parentPowerSupply.WriteString(String.Format("SOURce:VOLTage:LEVel:IMMediate:AMPLitude {0}", Voltage));
                }
                protected virtual double GetVoltageReading()
                {
                    _parentPowerSupply.ClearEventRegisters();
                    if (!String.IsNullOrWhiteSpace(Name)) {
                        _parentPowerSupply.WriteString(String.Format("MEASure:VOLTage:DC? {0}", Name));
                    } else {
                        _parentPowerSupply.WriteString("MEASure:VOLTage:DC?");
                    }
                    _parentPowerSupply.WaitForMeasurementToComplete(_parentPowerSupply.Timeout);
                    _voltageReading = (double)_parentPowerSupply.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _voltageReading;
                }
                protected virtual double GetCurrentLimit()
                {
                    _parentPowerSupply.ClearEventRegisters();
                    if (!String.IsNullOrWhiteSpace(Name)) {
                        _parentPowerSupply.WriteString(String.Format("INSTrument:SELect {0}", Name));
                    }
                    _parentPowerSupply.WriteString("SOURce:CURRent:LEVel:IMMediate:AMPLitude?");
                    _parentPowerSupply.WaitForMeasurementToComplete(_parentPowerSupply.Timeout);
                    _currentLimit = (double)_parentPowerSupply.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _currentLimit;
                }
                protected virtual void SetCurrentLimit(double CurrentLimit)
                {
                    _currentLimit = CurrentLimit;
                    if (!String.IsNullOrWhiteSpace(Name)) {
                        _parentPowerSupply.WriteString(String.Format("INSTrument:SELect {0}", Name));
                    }
                    _parentPowerSupply.WriteString(String.Format("SOURce:CURRent:LEVel:IMMediate:AMPLitude {0}", CurrentLimit));
                }
                protected virtual double GetCurrent()
                {
                    _parentPowerSupply.ClearEventRegisters();
                    if (!String.IsNullOrWhiteSpace(Name)) {
                        _parentPowerSupply.WriteString(String.Format("MEASure:CURRent:DC? {0}", Name));
                    } else {
                        _parentPowerSupply.WriteString("MEASure:CURRent:DC?");
                    }
                    _parentPowerSupply.WaitForMeasurementToComplete(_parentPowerSupply.Timeout);
                    _current = (double)_parentPowerSupply.ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                    return _current;
                }
            }
            public class ChannelCollectionClass : IEnumerable<ChannelClass> {

                // Variables
                private PowerSupply _parentPowerSupply;
                private Dictionary<string, ChannelClass> _channels;

                // Properties
                public int Count
                {
                    get { return this._channels.Count; }
                }
                public string[] Names
                {
                    get { return _channels.Keys.ToArray(); }
                }

                // Constructor
                internal ChannelCollectionClass(PowerSupply ParentPowerSupply)
                {
                    _parentPowerSupply = ParentPowerSupply;
                    _channels = new Dictionary<string, ChannelClass>();
                }

                // Public Methods
                public bool Add(string Name)
                {
                    if (_channels.ContainsKey(Name))
                        return false;

                    _channels.Add(Name, new ChannelClass(Name, _parentPowerSupply));
                    return true;
                }
                public bool Delete(string Name)
                {
                    if (!_channels.ContainsKey(Name))
                        return false;

                    _channels.Remove(Name);
                    return true;
                }
                public IEnumerator<ChannelClass> GetEnumerator()
                {
                    return this._channels.Values.GetEnumerator();
                }

                // Unused Interface Methods
                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }

            }

            // Variables
            private bool _outputState;

            // Properties
            public bool OutputState
            {
                get { return this.GetOutputState(); }
                set { this.SetOutputState(value); }
            }
            public ChannelCollectionClass Channels
            { get; private set; }

            // Constructors
            public PowerSupply()
                : base()
            {
                Channels = new ChannelCollectionClass(this);
            }
            public PowerSupply(string[] ChannelNames)
                : this()
            {
                foreach (string ChannelName in ChannelNames) {
                    if (Channels.Add(ChannelName) == false)
                        throw new System.ArgumentException("Channel Name already existed! Failed to create channel.", "ChannelNames");
                }
            }

            // Protected Methods
            protected virtual bool GetOutputState()
            {
                ClearEventRegisters();
                WriteString("OUTput:STATe?");
                WaitForMeasurementToComplete(Timeout);
                _outputState = (((byte)ReadNumber(IEEEASCIIType.ASCIIType_UI1, true)) == 1);
                return _outputState;
            }
            protected virtual void SetOutputState(bool OutputState)
            {
                _outputState = OutputState;
                WriteString(String.Format("OUTput:STATe {0}", OutputState ? "1" : "0"));
            }
        }

    }
}
