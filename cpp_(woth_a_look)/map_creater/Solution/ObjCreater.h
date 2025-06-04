#ifndef OBJ_CREATER_H
#define OBJ_CREATER_H
#include <vector>
#include <map>
#include <fstream>

class ObjCreater {
public:
    // Loads a grayscale image and writes the corresponding 3D mesh to an OBJ file
    bool Load(const char* filename, const char* outfilename);

private:
    // Writes a single vertex to the OBJ file
    void write_obj(std::ofstream& file, int x, int y, float z);

    // Calculates height from pixel and optionally writes vertex to OBJ
    float CreateVertex(
        int x, int y,
        unsigned char* img,
        int width,
        int& vertexIndexCounter,
        std::map<int, int>& vertex_map,
        std::ofstream& file);

    // Represents a triangle of vertex indices
    struct Triangel {
        int v1, v2, v3;
    };
};

#endif 