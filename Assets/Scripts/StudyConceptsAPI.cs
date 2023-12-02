using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StudyConceptsAPI
{
    public class StudyConcept
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public Grade Grade { get; set; }
        public Cluster Cluster { get; set; }
        public Mastery Mastery { get; set; }
        public Domain Domain { get; set; }
        public Standard Standard { get; set; }


        public StudyConcept(SerializedConcept concept)
        {
            Id = concept.id;
            Subject = new Subject(concept.subject);
            Grade = new Grade(concept.grade);
            Cluster = new Cluster(concept.cluster);
            Mastery = (Mastery)concept.mastery;
            Domain = new Domain(concept.domainid, concept.domain);
            Standard = new Standard(concept.standardid, concept.standarddescription);
        }


        override public string ToString()
        { 
            return Standard.Id + " " + Standard.Description;
        }
    }
    public class StudyConcepts
    {
        public List<StudyConcept> studyConcepts;

        public async Task init()
        {
            await populateStudyConcepts(new SerializedConcepts());   
        }
        
        private async Task populateStudyConcepts(SerializedConcepts concepts)
        {

            await concepts.FetchJSON();

            studyConcepts = concepts.TempSerializedConcepts.Select(concept => new StudyConcept(concept)).ToList();
        }

    }
    public class Domain
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public Domain(string id, string name)
        {
            Id = id;
            Name = name;
        }


    }
    public class Cluster
    {
        public string Name { get; private set; }
        public Cluster(string name)
        {
            Name = name;
        }
    }
    public class Subject
    {

        public string Name{ get; private set; }
        public Subject(string name)
        {
            Name = name;
        }
    }

    public class Standard {

        public string Id { get; private set; }
        public string Description { get; private set; }

        public Standard(string id, string description)
        {
            Id = id;
            Description = description;

        }

    }
    public class Grade {
        public string Name { get; set; }

        public Grade(string name)
        {
            Name = name;
        }
    }


    public enum Mastery { Unlearned, Learned, Mastered }


    public class SerializedConcepts
    {
        public List<SerializedConcept> TempSerializedConcepts { get; set; }

        public async Task FetchJSON()
        {
            string url = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonData = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(jsonData); // Log the JSON data

                    // Deserialize the JSON directly into a List of SerializedConcept
                    TempSerializedConcepts = JsonConvert.DeserializeObject<List<SerializedConcept>>(jsonData);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }
                catch (JsonSerializationException jsonEx)
                {
                    Console.WriteLine($"JSON Serialization Exception: {jsonEx.Message}");
                }
            }
        }
    }

    [Serializable]
    public class SerializedConcept
    {
        public int id;
        public string subject;
        public string grade;
        public int mastery;
        public string domainid;
        public string domain;
        public string cluster;
        public string standardid;
        public string standarddescription;
    }
}