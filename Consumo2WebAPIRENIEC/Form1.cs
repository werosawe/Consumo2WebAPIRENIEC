using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Consumo2WebAPIRENIEC.BE.Entities;
using Consumo2WebAPIRENIEC.BL.Formatting;
using Consumo2WebAPIRENIEC.BL.Services;

namespace Consumo2WebAPIRENIEC
{
    public partial class Form1 : Form
    {
        private readonly ReniecService _reniecService;

        public Form1()
        {
            InitializeComponent();
            _reniecService = new ReniecService();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
