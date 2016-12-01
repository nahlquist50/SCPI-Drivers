using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ivi.Visa.Interop;

namespace SCPI {

    namespace MultimeterDrivers {

        public class Multimeter : SCPIDriver {

            // Enumerations
            public enum MeasurementType { Voltage, Current, Resistance, Unknown };
            public enum TriggerSource { Bus, Immediate, External };

            // Variables
            private MeasurementType _measurementType;
            private TriggerSource _triggerSource;
            private uint _sampleCount;

            // Properties
            public MeasurementType _MeasurementType
            {
                get { return this.GetMeasurementType(); }
                set { this.SetMeasurementType(value); }
            }
            public TriggerSource _TriggerSource
            {
                get { return this.GetTriggerSource(); }
                set { this.SetTriggerSource(value); }
            }
            public uint SampleCount
            {
                get { return this.GetSampleCount(); }
                set { this.SetSampleCount(value); }
            }

            // Constructor
            public Multimeter()
                : base()
            {
            }

            // Protected Methods
            protected virtual MeasurementType GetMeasurementType()
            {
                string retVal;
                ClearEventRegisters();
                WriteString("SENSe:FUNCtion?");
                WaitForMeasurementToComplete(Timeout);
                retVal = ReadString();
                if (retVal.Contains("VOLT")) {
                    return MeasurementType.Voltage;
                } else if (retVal.Contains("CURR")) {
                    return MeasurementType.Current;
                } else if (retVal.Contains("RES")) {
                    return MeasurementType.Resistance;
                } else {
                    return MeasurementType.Unknown;
                }
            }
            protected virtual void SetMeasurementType(MeasurementType measurementType)
            {
                _measurementType = measurementType;
                switch (measurementType) {
                    case MeasurementType.Voltage:
                        WriteString("SENSe:FUNCtion 'VOLTage:DC'");
                        break;
                    case MeasurementType.Current:
                        WriteString("SENSe:FUNCtion 'CURRent:DC'");
                        break;
                    case MeasurementType.Resistance:
                        WriteString("SENSe:FUNCtion 'RESistance'");
                        break;
                    default:
                        break;
                }
            }
            protected virtual TriggerSource GetTriggerSource()
            {
                string retVal;
                ClearEventRegisters();
                WriteString("CONFigure?");
                WaitForMeasurementToComplete(Timeout);
                retVal = ReadString();
                if (retVal.Contains("BUS")) {
                    return TriggerSource.Bus;
                } else if (retVal.Contains("IMM")) {
                    return TriggerSource.Immediate;
                } else {
                    return TriggerSource.External;
                }
            }
            protected virtual void SetTriggerSource(TriggerSource triggerSource)
            {
                _triggerSource = triggerSource;
                switch (triggerSource) {
                    case TriggerSource.Bus:
                        WriteString("TRIGger:SOURce BUS");
                        break;
                    case TriggerSource.Immediate:
                        WriteString("TRIGger:SOURce IMMediate");
                        break;
                    case TriggerSource.External:
                        WriteString("TRIGger:SOURce EXTernal");
                        break;
                }
            }
            protected virtual uint GetSampleCount()
            {
                ClearEventRegisters();
                WriteString("SAMPle:COUNt?");
                WaitForMeasurementToComplete(Timeout);
                return (uint)ReadNumber(IEEEASCIIType.ASCIIType_I4, true);
            }
            protected virtual void SetSampleCount(uint SampleCount)
            {
                _sampleCount = SampleCount;
                WriteString(String.Format("SAMPle:COUNt {0}", SampleCount));
            }

            // Public Methods
            /// <summary>
            /// Gets readings from the multimeter based on the Sample Count and Trigger Source.
            /// If the Trigger source is BUS or IMMEDIATE, this function will trigger the DMM for a measurement.
            /// If the Trigger source is EXTERNAL, this function will put the DMM into a 'wait for trigger' state and will return data when a trigger is captured.
            /// </summary>
            /// <returns></returns>
            public double[] GetReadings()
            {
                if (_sampleCount != 0) {

                    ClearEventRegisters();
                    WriteString("INITiate:IMMediate");
                    if (_triggerSource == TriggerSource.Bus) {
                        WriteString("*TRG");
                    }
                    WriteString("FETCh?");
                    if (_triggerSource != TriggerSource.External) {
                        WaitForMeasurementToComplete(Timeout);
                    } else {
                        WaitForMeasurementToComplete(InfiniteWait);
                    }

                    if (_sampleCount == 1) {
                        return new double[] { (double)ReadNumber(IEEEASCIIType.ASCIIType_R8, true) };
                    } else {
                        return (double[])ReadList(IEEEASCIIType.ASCIIType_R8, ",");
                    }

                } else {
                    return new double[] { };
                }
            }
        }

    }

}