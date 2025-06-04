#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"
#include "ObjCreater.h"

#include <iostream>

void ObjCreater::write_obj(std::ofstream& file, int x, int y, float z) {
    file << "v " << x << " " << y << " " << z << "\n";
    }
float ObjCreater::CreateVertex(
    int x, int y,
    unsigned char* img,
    int width,
    int& vertexIndexCounter,
    std::map<int, int>& vertex_map,
    std::ofstream& file)
    {
    int index = y * width + x;
    unsigned char pixel = img[index];
    float z = (pixel / 255.0f) * 200.0f;

    // create vertex if height is greater than 0
    if (z != 0) {
        vertex_map[index] = ++vertexIndexCounter;
        write_obj(file, x, y, z);
    }
    return z;
}
        
   
    
bool ObjCreater::Load(const char* filename, const char* outfilename) {
    // Load an image
    int width, height, channels;
    unsigned char* img = stbi_load(filename, &width, &height, &channels, 1);

    // Check if the image was loaded successfully
    if (img == NULL) {
        std::cerr << "Failed to open img: " << filename << "\n";
        return false;
    }

          
    // Try to open the file for writing
    std::ofstream file(outfilename);
    if (!file.is_open()) {
        std::cerr << "Failed to open file: " << outfilename << "\n";
        return false;
     }


	//channels is the original image's channels but only 1 chanel is used actually
    std::cout << "Image dimensions: " << width << "x" << height<< ", Channels: " << channels << std::endl;


    std::vector<Triangel*> triangels;
    std::map<int, int> vertex_map;
    int VertexIndex = 0;
 
       /*
    The algorithm:
    it starts from the second row and the second column.
    First it checks if current height is greater than 0 and if so then we save the vertex and put the vertex's index in the vertex_map.
    Then we try to create two triangles:
	    Left triangle: current vertex -> vertex above and to the left -> vertex left
	    Right triangle: current vertex -> vertex above -> vertex above and to the left
    If any height for the vertex of the triangle is 0 then we skip the triangle.
       */

    for (int x = 0; x < width; ++x) {
    //create vertex for the first row
        CreateVertex(x, 0, img, width, VertexIndex, vertex_map, file);
    
    
    }
    float z;
    for (int y = 1; y < height; ++y) {
    
        //create vertex for the first column
        CreateVertex(y, 0, img, width, VertexIndex, vertex_map, file);
    
    
        for (int x = 1; x < width; ++x) {
    
            z = CreateVertex(x, y, img, width, VertexIndex, vertex_map, file);
            //ignore if hight is 0
            if (z != 0) {
    
                //igonre a triangle if any corner's height is 0
                if (!(img[(y - 1) * width + x] == 0 || img[(y - 1) * width + x - 1] == 0)) {
        
                    Triangel* triangel = new Triangel();
                    triangel->v1 = vertex_map[y * width + x];
                    triangel->v2 = vertex_map[(y - 1) * width + x];
                    triangel->v3 = vertex_map[(y - 1) * width + x - 1];

                   triangels.push_back(triangel);
                }
                if (!(img[(y - 1) * width + (x - 1)] == 0 || img[y * width + (x - 1)] == 0)) {

                    Triangel* triangel = new Triangel();
                    triangel->v1 = vertex_map[y * width + x];
                    triangel->v2 = vertex_map[(y - 1) * width + (x - 1)];
                    triangel->v3 = vertex_map[y * width + (x - 1)];

                    triangels.push_back(triangel);

                }
            }
        }
    }

    // Write faces to the OBJ file
    for (int index = 0; index < triangels.size(); ++index) {

        file << "f " << triangels[index]->v1 << " " << triangels[index]->v2 << " " << triangels[index]->v3 << "\n";
    }

    file.close();
	// Free memory
    stbi_image_free(img);
    for (int index = 0; index < triangels.size(); ++index) {
	    delete triangels[index];
    }
    triangels.clear();
    return true;
}
