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
                int maxRecords = 10; // 设置最多插入的记录数

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
                    Console.WriteLine($"Inserting Person: Nconst = {person.Nconst}, PrimaryName = {person.PrimaryName}, BirthYear = {person.BirthYear}, DeathYear = {person.DeathYear}");

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
                foreach (var personProfession in personProfessionsList)
                {
                    Console.WriteLine($"PersonProfession - Nconst: {personProfession.Nconst}, ProfessionId: {personProfession.ProfessionId}");
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
            var movieToGenreName = new Dictionary<string, List<string>>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine(); // Skip header

                while ((line = reader.ReadLine()) != null)
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
                    Console.WriteLine($"Inserting Movie: Tconst = {movie.Tconst}, TitleType = {movie.TitleType}, PrimaryTitle = {movie.PrimaryTitle}, OriginalTitle = {movie.OriginalTitle}, IsAdult = {movie.IsAdult}");

                    moviesList.Add(movie);

                    // 处理 Genre
                    var genres = fields[8].Split(',');
                    foreach (var genre in genres)
                    {
                        var existingGenre = genresList.FirstOrDefault(g => g.GenreName == genre);
                        if (existingGenre == null)
                        {
                            var newGenre = new Genre { GenreName = genre };
                            genresList.Add(newGenre);
                            existingGenre = newGenre;
                        }
                        movieGenresList.Add(new MovieGenre
                        {
                            Tconst = movie.Tconst,
                            GenreId = existingGenre.GenreId
                        });
                        Console.WriteLine($"Inserting MovieGenre: Tconst = {movie.Tconst}, GenreId = {existingGenre.GenreId}");

                    }
                }
            }
            Console.WriteLine($"Inserting {moviesList.Count} movies and {movieGenresList.Count} movie genres into the database");

            using (var context = new ImdbDbContext())
            {
                // 确保只插入实际表列
                context.BulkInsert(moviesList);
                context.BulkInsert(genresList);
                context.BulkInsert(movieGenresList);
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

                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split('\t');
                    var tconst = fields[0];

                    // 处理 Directors
                    if (fields[1] != "\\N")
                    {
                        var directors = fields[1].Split(',');
                        foreach (var director in directors)
                        {
                            movieDirectorsList.Add(new MovieDirector { Tconst = tconst, Nconst = director });
                            Console.WriteLine($"Inserting MovieDirector: Tconst = {tconst}, Nconst = {director}");

                        }
                    }

                    // 处理 Writers
                    if (fields[2] != "\\N")
                    {
                        var writers = fields[2].Split(',');
                        foreach (var writer in writers)
                        {
                            movieWritersList.Add(new MovieWriter { Tconst = tconst, Nconst = writer });
                            Console.WriteLine($"Inserting MovieWriter: Tconst = {tconst}, Nconst = {writer}");

                        }
                    }
                }
            }
            Console.WriteLine($"Inserting {movieDirectorsList.Count} movie directors and {movieWritersList.Count} movie writers into the database");

            using (var context = new ImdbDbContext())
            {
                // 确保只插入实际表列
                context.BulkInsert(movieDirectorsList);
                context.BulkInsert(movieWritersList);
            }
        }

    }

}

