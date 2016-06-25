using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars
{
    public class Country
    {
        private string _key;
        private string _name;
        private string _demonym;
        private string _flag;

        public Country()
        {
        }
        public Country(string key, string name, string demonym, string flag)
        {
            _key = key;
            _name = name;
            _demonym = demonym;
            _flag = flag;
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// The name of the country.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The identifier of the countries residents.
        /// Example: The demonym of France is French
        /// </summary>
        public string Demonym
        {
            get { return _demonym; }
            set { _demonym = value; }
        }

        /// <summary>
        /// The texture ID for the flag of the country.
        /// </summary>
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
    }
}
