using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    interface IRecolectable : IInteractuable
    {

        void Recolectar(Personaje personaje);
    }
}
