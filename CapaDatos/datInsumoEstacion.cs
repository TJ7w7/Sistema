using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datInsumoEstacion
    {
        #region Singleton
        private static readonly datInsumoEstacion _instancia = new datInsumoEstacion();
        public static datInsumoEstacion Instancia
        {
            get { return _instancia; }
        }
        #endregion

        #region Métodos

        // Asignar insumo a estación
        public bool AsignarInsumoEstacion(entInsumoEstacion ie)
        {
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_AsignarInsumoEstacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsumoId", ie.InsumoId);
                cmd.Parameters.AddWithValue("@EstacionId", ie.EstacionId);
                cmd.Parameters.AddWithValue("@StockActual", ie.StockActual);
                cmd.Parameters.AddWithValue("@StockMinimo", ie.StockMinimo);
                cn.Open();

                cmd.ExecuteNonQuery();
                resultado = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return resultado;
        }

        // Listar estaciones de un insumo
        public List<entInsumoEstacion> ListarEstacionesPorInsumo(int insumoId)
        {
            SqlCommand cmd = null;
            List<entInsumoEstacion> lista = new List<entInsumoEstacion>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarEstacionesPorInsumo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsumoId", insumoId);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entInsumoEstacion ie = new entInsumoEstacion
                    {
                        InsumoEstacionId = Convert.ToInt32(dr["InsumoEstacionId"]),
                        EstacionId = Convert.ToInt32(dr["EstacionId"]),
                        NombreEstacion = dr["NombreEstacion"].ToString(),
                        StockActual = Convert.ToDecimal(dr["StockActual"]),
                        StockMinimo = Convert.ToDecimal(dr["StockMinimo"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    lista.Add(ie);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return lista;
        }

        // Actualizar stock
        public bool ActualizarStock(int insumoEstacionId, decimal stockActual)
        {
            SqlCommand cmd = null;
            bool actualiza = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ActualizarStock", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsumoEstacionId", insumoEstacionId);
                cmd.Parameters.AddWithValue("@StockActual", stockActual);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) actualiza = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return actualiza;
        }

        // Eliminar asignación
        public bool EliminarInsumoEstacion(int insumoEstacionId)
        {
            SqlCommand cmd = null;
            bool elimina = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EliminarInsumoEstacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsumoEstacionId", insumoEstacionId);
                cn.Open();

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) elimina = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return elimina;
        }

        // Listar insumos con estaciones (vista completa)
        public List<entInsumo> ListarInsumosConEstaciones()
        {
            SqlCommand cmd = null;
            Dictionary<int, entInsumo> dicInsumos = new Dictionary<int, entInsumo>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarInsumosConEstaciones", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int insumoId = Convert.ToInt32(dr["InsumoId"]);

                    // Si el insumo no existe en el diccionario, agregarlo
                    if (!dicInsumos.ContainsKey(insumoId))
                    {
                        dicInsumos[insumoId] = new entInsumo
                        {
                            InsumoId = insumoId,
                            Nombre = dr["Nombre"].ToString(),
                            UnidadMedida = dr["UnidadMedida"].ToString(),
                            Estado = Convert.ToBoolean(dr["InsumoActivo"]),
                            Estaciones = new List<entInsumoEstacion>()
                        };
                    }

                    // Si tiene estación asignada, agregarla
                    if (dr["EstacionId"] != DBNull.Value)
                    {
                        dicInsumos[insumoId].Estaciones.Add(new entInsumoEstacion
                        {
                            InsumoEstacionId = Convert.ToInt32(dr["InsumoEstacionId"]),
                            InsumoId = insumoId,
                            EstacionId = Convert.ToInt32(dr["EstacionId"]),
                            NombreEstacion = dr["NombreEstacion"].ToString(),
                            StockActual = Convert.ToDecimal(dr["StockActual"]),
                            StockMinimo = Convert.ToDecimal(dr["StockMinimo"]),
                            EsBajoStock = Convert.ToBoolean(dr["EsBajoStock"])
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return new List<entInsumo>(dicInsumos.Values);
        }

        // Listar insumos por estación (FALTANTE)
        public List<entInsumoEstacion> ListarInsumosPorEstacion(int estacionId)
        {
            SqlCommand cmd = null;
            List<entInsumoEstacion> lista = new List<entInsumoEstacion>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListarInsumosPorEstacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstacionId", estacionId); // ✅ Aquí se pasa el valor, no una variable no declarada
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entInsumoEstacion ie = new entInsumoEstacion
                    {
                        InsumoEstacionId = Convert.ToInt32(dr["InsumoEstacionId"]),
                        InsumoId = Convert.ToInt32(dr["InsumoId"]),
                        Nombre = dr["Nombre"].ToString(),
                        UnidadMedida = dr["UnidadMedida"].ToString(),
                        EstacionId = Convert.ToInt32(dr["EstacionId"]),
                        NombreEstacion = dr["NombreEstacion"].ToString(),
                        StockActual = Convert.ToDecimal(dr["StockActual"]),
                        StockMinimo = Convert.ToDecimal(dr["StockMinimo"]),
                        Estado = Convert.ToBoolean(dr["Estado"]),
                        EsBajoStock = Convert.ToBoolean(dr["EsBajoStock"])
                    };
                    lista.Add(ie);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return lista;
        }

        #endregion
    }
}
