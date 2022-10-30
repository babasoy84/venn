using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venn.Client.Services
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : ViewModelBase;
    }
}
