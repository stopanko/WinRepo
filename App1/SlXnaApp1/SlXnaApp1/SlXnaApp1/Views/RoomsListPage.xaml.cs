using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Threading;

namespace SlXnaApp1
{
    public partial class RoomsListPage : PhoneApplicationPage
    {
        
        public RoomsListPage()
        {
            InitializeComponent();
            WarpClient game = WarpClient.GetInstance();
            //game.AddConnectionRequestListener(new ConListen(this));
            game.AddZoneRequestListener(new ZoneReqListener(this));
            //game.AddRoomRequestListener(new RoomsListener(this));
            //game.AddRoomRequestListener(new RoomReqListener(this));
            //game.AddNotificationListener(new NotificationListener(this));
            //game.AddLobbyRequestListener(new LobbyReqListen(this));
            //listBox1.ItemsSource = list;
            WarpClient.GetInstance().GetAllRooms();
        }


        public void showResult(String result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //MessageBox.Show("Connection Error. Ensure that your keys are correct.");
                txt_Result.Text = result;

            });

        }

        private void btn_CreateRoom_Click(object sender, RoutedEventArgs e)
        {
            //_game
            //WarpClient.GetInstance().SetCustomRoomData(UserDates._UserName, "");
            WarpClient.GetInstance().CreateRoom(UserDates._UserName + "Room", UserDates._UserName, 2);
            //WarpClient.GetInstance().SetCustomRoomData(UserDates._UserName, "");
            //WarpClient.GetInstance().SetCustomRoomData("123"
            //WarpClient.GetInstance().JoinRoom(UserDates._UserName);
            //WarpClient.GetInstance().SubscribeRoom(UserDates._UserName);
            //WarpClient.GetInstance().GetLiveRoomInfo(UserDates._UserName);
            
        }

       

        

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            WarpClient.GetInstance().GetAllRooms();

            //Thread.Sleep(500);//засипаємо бо ZoneListener працює в новому поточі
            //createRoomsList(ZoneReqListener.Str);

            //List<Control> list = new List<Control>();
            //HyperlinkButton b = new HyperlinkButton();            
            //b.Content = ZoneReqListener.Str[0];
            //b.Click += new RoutedEventHandler(b_Click);
            //list.Add(b);
            //listBox1.ItemsSource = list;
            //listBox1.ItemsSource = list;
            //listBox1.ItemsSource = list;
            //txt_Result.Text = ZoneReqListener.Str[0];
            //foreach (string s in ZoneReqListener.Str)
            //{
            //    HyperlinkButton b = new HyperlinkButton();
            //    b.Content = s;
            //    b.Click += new RoutedEventHandler(b_Click);
            //    list.Add(b);
            //}

            //listBox1.ItemsSource = list;
            //foreach (HyperlinkButton b in ZoneReqListener.list)
            //{
            //    txt_Result.Text += (b.Content.ToString());
            //    //listBox1.Items.Add(b.Content.ToString());
            //}

        }

        public void b_Click(object sender, EventArgs e)
        {
            //WarpClient.GetInstance().JoinRoom((((HyperlinkButton)sender).Content).ToString());
            //WarpClient.GetInstance().SubscribeRoom((((HyperlinkButton)sender).Content).ToString());
            //WarpClient.GetInstance().GetLiveRoomInfo((((HyperlinkButton)sender).Content).ToString());
            UserDates._RoomId = (((HyperlinkButton)sender).Content).ToString();

            NavigationService.Navigate(new Uri("/Views/RoomPage.xaml", UriKind.Relative));
        }
        

        public void createRoomsList(string[] str)
        {
            List<Control> list = new List<Control>();
            foreach (string s in str)
            {                
                HyperlinkButton b = new HyperlinkButton();
                b.Content = s;
                b.Click += new RoutedEventHandler(b_Click);
                list.Add(b);
            }
            listBox1.ItemsSource = list;
        }

       


    }
}