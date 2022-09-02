using UnityEngine;

public class SequenceManager : MonoBehaviour
{
   private static SequenceManager I;

    private void Awake()
    {
        #region Static Referencing
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        #endregion
    }

}
