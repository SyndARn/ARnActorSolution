﻿using Actor.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Amoeba
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ResultActor resultActor;
        private void button1_Click(object sender, EventArgs e)
        {
            var amoeba = new AmoebaActor();
            resultActor = new ResultActor();
            amoeba.Launch(resultActor);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (resultActor != null)
            {
                IMessageParam<long, long> data = await resultActor.GetResult().ResultAsync();
                textBox1.AppendText(string.Format("Somme {0} Quantité {1} \n", data.Item1, data.Item2)) ;
            }
        }

        PiActor pi;
        private void button3_Click(object sender, EventArgs e)
        {
            pi = new PiActor();
            pi.Launch(100000);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (pi != null)
            {
                var data = await pi.GetResultAsync();
                textBox1.AppendText(data);
            }
        }
    }
}
