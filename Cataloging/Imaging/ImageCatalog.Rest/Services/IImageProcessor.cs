using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog.Rest.Services
{
    public interface IImageProcessor : IDisposable
    {
        void Start();
    }
}
