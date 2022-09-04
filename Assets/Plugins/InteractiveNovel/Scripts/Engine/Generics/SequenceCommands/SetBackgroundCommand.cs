namespace Arcturus.InteractiveNovel
{
    public class SetBackgroundCommand : SequenceCommandBase
    {
        public override void HandleCommand(ref int sequenceIndex, ref int lineIndex, string commandData)
        {
            BackgroundManager.SetBackground(commandData);
        }

        public override bool MatchCommand(string commandName)
        {
            return commandName.ToLower() == "setbg";
        }
    }
}
