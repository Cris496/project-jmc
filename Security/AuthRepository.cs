using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LoginWinFormsDb.Security;

namespace LoginWinFormsDb.Data
{
    public class AuthRepository
    {
        // Cadena de conexión desde App.config
        private readonly string _cs = ConfigurationManager.ConnectionStrings["LoginDb"].ConnectionString;

        /// <summary>
        /// Verifica si un usuario existe
        /// </summary>
        public bool ExisteUsuario(string usuario)
        {
            const string sql = "SELECT 1 FROM Usuarios WHERE Usuario = @u";

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = usuario;
                cn.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        /// <summary>
        /// Crea un nuevo usuario con contraseña hasheada
        /// </summary>
        public void CrearUsuario(string usuario, string password, byte estado = 1)
        {
            const string sql = "INSERT INTO Usuarios (Usuario, Hash, Salt, Iteraciones, Estado) " +
                               "VALUES (@u, @h, @s, @it, @e)";

            // Genera hash, salt y número de iteraciones
            var (hash, salt, it) = PasswordHasher.HashPassword(password);

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = usuario;
                cmd.Parameters.Add("@h", SqlDbType.VarBinary, 32).Value = hash;
                cmd.Parameters.Add("@s", SqlDbType.VarBinary, 16).Value = salt;
                cmd.Parameters.Add("@it", SqlDbType.Int).Value = it;
                cmd.Parameters.Add("@e", SqlDbType.Bit).Value = estado;

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Obtiene las credenciales de un usuario (hash, salt, iteraciones y estado)
        /// </summary>
        public (byte[] Hash, byte[] Salt, int Iteraciones, bool Activo)? ObtenerCredenciales(string usuario)
        {
            const string sql = "SELECT Hash, Salt, Iteraciones, Estado FROM Usuarios WHERE Usuario = @u";

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = usuario;
                cn.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;

                    return (
                        (byte[])rd["Hash"],
                        (byte[])rd["Salt"],
                        (int)rd["Iteraciones"],
                        (bool)rd["Estado"]
                    );
                }
            }
        }
    }
}
