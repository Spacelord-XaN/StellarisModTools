namespace Xan.StellarisModTools.Release
{
    public class Job
    {
        protected readonly IMessageConsole console;

        public Job(IMessageConsole Console)
        {
            this.console = Console;
        }

        public virtual void Execute()
        {
        }

        protected void Message(string Message)
        {
            this.console.Message(Message);
        }
    }
}
