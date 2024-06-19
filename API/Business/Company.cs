using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Business
{
    internal class Company
    {
        private int _id;
        private string _name;
        private string _domain;
        private string _sector;

        public int Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Domain { get { return _domain; } set { _domain = value; } }
        public string Sector { get { return _sector; } set { _sector = value; } }

        public Company() {
            _id = 0;
            _name = string.Empty;
            _domain = string.Empty;
            _sector = string.Empty;
        }

        public Company(string Name, string Domain, string Sector)
        {
            _name = Name;
            _domain = Domain;
            _sector = Sector;
        }

    }
}
