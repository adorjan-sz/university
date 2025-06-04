#include "ObjCreater.h"
#include <iostream>

int main() {
    ObjCreater creater;

    const char* inputImage = "../heightmaps/img_1.png";     
    const char* outputObj = "img_1.obj";          

    if (creater.Load(inputImage, outputObj)) {
        std::cout << "OBJ file generated successfully: " << outputObj << "\n";
    }
    else {
        std::cerr << "Failed to generate OBJ file.\n";
    }


    inputImage = "../heightmaps/img_2.png";
    outputObj = "img_2.obj";

    if (creater.Load(inputImage, outputObj)) {
        std::cout << "OBJ file generated successfully: " << outputObj << "\n";
    }
    else {
        std::cerr << "Failed to generate OBJ file.\n";
    }

    return 0;
}
