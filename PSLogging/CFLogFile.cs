﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace CoreFile {
    public class CFLogFile : CFRotatableTextFile,IDisposable {
        /*
        protected TextWriter textWriter;
        protected readonly Object syncObject; //= new Object();  //static
        protected string path = null;
        bool disposed = false;

        public string Path {
            get { return this.path; }
        }
        */
        public CFLogFile(string path) : base(path) {
            /*
            if(path.Contains("\\")) {
                string directory = path.Substring(0, path.LastIndexOf("\\"));
                if(!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }
            }

            this.path = path;
            this.syncObject = new Object();
            this.textWriter = TextWriter.Synchronized(File.AppendText(this.path));
            */
        }

        public CFLogFile(string path, int maxsize)
            : base(path, maxsize) {
        }

        public override void Write(string message) {
            string datetimeString = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            /*
            string datetimeString = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            lock(syncObject) {
                this.textWriter.WriteLine(datetimeString + " " + message);
                this.textWriter.Flush();
            }
            */
            base.Write(datetimeString + " " + message);
        }
        
        public void Write(string message, CFLogEntryType type) {
            if(type == CFLogEntryType.Info) {
                message = "INFO " + message;
            } else if(type == CFLogEntryType.Warning) {
                message = "WARNING " + message;
            } else if(type == CFLogEntryType.Error) {
                message = "ERROR " + message;
            } else if(type == CFLogEntryType.Severe) {
                message = "SEVERE " + message;
            } else if(type == CFLogEntryType.Debug) {
                message = "DEBUG " + message;
            }

            this.Write(message);
        }

        /*
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if(this.disposed) {
                return;
            }

            if(disposing) {
                this.textWriter.Dispose();
            }
            this.disposed = true;
            return;
        }
        */
    }
}
