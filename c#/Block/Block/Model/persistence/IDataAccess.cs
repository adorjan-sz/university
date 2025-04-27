using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block.Model.persistence
{
    public interface IDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<(BlockType[,], int)> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(String path, BlockType[,] table,int score);
    }
    public class  DataAcces : IDataAccess
    {
        public async Task<(BlockType[,],int)> LoadAsync(String path)
        {
            
            
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] strings; // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    int score = int.Parse(line);


                    BlockType[,] table = new BlockType[4, 4];
                    for (Int32 i = 0; i < 4; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        strings = line.Split(' ');

                        for (Int32 j = 0; j < 4; j++)
                        {
                            table[i,j] = (BlockType)Enum.Parse(typeof(BlockType), strings[j]); 
                        }
                    }


                    return (table,score);
                }
            
            
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, BlockType[,] table,int score)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {

                    writer.Write(score);
                    await writer.WriteLineAsync();
                    for (Int32 i = 0; i < 4; i++)
                    {
                        for (Int32 j = 0; j < 4; j++)
                        {
                            await writer.WriteAsync(table[i, j].ToString() + " "); // kiírjuk az értékeket
                        }
                        await writer.WriteLineAsync();
                    }

                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }

}
