cmake_minimum_required(VERSION 3.5)
project(OeadWrapper)

add_subdirectory(oead)

set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(CMAKE_CXX_STANDARD 20)

include_directories(${CMAKE_SOURCE_DIR}/Include)
include_directories(${CMAKE_SOURCE_DIR}/oead/src/include)
include_directories(${CMAKE_SOURCE_DIR}/lib/nonstd)

file(GLOB_RECURSE cxxsourcefiles "${CMAKE_SOURCE_DIR}/Source/*.cpp")
add_library(OeadWrapper SHARED ${cxxsourcefiles})
target_link_libraries(OeadWrapper PUBLIC oead)
