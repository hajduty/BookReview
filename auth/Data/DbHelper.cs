using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;

public class DbHelper
{
    private readonly string connectionString;

    public DbHelper()
    {
        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Get the connection string
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public List<T> GetData<T>()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Query
            string query = $"SELECT * FROM {typeof(T).Name}s";
            return connection.Query<T>(query).ToList();
        }
    }
    public T GetData<T>(string condition, object parameters = null)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Query
            string query = $"SELECT * FROM {typeof(T).Name}s WHERE {condition}";
            return connection.QueryFirstOrDefault<T>(query, parameters);
        }
    }
    public void DeleteData(int id, string table)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Delete
            string deleteQuery = "DELETE FROM "+table+" WHERE Id = @Id";
            connection.Execute(deleteQuery, new { Id = id });
        }
    }
    public void InsertData<T>(T entity, string tableName)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Insert
            string columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
            string values = string.Join(", ", typeof(T).GetProperties().Select(p => $"@{p.Name}"));
            string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            connection.Execute(insertQuery, entity);
        }
    }
}