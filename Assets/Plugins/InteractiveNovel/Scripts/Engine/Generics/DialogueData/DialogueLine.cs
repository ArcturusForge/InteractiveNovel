using System.Collections.Generic;

namespace Arcturus.InteractiveNovel
{
    [System.Serializable]
    public class DialogueLine
    {
        public string Name { get { return name; } }
        private readonly string name;
        public string Content { get { return content; } }
        private readonly string content;
        public string Pose { get { return pose; } }
        private readonly string pose;
        public string Position { get { return position; } }
        private readonly string position;
        public List<DialogueChoice> Choices { get { return choices; } }
        private readonly List<DialogueChoice> choices;

        public DialogueLine(string name, string content, string pose, string position, List<DialogueChoice> choices = null)
        {
            this.name = name;
            this.content = content;
            this.pose = pose;
            this.position = position;
            if (choices != null)
                this.choices = choices;
            else
                this.choices = new List<DialogueChoice>();
        }
    }
}
