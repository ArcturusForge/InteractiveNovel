using System.IO;
using UnityEditor;

public class StaticMono : CreatorProfile
{
    [MenuItem("Assets/Create/Scripts/StaticMono", priority = 20)]
    public static void CreateMenu()
    {
        FileCreator.GenerateWindow<StaticMono>();
    }

    public override void GenerateFile(string path, string scriptName)
    {
        using StreamWriter outfile = new StreamWriter(path);
        outfile.WriteLine("using UnityEngine;");
        outfile.WriteLine("");
        outfile.WriteLine($"public class {scriptName} : MonoBehaviour");
        outfile.WriteLine("{");
        outfile.WriteLine($"   private static {scriptName} I;");
        outfile.WriteLine("");
        outfile.WriteLine("    private void Awake()");
        outfile.WriteLine("    {");
        outfile.WriteLine("        #region Static Referencing");
        outfile.WriteLine("        if (I == null) I = this;");
        outfile.WriteLine("        else if (I != this) Destroy(gameObject);");
        outfile.WriteLine("        #endregion");
        outfile.WriteLine("    }");
        outfile.WriteLine("");
        outfile.WriteLine("}");
    }
}
