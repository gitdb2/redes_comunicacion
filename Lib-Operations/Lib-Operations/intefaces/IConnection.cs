﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.LibOperations.intefaces
{
    public interface IConnection
    {
        public void WriteToStream(char[] data);
    }
}
