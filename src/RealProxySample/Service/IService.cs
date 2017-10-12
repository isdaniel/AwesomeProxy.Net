using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealProxySample.Service
{
    public interface IService<T>
    {
        T add(T t1, T t2);
    }
}