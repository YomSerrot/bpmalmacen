﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace bpmalmacen
{
    public partial class catpartidas : Form
    {
        dbcom obj = new dbcom();
        private static string clickclave;
        private static string clicknombre;
        private static string clickdepto;
        // private static string clickjefe;
        private static string amodificar;
        public catpartidas()
        {
            InitializeComponent();
        }

        private void catpartidas_Load(object sender, EventArgs e)
        {
            foto.Image = Image.FromFile(entrada.fotousuarioruta);
            usuario1.Text = " " + entrada.nombreusario;
            this.Text = entrada.tituloventana;
            try
            {
                this.dataGridView1.DataSource = obj.sql("SELECT clave as 'Clave', Nombre as 'Nombre', clasifica as 'Clasificación' FROM partidas order by clave;");
                imprimir.Enabled = true;
                amodificar = "no";
            }
            catch (Exception)
            {
                MessageBox.Show("Error en catalogo de Partidas");
                imprimir.Enabled = false;

            }
        }

        private void salir_Click(object sender, EventArgs e)
        {
            // principal frm = new principal();
            // frm.Show();
            this.Hide();
        }

        private void agregar_Click(object sender, EventArgs e)
        {
            clave1.Enabled = true;
            clasifica1.Enabled = true;
            nombre1.Enabled = true;
            nombre1.Focus();
            grabar.Enabled = true;
            cancelar.Enabled = true;
            agregar.Enabled = false;
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            clave1.Enabled = false;
            clasifica1.Enabled = false;
            nombre1.Enabled = false;
            grabar.Enabled = false;
            cancelar.Enabled = false;
            agregar.Enabled = true;
            Modificar.Enabled = false;
            amodificar = "no";
        }

        private void grabar_Click(object sender, EventArgs e)
        {
            dbcom obj = new dbcom();
            clave1.Enabled = false;
            clasifica1.Enabled = false;
            nombre1.Enabled = false;
            grabar.Enabled = false;
            cancelar.Enabled = false;
            agregar.Enabled = true;
            imprimir.Enabled = true;

            if (amodificar == "si")
            {
                try
                {
                    obj.sql("UPDATE partidas SET nombre = '" +
                            this.nombre1.Text.ToUpper()
                            + "',clasifica='" +
                            this.clasifica1.Text
                             + "' WHERE clave = " +
                             this.dataGridView1.CurrentRow.Cells[0].Value.ToString()
                            + ";");
                    this.dataGridView1.DataSource = obj.sql("SELECT clave as 'Clave', Nombre as 'Nombre', clasifica as 'Clasificación' FROM partidas order by clave;");

                    //this.ClearText();
                    clave1.Text = "";
                    clasifica1.Text = "";
                    nombre1.Text = "";
                     amodificar = "no";
                    MessageBox.Show("Modificacion de Partida terminada...");
                }
                catch (Exception)
                {
                    amodificar = "no";
                    MessageBox.Show("Error de modificación de Partida...");
                }

            }
            else
            {
                MySqlDataReader reader = obj.Estructura("SELECT clave FROM partidas where clave = '" + this.clave1.Text + "';");
                if (reader.Read() == false)
                {

                    try
                    {
                         obj.sql("INSERT INTO partidas (clave,nombre,clasifica) VALUES('" + this.clave1.Text + "','" +  this.nombre1.Text  + "','" + this.clasifica1.Text +  "');");

                      /*  obj.sql("INSERT INTO area (nombre,clavedepto) VALUES('" +
                       this.nombre1.Text
                       + "','" +
                        this.depto1.Text
                       + "','" +
                        "');");*/

                    this.dataGridView1.DataSource = obj.sql("SELECT clave as 'Clave', Nombre as 'Nombre', clasifica as 'Clasificación' FROM partidas order by clave;");

                        clave1.Text = "";
                        clasifica1.Text = "";
                        nombre1.Text = "";
                        amodificar = "no";
                        MessageBox.Show("Partida se agrego correctamente...");
                    }
                    catch (Exception)
                    {
                        amodificar = "no";
                        MessageBox.Show("Error de agregar Partida...");
                    }
                }
                else
                {
                    MessageBox.Show(" Partida ya existe");
                    amodificar = "no";
                    nombre1.Focus();
                }


            } // if amodificar
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                clickclave = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                clicknombre = this.dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                clickdepto = this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                // clickjefe = this.dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                clave1.Text = clickclave;
                nombre1.Text = clicknombre;
                clasifica1.Text = clickdepto;
                // jefedepto1.Text = clickjefe;
                Modificar.Enabled = true;
                agregar.Enabled = false;
                cancelar.Enabled = true;

            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione clave de Partida...");
            }
        }
    }
}
