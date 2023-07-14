using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Model
{
    public class Boletim
    {
        public int Id { get; set; }
        public DateTime DataNascimento { get; set; }
        public virtual Aluno Aluno { get; set; }

    }
}
