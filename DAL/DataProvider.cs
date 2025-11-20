using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;
using Common;
using Abstraction;

#pragma warning disable SYSLIB0011

namespace DAL
{
    public class BinaryProvider<T> : IDataProvider<T> where T : Person
    {
        public List<T> Load(string filePath)
        {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                return new List<T>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (List<T>)formatter.Deserialize(fs);
            }
        }
        public void Save(string filePath, List<T> data)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }
    }

    public class XmlProvider<T> : IDataProvider<T> where T : Person
    {
        public List<T> Load(string filePath)
        {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                return new List<T>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (List<T>)serializer.Deserialize(fs);
            }
        }
        public void Save(string filePath, List<T> data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, data);
            }
        }
    }

    public class JsonProvider<T> : IDataProvider<T> where T : Person
    {
        public List<T> Load(string filePath)
        {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                return new List<T>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        public void Save(string filePath, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }

    public class CustomProvider<T> : IDataProvider<T> where T : Person
    {
        private string GetLine(T entity)
        {
            if (entity is Student s)
            {
                return $"Student|{s.FirstName}|{s.LastName}|{s.Passport}|{s.StudentID}|{s.Course}|{s.MilitaryID}";
            }
            else if (entity is FootballPlayer fp)
            {
                return $"FootballPlayer|{fp.FirstName}|{fp.LastName}|{fp.Passport}|{fp.Team}";
            }
            else if (entity is Lawyer l)
            {
                return $"Lawyer|{l.FirstName}|{l.LastName}|{l.Passport}|{l.Company}";
            }
            return "";
        }

        public List<T> Load(string filePath)
        {
            List<T> data = new List<T>();
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                return data;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split('|');
                if (parts.Length > 0 && parts[0] == typeof(T).Name)
                {
                    try
                    {
                        if (parts[0] == "Student")
                        {
                            data.Add((T)(object)new Student(parts[1], parts[2], parts[3], parts[4], int.Parse(parts[5]), parts[6]));
                        }
                        else if (parts[0] == "FootballPlayer")
                        {
                            data.Add((T)(object)new FootballPlayer(parts[1], parts[2], parts[3], parts[4]));
                        }
                        else if (parts[0] == "Lawyer")
                        {
                            data.Add((T)(object)new Lawyer(parts[1], parts[2], parts[3], parts[4]));
                        }
                    }
                    catch { }
                }
            }
            return data;
        }

        public void Save(string filePath, List<T> data)
        {
            List<string> lines = new List<string>();
            foreach (T entity in data)
            {
                lines.Add(GetLine(entity));
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}