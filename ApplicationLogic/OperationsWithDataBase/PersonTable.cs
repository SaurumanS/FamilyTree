using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLogic.OperationsWithDataBase
{
    public class PersonTable
    {
        ApplicationContexts.ApplicationContext dataBase;
        public PersonTable()
        {
            dataBase = new ApplicationContexts.ApplicationContext();
            dataBase.People.Load();
        }
        public DataBaseTableInstances.Person ReturnPersonInFullName(string fullName)
        {
            return dataBase.People.Local.First();
        }
    }
}
