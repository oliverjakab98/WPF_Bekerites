using Bekerit.Persistence;
using Bekerites.Persistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bekerites.Persistence.File
{
    public class BekeritFileDataAccess : BekeritDataAccess
    {
        private String _saveDirectory;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="saveDirectory">Mentések útvonala.</param>
        public BekeritFileDataAccess(String saveDirectory)
        {
            _saveDirectory = saveDirectory;
        }

        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        /// 
        public async Task<BekeritTable> LoadAsync(String name)
        {
            String path = Path.Combine(_saveDirectory, name + ".stl"); // útvonal előállítása
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla méretét  
                    Int32 gameSteps = Int32.Parse(numbers[1]);
                    BekeritTable table = new BekeritTable(tableSize,gameSteps); // létrehozzuk a táblát
                    //table._gameSteps = gameSteps;

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');
                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            table.SetValue(i, j, Int32.Parse(numbers[j]));
                        }
                    }

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        String[] locks = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            if (locks[j] == "1")
                            {
                                table.LockValue(i, j);
                            }
                        }
                    }

                    return table;
                }
            }
            catch
            {
                throw new BekeritDataException();
            }
        }



        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String name, BekeritTable table)
        {
            String path = Path.Combine(_saveDirectory, name + ".stl"); // útvonal előállítása
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(table.Size); // kiírjuk a méreteket
                    await writer.WriteLineAsync(" " + table._gameSteps);
                    for (Int32 i = 0; i < table.Size; i++)
                    {
                        for (Int32 j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync(table[i, j] + " "); // kiírjuk az értékeket
                        }
                        await writer.WriteLineAsync();
                    }
                    for (Int32 i = 0; i < table.Size; i++)
                    {
                        for (Int32 j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync((table.IsLocked(i, j) ? "1" : "0") + " "); // kiírjuk a zárolásokat
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new BekeritDataException();
            }
        }


        /// <summary>
	    /// Játékállapot mentések lekérdezése.
	    /// </summary>
		public async Task<ICollection<SaveEntry>> ListAsync()
        {
            try
            {
                return Directory.GetFiles(_saveDirectory, "*.stl")
                    .Select(path => new SaveEntry
                    {
                        Name = Path.GetFileNameWithoutExtension(path),
                        //Time = File.GetCreationTime(path)
                    })
                    .ToList();
            }
            catch
            {
                throw new BekeritDataException();
            }
        }

    }
}
