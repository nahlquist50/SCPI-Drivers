using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ivi.Visa.Interop;

namespace SCPI {

    public abstract class SCPIDriver {
        
        // Variables
        private FormattedIO488 _driver;
        private bool _closed;
        protected volatile bool _continueWaiting;
        private int _timeout;
        protected const int InfiniteWait = -1;

        // Properties
        public bool Initialized { get; private set; }
        public string GPIBAddress { get; private set; }
        public int Timeout
        {
            get { return this._timeout; }
            set { this.SetTimeout(value); }
        }
        public string InstrumentID
        {
            get { return this.GetInstrumentID(); }
        }
        private bool DriverIsReady { get { return (Initialized && !_closed); } }
        
        // Constructors
        internal protected SCPIDriver()
        {
            _driver = new FormattedIO488();
            Initialized = false;
            _closed = false;
            GPIBAddress = null;
            _timeout = 0;
        }

        // Protected Methods (Only used by derived classes, not by end user)
        protected Int16 ReadStatusByte()
        {
            if (DriverIsReady) {
                try {
                    return _driver.IO.ReadSTB();
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute ReadStatusByte(). Driver not initialized or is closed.");
        }
        protected Int16 ReadStandardEventByte()
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteString("*ESR?");
                    return (Int16)_driver.ReadNumber(IEEEASCIIType.ASCIIType_I2, true);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute ReadStandardEventByte(). Driver not initialized or is closed.");
        }
        protected void WriteStandardEventRegisterMask(byte mask)
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteString(String.Format("*ESE {0}", mask));
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute WriteStandardEventRegisterMask(). Driver not initialized or is closed.");
        }
        protected void WriteString(string data, bool flushAndEND = true)
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteString(data, flushAndEND);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute WriteString(...). Driver not initialized or is closed.");
        }
        protected void WriteNumber(object data, IEEEASCIIType type = IEEEASCIIType.ASCIIType_Any, bool flushAndEND = true)
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteNumber(data, type, flushAndEND);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute WriteNumber(...). Driver not initialized or is closed.");
        }
        protected void WriteIEEEBlock(string Command, object data, bool flushAndEND = true)
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteIEEEBlock(Command, data, flushAndEND);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute WriteIEEEBlock(...). Driver not initialized or is closed.");
        }
        protected void WriteList(ref object data, IEEEASCIIType type = IEEEASCIIType.ASCIIType_Any, string listSeparator = ",;", bool flushAndEND = true)
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteList(ref data, type, listSeparator, flushAndEND);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute WriteList(...). Driver not initialized or is closed.");
        }
        protected void FlushWrite(bool sendEND = true)
        {
            if (DriverIsReady) {
                try {
                    _driver.FlushWrite(sendEND);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute FlushWrite(...). Driver not initialized or is closed.");
        }
        protected string ReadString()
        {
            if (DriverIsReady) {
                try {
                    return _driver.ReadString();
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute ReadString(). Driver not initialized or is closed.");
        }
        protected dynamic ReadList(IEEEASCIIType type = IEEEASCIIType.ASCIIType_Any, string listSeparator = ",;")
        {
            if (DriverIsReady) {
                try {
                    return _driver.ReadList(type, listSeparator);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute ReadList(...). Driver not initialized or is closed.");
        }
        protected dynamic ReadNumber(IEEEASCIIType type = IEEEASCIIType.ASCIIType_Any, bool flushToEND = true)
        {
            if (DriverIsReady) {
                try {
                    return _driver.ReadNumber(type, flushToEND);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute ReadNumber(...). Driver not initialized or is closed.");
        }
        protected dynamic ReadIEEEBlock(IEEEBinaryType type, bool seekToBlock = false, bool flushToEND = true)
        {
            if (DriverIsReady) {
                try {
                    return _driver.ReadIEEEBlock(type, seekToBlock, flushToEND);
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute ReadIEEEBlock(...). Driver not initialized or is closed.");
        }
        protected void FlushRead()
        {
            if (DriverIsReady) {
                try {
                    _driver.FlushRead();
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute FlushRead(). Driver not initialized or is closed.");
        }
        protected virtual void ClearEventRegisters()
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteString("*CLS");
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute ClearEventRegisters(...). Driver not initialized or is closed.");
        }
        protected virtual void WaitFunction_Measurement()
        {
            // poll the message available bit in the status byte
            short stb;
            _continueWaiting = true;
            do {
                stb = ReadStatusByte();
            } while ((((int)stb & 0x10) != 0x10) && _continueWaiting);
            _continueWaiting = true;
        }
        protected virtual void StopWaiting()
        {
            _continueWaiting = false;
        }
        /// <summary>
        /// Waits for the system measurement to complete.
        /// </summary>
        /// <param name="Milliseconds">Timeout value in ms.  If this number is less than 0, the function will wait infinitely until the measurement completes.</param>
        protected virtual void WaitForMeasurementToComplete(int Milliseconds)
        {
            Thread WaitThread = new Thread(this.WaitFunction_Measurement);
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
        }
        protected virtual string GetInstrumentID()
        {
            ClearEventRegisters();
            WriteString("*IDN?");
            WaitForMeasurementToComplete(Timeout);
            return ReadString();
        }

        // Public Methods (Visible to end user)
        public void Initialize(string GPIBAddress, int Timeout)
        {
            if (!Initialized && !_closed) {

                this.GPIBAddress = GPIBAddress;
                _timeout = Timeout;

                try {

                    // Open the driver
                    _driver.IO = SCPIRsrc.Mgr.Open(GPIBAddress, AccessMode.NO_LOCK, Timeout);
                    // Set the termination character to linefeed <LF>
                    _driver.IO.TerminationCharacter = 10;
                    _driver.IO.TerminationCharacterEnabled = true;

                    Initialized = true;

                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute Initialize(...). Driver is already initialized or has already been closed.");
        }
        public virtual void Reset()
        {
            if (DriverIsReady) {
                try {
                    _driver.WriteString("*RST");
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            } else
                throw new System.InvalidOperationException("Cannot execute Reset(...). Driver not initialized or is closed.");
        }
        public virtual string ReadFromErrorQueue()
        {
            WriteString("SYSTem:ERRor?");
            return ReadString();
        }
        public void Close()
        {
            if (DriverIsReady) {
                try {
                    Initialized = false;
                    _closed = true;
                    _driver.IO.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_driver);
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
        }

        // Private Methods (only visible to this class)
        private void SetTimeout(int Timeout)
        {
            // argument validation
            if (Timeout < 0)
                throw new System.ArgumentException("Timeout value must be greater than 0.", "timeout");

            _timeout = Timeout;

            if (DriverIsReady) {
                try {
                    _driver.IO.Timeout = Timeout;
                }
                catch (Exception ex) {
                    Close();
                    throw ex;
                }
            }
        }

    }

}
