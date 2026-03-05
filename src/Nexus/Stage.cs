using Nexus.Logging;
namespace Nexus
{
    public abstract class InterpreterStage<TInput, TYOutput, TULog>
        where TInput : notnull
        where
        TYOutput : class, new()
        where TULog : ILogger, new()
    {
        public readonly TULog Log = new();

        public abstract TYOutput Execute(TInput input);
    }
}
