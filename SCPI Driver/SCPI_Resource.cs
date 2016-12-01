using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ivi.Visa.Interop;

namespace SCPI {

    internal sealed class SCPIRsrc : IDisposable {

        // Members
        private bool _disposed;
        private ResourceManager _rMgr;

        // Singelton Instance
        static readonly SCPIRsrc _mgr = new SCPIRsrc();
        public static SCPIRsrc Mgr { get { return _mgr; } }

        // Constructor
        private SCPIRsrc()
        {
            _rMgr = new ResourceManager();
            _disposed = false;
        }

        // Public Methods
        public IMessage Open(string address, AccessMode mode = AccessMode.NO_LOCK, int openTimeout = 2000, string options = "")
        {
            if (!_disposed) {
                return (IMessage)_rMgr.Open(address, mode, openTimeout, options);
            } else {
                throw new System.ObjectDisposedException("_rMgr", "This object has already been disposed by Garbage Collector.");
            }
        }
        public string[] FindRsrc(string expression)
        {
            if (!_disposed) {
                return _rMgr.FindRsrc(expression);
            } else {
                throw new System.ObjectDisposedException("_rMgr", "This object has already been disposed by Garbage Collector.");
            }
        }

        // Disposal Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (_disposed) {
                return;
            }


            if (disposing) {
                // free managed resources
            }

            // free unmanaged resources
            try {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_rMgr);
            }
            catch { }

            _disposed = true;
        }
        ~SCPIRsrc()
        {
            Dispose(false);
        }
    }

}
