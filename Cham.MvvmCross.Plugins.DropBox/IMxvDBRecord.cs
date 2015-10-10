using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public interface IMvxDBRecord : IMvxDBFields
    {
        string Id { get; }

        bool IsDeleted { get; }


        void DeleteRecord();

    }
}
