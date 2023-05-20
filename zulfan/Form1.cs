using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace zulfan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class databaseHelper
        {
            string connString = "Host=localhost;Username=postgres;Password=1;Database=Gudang";
            NpgsqlConnection conn;

            public void connect()
            {
                if (conn == null)
                {
                    conn = new NpgsqlConnection(connString);
                }
                conn.Open();
            }

            public DataTable getData(string sql)
            {
                DataTable table = new DataTable();
                connect();
                try
                {
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    conn.Close();
                }
                return table;
            }

            public void exc(String sql)
            {
                connect();
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch
                {

                }
                finally
                {
                    conn.Close();
                }
            }
        }

        databaseHelper db = new databaseHelper();


        private void Form1_Load(object sender, EventArgs e)
        {
            DataBuah();
        }

        private void DataBuah()
        {
            string sql = "select * from buah";
            dataGridView1.DataSource = db.getData(sql);

            dataGridView1.Columns["id_buah"].HeaderText = "ID Buah";
            dataGridView1.Columns["nama"].HeaderText = "Nama";
            dataGridView1.Columns["ukuran"].HeaderText = "Ukuran";
            dataGridView1.Columns["tanggal_panen"].HeaderText = "Tanggal Panen";
            dataGridView1.Columns["edit"].DisplayIndex = 5;
            dataGridView1.Columns["delete"].DisplayIndex = 5;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                button1.Enabled = false;
                textBox1.Enabled = false;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["id_buah"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["nama"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["ukuran"].Value.ToString();
                dateTimePicker1.Text = dataGridView1.Rows[e.RowIndex].Cells["tanggal_panen"].Value.ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                var id = dataGridView1.Rows[e.RowIndex].Cells["id_buah"].Value.ToString();
                string sql = $"delete from buah where id_buah = '{id}'";

                DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "DELETE DATA BUAH", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Berhasil!");
                    db.exc(sql);
                    DataBuah();
                    button3.PerformClick();
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Gagal!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = $"INSERT INTO buah(id_buah,nama,ukuran,tanggal_panen) VALUES ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{dateTimePicker1.Value.ToString()}')";
            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "INSERT DATA BUAH", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                MessageBox.Show("Berhasil!");
                db.exc(sql);
                DataBuah();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = $"update buah set nama = '{textBox2.Text}', ukuran = '{textBox3.Text}', tanggal_panen = '{dateTimePicker1.Value.ToString()}' where id_buah = '{textBox1.Text}'";

            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "UPDATE DATA BUAH", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                MessageBox.Show("Berhasil!");
                db.exc(sql);
                DataBuah();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            button1.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
