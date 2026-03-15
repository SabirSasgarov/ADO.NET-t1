using Azure.Core;
using Microsoft.Data.SqlClient;
using System;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ado.Net_T1
{
	internal class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine("1 - Get all\n2 - Add\n0 - Quit");
				string operation = Console.ReadLine();
				switch (operation)
				{
					case "1":
						foreach (Student student in GetAllStudents())
							Console.WriteLine(student);
						break;
					
					case "2":
						Console.WriteLine("Enter name and age");
						string name = Console.ReadLine();
						int age = int.Parse(Console.ReadLine());
						AddStudent(name, age);
						Console.Clear();
						Console.WriteLine("Added successfully!");
						break;
					
					case "0":
						Console.WriteLine("---Exit---");
						return;
					
					default:
						Console.WriteLine("Wrong operation!");
						break;
				}
			}
			//task2



		}

		static SqlConnection Connection()
		{
			string connStr = "Server=SABIR\\MSSQLSERVER01;Database=Students;Trusted_Connection=True; Trust Server Certificate = True;";
			SqlConnection conn = new SqlConnection(connStr);
			conn.Open();
			Console.WriteLine("Connection successful");
			return conn;
		}

		static List<Student> GetAllStudents()
		{
			List<Student> students = new List<Student>();
			using SqlConnection conn = Connection();
			string query = "SELECT * FROM StudentsTB";
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				if (reader.HasRows)
				{
					int id = Convert.ToInt32(reader["Id"]);
					string name = Convert.ToString(reader["Name"]);
					int age = Convert.ToInt32(reader["Age"]);
					Student newStudent = new Student(id, name, age);
					students.Add(newStudent);
				}
			}
			return students;
		}

		static void AddStudent(string name, int age)
		{
			using SqlConnection conn = Connection();
			string query = "INSERT INTO StudentsTB (Name,Age) VALUES (@name, @age)";
			SqlCommand cmd = new SqlCommand(query, conn);
			cmd.Parameters.AddWithValue("@name", name);
			cmd.Parameters.AddWithValue("@age", age);
			cmd.ExecuteNonQuery();
		}







	}
}
