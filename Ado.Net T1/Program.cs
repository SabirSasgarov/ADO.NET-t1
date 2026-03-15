using Azure.Core;
using Microsoft.Data.SqlClient;
using System;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ado.Net_T1
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("T - Test connection\n1 - Get all\n2 - Add\n3 - Search by name\n" +
				"4 - Update student\n5 - Delete student\n0 - Clear\nQ - Quit");
			while (true)
			{
				Console.Write("Enter new operation: \n->");
				string operation = Console.ReadLine();
				switch (operation)
				{
					case "1":
						foreach (Student student in GetAllStudents())
							Console.WriteLine(student);
						break;

					case "2":
						Console.WriteLine("Enter student name: ");
						string addName = Console.ReadLine();
						Console.WriteLine("Enter student age: ");
						int addAge = int.Parse(Console.ReadLine());
						AddStudent(addName, addAge);
						Console.WriteLine("Added successfully!");
						break;

					case "3":
						Console.WriteLine("Enter name to search: ");
						string searchName = Console.ReadLine();
						foreach (Student student in SearchStudentByName(searchName))
						{
							Console.WriteLine(student);
						}
						break;

					case "4":
						Console.WriteLine("Enter id to update: ");
						int updateId = int.Parse(Console.ReadLine());
						UpdateStudent(updateId);
						break;

					case "5":
						Console.WriteLine("Enter id to delete: ");
						int deleteId = int.Parse(Console.ReadLine());
						DeleteById(deleteId);
						break;

					case "q" or "Q":
						Console.WriteLine("---Exit---");
						return;

					case "t" or "T":
						Connection();
						Console.WriteLine("Connection successful!\n");
						break;
					case "0":
						Console.Clear();
						Console.WriteLine("T - Test connection\n1 - Get all\n2 - Add\n3 - Search by name" +
							"\n4 - Update student\n5 - Delete student\n0 - Clear\nQ - Quit");
						break;
					default:
						Console.WriteLine("Wrong operation!\n");
						break;
				}
			}

		}

		//1
		static SqlConnection Connection()
		{
			string connStr = "Server=SABIR\\MSSQLSERVER01;Database=Students;Trusted_Connection=True; Trust Server Certificate = True;";
			SqlConnection conn = new SqlConnection(connStr);
			conn.Open();
			return conn;
		}

		//2
		static void AddStudent(string name, int age)
		{
			using SqlConnection conn = Connection();
			string query = "INSERT INTO StudentsTB (Name,Age) VALUES (@name, @age)";
			SqlCommand cmd = new SqlCommand(query, conn);
			cmd.Parameters.AddWithValue("@name", name);
			cmd.Parameters.AddWithValue("@age", age);
			cmd.ExecuteNonQuery();
		}

		//3
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
		//4
		static List<Student> SearchStudentByName(string name)
		{
			using SqlConnection conn = Connection();
			string query = "SELECT * FROM StudentsTB WHERE Name LIKE @name";
			SqlCommand cmd = new SqlCommand(query, conn);
			cmd.Parameters.AddWithValue("@name", $"%{name}%");
			SqlDataReader reader = cmd.ExecuteReader();

			List<Student> students = new List<Student>();
			while (reader.Read())
			{
				if (reader.HasRows)
				{
					Student newStudent = new Student();
					newStudent.Id = Convert.ToInt32(reader["Id"]);
					newStudent.Name = Convert.ToString(reader["Name"]);
					newStudent.Age = Convert.ToInt32(reader["Age"]);
					students.Add(newStudent);
				}
			}
			return students;
		}

		//5
		static void UpdateStudent(int id)
		{
			using SqlConnection conn = Connection();
			string checkQuery = "SELECT * FROM StudentsTB WHERE Id = @id";
			SqlCommand cmd = new SqlCommand(checkQuery, conn);
			cmd.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = cmd.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Close();
				string queryUpdate = "UPDATE StudentsTB SET Age = @age WHERE Id = @id";
				cmd = new SqlCommand(queryUpdate, conn);
				Console.WriteLine("Enter new age for the student: ");
				int age = int.Parse(Console.ReadLine());
				cmd.Parameters.AddWithValue("@age", age);
				cmd.Parameters.AddWithValue("@id", id);
				cmd.ExecuteReader();
				Console.WriteLine("Updated succesfully!\n");
			}
			else
				Console.WriteLine("No such student!\n");
		}

		//6
		static void DeleteById(int id)
		{
			using SqlConnection conn = Connection();
			string queryCheck = "SELECT * FROM StudentsTB WHERE Id = @id";
			SqlCommand cmd = new SqlCommand(queryCheck, conn);
			cmd.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = cmd.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Close();
				string queryDelete = "DELETE FROM StudentsTB WHERE Id = @id";
				cmd = new SqlCommand(queryDelete, conn);
				cmd.Parameters.AddWithValue("@id", id);
				cmd.ExecuteNonQuery();
				Console.WriteLine("Student deleted!\n");
			}
			else
				Console.WriteLine("No such student!\n");
		}

		//7
		static void Pagination()
		{

		}
	}
}
