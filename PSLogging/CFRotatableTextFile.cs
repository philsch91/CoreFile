﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace CoreFile {
    public class CFRotatableTextFile : CFTextFile {

        private FileInfo fileInfo = null;
        private int maxsize;

        public CFRotatableTextFile(string path) : base(path){
            this.fileInfo = new FileInfo(path);
            this.maxsize = 0;
            this.MaxCount = 0;
        }

        public CFRotatableTextFile(string path, int maxsize)
            : base(path) {
            this.fileInfo = new FileInfo(path);
            this.maxsize = maxsize;
            this.MaxCount = 0;
        }

        public int MaxCount { get; set; }

        public override void Write(string message) {
            if(this.maxsize > 0) {
                this.rotateIfNeeded(message);
            }
            base.Write(message);
        }

        private void rotateIfNeeded(string message) {
            lock(this.syncObject) {
                this.fileInfo.Refresh();

                if((this.fileInfo.Length + message.Length) < this.maxsize) {
                    return;
                }

                //this.textWriter.Dispose();
                this.Dispose();

                if(this.MaxCount == 0) {
                    File.Delete(this.Path);
                    //this.textWriter = TextWriter.Synchronized(File.AppendText(this.path));
                    this.Reopen();
                    return;
                }

                String rotateFile = this.Path + ".rot";
                String newFile = this.Path + ".1";
                
                File.Move(this.Path, rotateFile);
                this.moveFilesIfNeeded();
                File.Move(rotateFile, newFile);
                //this.textWriter = TextWriter.Synchronized(File.AppendText(this.path));
                this.Reopen();
            }
        }

        private void moveFilesIfNeeded() {
            int i = 0;
            while(File.Exists(this.Path + "." + (i+1).ToString()) && (i+1) <= this.MaxCount) {
                i++;
            }

            if(!File.Exists(this.Path + "." + i.ToString())) {
                return;
            }
            
            if(i == this.MaxCount) {
                //delete the oldest file
                File.Delete(this.Path + "." + this.MaxCount);
                i--;
            }

            while(i >= 1) {
                string sourcePath = this.Path + "." + i.ToString();
                string destinationPath = this.Path + "." + (i + 1).ToString();
                File.Move(sourcePath, destinationPath);
                i--;
            }
        }
    }
}
