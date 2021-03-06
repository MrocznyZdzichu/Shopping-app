﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Zakupy : Form
    {
        public Zakupy()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sklepy sklepy_screen = new Sklepy();
            sklepy_screen.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Produkty prod_screen = new Produkty();
            prod_screen.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Transakcje trans_sceen = new Transakcje();
            trans_sceen.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Raporty screen = new Raporty();
            screen.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Custom_SQL screen = new Custom_SQL();
            screen.Show();
        }

        private void Zakupy_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Zakupy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Q)
            {
                DB_handling.close_connection();
                MessageBox.Show("Zamknięto połączenie do DB");
            }
        }
    }
}
