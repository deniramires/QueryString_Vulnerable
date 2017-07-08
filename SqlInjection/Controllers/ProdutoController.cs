using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SqlInjection.Controllers
{
    public class ProdutoController : Controller
    {
        [HttpGet]
        public ActionResult List(int? id)
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProdutosDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var consulta = "SELECT * FROM Produtos";

            try
            {
                using (var conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();

                    using (SqlCommand comando = new SqlCommand(consulta, conexao))
                    {
                        if (id.HasValue)
                        {
                            comando.CommandText = "SELECT * FROM Produtos WHERE Id = @id;";
                            comando.Parameters.Add(new SqlParameter("@id", id));
                        }

                        var leitor = comando.ExecuteReader();
                        if (leitor.HasRows)
                        {
                            var produtos = new List<string>();
                            while (leitor.Read())
                            {
                                produtos.Add(leitor["Descricao"].ToString());
                            }
                            ViewBag.Produtos = produtos;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Mensagem = "Erro: " + e.Message;
            }

            return View();
        }
    }
}