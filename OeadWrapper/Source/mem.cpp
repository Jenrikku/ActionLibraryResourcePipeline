#include <oead/types.h>

extern "C" void FreeData(u8* data)
{
    delete[] data;
}
