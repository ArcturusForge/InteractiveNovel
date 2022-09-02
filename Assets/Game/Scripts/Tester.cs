using Arcturus.InteractiveNovel;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public string playwrightPath;

    private void Awake()
    {
        SequenceParser.OnRequestPlaywright += GivePath;
    }

    private string GivePath()
    {
        SequenceParser.OnRequestPlaywright -= GivePath;
        Debug.Log("Requested Playwright.");
        return playwrightPath;
    }
}
