using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysLab
{
    public partial class MainForm : Form
    {
        List<unit> units;
        List<unit> units1;
        List<unit> units2;
        List<unit> units3;
        int planner1_pos;
        int planner2_pos;
        int planner3_pos;
        int planner_d = 5;
        int task_d = 20;
        int planner_d1 = 1;
        int task_d1 = 10;
        int secs = 10;
        //int checktime = 10000;
        double user_poss;
        int user_compl;
        int user_compl1;
        bool ok_user_compl = false;
        int takts = 0;
        int FifoTasks = 0;
        int M2Tasks = 0;
        int M3Tasks = 0;
        int M4Tasks = 0;
        int itask = 0;
        //bool timecheck;
        //int maxiter = 200;
        List<task> tasks = new List<task>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void ProcTimer_Tick(object sender, EventArgs e)
        {
            takts++;
            MethodFifo();
            Method2();
            Method3(takts);
            Method4(takts);

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            task new_task = new task();
            if (ok_user_compl)
            {
                new_task.min_complexity = user_compl;
                new_task.max_complexity = user_compl1;
            }
            new_task.generate_compl();
            new_task.generate_units();
            tasks.Add(new_task);

            string sUnts = "";
            string sComl = "";

            itask++;

            sComl = Convert.ToString(new_task.task_complexity);

            for (int i = 0; i < new_task.units.Count(); i++)
                sUnts += Convert.ToString(new_task.units[i]) + " ";

            string id = Convert.ToString(itask);
            string[] strs = { id, sUnts, sComl };
            ListViewItem item = new ListViewItem(strs);
            lTasks.Items.Add(item);

            /*if (timecheck == true)
            {
                MethodFifo(new_task);

                Method2(new_task);

                Method3(new_task);
            }

            if (timecheck == false)
            {
                double systime = units[0].loading;
                for (int i = 1; i < units.Count(); i++)
                {
                    if (systime < units[i].loading)
                    {
                        systime = units[i].loading;
                    }
                }
                double systime1 = units1[0].loading;
                for (int i = 1; i < units1.Count(); i++)
                {
                    if (systime1 < units1[i].loading)
                    {
                        systime1 = units1[i].loading;
                    }
                }
                double systime2 = units2[0].loading;
                for (int i = 1; i < units2.Count(); i++)
                {
                    if (systime2 < units2[i].loading)
                    {
                        systime2 = units2[i].loading;
                    }
                }

                if (systime < checktime)
                    MethodFifo(new_task);

                if (systime1 < checktime)
                    Method2(new_task);

                if (systime2 < checktime)
                    Method3(new_task);

                if ((systime >= checktime) && (systime1 >= checktime) && (systime2 >= checktime))
                {
                    TaskTimer.Stop();
                }
            }*/

        }

        private void MethodFifo()
        {
            int min_pos;
            task new_task;

            if (itask > FifoTasks)
            {
                new_task = tasks[FifoTasks];
                min_pos = new_task.units[0];
                units[min_pos].add_task(new_task);
                FifoTasks++;
            }

            for (int i = 0; i < units.Count(); i++)
            {
                units[i].do_task();
            }
        }

        private void Method2()
        {
            int min_pos;
            task new_task;

            while (itask > M2Tasks)
            {
                new_task = tasks[M2Tasks];
                if ((new_task.units.Count() == 1) && (new_task.units[0] == planner1_pos))
                {
                    M2Tasks++;
                    continue;
                }

                double minl = 100000000;
                int bpos = -1;

                for (int i = 0; i < new_task.units.Count(); i++)
                {
                    min_pos = new_task.units[i];
                    if (min_pos != planner1_pos)
                    {
                        if (units1[min_pos].loading < minl)
                        {
                            minl = units1[min_pos].loading;
                            bpos = min_pos;
                        }
                    }
                }

                if (bpos != -1)
                {
                    units1[bpos].add_task(new_task);
                    M2Tasks++;
                }
            }

            for (int i = 0; i < units1.Count(); i++)
            {
                units1[i].do_task();
            }
        }

        private void Method3(int t)
        {
            bool planer_reg;
            if (t % (planner_d + task_d) < planner_d) planer_reg = true;
            else planer_reg = false;
            int min_pos;
            task new_task;

            if (planer_reg == true)
            {
                while (itask > M3Tasks)
                {
                    new_task = tasks[M3Tasks];

                    double minl = 10000000;
                    int bpos = -1;

                    for (int i = 0; i < new_task.units.Count(); i++)
                    {
                        min_pos = new_task.units[i];
                        if (units2[min_pos].loading < minl)
                        {
                            minl = units2[min_pos].loading;
                            bpos = min_pos;
                        }
                    }

                    if (bpos != -1)
                    {
                        units2[bpos].add_task(new_task);
                        M3Tasks++;
                    }
                }

                for (int i = 0; i < units2.Count(); i++)
                {
                    if (i != planner2_pos)
                    {
                        units2[i].do_task();
                    }
                }
            }
            else
            {
                for (int i = 0; i < units2.Count(); i++)
                {
                    units2[i].do_task();
                }
            }
        }

        private void Method4(int t)
        {
            bool planer_reg;
            if (t % (planner_d1 + task_d1) < planner_d1) planer_reg = true;
            else planer_reg = false;
            int min_pos;
            task new_task;

            if (planer_reg == true)
            {
                while (itask > M4Tasks)
                {
                    new_task = tasks[M4Tasks];

                    double minl = 10000000;
                    int bpos = -1;

                    for (int i = 0; i < new_task.units.Count(); i++)
                    {
                        min_pos = new_task.units[i];
                        if (units3[min_pos].loading < minl)
                        {
                            minl = units3[min_pos].loading;
                            bpos = min_pos;
                        }
                    }

                    if (bpos != -1)
                    {
                        units3[bpos].add_task(new_task);
                        M4Tasks++;
                    }
                }

                for (int i = 0; i < units3.Count(); i++)
                {
                    if (i != planner3_pos)
                    {
                        units3[i].do_task();
                    }
                }
            }
            else
            {
                for (int i = 0; i < units3.Count(); i++)
                {
                    units3[i].do_task();
                }
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            units = new List<unit>();

            units1 = new List<unit>();

            units2 = new List<unit>();

            units3 = new List<unit>();

            for (int i = 0; i < 5; i++)
            {
                units.Add(new unit());
                units1.Add(new unit());
                units2.Add(new unit());
                units3.Add(new unit());
                await Task.Delay(10);
            }
        }

        private void Fifo_Starting()
        {
            int perf0 = units[0].max_perf + 1;
            try
            {
                perf0 = Convert.ToInt32(tUnit0Perf.Text);
            }
            catch
            {
                tUnit0Perf.Text = Convert.ToString(units[0].perf);
            }
            if (units[0].min_perf <= perf0 && perf0 <= units[0].max_perf)
                units[0].perf = perf0;
            else
                tUnit0Perf.Text = Convert.ToString(units[0].perf);

            int perf1 = units[0].max_perf + 1;
            try
            {
                perf1 = Convert.ToInt32(tUnit1Perf.Text);
            }
            catch
            {
                tUnit1Perf.Text = Convert.ToString(units[1].perf);
            }
            if (units[0].min_perf <= perf1 && perf1 <= units[0].max_perf)
                units[1].perf = perf1;
            else
                tUnit1Perf.Text = Convert.ToString(units[1].perf);

            int perf2 = units[0].max_perf + 1;
            try
            {
                perf2 = Convert.ToInt32(tUnit2Perf.Text);
            }
            catch
            {
                tUnit2Perf.Text = Convert.ToString(units[2].perf);
            }
            if (units[0].min_perf <= perf2 && perf2 <= units[0].max_perf)
                units[2].perf = perf2;
            else
                tUnit2Perf.Text = Convert.ToString(units[2].perf);

            int perf3 = units[0].max_perf + 1;
            try
            {
                perf3 = Convert.ToInt32(tUnit3Perf.Text);
            }
            catch
            {
                tUnit3Perf.Text = Convert.ToString(units[3].perf);
            }
            if (units[0].min_perf <= perf3 && perf3 <= units[0].max_perf)
                units[3].perf = perf3;
            else
                tUnit3Perf.Text = Convert.ToString(units[3].perf);

            int perf4 = units[0].max_perf + 1;
            try
            {
                perf4 = Convert.ToInt32(tUnit4Perf.Text);
            }
            catch
            {
                tUnit4Perf.Text = Convert.ToString(units[4].perf);
            }
            if (units[0].min_perf <= perf4 && perf4 <= units[0].max_perf)
                units[4].perf = perf4;
            else
                tUnit4Perf.Text = Convert.ToString(units[4].perf);
        }

        private void Planer_Starting()
        {
            for (int i = 0; i < units.Count(); i++)
            {
                units1[i].perf = units[i].perf;
            }
            int min_unit_perf = units1[0].perf;
            planner1_pos = 0;
            for (int i = 1; i < units1.Count(); i++)
                if (units1[i].perf < min_unit_perf)
                {
                    min_unit_perf = units1[i].perf;
                    planner1_pos = i;
                }
            //units1[planner1_pos].loading = 0;
            string[] str_planner = { "планировщик" };
            ListViewItem item_planner = new ListViewItem(str_planner);
            switch (planner1_pos)
            {
                case 0:
                    lUnit00.Items.Add(item_planner);
                    break;
                case 1:
                    lUnit11.Items.Add(item_planner);
                    break;
                case 2:
                    lUnit22.Items.Add(item_planner);
                    break;
                case 3:
                    lUnit33.Items.Add(item_planner);
                    break;
                default:
                    lUnit44.Items.Add(item_planner);
                    break;
            }
        }

        private void Method3_Starting()
        {
            for (int i = 0; i < units.Count(); i++)
            {
                units2[i].perf = units[i].perf;
            }
            int man_unit_perf = units2[0].perf;
            planner2_pos = 0;
            for (int i = 1; i < units2.Count(); i++)
                if (units2[i].perf > man_unit_perf)
                {
                    man_unit_perf = units2[i].perf;
                    planner2_pos = i;
                }

            string[] str_planner = { "планировщик" };
            ListViewItem item_planner = new ListViewItem(str_planner);
            switch (planner2_pos)
            {
                case 0:
                    lUnit000.Items.Add(item_planner);
                    break;
                case 1:
                    lUnit111.Items.Add(item_planner);
                    break;
                case 2:
                    lUnit222.Items.Add(item_planner);
                    break;
                case 3:
                    lUnit333.Items.Add(item_planner);
                    break;
                default:
                    lUnit444.Items.Add(item_planner);
                    break;
            }
        }

        private void Method4_Starting()
        {
            for (int i = 0; i < units.Count(); i++)
            {
                units3[i].perf = units[i].perf;
            }
            int man_unit_perf = units3[0].perf;
            planner3_pos = 0;
            for (int i = 1; i < units3.Count(); i++)
                if (units3[i].perf > man_unit_perf)
                {
                    man_unit_perf = units3[i].perf;
                    planner3_pos = i;
                }

            string[] str_planner = { "планировщик" };
            ListViewItem item_planner = new ListViewItem(str_planner);
            switch (planner3_pos)
            {
                case 0:
                    lUnit0000.Items.Add(item_planner);
                    break;
                case 1:
                    lUnit1111.Items.Add(item_planner);
                    break;
                case 2:
                    lUnit2222.Items.Add(item_planner);
                    break;
                case 3:
                    lUnit3333.Items.Add(item_planner);
                    break;
                default:
                    lUnit4444.Items.Add(item_planner);
                    break;
            }
            task_d1 = (int)(user_poss*40);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            bStart.BackColor = Color.Green;
            bStop.BackColor = Color.Red;
            bClear.BackColor = Color.White;
            bStart.Enabled = false;
            bStop.Enabled = true;
            bClear.Enabled = false;

            try
            {
                user_poss = Convert.ToDouble(tUserPoss.Text);
            }
            catch
            { }

            try
            {
                user_compl = Convert.ToInt32(tUserCompl.Text);
                user_compl1 = Convert.ToInt32(tUserCompl1.Text);
                if (1 <= user_compl && user_compl1 <= 1000 && user_compl1 >= user_compl)
                    ok_user_compl = true;
            }
            catch
            { }

            /*timecheck = true;
            if ((checkb.Checked == false) && (checkb1.Checked == true))
            {
                timecheck = false;
            }*/

            this.TaskTimer.Interval = (int)(10 / user_poss);

            Fifo_Starting();
            Planer_Starting();
            Method3_Starting();
            Method4_Starting();

            _10SecTimer.Start();
            TaskTimer.Start();
            ProcTimer.Start();

            /*if (checkTimers.Checked == true)
                TaskTimer.Start();
            else NoTimer();
            if (timecheck == true)
                _10SecTimer.Start();*/
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            bStart.BackColor = Color.FromArgb(0, 192, 0);
            bStop.BackColor = Color.DarkRed;
            bStop.Enabled = false;
            bClear.Enabled = true;

            ProcTimer.Stop();
            TaskTimer.Stop();
            _10SecTimer.Stop();
            //lTime.Text = "Секунд: 0";

            Conclusion();

            Conclusion2();

            Conclusion3();

            Conclusion4();
        }

        private void Conclusion()
        {
            int theor_perf = 0;
            int tasks_performed_all = 0;
            int operations_completed = 0;

            for (int i = 0; i < units.Count(); i++)
            {
                ListViewItem r1 = new ListViewItem("операции: " + Convert.ToString(units[i].operations));
                ListViewItem r2 = new ListViewItem("задач: " + Convert.ToString(units[i].tasks));
                switch (i)
                {
                    case 0:
                        lUnit0.Items.Add(r1);
                        lUnit0.Items.Add(r2);
                        break;
                    case 1:
                        lUnit1.Items.Add(r1);
                        lUnit1.Items.Add(r2);
                        break;
                    case 2:
                        lUnit2.Items.Add(r1);
                        lUnit2.Items.Add(r2);
                        break;
                    case 3:
                        lUnit3.Items.Add(r1);
                        lUnit3.Items.Add(r2);
                        break;
                    default:
                        lUnit4.Items.Add(r1);
                        lUnit4.Items.Add(r2);
                        break;
                }
                theor_perf += units[i].perf * takts;
                tasks_performed_all += units[i].tasks;
                operations_completed += units[i].operations;
            }
            lESE.Text = "КПД " + (operations_completed / (double)theor_perf / 1.2).ToString("0.####");
            lTasksNumber.Text = "Выполнено задач - " + Convert.ToString((int)(tasks_performed_all/1.2));
            lOperationsCompleted.Text = "Выполнено операций - " + Convert.ToString((int)(operations_completed/1.2));
        }

        private void Conclusion2()
        {
            int theor_perf = 0;
            int theor_perf_quoted = 0;
            int tasks_performed_all = 0;
            int operations_completed = 0;

            for (int i = 0; i < units1.Count(); i++)
            {
                ListViewItem r1 = new ListViewItem("операции: " + Convert.ToString(units1[i].operations));
                ListViewItem r2 = new ListViewItem("задач: " + Convert.ToString(units1[i].tasks));
                switch (i)
                {
                    case 0:
                        lUnit00.Items.Add(r1);
                        lUnit00.Items.Add(r2);
                        break;
                    case 1:
                        lUnit11.Items.Add(r1);
                        lUnit11.Items.Add(r2);
                        break;
                    case 2:
                        lUnit22.Items.Add(r1);
                        lUnit22.Items.Add(r2);
                        break;
                    case 3:
                        lUnit33.Items.Add(r1);
                        lUnit33.Items.Add(r2);
                        break;
                    default:
                        lUnit44.Items.Add(r1);
                        lUnit44.Items.Add(r2);
                        break;
                }
                theor_perf += units1[i].perf * takts;
                if (i != planner1_pos)
                    theor_perf_quoted += units1[i].perf * takts;
                tasks_performed_all += units1[i].tasks;
                operations_completed += units1[i].operations;
            }

            lESE1.Text = "КПД " + (operations_completed / (double)theor_perf).ToString("0.####");
            lESEquoted1.Text = "КПД' " + (operations_completed / (double)theor_perf_quoted).ToString("0.####");
            lTasksNumber1.Text = "Выполнено задач - " + Convert.ToString(tasks_performed_all);
            lOperationsCompleted1.Text = "Выполнено операций - " + Convert.ToString(operations_completed);
        }

        private void Conclusion3()
        {
            int theor_perf = 0;
            double theor_perf_quoted;
            int tasks_performed_all = 0;
            int operations_completed = 0;

            for (int i = 0; i < units2.Count(); i++)
            {
                ListViewItem r1 = new ListViewItem("операции: " + Convert.ToString(units2[i].operations));
                ListViewItem r2 = new ListViewItem("задач: " + Convert.ToString(units2[i].tasks));
                switch (i)
                {
                    case 0:
                        lUnit000.Items.Add(r1);
                        lUnit000.Items.Add(r2);
                        break;
                    case 1:
                        lUnit111.Items.Add(r1);
                        lUnit111.Items.Add(r2);
                        break;
                    case 2:
                        lUnit222.Items.Add(r1);
                        lUnit222.Items.Add(r2);
                        break;
                    case 3:
                        lUnit333.Items.Add(r1);
                        lUnit333.Items.Add(r2);
                        break;
                    default:
                        lUnit444.Items.Add(r1);
                        lUnit444.Items.Add(r2);
                        break;
                }
                theor_perf += units2[i].perf * takts;
                tasks_performed_all += units2[i].tasks;
                operations_completed += units2[i].operations;
            }
            theor_perf_quoted = theor_perf - units2[planner2_pos].perf * takts * planner_d / (planner_d + task_d);

            lESE2.Text = "КПД " + (operations_completed / (double)theor_perf).ToString("0.####");
            lESEquoted2.Text = "КПД' " + (operations_completed / theor_perf_quoted).ToString("0.####");
            lTasksNumber2.Text = "Выполнено задач - " + Convert.ToString(tasks_performed_all);
            lOperationsCompleted2.Text = "Выполнено операций - " + Convert.ToString(operations_completed);
        }

        private void Conclusion4()
        {
            int theor_perf = 0;
            double theor_perf_quoted;
            int tasks_performed_all = 0;
            int operations_completed = 0;

            for (int i = 0; i < units3.Count(); i++)
            {
                ListViewItem r1 = new ListViewItem("операции: " + Convert.ToString(units3[i].operations));
                ListViewItem r2 = new ListViewItem("задач: " + Convert.ToString(units3[i].tasks));
                switch (i)
                {
                    case 0:
                        lUnit0000.Items.Add(r1);
                        lUnit0000.Items.Add(r2);
                        break;
                    case 1:
                        lUnit1111.Items.Add(r1);
                        lUnit1111.Items.Add(r2);
                        break;
                    case 2:
                        lUnit2222.Items.Add(r1);
                        lUnit2222.Items.Add(r2);
                        break;
                    case 3:
                        lUnit3333.Items.Add(r1);
                        lUnit3333.Items.Add(r2);
                        break;
                    default:
                        lUnit4444.Items.Add(r1);
                        lUnit4444.Items.Add(r2);
                        break;
                }
                theor_perf += units3[i].perf * takts;
                tasks_performed_all += units3[i].tasks;
                operations_completed += units3[i].operations;
            }
            theor_perf_quoted = theor_perf - units3[planner3_pos].perf * takts * planner_d1 / (planner_d1 + task_d1);

            lESE3.Text = "КПД " + (operations_completed / (double)theor_perf).ToString("0.####");
            lESEquoted3.Text = "КПД' " + (operations_completed / theor_perf_quoted).ToString("0.####");
            lTasksNumber3.Text = "Выполнено задач - " + Convert.ToString(tasks_performed_all);
            lOperationsCompleted3.Text = "Выполнено операций - " + Convert.ToString(operations_completed);
        }

        private void Timer1_Tick_1(object sender, EventArgs e)
        {
            secs--;
            //lTime.Text = "Секунд: " + Convert.ToString(secs);
        }

        private async void Clear_Click(object sender, EventArgs e)
        {
            bStart.BackColor = Color.FromArgb(0, 192, 0);
            bStop.BackColor = Color.Red;
            bClear.BackColor = Color.LightGray;
            bClear.Enabled = false;

            lUnit0.Items.Clear();
            lUnit1.Items.Clear();
            lUnit2.Items.Clear();
            lUnit3.Items.Clear();
            lUnit4.Items.Clear();
            lUnit00.Items.Clear();
            lUnit11.Items.Clear();
            lUnit22.Items.Clear();
            lUnit33.Items.Clear();
            lUnit44.Items.Clear();
            lUnit000.Items.Clear();
            lUnit111.Items.Clear();
            lUnit222.Items.Clear();
            lUnit333.Items.Clear();
            lUnit444.Items.Clear();
            lUnit0000.Items.Clear();
            lUnit1111.Items.Clear();
            lUnit2222.Items.Clear();
            lUnit3333.Items.Clear();
            lUnit4444.Items.Clear();
            lTasks.Items.Clear();
            //tUnit0Perf.Text = "";
            //tUnit1Perf.Text = "";
            //tUnit2Perf.Text = "";
            //tUnit3Perf.Text = "";
            //tUnit4Perf.Text = "";
            //tUserPoss.Text = "1";
            //tUserCompl.Text = "";
            //tUserCompl1.Text = "";
            for (int i = 4; 0 <= i; i--)
            {
                units.RemoveAt(i);
                units1.RemoveAt(i);
                units2.RemoveAt(i);
                units3.RemoveAt(i);
                await Task.Delay(10);
            }

            for (int i = 0; i < 5; i++)
            {
                units.Add(new unit());
                units1.Add(new unit());
                units2.Add(new unit());
                units3.Add(new unit());
            }

            FifoTasks = 0;
            M2Tasks = 0;
            M3Tasks = 0;
            M4Tasks = 0;
            itask = 0;
            secs = 10;
            //lTime.Text = "Секунд: 10";
            lESE.Text = "";
            lTasksNumber.Text = "";
            lOperationsCompleted.Text = "";
            lESE1.Text = "";
            lESEquoted1.Text = "";
            lTasksNumber1.Text = "";
            lOperationsCompleted1.Text = "";
            lESE2.Text = "";
            lESEquoted2.Text = "";
            lTasksNumber2.Text = "";
            lOperationsCompleted2.Text = "";
            lESE3.Text = "";
            lESEquoted3.Text = "";
            lTasksNumber3.Text = "";
            lOperationsCompleted3.Text = "";
            takts = 0;
            bStart.Enabled = true;

            bStart.BackColor = Color.FromArgb(0, 192, 0);
            bStop.BackColor = Color.Red;
            bClear.BackColor = Color.LightGray;
            bClear.Enabled = false;

            lUnit0.Items.Clear();
            lUnit1.Items.Clear();
            lUnit2.Items.Clear();
            lUnit3.Items.Clear();
            lUnit4.Items.Clear();
            lUnit00.Items.Clear();
            lUnit11.Items.Clear();
            lUnit22.Items.Clear();
            lUnit33.Items.Clear();
            lUnit44.Items.Clear();
            lUnit000.Items.Clear();
            lUnit111.Items.Clear();
            lUnit222.Items.Clear();
            lUnit333.Items.Clear();
            lUnit444.Items.Clear();
            lUnit0000.Items.Clear();
            lUnit1111.Items.Clear();
            lUnit2222.Items.Clear();
            lUnit3333.Items.Clear();
            lUnit4444.Items.Clear();
            lTasks.Items.Clear();
            //tUnit0Perf.Text = "";
            //tUnit1Perf.Text = "";
            //tUnit2Perf.Text = "";
            //tUnit3Perf.Text = "";
            //tUnit4Perf.Text = "";
            //tUserPoss.Text = "1";
            //tUserCompl.Text = "";
            //tUserCompl1.Text = "";
            for (int i = 4; 0 <= i; i--)
            {
                units.RemoveAt(i);
                units1.RemoveAt(i);
                units2.RemoveAt(i);
                await Task.Delay(10);
            }

            for (int i = 0; i < 5; i++)
            {
                units.Add(new unit());
                units1.Add(new unit());
                units2.Add(new unit());
            }

            FifoTasks = 0;
            M2Tasks = 0;
            itask = 0;
            secs = 10;
            //lTime.Text = "Секунд: 10";
            lESE.Text = "";
            lTasksNumber.Text = "";
            lOperationsCompleted.Text = "";
            lESE1.Text = "";
            lESEquoted1.Text = "";
            lTasksNumber1.Text = "";
            lOperationsCompleted1.Text = "";
            lESE2.Text = "";
            lESEquoted2.Text = "";
            lTasksNumber2.Text = "";
            lOperationsCompleted2.Text = "";
            lESE3.Text = "";
            lESEquoted3.Text = "";
            lTasksNumber3.Text = "";
            lOperationsCompleted3.Text = "";
            takts = 0;
            bStart.Enabled = true;
        }
    }
}