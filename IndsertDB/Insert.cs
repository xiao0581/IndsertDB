using EFCore.BulkExtensions;
using IndsertDB.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB
{
    public class Insert
    {
        public void BulkInsertPeople(string filePath)
        {  
            var peopleList = new List<People>();
            var professionsList = new List<Profession>();
            var personProfessionsList = new List<PersonProfession>();
            var knownForList = new List<KnownFor>();

            // 使用字典保存 Person 和 ProfessionName 的映射
            var personToProfessionNames = new Dictionary<string, List<string>>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine(); // Skip header

                int counter = 0; // 初始化计数器
                int maxRecords = 60000; // 设置最多插入的记录数

                while ((line = reader.ReadLine()) != null && counter < maxRecords)
                {
                    var fields = line.Split('\t');
                    var person = new People
                    {
                        Nconst = fields[0],
                        PrimaryName = fields[1],
                        BirthYear = fields[2] == "\\N" ? null : (int?)int.Parse(fields[2]),
                        DeathYear = fields[3] == "\\N" ? null : (int?)int.Parse(fields[3])
                    };
                   

                    // Add to people list
                    peopleList.Add(person);

                    // 处理 professions 字段
                    if (fields.Length > 4 && !string.IsNullOrWhiteSpace(fields[4]))
                    {
                        var professions = fields[4].Split(',');

                        foreach (var professionName in professions)
                        {
                            var existingProfession = professionsList.FirstOrDefault(p => p.ProfessionName == professionName);

                            // 如果 professionsList 中不存在，则创建并添加
                            if (existingProfession == null)
                            {
                                var newProfession = new Profession
                                {
                                    ProfessionName = professionName
                                };
                                professionsList.Add(newProfession);
                            }

                            // 将 person 与 professionName 的关系保存到字典中
                            if (!personToProfessionNames.ContainsKey(person.Nconst))
                            {
                                personToProfessionNames[person.Nconst] = new List<string>();
                            }
                            personToProfessionNames[person.Nconst].Add(professionName);
                        }
                    }

                    // 处理 knownForTitles 字段
                    if (fields.Length > 5 && !string.IsNullOrWhiteSpace(fields[5]))
                    {
                        var knownForTitles = fields[5].Split(',');

                        foreach (var title in knownForTitles)
                        {
                            var knownFor = new KnownFor
                            {
                                Nconst = person.Nconst,
                                Tconst = title
                            };
                            knownForList.Add(knownFor);
                        }
                    }

                    counter++; // 递增计数器
                }
            }

            // 使用 BulkInsert 插入数据库
            using (var context = new ImdbDbContext())
            {
              
                // 插入 people 数据
                context.BulkInsert(peopleList);
                Console.WriteLine($"People inserted into the database.");

                // 插入 professionsList 并确保 professionId 已生成
                context.BulkInsert(professionsList);
                Console.WriteLine("Professions inserted into the database.");

                // 从数据库中重新读取 professionsList，以确保 professionId 正确更新
                var updatedProfessionsList = context.Professions.ToList();

                // 遍历字典以更新 personProfessionsList 中 professionId 的值
                foreach (var kvp in personToProfessionNames)
                {
                    var nconst = kvp.Key;
                    var professionNames = kvp.Value;

                    foreach (var professionName in professionNames)
                    {
                        // 在 updatedProfessionsList 中找到对应的 profession
                        var profession = updatedProfessionsList.FirstOrDefault(p => p.ProfessionName == professionName);
                        if (profession != null)
                        {
                            // 检查 personProfessionsList 中是否已经存在相同的 Nconst 和 ProfessionId
                            if (!personProfessionsList.Any(pp => pp.Nconst == nconst && pp.ProfessionId == profession.ProfessionId))
                            {
                                var personProfession = new PersonProfession
                                {
                                    Nconst = nconst,
                                    ProfessionId = profession.ProfessionId // 使用数据库中的 ProfessionId
                                };
                                personProfessionsList.Add(personProfession);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Could not find Profession with ProfessionName '{professionName}' in updatedProfessionsList.");
                        }
                    }
                }
               
                // 插入 person-professions 关系数据
                context.BulkInsert(personProfessionsList);
                Console.WriteLine("Person-Professions inserted into the database.");

                // 插入 knownFor 数据
                context.BulkInsert(knownForList);
                Console.WriteLine("KnownFor inserted into the database.");
            }
        }






        public void BulkInsertMovies(string filePath)
        {
            var moviesList = new List<Movie>();
            var genresList = new List<Genre>();
            var movieGenresList = new List<MovieGenre>();

            // 使用字典保存 Movie 和 GenreName 的映射
            var movieToGenreNames = new Dictionary<string, List<string>>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine(); // Skip header

                int counter = 0; // 初始化计数器
                int maxRecords = 60000; // 设置最多插入的记录数

                while ((line = reader.ReadLine()) != null && counter < maxRecords)
                {
                    var fields = line.Split('\t');
                    var movie = new Movie
                    {
                        Tconst = fields[0],
                        TitleType = fields[1],
                        PrimaryTitle = fields[2],
                        OriginalTitle = fields[3],
                        IsAdult = fields[4] == "1",
                        StartYear = fields[5] == "\\N" ? null : (int?)int.Parse(fields[5]),
                        EndYear = fields[6] == "\\N" ? null : (int?)int.Parse(fields[6]),
                        RuntimeMinutes = fields[7] == "\\N" ? null : (int?)int.Parse(fields[7])
                    };
                   

                    // Add movie to the list
                    moviesList.Add(movie);

                    // 处理 genres 字段
                    if (fields.Length > 8 && !string.IsNullOrWhiteSpace(fields[8]))
                    {
                        var genres = fields[8].Split(',');

                        foreach (var genreName in genres)
                        {
                            var existingGenre = genresList.FirstOrDefault(g => g.GenreName == genreName);

                            // 如果 genresList 中不存在该 genre，则创建并添加
                            if (existingGenre == null)
                            {
                                var newGenre = new Genre
                                {
                                    GenreName = genreName
                                };
                                genresList.Add(newGenre);
                            }

                            // 将 movie 与 genreName 的关系保存到字典中
                            if (!movieToGenreNames.ContainsKey(movie.Tconst))
                            {
                                movieToGenreNames[movie.Tconst] = new List<string>();
                            }
                            movieToGenreNames[movie.Tconst].Add(genreName);
                        }
                    }

                    counter++; // 递增计数器
                }
            }

            using (var context = new ImdbDbContext())
            {
               

                // 插入 movies 数据
                context.BulkInsert(moviesList);
                Console.WriteLine($"Movies inserted into the database.");

                // 插入 genresList 并确保 GenreId 已生成
                context.BulkInsert(genresList);
                Console.WriteLine("Genres inserted into the database.");

                // 从数据库中重新读取 genresList，以确保 GenreId 正确更新
                var updatedGenresList = context.Genres.ToList();

                // 遍历字典以更新 movieGenresList 中 GenreId 的值
                foreach (var kvp in movieToGenreNames)
                {
                    var tconst = kvp.Key;
                    var genreNames = kvp.Value;

                    foreach (var genreName in genreNames)
                    {
                        // 在 updatedGenresList 中找到对应的 genre
                        var genre = updatedGenresList.FirstOrDefault(g => g.GenreName == genreName);
                        if (genre != null)
                        {
                            // 检查 movieGenresList 中是否已经存在相同的 Tconst 和 GenreId
                            if (!movieGenresList.Any(mg => mg.Tconst == tconst && mg.GenreId == genre.GenreId))
                            {
                                var movieGenre = new MovieGenre
                                {
                                    Tconst = tconst,
                                    GenreId = genre.GenreId // 使用数据库中的 GenreId
                                };
                                movieGenresList.Add(movieGenre);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Could not find Genre with GenreName '{genreName}' in updatedGenresList.");
                        }
                    }
                }

              

                // 插入 movie-genres 关系数据
                context.BulkInsert(movieGenresList);
                Console.WriteLine("Movie-Genres inserted into the database.");
            }
        }

        public void BulkInsertCrew(string filePath)
        {
            var movieDirectorsList = new List<MovieDirector>();
            var movieWritersList = new List<MovieWriter>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine(); // Skip header

                int counter = 0; // 初始化计数器
                int maxRecords = 60000; // 设置最多插入的记录数

                while ((line = reader.ReadLine()) != null && counter < maxRecords)
                {
                    var fields = line.Split('\t');
                    var tconst = fields[0]; // movie ID (tconst)

                    // 处理 directors 字段
                    if (!string.IsNullOrWhiteSpace(fields[1]) && fields[1] != "\\N")
                    {
                        var directors = fields[1].Split(',');
                        foreach (var director in directors)
                        {
                            var movieDirector = new MovieDirector
                            {
                                Tconst = tconst,
                                Nconst = director
                            };
                            movieDirectorsList.Add(movieDirector);
                            
                        }
                    }

                    // 处理 writers 字段
                    if (!string.IsNullOrWhiteSpace(fields[2]) && fields[2] != "\\N")
                    {
                        var writers = fields[2].Split(',');
                        foreach (var writer in writers)
                        {
                            var movieWriter = new MovieWriter
                            {
                                Tconst = tconst,
                                Nconst = writer
                            };
                            movieWritersList.Add(movieWriter);
                            
                        }
                    }

                    counter++; // 递增计数器
                }
            }

            using (var context = new ImdbDbContext())
            {
                
                // 插入 movieDirectorsList 数据
                context.BulkInsert(movieDirectorsList);
                Console.WriteLine($"MovieDirectors inserted into the database.");

                // 插入 movieWritersList 数据
                context.BulkInsert(movieWritersList);
                Console.WriteLine("MovieWriters inserted into the database.");
            }
        }

    }

}

