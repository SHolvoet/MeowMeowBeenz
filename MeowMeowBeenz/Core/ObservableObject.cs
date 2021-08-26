using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MeowMeowBeenz.Core
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // CallerMemberName: https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=netcore-3.1
        // Vult automatisch de property name aan
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // ? -> Null conditional operator. Controleert of variable voor ? null is, zoniet, voer alles na ? uit (tot het volgende ?)
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            //var changedEvent = PropertyChanged;
            //if (changedEvent != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
        }
    }
}
