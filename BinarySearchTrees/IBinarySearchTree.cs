using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTs
{
    public interface IBinarySearchTree<T> where T : IComparable<T>
    {
        void insert(T value);
        void delete(T value);
        bool contains(T value);
    }
    public class BTSException : System.Exception
    {
        public BTSException() { }
        public BTSException(string message) : base(message) { }
    }
}
