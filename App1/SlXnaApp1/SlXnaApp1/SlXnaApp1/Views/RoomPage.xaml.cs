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
    public partial class RoomPage : PhoneApplicationPage
    {
        public RoomPage()
        {
            InitializeComponent();
            WarpClient game = WarpClient.GetInstance();
            //підключив то шо треба для кімнати і чату
            game.AddLobbyRequestListener(new LobbyReqListen(this));
            game.AddNotificationListener(new NotificationListener(this));
            game.AddRoomRequestListener(new RoomPageListener(this));
            //підписуюся на кімнату та лобі
            //WarpClient.GetInstance().JoinRoom(UserDates._RoomId);
            WarpClient.GetInstance().SubscribeRoom(UserDates._RoomId);
            WarpClient.GetInstance().JoinLobby();
            WarpClient.GetInstance().SubscribeLobby();
            WarpClient.GetInstance().JoinRoom(UserDates._RoomId);
            //Thread.Sleep(500);
            
            WarpClient.GetInstance().GetLiveRoomInfo(UserDates._RoomId);


            
        }




        public void showResult(String result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //MessageBox.Show("Connection Error. Ensure that your keys are correct.");
                txt_Result.Text = result;

            });

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.Relative));
        }

        private void btn_Chat_Click(object sender, RoutedEventArgs e)
        {
            WarpClient.GetInstance().SendChat(tb_Chat.Text);
        }
    }
}