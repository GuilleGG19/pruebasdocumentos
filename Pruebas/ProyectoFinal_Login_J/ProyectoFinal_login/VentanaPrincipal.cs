﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ProyectoFinal_login
{
    public partial class VentanaPrincipal : Form
    {
        Form1 inicio;
        public VentanaPrincipal()
        {
            InitializeComponent();
            cargar_datos_combo_box();
            tablaInventario.Hide();
            historial.Hide();
        }
        string[,] listaVenta = new string[200, 6];
        //cambio
        List<string> NombrePRoducto = new List<string>();
        int fila = 0;


        private void databainvetario()
        {
            //hecho por jefferson
            string consulta = "select * from compania_trabajadores.inventario;";
            MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;");
            MySqlCommand command = new MySqlCommand(consulta, con);
            try
            {
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                tablaInventario.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer data " + ex.Message);
            }

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tablaPrinciapal.Hide();
            historial.Hide();
            tablaInventario.Show();
            databainvetario();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            inicio = new Form1();
            inicio.Show();
            this.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeComponent();
            /*comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Add("Efectivo");
            comboBox2.Items.Add("Tarjtea");*/
        }

        private void tablaInventario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tablaInventario.Hide();
            historial.Hide();
            tablaPrinciapal.Show();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void VentanaPrincipal_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;");
            MySqlCommand comando = new MySqlCommand("SELECT * FROM  compania_trabajadores.inventario", con);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();

            while (registro.Read())
            {
                comboBox3.Items.Add(registro["producto"].ToString());
            }
            con.Close();
        }
        private void cargar_datos_combo_box()
        {
            string conString = "datasource=localhost;port=3306;username=root;";
            MySqlConnection con = new MySqlConnection(conString);
            MySqlCommand comando = new MySqlCommand("SELECT * FROM compania_trabajadores.inventario", con);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            while (registro.Read())
            {
                comboBox3.Items.Add(registro["producto"].ToString());
            }
            con.Close();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            String seleccion_producto = comboBox3.SelectedItem.ToString();
            // Cambio
            NombrePRoducto.Add(seleccion_producto);
            string conString = "datasource=localhost;port=3306;username=root;";
            MySqlConnection con = new MySqlConnection(conString);
            string cantidad_ingresada = textBox4.Text;
            int cantidadComparar = 0;
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT producto, precio, cantidad, ID FROM  compania_trabajadores.inventario WHERE producto = @vproducto", con);
                cmd.Parameters.AddWithValue("@vproducto", seleccion_producto);
                MySqlDataReader registro = cmd.ExecuteReader();
                //da.Fill(dt);
                if (registro.Read())
                {
                    textBox5.Text = registro["precio"].ToString();
                    cantidadComparar = int.Parse(registro["cantidad"].ToString());
                    textBox1.Text = registro["ID"].ToString();


                }
                else
                {
                    MessageBox.Show("No existe precio");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar precio: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            if (textBox4.Text != "")
            {
                double cantidad_calcular = double.Parse(textBox4.Text);
                if (cantidad_calcular <= cantidadComparar)
                {
                    double precio_calcular = double.Parse(textBox5.Text);
                    double resultado = cantidad_calcular * precio_calcular;
                    textBox6.Text = resultado.ToString();
                }
                else
                {
                    MessageBox.Show("La cantidad ingresada es superior a lo que se encuentran en existencia");
                }


            }
            else
            {
                double precio_calcular = double.Parse(textBox5.Text);
                textBox6.Text = precio_calcular.ToString();
            }

        }

        private void button4_KeyDown(object sender, KeyEventArgs e)
        {

        }


        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedItem != null && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    string nombreProducto = comboBox3.SelectedItem.ToString();
                    listaVenta[fila, 0] = textBox1.Text;
                    listaVenta[fila, 1] = nombreProducto;
                    listaVenta[fila, 2] = textBox4.Text;
                    listaVenta[fila, 3] = textBox5.Text;
                    listaVenta[fila, 4] = textBox6.Text;

                    tablaPrinciapal.Rows.Add(listaVenta[fila, 0], listaVenta[fila, 1], listaVenta[fila, 2], listaVenta[fila, 3], listaVenta[fila, 4]);
                    fila++;
                    comboBox3.ValueMember = "";
                    textBox1.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";

                }
                else
                {
                    MessageBox.Show("Ingrese todos los datos necesarios");
                }
            }
            catch
            {

            }
            totalPagar();

        }
        private void totalPagar()
        {
            double total = 0;
            foreach (DataGridViewRow row in tablaPrinciapal.Rows)
            {
                total += Convert.ToDouble(row.Cells["subtotal"].Value);
            }
            textBox2.Text = total.ToString();
        }

        private void tablaPrinciapal_SelectionChanged(object sender, EventArgs e)
        {
            var row = (sender as DataGridView).CurrentRow;
            comboBox3.SelectedItem = null;
            textBox1.Text = tablaPrinciapal.CurrentRow.Cells[0].Value.ToString();
            textBox4.Text = tablaPrinciapal.CurrentRow.Cells[2].Value.ToString();
            textBox5.Text = tablaPrinciapal.CurrentRow.Cells[3].Value.ToString();
            textBox6.Text = tablaPrinciapal.CurrentRow.Cells[4].Value.ToString();
            textBox7.Text = tablaPrinciapal.CurrentRow.Cells[4].Value.ToString();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int pos = tablaPrinciapal.CurrentRow.Index;
            tablaPrinciapal.Rows.RemoveAt(pos);
            totalPagar();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            tablaPrinciapal.Rows.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tablaPrinciapal.Hide();
            tablaInventario.Hide();
            historial.Show();
            textBox1.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

            Form formulario = new Historial();
            formulario.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string nombre_producto;
            int id = 0;
            
            string conString = "datasource=localhost;port=3306;username=root;";
            MySqlConnection con = new MySqlConnection(conString);
            string cantidad_ingresada = textBox4.Text;
            int cantidadComparar = 0;
            string seleccion_producto;
            for (int i = 0; i < NombrePRoducto.Count; i++)
            {
                nombre_producto = NombrePRoducto[i];
                seleccion_producto = nombre_producto;
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT producto, cantidad FROM  compania_trabajadores.inventario WHERE producto = @vproducto", con);
                    cmd.Parameters.AddWithValue("@vproducto", seleccion_producto);
                    MySqlDataReader registro = cmd.ExecuteReader();
                    if (registro.Read())
                    {
                        cantidadComparar = int.Parse(registro["cantidad"].ToString());
                    }
                    else
                    {
                        MessageBox.Show("Cantidad no encontrada");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar cantidad: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            
            foreach (DataGridViewRow row in tablaPrinciapal.Rows)
            {
                id = Convert.ToInt16(row.Cells["ID"].Value);

                int cantidad = Convert.ToInt16(row.Cells["cantidad"].Value);
                int cantidadActualizada = cantidadComparar - cantidad;
                double precio = Convert.ToDouble(row.Cells["precio"].Value);
                string precioString = precio.ToString();
                decimal descuentoProducto = 0;

                string update = "update compania_trabajadores.inventario " +
                    "set precio = '" + precioString + "'," +
                    "cantidad = " + cantidadActualizada + "," +
                    "descuento = " + descuentoProducto + " " +
                    "where id = " + id + ";";

                try
                {
                    MySqlConnection connection = new MySqlConnection(conString);
                    MySqlCommand command = new MySqlCommand(update, connection);
                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al Modificar: " + ex.Message);
                }
            }
            tablaPrinciapal.Rows.Clear();
        }
    }
}

