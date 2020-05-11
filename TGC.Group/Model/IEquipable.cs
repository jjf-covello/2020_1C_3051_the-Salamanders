﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    public interface IEquipable : IInteractuable
    {
        void Equipar(Personaje personaje);

        void FinDuracion(Personaje personaje);

        void DisminuirDuracion();

        float getDuracion();
    }
}
