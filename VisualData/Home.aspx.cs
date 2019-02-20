using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VisualData
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ShowData();
        }

        private void ShowData()
        {
            String myConnection = ConfigurationManager.ConnectionStrings["cnn"].ToString();
            SqlConnection con = new SqlConnection(myConnection);
            String query = "select top 20 strike_price from OptionCE_NIFTY where LTP =100 order by id desc";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable tb = new DataTable();
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                tb.Load(dr, LoadOption.OverwriteChanges);
                con.Close();
            }
            catch { }
               
            if(tb != null)
            {
                String chart = "";
                chart = "<canvas id=\"line-chart\" width=\"100%\" height=\"40\"></canvas>";
                chart += "<script>";
                chart += "new Chart(document.getElementById(\"line-chart\"), { type: 'line', data: {labels: [";

                // more detais
                for (int i = 0; i < 20; i++)
                {
                    chart += i.ToString() + ",";
                }                
                chart = chart.Substring(0, chart.Length - 1);

                chart += "],datasets: [{ data: [";   
                
                // get data from database and add to chart
                String value = "";
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    value += tb.Rows[i]["strike_price"].ToString() + ",";
                }
                
                value = value.Substring(0, value.Length - 1);
                chart += value;

                chart += "],label: \"Strike Price\",borderColor: \"#3e95cd\",fill: true}"; // Chart color
                chart += "]},options: { title: { display: true,text: 'Strike Price (oC)'} }"; // Chart title
                chart += "});";
                chart += "</script>";

                ltChart.Text = chart;
            }            
        }
    }
}