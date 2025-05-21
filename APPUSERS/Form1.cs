using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APPUSERS
{
    public partial class FRMRESULTADO : Form
    {
        string rutaBD = "usuarios.db";
        string cadenaConexion;

        public FRMRESULTADO()
        {
            InitializeComponent();
            cadenaConexion = $"Data Source={rutaBD};Version=3;";
            CrearBaseDeDatos();
            CargarUsuarios();
        }


        private void CrearBaseDeDatos()
        {
            if (!File.Exists(rutaBD))
            {
                SQLiteConnection.CreateFile(rutaBD);

                using (var conn = new SQLiteConnection(cadenaConexion))
                {
                    conn.Open();
                    string query = @"CREATE TABLE usuarios (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    nombre TEXT NOT NULL,
                                    correo TEXT NOT NULL)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string correo = txtCorreo.Text.Trim();

            if (nombre == "" || correo == "")
            {
                MessageBox.Show("Completa ambos campos.");
                return;
            }

            using (var conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                string query = "INSERT INTO usuarios (nombre, correo) VALUES (@nombre, @correo)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.ExecuteNonQuery();
                }
            }

            txtNombre.Clear();
            txtCorreo.Clear();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            using (var conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                string query = "SELECT * FROM usuarios";
                using (var da = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUsuarios.DataSource = dt;
                }
            }
        }
    }
}
