using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRUD.EntityLayer;
using CRUD.BusinessLayer;
using System.Globalization;

namespace CRUD.Departamentos
{
    public partial class Contact : Page
    {
        private static int idEmpleado = 0;
        DepartamentoBL departamentoBL = new DepartamentoBL();
        EmpleadoBL empleadoBL = new EmpleadoBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["idEmpleado"] != null)
                {
                    idEmpleado = Convert.ToInt32(Request.QueryString["idEmpleado"].ToString());

                    if(idEmpleado != 0)
                    {
                        lblTitulo.Text = "Editar Empleado";
                        btnSubmit.Text = "Actualizar";

                        Empleado empleado = empleadoBL.Obtener(idEmpleado);
                        txtNombreCompleto.Text = empleado.NombreCompleto;
                        CargarDepartamentos(empleado.Departamento.IdDepartamento.ToString());
                        txtSueldo.Text = empleado.Sueldo.ToString();
                        txtFechaContrato.Text = Convert.ToDateTime(empleado.FechaContrato, new CultureInfo("es-AR")).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        lblTitulo.Text = "Nuevo Empleado";
                        btnSubmit.Text = "Guardar";
                        CargarDepartamentos();
                    }
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        private void CargarDepartamentos(string idDepartamento = "")
        {
            List<Departamento> lista = departamentoBL.Lista();

            ddlDepartamento.DataTextField = "Nombre";
            ddlDepartamento.DataValueField = "IdDepartamento";

            ddlDepartamento.DataSource = lista;
            ddlDepartamento.DataBind();

            if (idDepartamento != "")
                ddlDepartamento.SelectedValue = idDepartamento;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Empleado entidad = new Empleado()
            {
                IdEmpleado = idEmpleado,
                NombreCompleto = txtNombreCompleto.Text,
                Departamento = new Departamento() { IdDepartamento = Convert.ToInt32(ddlDepartamento.SelectedValue)},
                Sueldo = Convert.ToDecimal(txtSueldo.Text, new CultureInfo("es-AR")),
                FechaContrato = txtFechaContrato.Text
            };

            bool respueta;

            if (idEmpleado != 0)
                respueta = empleadoBL.Editar(entidad);
            else
                respueta = empleadoBL.Crear(entidad);

            if (respueta)
                Response.Redirect("~/Default");
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('No se pudo realizar la operacion')", true);
        }
    }
}