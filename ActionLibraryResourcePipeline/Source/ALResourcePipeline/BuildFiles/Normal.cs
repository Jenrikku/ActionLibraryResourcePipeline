namespace ALResourcePipeline {
namespace BuildFiles {

        class Normal : IBuildFile
        {
            public Type GetInputType()
            {
                return Type.File;
            }

            public bool Identify(string fileName)
            {
                return true;
            }

            public byte[] Build(string path)
            {
                return File.ReadAllBytes(path);
            }
            public string EditName(string fileName)
            {
                return fileName;
            }
        }

    } // BuildFiles
} // namespace ALResourcePipeline
