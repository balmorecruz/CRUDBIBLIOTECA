using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDBIBLIOTECA.Models
{
    public class Libro
    {
        public int idlibro { get; set; }
        public string titulo { get; set; }
        public string isbn { get; set; }
        public int anio_edicion { get; set; }
        public string editorial { get; set; }
        public int id_autor { get; set; }

        public string descripcion { get; set; }

        public Autor autor { get; set; }


    }
}