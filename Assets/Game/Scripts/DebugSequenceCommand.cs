using Arcturus.InteractiveNovel;
using UnityEngine;

public class DebugSequenceCommand : SequenceCommandBase
{
    public override void HandleCommand(ref int sequenceIndex, ref int lineIndex, string commandData)
    {
        Debug.LogWarning($"From sequence: {commandData}");
    }

    public override bool MatchCommand(string commandName)
    {
        return commandName.ToLower() == "debug";
    }
}
