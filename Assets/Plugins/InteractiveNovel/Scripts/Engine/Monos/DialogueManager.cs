using UnityEngine;

namespace Arcturus.InteractiveNovel
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager I;

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

        }
    }
}
