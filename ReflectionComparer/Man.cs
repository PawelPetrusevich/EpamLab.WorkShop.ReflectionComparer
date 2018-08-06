using System.Collections.Generic;

namespace ReflectionComparer
{
    public class Man
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Children> Childrens { get; set; }

    }

    public class Children
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}