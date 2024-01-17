using System;

class Program
{
    static void Main(string[] args)
    {
        var person = GetPersonFromDatabase();

        // This will cause a NullReferenceException if the person is not found in the database
        Console.WriteLine(person.Name);
    }

    static Person GetPersonFromDatabase()
    {
        // Simulate a situation where the person is not found in the database
        return null;
    }
}

class Person
{
    public string Name { get; set; }
}