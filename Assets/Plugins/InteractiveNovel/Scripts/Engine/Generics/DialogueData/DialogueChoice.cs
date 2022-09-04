namespace Arcturus.InteractiveNovel
{
    [System.Serializable]
    public class DialogueChoice
    {
        public string Content { get { return content; } }
        private string content;
        public int Index { get { return index; } }
        private int index;

        public DialogueChoice(string content, int index)
        {
            this.content = content;
            this.index = index;
        }
    }

}
