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

namespace CRUDBIBLIOTECA.Controllers
{
    public class AutorController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();



        private static List<Autor> olista = new List<Autor>();

        // GET: Autor
        public ActionResult Inicio()
        {
            olista = new List<Autor>();
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Autor", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Autor nuevoAutor = new Autor();
                        nuevoAutor.idautor = Convert.ToInt32(dr["idautor"]);
                        nuevoAutor.nombre = dr["nombre"].ToString();
                        nuevoAutor.apellido = dr["apellido"].ToString();
                        nuevoAutor.nacionalidad = dr["nacionalidad"].ToString();
                        olista.Add(nuevoAutor);
                    }

                }

            }
            return View(olista);
        }


        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Autor oAutor)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Registrar_Autor", oconexion);
                cmd.Parameters.AddWithValue("nombre", oAutor.nombre);
                cmd.Parameters.AddWithValue("apellido", oAutor.apellido);
                cmd.Parameters.AddWithValue("nacionalidad", oAutor.nacionalidad);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Autor");
        }

        [HttpGet]
        public ActionResult Editar(int? idAutor)
        {
            if (idAutor == null)

                return RedirectToAction("Inicio", "Autor");
            Autor oAutor = olista.Where(c => c.idautor == idAutor).FirstOrDefault();

            return View(oAutor);
        }

        [HttpPost]
        public ActionResult Editar(Autor oAutor)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar_Autor", oconexion);
                cmd.Parameters.AddWithValue("idautor", oAutor.idautor);
                cmd.Parameters.AddWithValue("nombre", oAutor.nombre);
                cmd.Parameters.AddWithValue("apellido", oAutor.apellido);
                cmd.Parameters.AddWithValue("nacionalidad", oAutor.nacionalidad);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Autor");
        }

        [HttpGet]
        public ActionResult Eliminar(int? idAutor)
        {
            if (idAutor == null)
                return RedirectToAction("Inicio", "Autor");

            Autor oAutor = olista.Where(c => c.idautor == idAutor).FirstOrDefault();
            return View(oAutor);
        }

        [HttpPost]
        public ActionResult Eliminar(string IdAutor)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar_Autor", oconexion);
                cmd.Parameters.AddWithValue("idautor", IdAutor);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Autor");
        }
    }

}