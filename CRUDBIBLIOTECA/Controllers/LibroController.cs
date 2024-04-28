using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUDBIBLIOTECA.Models;
using System.Web.UI.WebControls;

namespace CRUDBIBLIOTECA.Controllers
{
    public class LibroController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();



        private static List<Libro> olista = new List<Libro>();



        // GET: Libro
        public ActionResult Inicio()
        {




            olista = new List<Libro>();
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM LIBRO", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Libro nuevoLibro = new Libro();
                        nuevoLibro.idlibro = Convert.ToInt32(dr["idlibro"]);
                        nuevoLibro.titulo = dr["titulo"].ToString();
                        nuevoLibro.isbn = dr["isbn"].ToString();
                        nuevoLibro.anio_edicion = Convert.ToInt32(dr["anio_edicion"]);
                        nuevoLibro.editorial = dr["editorial"].ToString();
                        nuevoLibro.id_autor = Convert.ToInt32(dr["id_autor"]);
                        nuevoLibro.descripcion = dr["descripcion"].ToString();
                        nuevoLibro.autor = obtenerAutor(nuevoLibro.id_autor);
                        olista.Add(nuevoLibro);
                    }

                }

            }
            return View(olista);
        }


        [HttpGet]
        public ActionResult Registrar()
        {
            List<Autor> autores = obtenerAutores();
            ViewBag.ListaAutores = autores;
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Libro oLibro)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Registrar_Libro", oconexion);
                cmd.Parameters.AddWithValue("titulo", oLibro.titulo);
                cmd.Parameters.AddWithValue("isbn", oLibro.isbn);
                cmd.Parameters.AddWithValue("anio_edicion", oLibro.anio_edicion);
                cmd.Parameters.AddWithValue("editorial", oLibro.editorial);

                cmd.Parameters.AddWithValue("id_autor", oLibro.id_autor);
                cmd.Parameters.AddWithValue("descripcion", oLibro.descripcion);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Libro");
        }

        [HttpGet]
        public ActionResult Editar(int? idLibro)
        {
            List<Autor> autores = obtenerAutores();
            ViewBag.ListaAutores = autores;
            if (idLibro == null)

                return RedirectToAction("Inicio", "Libro");
            Libro oLibro = olista.Where(c => c.idlibro == idLibro).FirstOrDefault();
            ViewBag.autorSeleccionado = oLibro.id_autor;

            return View(oLibro);
        }

        [HttpPost]
        public ActionResult Editar(Libro oLibro)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar_Libro", oconexion);
                cmd.Parameters.AddWithValue("idLibro", oLibro.idlibro);
                cmd.Parameters.AddWithValue("titulo", oLibro.titulo);
                cmd.Parameters.AddWithValue("isbn", oLibro.isbn);
                cmd.Parameters.AddWithValue("anio_edicion", oLibro.anio_edicion);
                cmd.Parameters.AddWithValue("editorial", oLibro.editorial);

                cmd.Parameters.AddWithValue("id_autor", oLibro.id_autor);
                cmd.Parameters.AddWithValue("descripcion", oLibro.descripcion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Libro");
        }

        [HttpGet]
        public ActionResult Eliminar(int? idLibro)
        {
            if (idLibro == null)
                return RedirectToAction("Inicio", "Libro");

            Libro oLibro = olista.Where(c => c.idlibro == idLibro).FirstOrDefault();
            return View(oLibro);
        }

        [HttpPost]
        public ActionResult Eliminar(string IdLibro)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar_Libro", oconexion);
                cmd.Parameters.AddWithValue("idlibro", IdLibro);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Libro");
        }

        public Autor obtenerAutor(int? idautor)
        {
            Autor autor = new Autor();
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AUTOR where idautor=@idautor", oconexion);
                cmd.Parameters.AddWithValue("@idautor", idautor);

                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        autor.idautor = Convert.ToInt32(dr["idautor"]);
                        autor.nombre = dr["nombre"].ToString();
                        autor.apellido = dr["apellido"].ToString();


                    }

                }

            }
            return autor;
        }



        public List<Autor> obtenerAutores()
        {
            List<Autor> autores = new List<Autor>();
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AUTOR", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Autor autor = new Autor();
                        autor.idautor = Convert.ToInt32(dr["idautor"]);
                        autor.nombre = dr["nombre"].ToString();
                        autor.apellido = dr["apellido"].ToString();


                        autores.Add(autor);
                    }

                }

            }
            return autores;
        }

    }

}