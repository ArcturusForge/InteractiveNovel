namespace Arcturus.InteractiveNovel
{
    public abstract class SequenceCommandBase
    {
        public abstract bool MatchCommand(string commandName);
        public abstract void HandleCommand(ref int sequenceIndex, ref int lineIndex, string commandData);
    }
}
