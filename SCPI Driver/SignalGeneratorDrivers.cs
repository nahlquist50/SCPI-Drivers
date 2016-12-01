using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ivi.Visa.Interop;

namespace SCPI {

    namespace SignalGeneratorDrivers {

        public class SignalGenerator : SCPIDriver {

            // Variables
            private double _frequency;
            private double _powerLevel;
            private bool _outputState;

            // Properties
            public double Frequency
            {
                get { return this.GetFrequency(); }
                set { this.SetFrequency(value); }
            }
            public double PowerLevel
            {
                get { return this.GetPowerLevel(); }
                set { this.SetPowerLevel(value); }
            }
            public bool OutputState
            {
                get { return this.GetOutputState(); }
                set { this.SetOutputState(value); }
            }
            public double MinimumFrequency
            {
                get { return this.GetMinimumFrequency(); }
            }
            public double MaximumFrequency
            {
                get { return this.GetMaximumFrequency(); }
            }
            public double MinimumPowerLevel
            {
                get { return this.GetMinimumPowerLevel(); }
            }
            public double MaximumPowerLevel
            {
                get { return this.GetMaximumPowerLevel(); }
            }

            // Protected Constructor
            public SignalGenerator()
                : base()
            {
            }

            // Protected Methods
            protected virtual double GetFrequency()
            {
                ClearEventRegisters();
                WriteString("SOURce:FREQuency:CW?");
                WaitForMeasurementToComplete(Timeout);
                _frequency = (double)ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                return _frequency;
            }
            protected virtual void SetFrequency(double Frequency)
            {
                _frequency = Frequency;
                WriteString(String.Format("SOURce:FREQuency:CW {0}HZ", Frequency));
            }
            protected virtual double GetPowerLevel()
            {
                ClearEventRegisters();
                WriteString("SOURce:POWer:LEVel:IMMediate:AMPLitude?");
                WaitForMeasurementToComplete(Timeout);
                _powerLevel = (double)ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
                return _powerLevel;
            }
            protected virtual void SetPowerLevel(double PowerLevel)
            {
                _powerLevel = PowerLevel;
                WriteString(String.Format("SOURce:POWer:LEVel:IMMediate:AMPLitude {0}DBM", PowerLevel));
            }
            protected virtual bool GetOutputState()
            {
                ClearEventRegisters();
                WriteString("OUTput:STATe?");
                WaitForMeasurementToComplete(Timeout);
                _outputState = ((byte)ReadNumber(IEEEASCIIType.ASCIIType_UI1, true) == 1);
                return _outputState;
            }
            protected virtual void SetOutputState(bool OutputState)
            {
                _outputState = OutputState;
                WriteString(String.Format("OUTput:STATe {0}", OutputState ? "ON" : "OFF"));
            }
            protected virtual double GetMinimumFrequency()
            {
                ClearEventRegisters();
                WriteString("SOURce:FREQuency:CW? MINimum");
                WaitForMeasurementToComplete(Timeout);
                return (double)ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
            }
            protected virtual double GetMaximumFrequency()
            {
                ClearEventRegisters();
                WriteString("SOURce:FREQuency:CW? MAXimum");
                WaitForMeasurementToComplete(Timeout);
                return (double)ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
            }
            protected virtual double GetMinimumPowerLevel()
            {
                ClearEventRegisters();
                WriteString("SOURce:POWer:LEVel:IMMediate:AMPLitude? MINimum");
                WaitForMeasurementToComplete(Timeout);
                return (double)ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
            }
            protected virtual double GetMaximumPowerLevel()
            {
                ClearEventRegisters();
                WriteString("SOURce:POWer:LEVel:IMMediate:AMPLitude? MAXimum");
                WaitForMeasurementToComplete(Timeout);
                return (double)ReadNumber(IEEEASCIIType.ASCIIType_R8, true);
            }
        }

    }

}
