using ALResourcePipeline.BuildFiles;
using System.Text.RegularExpressions;

namespace ALResourcePipeline
{

class Project {

    string ProjectPath;
    static IBuildFile[] BuildFiles = new IBuildFile[] { new Byaml() };

    static IBuildFile IdentifyType(string path)
    {
        foreach (IBuildFile file in BuildFiles)
            if (file.Identify(path))
                return file;
        return new Normal();
    }

    public Project(string path)
    {
        ProjectPath = path;
    }

    public void Process(string outPath)
    {
        foreach (var path in Directory.EnumerateFileSystemEntries(ProjectPath, "*.*", SearchOption.AllDirectories))
        { // file pass
            IBuildFile buildFile = IdentifyType(path);
            if (buildFile is Normal && Directory.Exists(path))
                continue;
            string outFile = Path.Combine(Path.GetDirectoryName(Path.Combine(outPath, Path.GetRelativePath(ProjectPath, path))), buildFile.EditName(Path.GetFileName(path)));
            if (File.Exists(Path.GetDirectoryName(outFile)))
                File.Delete(Path.GetDirectoryName(outFile));
            Directory.CreateDirectory(Path.GetDirectoryName(outFile));
            File.WriteAllBytes(outFile, buildFile.Build(path));
        }

        List<string> szsPaths = new List<string>();
        foreach (var path in Directory.EnumerateFileSystemEntries(outPath, "*.*", SearchOption.AllDirectories)) // can't delete while iterating
        {
            if (Directory.Exists(path) && path.EndsWith(".szs") && Regex.Matches(path, ".szs").Count == 1)
                szsPaths.Add(path);
            
        }

        foreach (var path in szsPaths)
        { // szs pass
            byte[] data = SZSArchiver.Archive(path);
            Directory.Delete(path, true);
            File.WriteAllBytes(path, data);
        }
    }
}

} // namespace ALResourcePipeline
