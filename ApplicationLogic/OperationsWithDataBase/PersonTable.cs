using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Person = ApplicationLogic.DataBaseTableInstances.Person;

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
        public Person ReturnPersonInFullName(string fullName)//Возврат экземпляра класса из базы данных по полному имени
        {
            string pattern = @"\s+";
            string target = " ";
            Regex regex = new Regex(pattern);
            fullName = regex.Replace(fullName, target);
            string[] name = fullName.Split(' ');
            string firstName = name[1];
            string secondName = name[0];
            string patronymic = name[2];
            var result = dataBase.People.FirstOrDefault(x => x.FirstName == firstName && x.SecondName == secondName && x.Patronymic == patronymic);
            return result;
        }
        public List<Person> ReturnSearchingResult(string secondName, string firstName, string patronymic)//Возвращает данные на страницу поиска по ФИО
        {
            bool fnIsEmpty = String.IsNullOrEmpty(firstName);
            bool snIsEmpty = String.IsNullOrEmpty(secondName);
            bool patIsEmpty = String.IsNullOrEmpty(patronymic);
            List<Person> result;
            if (fnIsEmpty && snIsEmpty && patIsEmpty) result = null;
            else if (!fnIsEmpty)
            {
                if (!snIsEmpty && !patIsEmpty) result = dataBase.People.Where(x => x.FirstName == firstName && x.SecondName == secondName && x.Patronymic == patronymic).ToList();
                else if (!snIsEmpty && patIsEmpty) result = dataBase.People.Where(x => x.FirstName == firstName && x.SecondName == secondName).ToList();
                else if (snIsEmpty && !patIsEmpty) result = dataBase.People.Where(x => x.FirstName == firstName && x.Patronymic == patronymic).ToList();
                else result = dataBase.People.Where(x => x.FirstName == firstName).ToList();
            }
            else if (!snIsEmpty)
            {
                if (fnIsEmpty && !patIsEmpty) result = dataBase.People.Where(x => x.SecondName == secondName && x.Patronymic == patronymic).ToList();
                else result = dataBase.People.Where(x => x.SecondName == secondName).ToList();
            }
            else result = dataBase.People.Where(x => x.Patronymic == patronymic).ToList();
            return result;

        }
        public void SaveNewPerson(Person newPerson)//Занести новый профиль в базу
        {
            dataBase.People.Add(newPerson);
            dataBase.SaveChanges();
        }
        public List<string> ReturnAllSecondNamesInDataBase
        {
            get
            {
                List<string> result = dataBase.People.Select(x => x.SecondName).ToList();
                return result;
            }
        }
        public List<string> ReturnFirstNameMembersFamily(string secondName, bool isMan)//В зависимости от фамилии и пола вернёт все имена под этой фамилией
        {
            List<string> result;
            if (isMan) result = dataBase.People.Where(x => x.SecondName == secondName && x.Gender == "Мужской").Select(x => x.FirstName).ToList();
            else result = dataBase.People.Where(x => x.SecondName == secondName && x.Gender == "Женский").Select(x => x.FirstName).ToList();
            return result;
        }
        public List<string> ReturnPatronymicsMembersFamily(string secondName,string firstName, bool isMan)//В зависимости от фамилии и пола вернёт все имена под этой фамилией
        {
            List<string> result;
            if (isMan) result = dataBase.People.Where(x => x.SecondName == secondName && x.FirstName==firstName && x.Gender == "Мужской").Select(x => x.Patronymic).ToList();
            else result = dataBase.People.Where(x => x.SecondName == secondName && x.FirstName == firstName && x.Gender == "Женский").Select(x => x.Patronymic).ToList();
            return result;
        }


    }
}
