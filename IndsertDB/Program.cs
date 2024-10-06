// See https://aka.ms/new-console-template for more information
using IndsertDB;

Console.WriteLine("Hello, World!");
// 创建 Insert 类的实例
Insert insert = new Insert();

// 定义数据文件的路径 (确保你指定了正确的文件路径)
string peopleFilePath = @"C:\Users\xiao\Desktop\sql\name.basics.tsv";
string moviesFilePath = @"C:\Users\xiao\Desktop\sql\title.basics.tsv";
string crewFilePath = @"C:\Users\xiao\Desktop\sql\title.crew.tsv";

try
{
    // 检查文件是否存在
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
    // 捕获并显示插入过程中发生的任何错误
    Console.WriteLine($"An error occurred during data insertion: {ex.Message}");
}

Console.WriteLine("Data insertion process completed.");