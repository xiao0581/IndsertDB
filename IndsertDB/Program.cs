
using IndsertDB;

Console.WriteLine("starting insert!");

Insert insert = new Insert();


string peopleFilePath = @"C:\Users\xiaohui\Desktop\4.Sql\name.basics.tsv";
string moviesFilePath = @"C:\Users\xiaohui\Desktop\4.Sql\title.basics.tsv";
string crewFilePath = @"C:\Users\xiaohui\Desktop\4.Sql\title.crew.tsv";

try
{

    if (File.Exists(peopleFilePath))
    {
        insert.BulkInsertPeople(peopleFilePath);
        Console.WriteLine("People data inserted successfully!");
    }
    else
    {
        Console.WriteLine($"People file not found: {peopleFilePath}");
    }

    if (File.Exists(moviesFilePath))
    {
        insert.BulkInsertMovies(moviesFilePath);
        Console.WriteLine("Movie data inserted successfully!");
    }
    else
    {
        Console.WriteLine($"Movie file not found: {moviesFilePath}");
    }

    if (File.Exists(crewFilePath))
    {
        insert.BulkInsertCrew(crewFilePath);
        Console.WriteLine("Crew data inserted successfully!");
    }
    else
    {
        Console.WriteLine($"Crew file not found: {crewFilePath}");
    }
}
catch (Exception ex)
{
   
    Console.WriteLine($"An error occurred during data insertion: {ex.Message}");
}

Console.WriteLine("Data insertion process completed.");