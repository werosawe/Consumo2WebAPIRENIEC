using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;



namespace Consumo2WebAPIRENIEC
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient _httpClient;

        static Form1()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://pkioc.reniec.gob.pe/afiliacionpp/backend-afiliacion/")
            };
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int a = 1;
        }

      

     

    

       
    }
}
