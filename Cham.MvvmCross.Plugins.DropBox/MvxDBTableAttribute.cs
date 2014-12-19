using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MvxDBTableAttribute : Attribute
    {
        public readonly string Name;

        public MvxDBTableAttribute(string name)
        {
            Name = name;
        }
    }
}
