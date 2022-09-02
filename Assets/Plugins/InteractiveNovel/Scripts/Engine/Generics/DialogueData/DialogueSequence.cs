using System.Collections.Generic;

namespace Arcturus.InteractiveNovel
{
    [System.Serializable]
    public class DialogueSequence
    {
        public List<DialogueLine> DialogueLines { get { return dialogueLines; } }
        private List<DialogueLine> dialogueLines;
        public List<string> SequenceEndActions { get { return sequenceEndActions; } }
        private List<string> sequenceEndActions;

        public DialogueSequence()
        {
            this.dialogueLines = new List<DialogueLine>();
            this.sequenceEndActions = new List<string>();
        }
    }
}
