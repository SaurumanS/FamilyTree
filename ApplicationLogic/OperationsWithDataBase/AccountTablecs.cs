using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLogic.OperationsWithDataBase
{
    public class AccountTable
    {
        static ApplicationContexts.ApplicationContext dataBase;
        public AccountTable()
        {
            dataBase = new ApplicationContexts.ApplicationContext();
            dataBase.Accounts.Load();
        }
        public bool IsCorrectAccount(string login, string password, string nameFamily) //Существует ли данный аккаунт
        {
            var test = dataBase.Accounts.Local;
            var result = dataBase.Accounts.Local.Where(x => x.Login == login && x.Password == password && x.NameFamily == nameFamily).Select(x=>x);
            if (result.Count() == 1) return true;
            else return false;
        }
    }
}
