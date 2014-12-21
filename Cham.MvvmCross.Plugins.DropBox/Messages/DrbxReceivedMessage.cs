using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox.Messages
{
    public class DrbxReceivedMessage<TEntityType> : MvxMessage
    {
        public readonly IMvxDBRecord Record;

        public DrbxReceivedMessage(object sender, IMvxDBRecord record)
            : base(sender)
        {
            Record = record;
        }
    }
}
