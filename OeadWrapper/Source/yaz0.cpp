#include <oead/yaz0.h>

extern "C" const u8* Yaz0Decompress(const u8* inData, int size)
{
    std::vector<u8> decompressed = oead::yaz0::Decompress(tcb::span<const u8>(inData, size));

    u8* data = new u8[sizeof(u32) + decompressed.size()];

    std::memcpy(data + sizeof(u32), decompressed.data(), decompressed.size());
    *reinterpret_cast<u32*>(data) = decompressed.size();

    return data;
}

extern "C" const u8* Yaz0Compress(const u8* inData, int size, int level = 7, int alignment = 0)
{
    std::vector<u8> compressed = oead::yaz0::Compress(tcb::span<const u8>(inData, size), alignment, level);

    u8* data = new u8[sizeof(u32) + compressed.size()];

    std::memcpy(data + sizeof(u32), compressed.data(), compressed.size());
    *reinterpret_cast<u32*>(data) = compressed.size();

    return data;
}
