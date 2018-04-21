using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Xan.StellarisModTools.Release
{
    public class ReleaseCreator : IMessageConsole
    {
        public event EventHandler<ReleaseCreatorEventArgs> MessageReady = delegate { };

        private readonly BackgroundWorker worker;

        public ReleaseCreator()
        {
            this.worker = new BackgroundWorker( );
            this.worker.DoWork += this.WorkerDoWork;
            this.worker.RunWorkerCompleted += this.WorkerRunWorkerCompleted;
            this.worker.ProgressChanged += this.WorkerProgressChanged;
            this.worker.WorkerReportsProgress = true;
        }

        public void RunAsync()
        {
            this.worker.RunWorkerAsync();
        }

        private void WorkerProgressChanged(object Sender, ProgressChangedEventArgs E)
        {
            this.MessageReady(this, new ReleaseCreatorEventArgs(E.UserState as string));
        }

        private void WorkerRunWorkerCompleted(object Sender, RunWorkerCompletedEventArgs E)
        {
            if (E.Error != null)
            {
                this.MessageReady(this, new ReleaseCreatorEventArgs(E.Error.ToString()));
            }
        }

        private void WorkerDoWork(object Sender, DoWorkEventArgs E)
        {
            string path = this.Setup.RepositoryPath;
            if (string.IsNullOrEmpty(path))
            {
                this.Message("RepositoryPath is empty or null");
                return;
            }

            if (!Directory.Exists(path))
            {
                this.Message(string.Format("RepositoryPath: {0} does not exit.", path));
                return;
            }

            this.Message("Starting release...");

            List<Job> jobs = new List<Job>();
            jobs.Add(new ReleasePortraitsJob(this, this.Setup));
            jobs.Add(new ReleaseFlagsJob(this, this.Setup));
            jobs.Add(new ReleaseCopyToTarget(this, this.Setup));

            foreach (Job job in jobs)
            {
                job.Execute();
            }

            this.Message("");
            this.Message("Release done.");
        }

        public void Message(string Message)
        {
            this.worker.ReportProgress(0, Message);
        }

        public Setup Setup
        {
            get;
            set;
        }
    }
}
