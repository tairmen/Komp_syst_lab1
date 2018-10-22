using System;
using System.Collections.Generic;
using System.Linq;

namespace SysLab
{
    class unit
    {
        public int perf;
        public double loading = 0;
        public int min_perf = 1;
        public int max_perf = 10;
        public int doing;
        public int tasks;
        public int operations;
        public List<task> tstak = new List<task>();
        private int op;

        public unit()
        {
            Random rnd = new Random();
            perf = rnd.Next(min_perf, max_perf + 1);
            doing = 0;
            tasks = 0;
            operations = 0;
        }

        public void add_task(task itask)
        {
            loading += itask.task_complexity / perf;
            tstak.Add(itask);
        }

        public void get_task()
        {
            op = tstak[0].task_complexity;
            doing = op;
        }

        public void do_task()
        {
            if (doing == 0)
            {
                if (tstak.Count() > 0)
                {
                    get_task();
                }
                return;
            }   
            
            if (doing <= perf)
            {
                int tmp = perf - doing;
                doing = 0;
                operations += op;
                tasks++;
                tstak.RemoveAt(0);
                if (tstak.Count() > 0)
                {
                    get_task();
                    doing -= tmp;
                }
                return;
            }

            doing -= perf;
        }
    }
}
