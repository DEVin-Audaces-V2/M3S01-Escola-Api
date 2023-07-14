using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Model
{
    public class NotasMateria
    {
        public int Id { get; set; }
        public Boletim Boletim { get; set; }
        public int BoletimId { get; set; }
        public int Materia { get; set; }
        public int MateriaId { get; set; }
        public int Nota { get; set; }


    }
}
