using NARCSharp;
using NewGear.Trees.TrueTree;

namespace ALResourcePipeline
{

class SZSArchiver {
    private static BranchNode<byte[]> CreateNARCDir(in NARC narc, string dir)
    {
        string[] dirs = dir.Split('/');
        var currentNode = narc.RootNode;
        if (dirs.Length == 1)
            return currentNode;

        foreach (string curDir in dirs)
        {
            Console.WriteLine("curDir "+  curDir);
            foreach (var searchNode in currentNode.ChildBranches)
                if (searchNode.Name == curDir)
                {
                    currentNode = searchNode;
                    continue;
                }
            var node = new BranchNode<byte[]>(curDir);
            currentNode.ChildBranches.Add(node);
            currentNode = node;
        }
        return currentNode;
    }

    public static byte[] Archive(string path)
    {
        NARC narc = new NARC();
        foreach (var filePath in Directory.EnumerateFileSystemEntries(path, "*.*", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(path, filePath);
            if (Directory.Exists(filePath) && filePath.EndsWith(".szs"))
            {
                int index = relativePath.LastIndexOf('/');
                var node = CreateNARCDir(in narc, index >= 0 ? relativePath.Substring(0, index) : "");
                node.ChildLeaves.Add(new LeafNode<byte[]>(Path.GetFileName(filePath), Archive(filePath)));
            }
            else
            {
                int index = relativePath.LastIndexOf('/');
                var node = CreateNARCDir(in narc, index >= 0 ? relativePath.Substring(0, index) : "");
                node.ChildLeaves.Add(new LeafNode<byte[]>(Path.GetFileName(filePath), File.ReadAllBytes(filePath)));
            }
        }
        narc.Nameless = false;
        return NARCParser.Write(narc);
    }
}

} // namespace ALResourcePipeline
