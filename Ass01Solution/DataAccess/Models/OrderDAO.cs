using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class OrderDAO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int orderId;
        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; OnPropertyChanged("OrderId"); }
        }

        private int memberId;
        public int MemberId
        {
            get { return memberId; }
            set { memberId = value; OnPropertyChanged("MemberId"); }
        }

        private DateTime orderDate;
        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; OnPropertyChanged("OrderDate"); }
        }

        private DateTime? requiredDate;
        public DateTime? RequiredDate
        {
            get { return requiredDate; }
            set { requiredDate = value; OnPropertyChanged("RequiredDate"); }
        }

        private DateTime? shippedDate;
        public DateTime? ShippedDate
        {
            get { return shippedDate; }
            set { shippedDate = value; OnPropertyChanged("ShippedDate"); }
        }

        private decimal? freight;
        public decimal? Freight
        {
            get { return freight; }
            set { freight = value; OnPropertyChanged("Freight"); }
        }


        private ObservableCollection<OrderDetailDAO> orderDetailDAOs;
        public ObservableCollection<OrderDetailDAO> OrderDetailDAOs
        {
            get { return orderDetailDAOs; }
            set { orderDetailDAOs = value; OnPropertyChanged("OrderDetailDAOs"); }
        }

    }
}
