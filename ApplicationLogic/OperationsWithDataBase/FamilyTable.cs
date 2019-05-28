using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using Family = ApplicationLogic.DataBaseTableInstances.Family;
using System.IO;

namespace ApplicationLogic.OperationsWithDataBase
{
    public class FamilyTable
    {
        static ApplicationContexts.ApplicationContext dataBase;
        public FamilyTable()
        {
            dataBase = new ApplicationContexts.ApplicationContext();
            dataBase.Families.Load();
        }
        public IEnumerable<Family> RerurnListFamilyes//Возврат списка семей
        {
            get
            {
                var RerurnListFamilyes = dataBase.Families.Local.ToBindingList();
                return RerurnListFamilyes;
            }
        }
        public void Add(Family family)
        {
            //dataBase.Families.Add(family);
            //dataBase.SaveChanges();
        }
    }
}
