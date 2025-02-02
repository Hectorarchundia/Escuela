using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Application;
using Domain;

namespace Infrastructure
{
    public class AlumnosDbContext
    {
        private const string V = "Foto";
        private readonly string _connectionString;

        public AlumnosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Alumno> List()
        {
            var data = new List<Alumno>();

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT [Id],[Nombre],[Edad],[Promedio], [Foto] FROM [Alumno]", con)) // Agregar Foto a la consulta
            {
                try
                {
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            data.Add(new Alumno
                            {
                                Id = (Guid)dr["Id"],
                                Nombre = (string)dr["Nombre"],
                                Edad = (int)dr["Edad"],
                                Promedio = (double)dr["Promedio"],
                                Foto = dr["Foto"] == DBNull.Value ? null : (string)dr["Foto"] // Asegurarse que Foto está en la consulta
                            });
                        }
                    }
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar alumnos", ex);
                }
            }
        }

        public Alumno Details(Guid id)
        {
            var data = new Alumno();

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT [Id],[Nombre],[Edad],[Promedio], [Foto] FROM [Alumno] WHERE [Id] = @id", con); // Agregar Foto a la consulta
            {
                cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                try
                {
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            data.Id = (Guid)dr["Id"];
                            data.Nombre = (string)dr["Nombre"];
                            data.Edad = (int)dr["Edad"];
                            data.Promedio = (double)dr["Promedio"];
                            data.Foto = dr["Foto"] == DBNull.Value ? null : (string)dr["Foto"]; //  Asegurarse de obtener Foto
                        }
                    }
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los detalles del alumno", ex);
                }
            }
        }

        public void Create(Alumno data)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("INSERT INTO [Alumno] ([Id],[Nombre],[Edad],[Promedio], [Foto]) VALUES (@id,@nombre,@edad,@promedio,@foto)", con)) //  Agregar Foto
            {
                cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = data.Id;
                cmd.Parameters.Add("nombre", SqlDbType.NVarChar, 128).Value = data.Nombre;
                cmd.Parameters.Add("edad", SqlDbType.Int).Value = data.Edad;
                cmd.Parameters.Add("Promedio", SqlDbType.Float).Value = data.Promedio;
                cmd.Parameters.Add("Foto", SqlDbType.NVarChar).Value = data.Foto == null ? DBNull.Value : data.Foto; //  Manejo de Foto nula

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al crear el alumno", ex);
                }
            }
        }

        public void Edit(Alumno data)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE [Alumno] SET [Nombre] = @nombre, [Edad] = @edad, [Promedio] = @promedio, [Foto] = @foto WHERE [Id] = @id", con)) //  Agregar Foto
            {
                cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = data.Id;
                cmd.Parameters.Add("nombre", SqlDbType.NVarChar, 128).Value = data.Nombre;
                cmd.Parameters.Add("edad", SqlDbType.Int).Value = data.Edad;
                cmd.Parameters.Add("Promedio", SqlDbType.Float).Value = data.Promedio;
                cmd.Parameters.Add("Foto", SqlDbType.NVarChar).Value = data.Foto == null ? DBNull.Value : data.Foto; //  Manejo de Foto nula

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al editar el alumno", ex);
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM [Alumno] WHERE [Id] = @id", con))
            {
                cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el alumno", ex);
                }
            }
        }
    }
}