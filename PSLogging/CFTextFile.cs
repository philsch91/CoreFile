﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CoreFile {
    public class CFTextFile : IDisposable {

        protected FileStream fileStream;
        protected TextWriter textWriter;
        protected TextReader textReader;
        protected readonly Object syncObject;   //static
        protected string path = null;
        protected bool disposed = false;

        public bool Append {
            set {
                if(value) {
                    this.fileStream.Position = this.fileStream.Length;
                }
            }
        }

        public string Path {
            get { return this.path; }
        }

        public CFTextFile(string path) {
            if(path.Contains("\\")) {
                string directory = path.Substring(0, path.LastIndexOf("\\"));
                if(!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }
            }

            this.path = path;
            this.syncObject = new Object();
            
            this.fileStream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if(fileStream.Length > 0) {
                this.fileStream.Position = this.fileStream.Length;
            }
            //this.textWriter = TextWriter.Synchronized(File.AppendText(this.path));
            this.textWriter = TextWriter.Synchronized(new StreamWriter(this.fileStream));
            this.textReader = TextReader.Synchronized(new StreamReader(this.fileStream));
        }

        public virtual void Write(string message) {
            lock(syncObject) {
                this.textWriter.WriteLine(message);
                this.textWriter.Flush();
            }
        }
        
        public virtual void WriteData(string message) {
            lock(syncObject) {
                this.textWriter.Write(message);
                this.textWriter.Flush();
            }
        }

        public virtual void OverWrite(string message) {
            lock(syncObject) {
                this.fileStream.SetLength(0);
                this.textWriter.Write(message);
            }
        }

        public virtual void OverWriteLine(string message) {
            lock(syncObject) {
                this.fileStream.SetLength(0);
                this.textWriter.WriteLine(message);
            }
        }

        public virtual string ReadToEnd() {
            lock(syncObject) {
                //return File.ReadAllText(this.path);
                return this.textReader.ReadToEnd();
            }
        }

        protected virtual void Reopen() {
            if(!this.disposed) {
                return;
            }
            
            this.fileStream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            this.textWriter = TextWriter.Synchronized(new StreamWriter(this.fileStream));
            this.textReader = TextReader.Synchronized(new StreamReader(this.fileStream));
            this.disposed = false;
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if(this.disposed) {
                return;
            }

            if(disposing) {
                //this.fileStream.Dispose();
                this.textWriter.Dispose();
                this.textReader.Dispose();
                this.fileStream.Dispose();
            }
            this.disposed = true;
            
            return;
        }

    }
}
