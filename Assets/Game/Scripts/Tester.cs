using Arcturus.InteractiveNovel;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public string playwrightPath;

    private void Start()
    {
        SequenceParser.ParseFile(playwrightPath);
    }
}
