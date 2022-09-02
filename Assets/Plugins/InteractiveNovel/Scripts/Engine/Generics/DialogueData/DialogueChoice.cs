namespace Arcturus.InteractiveNovel
{
    [System.Serializable]
    public class DialogueChoice
    {
        private string content;
        private int index;

        public DialogueChoice(string content, int index)
        {
            this.content = content;
            this.index = index;
        }
    }

}
