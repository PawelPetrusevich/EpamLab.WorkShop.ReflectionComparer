using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            var comparer = new GenericComparer();
            var firstMan = new Man
            {
                Age = 30,
                Name = "FirstMan",
                Childrens = new List<Children>
                {
                    new Children
                    {
                        Age = 10,
                        Name = "FirstChildrenFirsMan"
                    },
                    new Children
                    {
                        Age = 2,
                        Name = "SecondtChildrenFirsMan"
                    }
                }
            };

            var secondMan = new Man
            {
                Age = 30,
                Name = "SecondMan",
                Childrens = new List<Children>
                {
                    new Children
                    {
                        Age = 10,
                        Name = "FirstChildrenSecondMan"
                    },
                    new Children
                    {
                        Age = 2,
                        Name = "SecondtChildrenSecondMan"
                    }
                }
            };

            Console.WriteLine("firstMan == firstMan:" + comparer.Comparer(firstMan,firstMan));

            Console.WriteLine("firstMan == secondMan:" + comparer.Comparer(firstMan,secondMan));

            Console.ReadLine();
        }
    }
}
