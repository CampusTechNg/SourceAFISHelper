using SourceAFIS.Simple;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceAFISHelper
{
    [Serializable]
    public class IdpPerson : Person
    {
        public string ID { get; set; }

        public DateTime DateRegistered { get; set; }
        public string DateRegisteredString { get; set; }
        public DateTime DoB { get; set; }
        public string DoBString { get; set; }
        public string FirstName { get; set; }
        public string[] FingerprintLocations { get; set; } = new string[10];
        public string Gender { get; set; }
        public string LastName { get; set; }
        public string LGA { get; set; }
        public string MaritalStatus { get; set; }
        public string OtherNames { get; set; }
        public Image Photo { get; set; }
        public string PhotoLocation { get; set; }
        public string State { get; set; }
        public int YoB { get; set; }
        public string YoBString { get; set; }

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }
        public string ToJSON()
        {
            List<string> jsonFields = new List<string>();
            if (this.ID != null)
            {
                jsonFields.Add($"\"id\": \"{this.ID}\"");
            }
            if (this.DateRegisteredString != null)
            {
                jsonFields.Add($"\"date_registered\": \"{this.DateRegisteredString}\"");
            }

            if (this.DoBString != null)
            {
                jsonFields.Add($"\"dob\": \"{this.DoBString}\"");
            }
            if (this.FirstName != null)
            {
                jsonFields.Add($"\"first_name\": \"{this.FirstName}\"");
            }
            if (this.Gender != null)
            {
                jsonFields.Add($"\"gender\": \"{this.Gender}\"");
            }
            if (this.LastName != null)
            {
                jsonFields.Add($"\"last_name\": \"{this.LastName}\"");
            }
            if (this.LGA != null)
            {
                jsonFields.Add($"\"lga\": \"{this.LGA}\"");
            }
            if (this.MaritalStatus != null)
            {
                jsonFields.Add($"\"marital_status\": \"{this.MaritalStatus}\"");
            }
            if (this.OtherNames != null)
            {
                jsonFields.Add($"\"other_names\": \"{this.OtherNames}\"");
            }
            if (this.PhotoLocation != null)
            {
                jsonFields.Add($"\"photo\": \"{this.PhotoLocation}\"");
            }
            if (this.State != null)
            {
                jsonFields.Add($"\"state\": \"{this.State}\"");
            }
            if (this.YoBString != null)
            {
                jsonFields.Add($"\"yob\": \"{this.YoBString}\"");
            }
            //still contemplating if I need to allow figers to be requested for...

            string json = "{" + string.Join(",", jsonFields.ToArray()) + "}";
            return json;
        }
        public string ToJSON(string[] fields)
        {
            List<string> jsonFields = new List<string>();
            if (this.ID != null) //ID is always sent
            {
                jsonFields.Add($"\"id\": \"{this.ID}\"");
            }
            if (fields.Any(f => f.ToLower() == "date_registered") && this.DateRegisteredString != null)
            {
                jsonFields.Add($"\"date_registered\": \"{this.DateRegisteredString}\"");
            }

            if (fields.Any(f => f.ToLower() == "dob") && this.DoBString != null)
            {
                jsonFields.Add($"\"dob\": \"{this.DoBString}\"");
            }
            if (fields.Any(f => f.ToLower() == "first_name") && this.FirstName != null)
            {
                jsonFields.Add($"\"first_name\": \"{this.FirstName}\"");
            }
            if (fields.Any(f => f.ToLower() == "gender") && this.Gender != null)
            {
                jsonFields.Add($"\"gender\": \"{this.Gender}\"");
            }
            if (fields.Any(f => f.ToLower() == "last_name") && this.LastName != null)
            {
                jsonFields.Add($"\"last_name\": \"{this.LastName}\"");
            }
            if (fields.Any(f => f.ToLower() == "lga") && this.LGA != null)
            {
                jsonFields.Add($"\"lga\": \"{this.LGA}\"");
            }
            if (fields.Any(f => f.ToLower() == "marital_status") && this.MaritalStatus != null)
            {
                jsonFields.Add($"\"marital_status\": \"{this.MaritalStatus}\"");
            }
            if (fields.Any(f => f.ToLower() == "other_names") && this.OtherNames != null)
            {
                jsonFields.Add($"\"other_names\": \"{this.OtherNames}\"");
            }
            if (fields.Any(f => f.ToLower() == "photo") && this.PhotoLocation != null)
            {
                jsonFields.Add($"\"photo\": \"{this.PhotoLocation}\"");
            }
            if (fields.Any(f => f.ToLower() == "state") && this.State != null)
            {
                jsonFields.Add($"\"state\": \"{this.State}\"");
            }
            if (fields.Any(f => f.ToLower() == "yob") && this.YoBString != null)
            {
                jsonFields.Add($"\"yob\": \"{this.YoBString}\"");
            }
            //still contemplating if I need to allow figers to be requested for...

            string json = "{" + string.Join(",", jsonFields.ToArray()) + "}";
            return json;
        }
    }
}
