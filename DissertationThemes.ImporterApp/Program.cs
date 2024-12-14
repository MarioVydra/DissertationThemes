using CsvHelper;
using DissertationThemes.SharedLibrary;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace DissertationThemes.ImporterApp
{
    internal class Program
    {
        static int Main(string[] args)
        {
            string path = string.Empty;
            bool removeData = false;

            if (args.Length > 2)
            {
                Console.Error.WriteLine("Error: Too many arguments.");
                return 1;
            }

            if (args.Length == 0)
            {
                Console.Write("Insert path for CSV file: ");
                path = Console.ReadLine();
            }
            else if (args.Length == 1) 
            {
                if (args[0] == "-r" || args[0] == "--remove-previous-data")
                {
                    removeData = true;
                    Console.Write("Insert path for CSV file: ");
                    path = Console.ReadLine();
                }
                else 
                {
                    path = args[0];
                }
            }
            else
            {
                path = args[0];
                removeData = args[1] == "-r" || args[1] == "--remove-previous-data";
            }

            if (!File.Exists(path))
            {
                Console.Error.WriteLine("Error: File does not exists.");
                return 1;
            }

            try
            {
                using (var db = new DissertationThemesContext()) 
                {
                    if (removeData) 
                    {
                        db.Themes.RemoveRange(db.Themes);
                        db.Supervisors.RemoveRange(db.Supervisors);
                        db.StProgram.RemoveRange(db.StProgram);
                        db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'StProgram';");
                        db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Themes';");
                        db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Supervisors';");
                        db.SaveChanges();
                        Console.WriteLine("Previous data was removed successfully.");
                    }

                    using (var reader = new StreamReader(path))
                    using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";",
                        NewLine = "\r\n",
                    }))
                    { 
                        var records = csvReader.GetRecords<ThemeRecord>().ToList();

                        foreach (var record in records)
                        {
                            var program = db.StProgram.FirstOrDefault(p => p.Name == record.StProgram);

                            if (program == null)
                            {
                                program = new StProgram { Name = record.StProgram, FieldOfStudy = record.FieldOfStudy };
                                db.StProgram.Add(program);
                                db.SaveChanges();
                            }

                            var supervisor = db.Supervisors.FirstOrDefault(s => s.FullName == record.Supervisor);

                            if (supervisor == null)
                            {
                                supervisor = new Supervisor { FullName = record.Supervisor };
                                db.Supervisors.Add(supervisor);
                                db.SaveChanges();
                            }

                            var theme = new Theme
                            {
                                Name = record.Name,
                                Description = record.Descrition.Replace("<br>", Environment.NewLine),
                                IsFullTimeStudy = record.IsFullTimeStudy,
                                IsExternalStudy = record.IsExternalStudy,
                                ResearchType = ResearchTypeConverter.FromSlovakString(record.ResearchType),
                                Created = DateTime.Parse(record.Created),
                                Supervisor = supervisor,
                                StProgram = program
                            };

                            db.Themes.Add(theme);
                            db.SaveChanges();
                        }

                        Console.WriteLine("Data imported successfully.");
                    }
                }
                return 0;
            }
            catch (Exception ex) {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }
    }
}
