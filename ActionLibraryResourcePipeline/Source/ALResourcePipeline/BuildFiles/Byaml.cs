using System.Text;

namespace ALResourcePipeline {
namespace BuildFiles {

        class Byaml : IBuildFile
        {
            public Type GetInputType()
            {
                return Type.File;
            }

            public bool Identify(string fileName)
            {
                return fileName.EndsWith(".yml");
            }

            public byte[] Build(string path)
            {
                return OeadWrapper.YamlToByaml(File.ReadAllText(path));
            }

            public string EditName(string fileName)
            {
                return fileName.Replace(".yml", ".byml");
            }
        }

    } // BuildFiles
} // namespace ALResourcePipeline
