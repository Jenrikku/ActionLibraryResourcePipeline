using System.Runtime.InteropServices;
using System.Text;

class OeadWrapper {

    class Internal {

        [DllImport("OeadWrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr YamlToByaml(string yamlData, int version = 1, bool bigEndian = false);

        [DllImport("OeadWrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Yaz0Decompress(IntPtr data, int size);

        [DllImport("OeadWrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Yaz0Compress(IntPtr data, int size, int level, int alignment);

        [DllImport("OeadWrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeData(IntPtr data);

    }

    public static byte[] YamlToByaml(string yamlData, int version = 1, bool bigEndian = false)
    {
        IntPtr data = Internal.YamlToByaml(yamlData, version, bigEndian);

        byte[] sizeData = new byte[Marshal.SizeOf<UInt32>()];
        Marshal.Copy(data, sizeData, 0, Marshal.SizeOf<UInt32>());
        UInt32 size = BitConverter.ToUInt32(sizeData);

        byte[] byamlData = new byte[size];
        Marshal.Copy(data + Marshal.SizeOf<UInt32>(), byamlData, 0, (int) size);

        Internal.FreeData(data);

        return byamlData;
    }

    public static byte[] Yaz0Decompress(byte[] compressed)
    {
        IntPtr compressedUnmanaged = Marshal.AllocHGlobal(compressed.Length);
        Marshal.Copy(compressed, 0, compressedUnmanaged, compressed.Length);

        IntPtr data = Internal.Yaz0Decompress(compressedUnmanaged, compressed.Length);
        Marshal.FreeHGlobal(compressedUnmanaged);

        byte[] sizeData = new byte[Marshal.SizeOf<UInt32>()];
        Marshal.Copy(data, sizeData, 0, Marshal.SizeOf<UInt32>());
        UInt32 size = BitConverter.ToUInt32(sizeData);

        byte[] decompressed = new byte[size];
        Marshal.Copy(data + Marshal.SizeOf<UInt32>(), decompressed, 0, (int) size);

        Internal.FreeData(data);

        return decompressed;
    }

    public static byte[] Yaz0Compress(byte[] decompressed, int level = 7, int alignment = 0)
    {
        IntPtr decompressedUnmanaged = Marshal.AllocHGlobal(decompressed.Length);
        Marshal.Copy(decompressed, 0, decompressedUnmanaged, decompressed.Length);

        IntPtr data = Internal.Yaz0Compress(decompressedUnmanaged, decompressed.Length, level, alignment);
        Marshal.FreeHGlobal(decompressedUnmanaged);

        byte[] sizeData = new byte[Marshal.SizeOf<UInt32>()];
        Marshal.Copy(data, sizeData, 0, Marshal.SizeOf<UInt32>());
        UInt32 size = BitConverter.ToUInt32(sizeData);

        byte[] compressed = new byte[size];
        Marshal.Copy(data + Marshal.SizeOf<UInt32>(), compressed, 0, (int) size);

        Internal.FreeData(data);

        return compressed;
    }

}
