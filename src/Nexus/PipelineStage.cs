using Nexus.Logging;
using System.Diagnostics;
namespace Nexus
{
    public abstract class PipelineStage<TInput, TYOutput, TULog> 
        where TInput : notnull where TYOutput : class, new() where TULog : ILogger, new()
    {
        readonly Stopwatch _stopwatch = new();
        readonly string _stageName;

        public bool IsDebugMode { get; set; } = false;
        public readonly TULog Log = new();

        public PipelineStage(string stageName)
        {
            _stageName = stageName;
        }

        public TYOutput Execute(TInput input)
        {
            if (IsDebugMode)
            {
                _stopwatch.Restart();
                Log.Debug("{Name} started execution with input: {Input}", _stageName, input);
            }

            var output = OnExecute(input);

            if (IsDebugMode)
            {
                _stopwatch.Stop();
                Log.Debug("{Name} completed execution with output: {Output} in {ElapsedTime}ms", _stageName, output, _stopwatch.ElapsedMilliseconds);
            }

            return output;
        }
        
        protected abstract TYOutput OnExecute(TInput input);
    }
}
