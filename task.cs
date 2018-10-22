using System;
using System.Collections.Generic;
using System.Linq;

namespace SysLab
{

    class task
    {
        Random rnd;
        public List<int> units;
        public int task_complexity;
        public int min_complexity = 20;
        public int max_complexity = 200;

        public task()
        {
            rnd = new Random();
            units = new List<int>();
        }

        public void generate_compl()
        {
            task_complexity = rnd.Next(min_complexity, max_complexity + 1);
        }

        public void generate_units()
        {
            int units_number = rnd.Next(1, 5);
            for (int i = 0; i < units_number; i++)
            {
                int curr = 0;
                bool was_this = true;
                while (was_this)
                {
                    was_this = false;
                    curr = rnd.Next(5);
                    for (int j = 0; j < units.Count(); j++)
                        if (curr == units[j])
                            was_this = true;
                }
                units.Add(curr);
            }
            units.Sort();
        }

        /*public bool is_this_unit(int nmb)
        {
            for (int i = 0; i < units.Count(); i++)
                if (units[i] == nmb)
                    return true;
            return false;
        }*/
    }
}
