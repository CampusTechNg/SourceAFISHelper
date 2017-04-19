using SourceAFIS.Simple;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceAFISHelper
{
    class Program
    {
        static AfisEngine Afis = new AfisEngine();
        static IdpDb db = new IdpDb();

        static Dictionary<ArgType, string> commandLineArgs = new Dictionary<ArgType, string>();
        static ArgType currentArgType;

        static void Main(string[] args)
        {
            try
            {
                //assume the first arg to be the input file
                currentArgType = ArgType.Action;

                //then look for other args
                for (int index = 0; index < args.Length; index++)
                {
                    if (args[index].StartsWith("-")) //an attribute
                    {
                        if (args[index] == "--action" || args[index] == "-a")
                        {
                            currentArgType = ArgType.Action;
                        }
                        else if (args[index] == "--data" || args[index] == "-d")
                        {
                            currentArgType = ArgType.Data;
                        }
                    }
                    else //a value
                    {
                        commandLineArgs.Add(currentArgType, args[index]);
                    }
                }

                if (commandLineArgs[ArgType.Action] == "add-activity")
                {
                    AddActivity(commandLineArgs[ArgType.Data]);
                }
                else if (commandLineArgs[ArgType.Action] == "enroll")
                {
                    Enroll(commandLineArgs[ArgType.Data]);
                }
                else if (commandLineArgs[ArgType.Action] == "get")
                {
                    Get(commandLineArgs[ArgType.Data]);
                }
                else if (commandLineArgs[ArgType.Action] == "identify")
                {
                    Identify(commandLineArgs[ArgType.Data]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($">> Error : {ex.Message}");
            }
            Console.Read();
        }

        static void AddActivity(string data)
        {
            data = data.Trim();
            string id = null, activity = null, date = null;

            string[] dataParts = data.Split(';');
            dataParts.Where(part => part.Contains('=')).ToList().ForEach(part =>
            {
                var key = part.Substring(0, part.IndexOf('=')).Trim();
                var value = part.Substring(part.IndexOf('=') + 1).Trim();

                if (key.ToLower() == "id")
                    id = value;
                else if (key.ToLower() == "activity")
                    activity = value;
                else if (key.ToLower() == "date")
                    date = value;
            });
            if (db.SaveActivity(id, activity, date))
            {
                Console.WriteLine("SourceAFISHelper.exe/add-activity/success:true");
            }
            else
            {
                Console.WriteLine("SourceAFISHelper.exe/add-activity/failure:Could not save activity to the database");
            }
            //Console.Read();
        }
        static void Enroll(string data)
        {
            data = data.Trim();
            IdpRecord record = new IdpRecord();

            string[] dataParts = data.Split(';');
            dataParts.Where(part => part.Contains('=')).ToList().ForEach(part =>
            {
                var key = part.Substring(0, part.IndexOf('=')).Trim();
                var value = part.Substring(part.IndexOf('=') + 1).Trim();

                if (key.ToLower() == "id")
                    record.ID = value;
                else if (key.ToLower() == "first_name")
                    record.FirstName = value;
                else if (key.ToLower() == "last_name")
                    record.LastName = value;
                else if (key.ToLower() == "other_names")
                    record.OtherNames = value;
                else if (key.ToLower() == "dob")
                    record.DoB = value;
                else if (key.ToLower() == "yob")
                    record.YoB = value;
                else if (key.ToLower() == "gender")
                    record.Gender = value;
                else if (key.ToLower() == "marital_status")
                    record.MaritalStatus = value;
                else if (key.ToLower() == "state")
                    record.State = value;
                else if (key.ToLower() == "lga")
                    record.LGA = value;
                else if (key.ToLower() == "photo")
                    record.Photo = value;
                else if (key.ToLower().StartsWith("finger_"))
                {
                    int position = int.Parse(key.Substring(7));
                    record.Fingers[position - 1] = value;
                }
            });
            string id;
            if(db.SaveRecord(record, out id))
            {
                Console.WriteLine("SourceAFISHelper.exe/enroll/success:" + id);
            }
            else
            {
                Console.WriteLine("SourceAFISHelper.exe/enroll/failure:Could not save record to the database");
            }
            //Console.Read();
        }
        static void Get(string data)
        {
            data = data.Trim();
            string id = null;
            string[] fields = null;

            if (data.Contains('='))
            {
                string[] dataParts = data.Split(';');
                dataParts.Where(part => part.Contains('=')).ToList().ForEach(part =>
                {
                    var key = part.Substring(0, part.IndexOf('=')).Trim();
                    var value = part.Substring(part.IndexOf('=') + 1).Trim();

                    if (key.ToLower() == "id")
                        id = value;
                    else if (key.ToLower() == "fields")
                        fields = value.Split(',').Select<string, string>(f => f.Trim()).ToArray();
                });
            }
            else
            {
                id = data;
            }

            if (id != null)
            {
                var person = db.GetPerson(id);
                if (person != null)
                {
                    if (fields != null)
                    {
                        Console.WriteLine("SourceAFISHelper.exe/get/success:" + person.ToJSON(fields));
                    }
                    else
                    {
                        Console.WriteLine("SourceAFISHelper.exe/get/success:" + person.ToJSON());
                    }
                }
                else
                {
                    Console.WriteLine("SourceAFISHelper.exe/get/failure:Could not find person with the specified ID");
                }
            }
            else
            {
                Console.WriteLine("SourceAFISHelper.exe/get/failure:No ID specified");
            }
        }
        static void Identify(string data)
        {
            data = data.Trim();

            string fingerImage = null;
            if (data.Contains('='))
            {
                fingerImage = data.Substring(data.IndexOf('=') + 1).Trim();
            }
            else
            {
                fingerImage = data;
            }

            //incoming finger
            Fingerprint fp = new Fingerprint();
            fp.AsBitmap = new Bitmap(Bitmap.FromFile(fingerImage));
            Person person = new Person();
            person.Fingerprints.Add(fp);
            Afis.Extract(person);

            var testing = fp.Template;
            var testing2 = person.Fingerprints[0].Template;
            var th = Afis.Threshold;

            //all candidates
            var allPersons = db.GetPersons();
            IdpPerson match = (Afis.Identify(person, allPersons).FirstOrDefault() as IdpPerson);
            if(match != null)
            {
                Console.WriteLine("SourceAFISHelper.exe/identify/success:" + match.ID);
            }
            else
            {
                Console.WriteLine("SourceAFISHelper.exe/identify/failure:Could not retrieve record from the database");
            }
        }

        enum ArgType
        {
            Action = 0,
            Data,
        }
    }
}
