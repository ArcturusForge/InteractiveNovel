namespace Arcturus.InteractiveNovel
{
    public class PlaySequenceCommand : SequenceCommandBase
    {
        public override void HandleCommand(ref int sequenceIndex, ref int lineIndex, string commandData)
        {
            sequenceIndex = int.Parse(commandData);
        }

        public override bool MatchCommand(string commandName)
        {
            return commandName.ToLower() == "play";
        }
    }
}
