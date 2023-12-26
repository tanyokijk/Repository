using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Studio
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Countries { get; set; }

        public string Cities { get; set; }

        public List<Game> Games { get; set; } = new();
    }
}
