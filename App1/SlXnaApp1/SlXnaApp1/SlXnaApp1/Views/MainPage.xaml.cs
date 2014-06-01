using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
    


namespace SlXnaApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
               
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //settings винести в окремий клас
            /*WarpClient.initialize("c27b5f96a94fe0e53183643fd1221af45ccbd94894201a0a51eacd5694bf0d36", "9dc629fdd584be0922cf38415057d1d80868a03ca60ac51c7048b68696a4ff4e");
            WarpClient game = WarpClient.GetInstance();
            game.AddConnectionRequestListener(new ConListen(this));
            game.AddZoneRequestListener(new ZoneReqListener(this));
            game.AddRoomRequestListener(new RoomReqListener(this));
            game.AddNotificationListener(new NotificationListener(this));
            game.AddLobbyRequestListener(new LobbyReqListen(this));*/
            
        }

        // Simple button Click event handler to take us to the second page
        public static void GetConnect(string API_key, string Secret_key)
        {
            ///не пахає
            MainPage _MPage = new MainPage();
            Random R = new Random();
            _MPage.textBlock1.Text = "";
            WarpClient.initialize(API_key, Secret_key);
            WarpClient game = WarpClient.GetInstance();
            game.AddConnectionRequestListener(new ConListen(_MPage));
            //game.AddZoneRequestListener(new ZoneReqListener(this));
            //game.AddRoomRequestListener(new RoomReqListener(this));
            //game.AddNotificationListener(new NotificationListener(this));
            //game.AddLobbyRequestListener(new LobbyReqListen(this));
            Thread.Sleep(500);
            UserDates._UserName = _MPage.textBox1.Text + R.Next(2000).ToString();
            WarpClient.GetInstance().Connect(UserDates._UserName);         
           
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //btn1.Content = "s";
            

           
            //myGame.Connect("TestUserName");
            //myGame.JoinRoom("1995630518");
            //myGame.SubscribeRoom("1995630518");
            //byte[] mByte = { Byte.Parse("123") };
            //WarpClient.GetInstance().SendUpdatePeers(mByte);
            //myGame.AddConnectionRequestListener(listenerObj);        


            
            NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.Relative));

        }

        public void showResult(String result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //MessageBox.Show("Connection Error. Ensure that your keys are correct.");
                textBlock1.Text = result;
                
            });


          
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //settings винести в окремий клас
            WarpClient.GetInstance().JoinRoom("1995630518");
            WarpClient.GetInstance().SubscribeRoom("1995630518");
            WarpClient.GetInstance().GetLiveRoomInfo("1995630518");
            //WarpClient.GetInstance().SubscribeRoom("1995630518");
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //WarpClient game = WarpClient.GetInstance();
            WarpClient.GetInstance().SendChat("Chat");
            //The information includes the names of the currently joined users, the rooms properties and any associated customData.
            
            //Retrieves usernames of all the users connected (online) to the server
            //WarpClient.GetInstance().GetOnlineUsers();  
            //WarpClient.GetInstance().GetAllRooms(); 
            
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            ///JObject jObj = new JObject();
            //jObj.Add("name", "stefan");
            string name= textBox1.Text;
            
            WarpClient.GetInstance().SendUpdatePeers(System.Text.Encoding.UTF8.GetBytes(name));
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            WarpClient.GetInstance().Connect(textBox1.Text);
        }

        private void btn_USA_server_Click(object sender, RoutedEventArgs e)
        {
            Random R = new Random();
            textBlock1.Text = "";
            WarpClient.initialize("c27b5f96a94fe0e53183643fd1221af45ccbd94894201a0a51eacd5694bf0d36", "9dc629fdd584be0922cf38415057d1d80868a03ca60ac51c7048b68696a4ff4e");
            WarpClient game = WarpClient.GetInstance();
            game.AddConnectionRequestListener(new ConListen(this));
            //game.AddZoneRequestListener(new ZoneReqListener(this));
            //game.AddRoomRequestListener(new RoomReqListener(this));
            //game.AddNotificationListener(new NotificationListener(this));
            //game.AddLobbyRequestListener(new LobbyReqListen(this));
            Thread.Sleep(500);
            UserDates._UserName = textBox1.Text + R.Next(2000).ToString();
            WarpClient.GetInstance().Connect(UserDates._UserName);
           
            //GetConnect("c27b5f96a94fe0e53183643fd1221af45ccbd94894201a0a51eacd5694bf0d36", "9dc629fdd584be0922cf38415057d1d80868a03ca60ac51c7048b68696a4ff4e");
        }

      
       

       

        
    }
}