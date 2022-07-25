using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using DataLayer;

namespace BussinesLayer
{
    public class BL_Report
    {
        private DL_Report objDataLayer = new DL_Report();

        public List<Report> Ventas(string startdate, string finishdate, string idtransaction)
        {
            return objDataLayer.Ventas(startdate, finishdate, idtransaction);
        }

        public Dashboard ShowDashboard()
        {
            return objDataLayer.ShowDashboard();
        }

    }
}
