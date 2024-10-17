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

            // use dictionary to save the mapping of person and profession names
            var personToProfessionNames = new Dictionary<string, List<string>>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine(); // Skip header

                int counter = 0; 
                int maxRecords = 6000; // herwe set the max records to insert

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
                   

                    
                    peopleList.Add(person);

                    // handle professions field
                    if (fields.Length > 4 && !string.IsNullOrWhiteSpace(fields[4]))
                    {
                        var professions = fields[4].Split(',');

                        foreach (var professionName in professions)
                        {
                            var existingProfession = professionsList.FirstOrDefault(p => p.ProfessionName == professionName);

                            
                            if (existingProfession == null)
                            {
                                var newProfession = new Profession
                                {
                                    ProfessionName = professionName
                                };
                                professionsList.Add(newProfession);
                            }

                           
                            if (!personToProfessionNames.ContainsKey(person.Nconst))
                            {
                                personToProfessionNames[person.Nconst] = new List<string>();
                            }
                            personToProfessionNames[person.Nconst].Add(professionName);
                        }
                    }

                    // handle knownForTitles field
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

                    counter++; 
                }
            }

            // use the bulk insert to insert the data
            using (var context = new ImdbDbContext())
            {
              
              
                context.BulkInsert(peopleList);
                Console.WriteLine($"People inserted into the database.");

               
                context.BulkInsert(professionsList);
                Console.WriteLine("Professions inserted into the database.");

                // re-read professionsList from the database to ensure ProfessionId is updated
                var updatedProfessionsList = context.Professions.ToList();

                // loop through the dictionary to update the ProfessionId in personProfessionsList
                foreach (var kvp in personToProfessionNames)
                {
                    var nconst = kvp.Key;
                    var professionNames = kvp.Value;

                    foreach (var professionName in professionNames)
                    {
                        // find the profession in updatedProfessionsList
                        var profession = updatedProfessionsList.FirstOrDefault(p => p.ProfessionName == professionName);
                        if (profession != null)
                        {
                            // check if the same Tconst and ProfessionId already exists in personProfessionsList
                            if (!personProfessionsList.Any(pp => pp.Nconst == nconst && pp.ProfessionId == profession.ProfessionId))
                            {
                                var personProfession = new PersonProfession
                                {
                                    Nconst = nconst,
                                    ProfessionId = profession.ProfessionId // use the ProfessionId from the database
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
               
               
                context.BulkInsert(personProfessionsList);
                Console.WriteLine("Person-Professions inserted into the database.");

        
                context.BulkInsert(knownForList);
                Console.WriteLine("KnownFor inserted into the database.");
            }
        }






        public void BulkInsertMovies(string filePath)
        {
            var moviesList = new List<Movie>();
            var genresList = new List<Genre>();
            var movieGenresList = new List<MovieGenre>();

            // use dictionary to save the mapping of movie and genre names
            var movieToGenreNames = new Dictionary<string, List<string>>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine(); // Skip header

                int counter = 0; 
                int maxRecords = 6000; // set the max records to insert

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
                   

                 
                    moviesList.Add(movie);

                    // handle genres field
                    if (fields.Length > 8 && !string.IsNullOrWhiteSpace(fields[8]))
                    {
                        var genres = fields[8].Split(',');

                        foreach (var genreName in genres)
                        {
                            var existingGenre = genresList.FirstOrDefault(g => g.GenreName == genreName);

                            
                            if (existingGenre == null)
                            {
                                var newGenre = new Genre
                                {
                                    GenreName = genreName
                                };
                                genresList.Add(newGenre);
                            }

                          
                            if (!movieToGenreNames.ContainsKey(movie.Tconst))
                            {
                                movieToGenreNames[movie.Tconst] = new List<string>();
                            }
                            movieToGenreNames[movie.Tconst].Add(genreName);
                        }
                    }

                    counter++; 
                }
            }
            // use the bulk insert to insert the data
            using (var context = new ImdbDbContext())
            {
               

                
                context.BulkInsert(moviesList);
                Console.WriteLine($"Movies inserted into the database.");

               
                context.BulkInsert(genresList);
                Console.WriteLine("Genres inserted into the database.");

             
                var updatedGenresList = context.Genres.ToList();

                // loop through the dictionary to update the GenreId in movieGenresList
                foreach (var kvp in movieToGenreNames)
                {
                    var tconst = kvp.Key;
                    var genreNames = kvp.Value;

                    foreach (var genreName in genreNames)
                    {
                        
                        var genre = updatedGenresList.FirstOrDefault(g => g.GenreName == genreName);
                        if (genre != null)
                        {
                          
                            if (!movieGenresList.Any(mg => mg.Tconst == tconst && mg.GenreId == genre.GenreId))
                            {
                                var movieGenre = new MovieGenre
                                {
                                    Tconst = tconst,
                                    GenreId = genre.GenreId 
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
                reader.ReadLine(); 

                int counter = 0; 
                int maxRecords = 6000; // set the max records to insert

                while ((line = reader.ReadLine()) != null && counter < maxRecords)
                {
                    var fields = line.Split('\t');
                    var tconst = fields[0]; // movie ID (tconst)

                    //handle directors field
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

                    // handle writers field
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

                    counter++; 
                }
            }
            // use the bulk insert to insert the data
            using (var context = new ImdbDbContext())
            {
                
               
                context.BulkInsert(movieDirectorsList);
                Console.WriteLine($"MovieDirectors inserted into the database.");

                context.BulkInsert(movieWritersList);
                Console.WriteLine("MovieWriters inserted into the database.");
            }
        }

    }

}

