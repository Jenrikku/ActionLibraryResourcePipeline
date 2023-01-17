#include <cstdint>
#include <oead/byml.h>

extern "C" const u8* YamlToByaml(const char* yamlData, int version = 1, bool bigEndian = false)
{
    oead::Byml byml(oead::Byml::FromText(yamlData));

    int convertVersion = version == 1 ? 2 : version; // use version 2 instead because its the same but oead doesnt realize that and says it can't do v1
    std::vector<u8> dataVec = byml.ToBinary(bigEndian, convertVersion);
    u8* data = new u8[sizeof(u32) + dataVec.size()];

    std::memcpy(data + sizeof(u32), dataVec.data(), dataVec.size());

    struct {
        u32 size;
        char magic[2];
        u16 version;
    }* header = reinterpret_cast<typeof(header)>(data);
    header->size = dataVec.size();

    if (version == 1)
        header->version = 1;

    return data;
}
