namespace ALResourcePipeline {
namespace BuildFiles {

enum Type {
    File, Folder
}

interface IBuildFile {
    public Type GetInputType();
    public bool Identify(string fileName);
    public byte[] Build(string path);
    public string EditName(string fileName);
}

} // BuildFiles
} // namespace ALResourcePipeline
