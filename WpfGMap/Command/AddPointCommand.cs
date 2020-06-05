using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGMap.Command
{
    class AddPointCommand : ICommand<DrawData>
    {
        public DrawData Do(DrawData input)
        {
            throw new NotImplementedException();
        }

        public DrawData Undo(DrawData input)
        {
            throw new NotImplementedException();
        }
    }
}
