using BlazorApp_SignalR.Models;
using GalaSoft.MvvmLight;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApp1
{
    public class MainViewModel : ViewModelBase
    {
        string myToken = "qsjdhfqdfqDFGDFDFD3";
        public bool IsConnected
        {
            get
            {
                return hubConnection?.State == HubConnectionState.Connected;
            }
        }

        private HubConnection hubConnection;

        private BackgroundWorker backgrounWorker = new BackgroundWorker();

        private ObservableCollection<string> messageList = new ObservableCollection<string>();
        public ObservableCollection<string> MessageList
        {
            get { return messageList; }
            set { messageList = value; }
        }

        private ObservableCollection<Employee> empList = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> EmpList
        {
            get { return empList; }
            set
            {
                empList = value;
            }
        }

        public string updatedOn;

        public MainViewModel()
        {
            backgrounWorker.DoWork += BackgrounWorker_DoWork;

            //EmpList = new ObservableCollection<Employee>();
            //MessageList = new ObservableCollection<string>();
            LoadSignalR();
        }

        private void BackgrounWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5074/signalhub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(myToken);
            })
            .Build();

            hubConnection.On<Employee>("ReceiveEmployee", (incomingEmployee) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(
                    (Action)delegate ()
                    {
                        EmpList.Add(incomingEmployee);
                    });
                updatedOn = DateTime.Now.ToLongTimeString();
            });

            hubConnection.On<string>("ReceiveMessage", (incomingMessage) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(
                    (Action)delegate ()
                    {
                        MessageList.Add(incomingMessage);
                    });
                updatedOn = DateTime.Now.ToLongTimeString();
            });

            hubConnection.StartAsync();
        }

        private void LoadSignalR()
        {
            backgrounWorker.RunWorkerAsync();
        }
    }
}
